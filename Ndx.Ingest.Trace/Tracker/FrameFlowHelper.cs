using Ndx.Model;
using PacketDotNet;
using System;

namespace Ndx.Ipflow
{
    public class FrameFlowHelper : IFlowHelper<Frame>
    {
        /// <summary>
        /// Gets <see cref="FlowKey"/> for the passsed <see cref="Frame"/>. 
        /// </summary>
        /// <param name="frame">The raw frame for which flow key is provided.</param>
        /// <returns><see cref="FlowKey"/> instance or null if provided frame does not contain TCP or UDP segment nor bytes.</returns>
        public (FlowKey,FlowFlags) GetFlowKey(Frame frame)
        {
            if (frame.HasBytes)
            {
                var packet = Packet.ParsePacket((LinkLayers)frame.LinkType, frame.Bytes);
                return GetFlowKey(packet);
            }
            else
            {
                return (null, FlowFlags.None);
            }
        }

        public (FlowKey, FlowFlags) GetFlowKey(Packet packet)
        {
            
            switch ((TransportPacket)packet.Extract(typeof(TransportPacket)))
            {
                case UdpPacket udp: return GetFlowKey(udp);
                case TcpPacket tcp: return GetFlowKey(tcp);
                default:
                    switch ((InternetPacket)packet.Extract(typeof(InternetPacket)))
                    {
                        case IpPacket ip: return GetFlowKey(ip);
                        default: return (FlowKey.None, FlowFlags.None);
                    }
            }
        }


        public (FlowKey, FlowFlags) GetFlowKey(UdpPacket packet)
        {
            return (new FlowKey()
            {
                Type = FlowType.NetworkFlow,
                IpProtocol = IpProtocolType.Udp,
                SourceIpAddress = (packet.ParentPacket as IpPacket).SourceAddress,
                DestinationIpAddress = (packet.ParentPacket as IpPacket).DestinationAddress,
                SourcePort = packet.SourcePort,
                DestinationPort = packet.DestinationPort
            }, FlowFlags.None);
        }
        public (FlowKey, FlowFlags) GetFlowKey(TcpPacket packet)
        {
            return (new FlowKey()
            {
                Type = FlowType.NetworkFlow,
                IpProtocol = IpProtocolType.Tcp,
                SourceIpAddress = (packet.ParentPacket as IpPacket).SourceAddress,
                DestinationIpAddress = (packet.ParentPacket as IpPacket).DestinationAddress,
                SourcePort = packet.SourcePort,
                DestinationPort = packet.DestinationPort
            }, packet.Syn && !packet.Ack ? FlowFlags.StartNewConversation : FlowFlags.None);
        }

        public (FlowKey, FlowFlags) GetFlowKey(IpPacket packet)
        {
            return (new FlowKey()
            {
                Type = FlowType.NetworkFlow,
                IpProtocol = (IpProtocolType)packet.Protocol,
                SourceIpAddress = packet.SourceAddress,
                SourcePort = 0,
                DestinationIpAddress = packet.DestinationAddress,
                DestinationPort = 0
            }, FlowFlags.None);
        }

        /// <summary>
        /// This methods performs flow update calculations for <see cref="Frame"/> record source.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="flowAttributes">Flow attributes to be updated.</param>
        /// <returns>The frame number.</returns>
        public long UpdateConversation(Frame frame, FlowAttributes flowAttributes)
        {
            var packet = frame.Parse();
            var transportPacket = (TransportPacket)packet.Extract(typeof(TransportPacket));
            flowAttributes.Octets += transportPacket.PayloadPacket.BytesHighPerformance.Length;
            flowAttributes.Packets += 1;
            flowAttributes.FirstSeen = Math.Min(flowAttributes.FirstSeen, frame.TimeStamp);
            flowAttributes.LastSeen = Math.Max(flowAttributes.FirstSeen, frame.TimeStamp);
            flowAttributes.MaximumInterarrivalTime = 0;
            flowAttributes.MaximumPayloadSize = Math.Max(flowAttributes.MaximumPayloadSize, transportPacket.PayloadPacket.BytesHighPerformance.Length);
            flowAttributes.MeanInterarrivalTime = 0;
            flowAttributes.MeanPayloadSize = (int)(flowAttributes.Octets / flowAttributes.Packets);
            flowAttributes.MinimumInterarrivalTime = 0;
            flowAttributes.MinimumPayloadSize = Math.Min(flowAttributes.MaximumPayloadSize, transportPacket.PayloadPacket.BytesHighPerformance.Length);
            flowAttributes.StdevInterarrivalTime = 0;
            flowAttributes.StdevPayloadSize = 0;

            return frame.FrameNumber;
        }
    }
}
