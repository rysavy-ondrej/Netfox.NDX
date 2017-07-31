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
    public class PacketAnalyzer : PacketVisitor
    {
        ConversationTracker m_tracker;
        MetaFrame m_metaFrame;
        Conversation m_conversation;
        public MetaFrame MetaFrame => m_metaFrame;
        public Conversation Conversation => m_conversation;
        public PacketAnalyzer(ConversationTracker tracker, RawFrame rawframe)
        {
            m_tracker = tracker;
            m_metaFrame = new MetaFrame()
            {
                FrameLength = rawframe.FrameLength,
                FrameNumber = rawframe.FrameNumber,
                FrameOffset = rawframe.FrameOffset,
                TimeStamp = rawframe.TimeStamp,
                LinkType = rawframe.LinkType,
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
            m_conversation = m_tracker.CreateNetworkConversation(flowKey, 0, out var flowAttributes, out var flowPackets, out var flowDirection);
            UpdateConversation(transportPacket, flowAttributes, flowPackets);
        }   

        private void GetConversation(TransportPacket transportPacket, FlowKey flowKey)
        {
            m_conversation = m_tracker.GetNetworkConversation(flowKey, 0, out var flowAttributes, out var flowPackets, out var flowDirection);
            UpdateConversation(transportPacket, flowAttributes, flowPackets);
        }

        private void UpdateConversation(TransportPacket transportPacket, FlowAttributes flowAttributes, IList<long> flowPackets)
        {
            flowAttributes.Octets += transportPacket.PayloadPacket.BytesHighPerformance.Length;
            flowAttributes.Packets += 1;
            flowAttributes.FirstSeen = Math.Min(flowAttributes.FirstSeen, m_metaFrame.TimeStamp);
            flowAttributes.LastSeen = Math.Max(flowAttributes.FirstSeen, m_metaFrame.TimeStamp);
            // TODO: Compute other attributes

            var networkPacket = transportPacket.ParentPacket;
            var datalinkPacket = networkPacket.ParentPacket;
            var applicationPacket = transportPacket.PayloadPacket;

            flowPackets.Add(m_metaFrame.FrameNumber);
            /*
            m_metaFrame.Datalink = new DatalinkPacketUnit() { Bytes = new ByteRange() { Offset = datalinkPacket.BytesHighPerformance.Offset, Length = datalinkPacket.BytesHighPerformance.Length } };
            m_metaFrame.Network = new NetworkPacketUnit() { Bytes = new ByteRange() { Offset = networkPacket.BytesHighPerformance.Offset, Length = networkPacket.BytesHighPerformance.Length } };
            m_metaFrame.Transport = new TransportPacketUnit() { Bytes = new ByteRange() { Offset = transportPacket.BytesHighPerformance.Offset, Length = transportPacket.BytesHighPerformance.Length } };
            m_metaFrame.Application = new ApplicationPacketUnit() { Bytes = new ByteRange() { Offset = applicationPacket.BytesHighPerformance.Offset, Length = applicationPacket.BytesHighPerformance.Length } };
            */
        }


        /// <summary>
        /// Gets <see cref="FlowKey"/> for the passsed <see cref="RawFrame"/>. 
        /// </summary>
        /// <param name="frame">The raw frame for which flow key is provided.</param>
        /// <returns><see cref="FlowKey"/> instance or null if provided frame does not contain TCP or UDP segment.</returns>
        public static FlowKey GetFlowKey(RawFrame frame)
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
