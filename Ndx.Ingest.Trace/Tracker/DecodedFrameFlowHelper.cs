using Ndx.Model;
using System;

namespace Ndx.Ipflow
{
    public class DecodedFrameFlowHelper : IFlowHelper<DecodedFrame>
    {
        /// <summary>
        /// Gets a network flow key for the current <see cref="DecodedFrame"/>.
        /// </summary>
        /// <param name=""></param>
        /// <param name="startNewConversation">Indicates that new conversation should be created becasue SYN flag was detected.</param>
        /// <returns></returns>
        public (FlowKey, FlowFlags) GetFlowKey(DecodedFrame packet)
        {
            var ipProto = (IpProtocolType)(packet.GetFieldValue("ip.proto", new Variant(0)).ToInt32());
            switch (ipProto)
            {
                case IpProtocolType.Tcp:
                    return (new FlowKey()
                    {
                        Type = FlowType.NetworkFlow,
                        IpProtocol = ipProto,
                        SourceIpAddress = packet.GetFieldValue("ip.src", new Variant("0.0.0.0")).ToIPAddress(),
                        SourcePort = (ushort)packet.GetFieldValue("tcp.srcport", new Variant(0)).ToInt32(),
                        DestinationIpAddress = (packet.GetFieldValue("ip.dst", "0.0.0.0")).ToIPAddress(),
                        DestinationPort = (ushort)packet.GetFieldValue("tcp.dstport", "0").ToInt32(),
                    },
                    (packet.GetFieldValue("tcp.flags.syn", new Variant(false)).Equals(new Variant(true)) 
                     &&    packet.GetFieldValue("tcp.flags.ack", new Variant(false)).Equals(new Variant(false))) ? FlowFlags.StartNewConversation : FlowFlags.None
                    );
                case IpProtocolType.Udp:
                    return (new FlowKey()
                    {
                        Type = FlowType.NetworkFlow,
                        IpProtocol = ipProto,
                        SourceIpAddress = packet.GetFieldValue("ip.src", new Variant("0.0.0.0")).ToIPAddress(),
                        SourcePort = (ushort)packet.GetFieldValue("tcp.srcport", new Variant(0)).ToInt32(),
                        DestinationIpAddress = (packet.GetFieldValue("ip.dst", "0.0.0.0")).ToIPAddress(),
                        DestinationPort = (ushort)packet.GetFieldValue("tcp.dstport", "0").ToInt32(),
                    }, FlowFlags.None);
                default:
                    return (new FlowKey()
                    {
                        Type = FlowType.NetworkFlow,
                        IpProtocol = ipProto,
                        SourceIpAddress = packet.GetFieldValue("ip.src", new Variant("0.0.0.0")).ToIPAddress(),
                        SourcePort = 0,
                        DestinationIpAddress = (packet.GetFieldValue("ip.dst", "0.0.0.0")).ToIPAddress(),
                        DestinationPort = 0,
                    }, FlowFlags.None);
            }
        }
        /// <summary>
        /// This methods performs flow update calculations for <see cref="DecodedFrame"/> record source.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="flowAttributes">Flow attributes to be updated.</param>
        /// <returns>The frame number.</returns>
        public long UpdateConversation(DecodedFrame packet, FlowAttributes flowAttributes)
        {
            var tcplen = packet.GetFieldValue("tcp.len", new Variant(0)).ToInt32();
            var udplen = packet.GetFieldValue("udp.length", 0).ToInt32();
            var iplen = packet.GetFieldValue("ip.len", 0).ToInt32();
            var framelen = packet.GetFieldValue("frame.len", 0).ToInt32();
            var payloadSize = tcplen >= 0 ? tcplen : (udplen >= 0 ? udplen : (iplen >= 0 ? iplen : framelen));
            flowAttributes.Octets += payloadSize;
            flowAttributes.Packets += 1;
            flowAttributes.FirstSeen = Math.Min(flowAttributes.FirstSeen, packet.Timestamp);
            flowAttributes.LastSeen = Math.Max(flowAttributes.FirstSeen, packet.Timestamp);
            flowAttributes.MaximumInterarrivalTime = 0;
            flowAttributes.MaximumPayloadSize = Math.Max(flowAttributes.MaximumPayloadSize, payloadSize);
            flowAttributes.MeanInterarrivalTime = 0;
            flowAttributes.MeanPayloadSize = (int)(flowAttributes.Octets / flowAttributes.Packets);
            flowAttributes.MinimumInterarrivalTime = 0;
            flowAttributes.MinimumPayloadSize = Math.Min(flowAttributes.MaximumPayloadSize, payloadSize);
            flowAttributes.StdevInterarrivalTime = 0;
            flowAttributes.StdevPayloadSize = 0;

            return packet.GetFieldValue("frame.number", new Variant(0)).ToInt64();
        }
    }
}
