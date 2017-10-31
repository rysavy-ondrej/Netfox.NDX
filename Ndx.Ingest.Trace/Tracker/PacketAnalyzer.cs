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
    /// This class extends <see cref="Packet"/> class, <see cref="Frame"/> class and <see cref="DecodedFrame"/> class 
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


        public static FlowKey GetFlowKey(this DecodedFrame packet)
        {
            return GetFlowKey(packet, out bool snc);
        }
        /// <summary>
        /// Gets a network flow key for the current <see cref="DecodedFrame"/>.
        /// </summary>
        /// <param name=""></param>
        /// <param name="startNewConversation">Indicates that new conversation should be created becasue SYN flag was detected.</param>
        /// <returns></returns>
        public static FlowKey GetFlowKey(this DecodedFrame packet, out bool startNewConversation)
        {
            startNewConversation = false;
            var ipProto = (IpProtocolType)(packet.GetFieldValue("ip.proto", new Variant(0)).ToInt32());
            switch (ipProto)
            {
                case IpProtocolType.Tcp:
                    startNewConversation = packet.GetFieldValue("tcp.flags.syn", new Variant(false)).Equals(new Variant(true)) &&
                        packet.GetFieldValue("tcp.flags.ack", new Variant(false)).Equals(new Variant(false));
                    return new FlowKey()
                    {
                        Type = FlowType.NetworkFlow,
                        IpProtocol = ipProto,
                        SourceIpAddress = packet.GetFieldValue("ip.src", new Variant("0.0.0.0")).ToIPAddress(),
                        SourcePort = (ushort) packet.GetFieldValue("tcp.srcport", new Variant(0)).ToInt32(),
                        DestinationIpAddress = (packet.GetFieldValue("ip.dst", "0.0.0.0")).ToIPAddress(),
                        DestinationPort = (ushort) packet.GetFieldValue("tcp.dstport", "0").ToInt32(),
                    };
                case IpProtocolType.Udp:
                    startNewConversation = false;
                    return new FlowKey()
                    {
                        Type = FlowType.NetworkFlow,
                        IpProtocol = ipProto,
                        SourceIpAddress = packet.GetFieldValue("ip.src", new Variant("0.0.0.0")).ToIPAddress(),
                        SourcePort = (ushort)packet.GetFieldValue("tcp.srcport", new Variant(0)).ToInt32(),
                        DestinationIpAddress = (packet.GetFieldValue("ip.dst", "0.0.0.0")).ToIPAddress(),
                        DestinationPort = (ushort)packet.GetFieldValue("tcp.dstport", "0").ToInt32(),
                    };
                default:
                    return new FlowKey()
                    {
                        Type = FlowType.NetworkFlow,
                        IpProtocol = ipProto,
                        SourceIpAddress = packet.GetFieldValue("ip.src", new Variant("0.0.0.0")).ToIPAddress(),
                        SourcePort = 0,
                        DestinationIpAddress = (packet.GetFieldValue("ip.dst", "0.0.0.0")).ToIPAddress(),
                        DestinationPort = 0,
                    };
            }
        }
    }
}
