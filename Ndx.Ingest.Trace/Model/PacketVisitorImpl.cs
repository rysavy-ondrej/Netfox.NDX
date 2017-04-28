//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using PacketDotNet;
using PacketDotNet.Ieee80211;


namespace Ndx.Ingest.Trace
{
    using AddressFamily = System.Net.Sockets.AddressFamily;
    internal class PacketVisitorImpl: PacketVisitor
    {
        PacketMetadata m_packetMetadata;

        public PacketVisitorImpl(PacketMetadata packetMetadata)
        {
            m_packetMetadata = packetMetadata;

        }

        public override void VisitWakeOnLanPacket(WakeOnLanPacket packet)
        {
        }

        public override void VisitUdpPacket(UdpPacket udp)
        {
            m_packetMetadata.SetTransport(udp.BytesHighPerformance.Offset, udp.BytesHighPerformance.Length);
            m_packetMetadata.Flow.Protocol = IPProtocolType.UDP;
            m_packetMetadata.Flow.DestinationPort = udp.DestinationPort;
            m_packetMetadata.Flow.SourcePort = udp.SourcePort;
        }

        public override void VisitTcpPacket(TcpPacket tcp)
        {
            m_packetMetadata.SetTransport(tcp.BytesHighPerformance.Offset,tcp.BytesHighPerformance.Length);
            m_packetMetadata.Flow.Protocol = IPProtocolType.TCP;
            m_packetMetadata.Flow.DestinationPort = tcp.DestinationPort;
            m_packetMetadata.Flow.SourcePort = tcp.SourcePort;
        }

        public override void VisitPPPPacket(PPPPacket packet)
        {
        }

        public override void VisitPPPoEPacket(PPPoEPacket packet)
        {
        }

        public override void VisitOSPFv2Packet(OSPFv2Packet packet)
        {
        }

        public override void VisitArpPacket(ARPPacket packet)
        {
            //m_flow.Protocol = EthernetPacketType.Arp;
        }

        public override void VisitEthernetPacket(EthernetPacket eth)
        {
            m_packetMetadata.SetLink(eth.BytesHighPerformance.Offset, eth.BytesHighPerformance.Length);
            m_packetMetadata.Flow.Protocol = IPProtocolType.NONE;
            m_packetMetadata.Flow.AddressFamily = AddressFamily.Unspecified;
            m_packetMetadata.Flow.SourceAddress = System.Net.IPAddress.None;
            m_packetMetadata.Flow.DestinationAddress = System.Net.IPAddress.None;
        }

        public override void VisitLLDPPacket(LLDPPacket packet)
        {
        }

        public override void VisitRadioPacket(RadioPacket packet)
        {
        }

        public override void VisitPpiPacket(PpiPacket packet)
        {
        }

        public override void VisitLinuxSLLPacket(LinuxSLLPacket packet)
        {
        }

        public override void VisitICMPv4Packet(ICMPv4Packet packet)
        {
        }

        public override void VisitICMPv6Packet(ICMPv6Packet packet)
        {
        }

        public override void VisitIGMPv2Packet(IGMPv2Packet packet)
        {
        }

        public override void VisitIPv4Packet(IPv4Packet ipv4)
        {
            m_packetMetadata.SetNetwork(ipv4.BytesHighPerformance.Offset, ipv4.BytesHighPerformance.Length);
            m_packetMetadata.Flow.AddressFamily = AddressFamily.InterNetwork;
            m_packetMetadata.Flow.Protocol = ipv4.Protocol;
            m_packetMetadata.Flow.SourceAddress = ipv4.SourceAddress;
            m_packetMetadata.Flow.DestinationAddress = ipv4.DestinationAddress;
        }

        public override void VisitIPv6Packet(IPv6Packet ipv6)
        {
            m_packetMetadata.SetNetwork(ipv6.BytesHighPerformance.Offset, ipv6.BytesHighPerformance.Length);
            m_packetMetadata.Flow.AddressFamily = AddressFamily.InterNetworkV6;
            m_packetMetadata.Flow.Protocol = ipv6.Protocol;
            m_packetMetadata.Flow.SourceAddress = ipv6.SourceAddress;
            m_packetMetadata.Flow.DestinationAddress = ipv6.DestinationAddress;
        }

        public override void VisitIeee8021QPacket(Ieee8021QPacket packet)
        {
        }

        public override void VisitIeee80211ManagementFrame(ManagementFrame frame)
        {
        }

        public override void VisitIeee80211ControlFrame(ControlFrame frame)
        {
        }

        public override void VisitIeee80211DataFrame(DataFrame frame)
        {
        }

        public override void VisitApplicationPacket(ApplicationPayloadPacket packet)
        {
            m_packetMetadata.SetPayload(packet.BytesHighPerformance.Offset, packet.BytesHighPerformance.Length);
        }
    }
}
