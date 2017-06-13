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
            var flowKey = new FlowKey()
            {
                IpProtocol = IpProtocolType.Icmp,
                
            }
        }

        public override void VisitICMPv6Packet(ICMPv6Packet packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitIeee80211ControlFrame(ControlFrame frame)
        {
            throw new NotImplementedException();
        }

        public override void VisitIeee80211DataFrame(DataFrame frame)
        {
            throw new NotImplementedException();
        }

        public override void VisitIeee80211ManagementFrame(ManagementFrame frame)
        {
            throw new NotImplementedException();
        }

        public override void VisitIeee8021QPacket(Ieee8021QPacket packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitIGMPv2Packet(IGMPv2Packet packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitIPv4Packet(IPv4Packet packet)
        {
            var conversation = m_tracker.GetNetworkConversation(parentConvId, packet.SourceAddress, packet.DestinationAddress);
            
        }

        public override void VisitIPv6Packet(IPv6Packet packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitLinuxSLLPacket(LinuxSLLPacket packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitLLDPPacket(LLDPPacket packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitOSPFv2Packet(OSPFv2Packet packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitPpiPacket(PpiPacket packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitPPPoEPacket(PPPoEPacket packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitPPPPacket(PPPPacket packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitRadioPacket(RadioPacket packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitTcpPacket(TcpPacket packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitUdpPacket(UdpPacket packet)
        {
            throw new NotImplementedException();
        }

        public override void VisitWakeOnLanPacket(WakeOnLanPacket packet)
        {
            throw new NotImplementedException();
        }
    }
}
