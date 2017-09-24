using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using PacketDotNet;
using PacketDotNet.Ieee80211;

namespace Ndx.Ingest
{
    /// <summary>
    /// This class implementes extension of <see cref="Packet"/> class and <see cref="Frame"/> class.
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
            var udpPacket = (UdpPacket)packet.Extract(typeof(UdpPacket));
            if (udpPacket != null) return GetFlowKey(udpPacket, out startNewConversation);
            var tcpPacket = (TcpPacket)packet.Extract(typeof(TcpPacket));
            if (tcpPacket != null) return GetFlowKey(tcpPacket, out startNewConversation);
            return FlowKey.None;
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
    }
}
