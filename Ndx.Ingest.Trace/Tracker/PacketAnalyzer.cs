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
    /// for extracting information from <see cref="Frame"/>. This information is used
    /// to provide conversation data for <see cref="ConversationTracker"/>.
    /// </summary>
    public class PacketAnalyzer : PacketVisitor
    {
        ConversationTracker m_tracker;
        Frame m_frame;
        public Frame Frame => m_frame;
        public PacketAnalyzer(ConversationTracker tracker, Frame rawframe)
        {
            m_tracker = tracker;
            m_frame = rawframe;
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
            var flowKey = GetFlowKey(packet, out bool startNewConversation);
            if (startNewConversation)
            {
                CreateConversation(packet, flowKey);
            }
            else
            {
                GetConversation(packet, flowKey);
            }
        }

        public override void VisitUdpPacket(UdpPacket packet)
        {
            var flowKey = GetFlowKey(packet, out bool startNewConversation);
            if (startNewConversation)
            {
                CreateConversation(packet, flowKey);
            }
            else
            {
                GetConversation(packet, flowKey);
            }
        }

        public override void VisitWakeOnLanPacket(WakeOnLanPacket packet)
        {
        }

        private void CreateConversation(TransportPacket transportPacket, FlowKey flowKey)
        {
            var conversation = m_tracker.CreateNetworkConversation(flowKey, 0, out var flowAttributes, out var flowPackets, out var flowDirection);
            UpdateConversation(transportPacket, flowAttributes, flowPackets);
            m_frame.ConversationId = conversation.ConversationId;
        }   

        private void GetConversation(TransportPacket transportPacket, FlowKey flowKey)
        {
            var conversation = m_tracker.GetNetworkConversation(flowKey, 0, out var flowAttributes, out var flowPackets, out var flowDirection);
            UpdateConversation(transportPacket, flowAttributes, flowPackets);
            m_frame.ConversationId = conversation.ConversationId;
        }

        private void UpdateConversation(TransportPacket transportPacket, FlowAttributes flowAttributes, IList<long> flowPackets)
        {
            flowAttributes.Octets += transportPacket.PayloadPacket.BytesHighPerformance.Length;
            flowAttributes.Packets += 1;
            flowAttributes.FirstSeen = Math.Min(flowAttributes.FirstSeen, m_frame.TimeStamp);
            flowAttributes.LastSeen = Math.Max(flowAttributes.FirstSeen, m_frame.TimeStamp);
            // TODO: Compute other attributes

            var networkPacket = transportPacket.ParentPacket;
            var datalinkPacket = networkPacket.ParentPacket;
            var applicationPacket = transportPacket.PayloadPacket;

            flowPackets.Add(m_frame.FrameNumber);
        }


        /// <summary>
        /// Gets <see cref="FlowKey"/> for the passsed <see cref="Frame"/>. 
        /// </summary>
        /// <param name="frame">The raw frame for which flow key is provided.</param>
        /// <returns><see cref="FlowKey"/> instance or null if provided frame does not contain TCP or UDP segment.</returns>
        public static FlowKey GetFlowKey(Frame frame)
        {
            var packet = Packet.ParsePacket((LinkLayers)frame.LinkType, frame.Bytes);
            return GetFlowKey(packet);
        }

        public static FlowKey GetFlowKey(Packet packet)
        {
            var udpPacket = (UdpPacket)packet.Extract(typeof(UdpPacket));
            if (udpPacket != null) return GetFlowKey(udpPacket, out bool udpNew);
            var tcpPacket = (TcpPacket)packet.Extract(typeof(TcpPacket));
            if (tcpPacket != null) return GetFlowKey(tcpPacket, out bool tcpNew);
            return null;
        }

        public static FlowKey GetFlowKey(UdpPacket packet, out bool startNewConversation)
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
        public static FlowKey GetFlowKey(TcpPacket packet, out bool startNewConversation)
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
