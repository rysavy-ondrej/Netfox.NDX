using Ndx.Decoders.Base;
using Ndx.Model;
using System;

namespace Ndx.Decoders
{
    public class PacketFlowHelper : IFlowHelper<Packet>
    {
        /// <summary>
        /// Gets a network flow key for the current <see cref="DecodedFrame"/>.
        /// </summary>
        /// <param name=""></param>
        /// <param name="startNewConversation">Indicates that new conversation should be created becasue SYN flag was detected.</param>
        /// <returns></returns>
        public (FlowKey, FlowFlags) GetFlowKey(Packet packet)
        {
            if (packet.HasProtocol(Packet.Types.Protocol.ProtocolTypeOneofCase.Ip))
            {
                return GetIpFlowKey(packet);
            }
            else if (packet.HasProtocol(Packet.Types.Protocol.ProtocolTypeOneofCase.Ipv6))
            {
                return GetIpv6FlowKey(packet);
            }
            else { return (FlowKey.None, FlowFlags.None); };
        }
        public (FlowKey, FlowFlags) GetIpFlowKey(Packet packet)
        {
            var ipProto = (IpProtocolType)(packet.Protocol<Ip>().IpProto);
            switch (ipProto)
            {
                case IpProtocolType.Tcp:
                    var startNewConversation = packet.Protocol<Tcp>().TcpFlagsSyn && packet.Protocol<Tcp>().TcpFlagsAck;
                    return (new FlowKey()
                    {
                        Type = FlowType.NetworkFlow,
                        IpProtocol = ipProto,
                        SourceIpAddress = packet.Protocol<Ip>().IpSrcAddress,
                        SourcePort = (ushort)packet.Protocol<Tcp>().TcpSrcport,
                        DestinationIpAddress = packet.Protocol<Ip>().IpDstAddress,
                        DestinationPort = (ushort)packet.Protocol<Tcp>().TcpDstport,
                    },
                    startNewConversation ? FlowFlags.StartNewConversation : FlowFlags.None
                    );
                case IpProtocolType.Udp:
                    return (new FlowKey()
                    {
                        Type = FlowType.NetworkFlow,
                        IpProtocol = ipProto,
                        SourceIpAddress = packet.Protocol<Ip>().IpSrcAddress,
                        SourcePort = (ushort)packet.Protocol<Udp>().UdpSrcport,
                        DestinationIpAddress = packet.Protocol<Ip>().IpDstAddress,
                        DestinationPort = (ushort)packet.Protocol<Udp>().UdpDstport,
                    },
                FlowFlags.None);
                default:
                    return (new FlowKey()
                    {
                        Type = FlowType.NetworkFlow,
                        IpProtocol = ipProto,
                        SourceIpAddress = packet.Protocol<Ip>().IpSrcAddress,
                        SourcePort = 0,
                        DestinationIpAddress = packet.Protocol<Ip>().IpDstAddress,
                        DestinationPort = 0,
                    },
                FlowFlags.None);
            }
        }
        public (FlowKey, FlowFlags) GetIpv6FlowKey(Packet packet)
        {

            if (packet.HasProtocol(Packet.Types.Protocol.ProtocolTypeOneofCase.Tcp))
            { 
                var startNewConversation = packet.Protocol<Tcp>().TcpFlagsSyn && packet.Protocol<Tcp>().TcpFlagsAck;
                return (new FlowKey()
                {
                    Type = FlowType.NetworkFlow,
                    IpProtocol = IpProtocolType.Tcp,
                    SourceIpAddress = packet.Protocol<Ipv6>().IpSrcAddress,
                    SourcePort = (ushort)packet.Protocol<Tcp>().TcpSrcport,
                    DestinationIpAddress = packet.Protocol<Ipv6>().IpDstAddress,
                    DestinationPort = (ushort)packet.Protocol<Tcp>().TcpDstport,
                },
                startNewConversation ? FlowFlags.StartNewConversation : FlowFlags.None
                );
            }
            else if (packet.HasProtocol(Packet.Types.Protocol.ProtocolTypeOneofCase.Udp))
            {
                return (new FlowKey()
                {
                    Type = FlowType.NetworkFlow,
                    IpProtocol = IpProtocolType.Udp,
                    SourceIpAddress = packet.Protocol<Ipv6>().IpSrcAddress,
                    SourcePort = (ushort)packet.Protocol<Udp>().UdpSrcport,
                    DestinationIpAddress = packet.Protocol<Ipv6>().IpDstAddress,
                    DestinationPort = (ushort)packet.Protocol<Udp>().UdpDstport,
                },
                FlowFlags.None);
            } else {
                return (new FlowKey()
                {
                    Type = FlowType.NetworkFlow,
                    IpProtocol = IpProtocolType.None,
                    SourceIpAddress = packet.Protocol<Ipv6>().IpSrcAddress,
                    SourcePort = 0,
                    DestinationIpAddress = packet.Protocol<Ipv6>().IpDstAddress,
                    DestinationPort = 0,
                },
                FlowFlags.None);
            }
        }

        /// <summary>
        /// This methods performs flow update calculations for <see cref="DecodedFrame"/> record source.
        /// </summary>
        /// <param name="packet">The packet.</param>
        /// <param name="flowAttributes">Flow attributes to be updated.</param>
        /// <returns>The frame number.</returns>
        public long UpdateConversation(Packet packet, FlowAttributes flowAttributes)
        {
            var tcplen = packet.Protocol<Tcp>()?.TcpLen;
            var udplen = packet.Protocol<Udp>()?.UdpLength;
            var iplen = packet.Protocol<Ip>()?.IpLen;
            var ip6len = packet.Protocol<Ipv6>()?.Ipv6Plen;
            var framelen = packet.Protocol<Base.Frame>().FrameLen;
            var payloadSize = (int) ( tcplen != null ? tcplen : (udplen != null ? udplen : (iplen != null ? iplen : framelen)) );
            flowAttributes.Octets += payloadSize;
            flowAttributes.Packets += 1;
            flowAttributes.FirstSeen = Math.Min(flowAttributes.FirstSeen, packet.TimeStamp);
            flowAttributes.LastSeen = Math.Max(flowAttributes.FirstSeen, packet.TimeStamp);
            flowAttributes.MaximumInterarrivalTime = 0;
            flowAttributes.MaximumPayloadSize = Math.Max(flowAttributes.MaximumPayloadSize, payloadSize);
            flowAttributes.MeanInterarrivalTime = 0;
            flowAttributes.MeanPayloadSize = (int)(flowAttributes.Octets / flowAttributes.Packets);
            flowAttributes.MinimumInterarrivalTime = 0;
            flowAttributes.MinimumPayloadSize = Math.Min(flowAttributes.MaximumPayloadSize, payloadSize);
            flowAttributes.StdevInterarrivalTime = 0;
            flowAttributes.StdevPayloadSize = 0;
            return packet.Protocol<Base.Frame>().FrameNumber;
        }
    }
}
