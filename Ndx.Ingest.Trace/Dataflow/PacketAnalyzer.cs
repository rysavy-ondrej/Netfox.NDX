using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using PacketDotNet;
using PacketDotNet.Ieee80211;

namespace Ndx.Metacap.Dataflow
{
    /// <summary>
    /// <see cref="PacketAnalyzer"/> implements <see cref="PacketDotNet.PacketVisitor"/>
    /// for extracting information from <see cref="RawFrame"/>. This information is used
    /// to provide conversation data for <see cref="ConversationTracker"/>.
    /// </summary>
    class PacketAnalyzer : PacketDotNet.PacketVisitor
    {
        ConversationTracker m_tracker;
        RawFrame m_rawFrame;
        public PacketAnalyzer(ConversationTracker tracker, RawFrame frame)
        {
            m_tracker = tracker;
            m_rawFrame = frame;
        }


        public override void VisitApplicationPacket(ApplicationPayloadPacket packet)
        {
        }

        public override void VisitArpPacket(ARPPacket packet)
        {
        }

        public override void VisitEthernetPacket(EthernetPacket packet)
        {
                
        }

        public override void VisitICMPv4Packet(ICMPv4Packet packet)
        {
        }

        public override void VisitICMPv6Packet(ICMPv6Packet packet)
        {
        }

        public override void VisitIeee80211ControlFrame(ControlFrame frame)
        {
        }

        public override void VisitIeee80211DataFrame(DataFrame frame)
        {
        }

        public override void VisitIeee80211ManagementFrame(ManagementFrame frame)
        {
        }

        public override void VisitIeee8021QPacket(Ieee8021QPacket packet)
        {
        }

        public override void VisitIGMPv2Packet(IGMPv2Packet packet)
        {
        }

        public override void VisitIPv4Packet(IPv4Packet packet)
        {                        
        }

        public override void VisitIPv6Packet(IPv6Packet packet)
        {
        }

        public override void VisitLinuxSLLPacket(LinuxSLLPacket packet)
        {
        }

        public override void VisitLLDPPacket(LLDPPacket packet)
        {
        }

        public override void VisitOSPFv2Packet(OSPFv2Packet packet)
        {
        }

        public override void VisitPpiPacket(PpiPacket packet)
        {
        }

        public override void VisitPPPoEPacket(PPPoEPacket packet)
        {
        }

        public override void VisitPPPPacket(PPPPacket packet)
        {
        }

        public override void VisitRadioPacket(RadioPacket packet)
        {
        }

        public override void VisitTcpPacket(TcpPacket packet)
        {
            var flowKey = new FlowKey()
            {
                Type = FlowType.NetworkFlow,
                IpProtocol = IpProtocolType.Tcp,
                SourceIpAddress = (packet.ParentPacket as IpPacket).DestinationAddress,
                DestinationIpAddress = (packet.ParentPacket as IpPacket).DestinationAddress,
                SourcePort = packet.SourcePort,
                DestinationPort = packet.DestinationPort
            };

            UpdateConversation(packet, flowKey);
        }

        private void UpdateConversation(Packet packet, FlowKey flowKey)
        {
            var conversation = m_tracker.GetNetworkConversation(flowKey, 0, out var flowAttributes, out var flowPackets, out var flowDirection);
            flowAttributes.Octets += packet.PayloadPacket.BytesHighPerformance.Length;
            flowAttributes.Packets += 1;
            flowAttributes.FirstSeen = Math.Min(flowAttributes.FirstSeen, m_rawFrame.TimeStamp);
            flowAttributes.LastSeen = Math.Max(flowAttributes.FirstSeen, m_rawFrame.TimeStamp);
            // TODO: Compute other attributes
            flowPackets.Add(m_rawFrame.FrameNumber);
        }

        public override void VisitUdpPacket(UdpPacket packet)
        {
            var flowKey = new FlowKey()
            {
                Type = FlowType.NetworkFlow,
                IpProtocol = IpProtocolType.Udp,
                SourceIpAddress = (packet.ParentPacket as IpPacket).DestinationAddress,
                DestinationIpAddress = (packet.ParentPacket as IpPacket).DestinationAddress,
                SourcePort = packet.SourcePort,
                DestinationPort = packet.DestinationPort
            };

            UpdateConversation(packet, flowKey);
        }

        public override void VisitWakeOnLanPacket(WakeOnLanPacket packet)
        {
        }
    }
}
