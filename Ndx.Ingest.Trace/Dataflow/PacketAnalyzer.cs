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
    /// <see cref="PacketAnalyzer"/> implements <see cref="PacketDotNet.PacketVisitor"/>
    /// for extracting information from <see cref="RawFrame"/>. This information is used
    /// to provide conversation data for <see cref="ConversationTracker"/>.
    /// </summary>
    class PacketAnalyzer : PacketDotNet.PacketVisitor
    {
        ConversationTracker m_tracker;
        MetaFrame m_metaFrame;
        public MetaFrame MetaFrame => m_metaFrame; 

        public PacketAnalyzer(ConversationTracker tracker, RawFrame rawframe)
        {
            m_tracker = tracker;
            m_metaFrame = new MetaFrame()
            {
                FrameLength = rawframe.FrameLength,
                FrameNumber = rawframe.FrameNumber,
                FrameOffset = rawframe.FrameOffset,
                TimeStamp = rawframe.TimeStamp,

            };
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
                SourceIpAddress = (packet.ParentPacket as IpPacket).SourceAddress,
                DestinationIpAddress = (packet.ParentPacket as IpPacket).DestinationAddress,
                SourcePort = packet.SourcePort,
                DestinationPort = packet.DestinationPort
            };

            UpdateConversation(packet, flowKey);
        }

        private void UpdateConversation(TransportPacket packet, FlowKey flowKey)
        {
            var conversation = m_tracker.GetNetworkConversation(flowKey, 0, out var flowAttributes, out var flowPackets, out var flowDirection);
            flowAttributes.Octets += packet.PayloadPacket.BytesHighPerformance.Length;
            flowAttributes.Packets += 1;
            flowAttributes.FirstSeen = Math.Min(flowAttributes.FirstSeen, m_metaFrame.TimeStamp);
            flowAttributes.LastSeen = Math.Max(flowAttributes.FirstSeen, m_metaFrame.TimeStamp);
            // TODO: Compute other attributes
            flowPackets.Add(m_metaFrame.FrameNumber);

            m_metaFrame.Network = new NetworkPacketUnit() { Bytes = new ByteRange() { Offset = packet.ParentPacket.BytesHighPerformance.Offset, Length = packet.ParentPacket.BytesHighPerformance.Length } };
            m_metaFrame.Transport = new TransportPacketUnit() { Bytes = new ByteRange() { Offset = packet.BytesHighPerformance.Offset, Length = packet.BytesHighPerformance.Length } };
            m_metaFrame.Application = new ApplicationPacketUnit() { Bytes = new ByteRange() { Offset = packet.PayloadPacket.BytesHighPerformance.Offset, Length = packet.PayloadPacket.BytesHighPerformance.Length } };
        }

        public override void VisitUdpPacket(UdpPacket packet)
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

            UpdateConversation(packet, flowKey);
        }

        public override void VisitWakeOnLanPacket(WakeOnLanPacket packet)
        {
        }
    }
}
