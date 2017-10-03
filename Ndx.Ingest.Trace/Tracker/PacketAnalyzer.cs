using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using PacketDotNet;
using PacketDotNet.Ieee80211;

namespace Ndx.Ingest
{
    /// <summary>
    /// This class extends <see cref="Packet"/> class, <see cref="Frame"/> class and <see cref="PacketFields"/> class 
    /// with methods for computing <see cref="FlowKey"/>.
    /// </summary>
    public static class PacketAnalyzer
    {
        /// <summary>
        /// Gets <see cref="FlowKey"/> for the passsed <see cref="Frame"/>. 
        /// </summary>
        /// <param name="frame">The raw frame for which flow key is provided.</param>
        /// <returns><see cref="FlowKey"/> instance or null if provided frame does not contain TCP or UDP segment nor bytes.</returns>
        public static FlowKey GetFlowKey(this Frame frame, out bool startNewConversation)
        {
            if (frame.HasBytes)
            {
                var packet = Packet.ParsePacket((LinkLayers)frame.LinkType, frame.Bytes);
                return GetFlowKey(packet, out startNewConversation);
            }
            else
            {
                startNewConversation = false;
                return null;
            }
        }
        public static FlowKey GetFlowKey(this Frame frame)
        {
            return GetFlowKey(frame, out var startNewConversation);
        }

        public static FlowKey GetFlowKey(this Packet packet, out bool startNewConversation)
        {
            startNewConversation = false;
            switch((TransportPacket)packet.Extract(typeof(TransportPacket)))
            {
                case UdpPacket udp: return GetFlowKey(udp, out startNewConversation);
                case TcpPacket tcp: return GetFlowKey(tcp, out startNewConversation);
                default:
                    switch ((InternetPacket)packet.Extract(typeof(InternetPacket)))
                    {
                        case IpPacket ip: return GetFlowKey(ip, out startNewConversation);
                        default: return FlowKey.None;
                    }
            }
        }

        public static FlowKey GetFlowKey(this Packet packet)
        {
            return GetFlowKey(packet, out var startNewConversation);
        }

        public static FlowKey GetFlowKey(this UdpPacket packet, out bool startNewConversation)
        {
            var flowKey = new FlowKey()
            {
                Type = FlowType.NetworkFlow,
                IpProtocol = IpProtocolType.Udp,
                SourceIpAddress = (packet.ParentPacket as IpPacket).SourceAddress,
                DestinationIpAddress = (packet.ParentPacket as IpPacket).DestinationAddress,
                SourcePort = packet.SourcePort,
                DestinationPort = packet.DestinationPort
            };
            startNewConversation = false;
            return flowKey;
        }
        public static FlowKey GetFlowKey(this TcpPacket packet, out bool startNewConversation)
        {
            var flowKey = new FlowKey()
            {
                Type = FlowType.NetworkFlow,
                IpProtocol = IpProtocolType.Tcp,
                SourceIpAddress = (packet.ParentPacket as IpPacket).SourceAddress,
                DestinationIpAddress = (packet.ParentPacket as IpPacket).DestinationAddress,
                SourcePort = packet.SourcePort,
                DestinationPort = packet.DestinationPort
            };
            startNewConversation = packet.Syn && !packet.Ack;
            return flowKey;
        }

        public static FlowKey GetFlowKey(this IpPacket packet, out bool startNewConversation)
        {
            startNewConversation = false;
            return new FlowKey()
            {
                Type = FlowType.NetworkFlow,
                IpProtocol = (IpProtocolType)packet.Protocol,
                SourceIpAddress = packet.SourceAddress,
                SourcePort = 0,
                DestinationIpAddress = packet.DestinationAddress,
                DestinationPort = 0
            };
        }


        public static FlowKey GetFlowKey(this PacketFields packet)
        {
            return GetFlowKey(packet, out bool snc);
        }
        /// <summary>
        /// Gets a network flow key for the current <see cref="PacketFields"/>.
        /// </summary>
        /// <param name=""></param>
        /// <param name="startNewConversation">Indicates that new conversation should be created becasue SYN flag was detected.</param>
        /// <returns></returns>
        public static FlowKey GetFlowKey(this PacketFields packet, out bool startNewConversation)
        {
            startNewConversation = false;
            var ipProto = (IpProtocolType)Int32.Parse(packet.GetFieldValue("ip_ip_proto", "0"));
            switch (ipProto)
            {
                case IpProtocolType.Tcp:
                    startNewConversation = packet.GetFieldValue("tcp_flags_tcp_flags_syn", "0").Equals("1") &&
                        packet.GetFieldValue("tcp_flags_tcp_flags_ack", "0").Equals("0");
                    return new FlowKey()
                    {
                        Type = FlowType.NetworkFlow,
                        IpProtocol = ipProto,
                        SourceIpAddress = IPAddress.Parse(packet.GetFieldValue("ip_ip_src", "0.0.0.0")),
                        SourcePort = UInt16.Parse(packet.GetFieldValue("tcp_tcp_srcport", "0")),
                        DestinationIpAddress = IPAddress.Parse(packet.GetFieldValue("ip_ip_dst", "0.0.0.0")),
                        DestinationPort = UInt16.Parse(packet.GetFieldValue("tcp_tcp_dstport", "0")),
                    };
                case IpProtocolType.Udp:
                    startNewConversation = false;
                    return new FlowKey()
                    {
                        Type = FlowType.NetworkFlow,
                        IpProtocol = ipProto,
                        SourceIpAddress = IPAddress.Parse(packet.GetFieldValue("ip_ip_src", "0.0.0.0")),
                        SourcePort = UInt16.Parse(packet.GetFieldValue("udp_udp_srcport", "0")),
                        DestinationIpAddress = IPAddress.Parse(packet.GetFieldValue("ip_ip_dst", "0.0.0.0")),
                        DestinationPort = UInt16.Parse(packet.GetFieldValue("udp_udp_dstport", "0")),
                    };
                default:
                    return new FlowKey()
                    {
                        Type = FlowType.NetworkFlow,
                        IpProtocol = ipProto,
                        SourceIpAddress = IPAddress.Parse(packet.GetFieldValue("ip_ip_src", "0.0.0.0")),
                        SourcePort = UInt16.Parse(packet.GetFieldValue("udp_udp_srcport", "0")),
                        DestinationIpAddress = IPAddress.Parse(packet.GetFieldValue("ip_ip_dst", "0.0.0.0")),
                        DestinationPort = UInt16.Parse(packet.GetFieldValue("udp_udp_dstport", "0")),
                    };
            }
        }
    }
}
