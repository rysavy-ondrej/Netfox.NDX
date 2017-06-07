//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Generic;
using Ndx.Model;
using PacketDotNet;
using PacketDotNet.Ieee80211;
namespace Ndx.Metacap
{
    using AddressFamily = System.Net.Sockets.AddressFamily;
    internal class PacketVisitorImpl: PacketVisitor
    {
        KeyValuePair<FlowKey, PacketUnit> m_packetMetadata;

        public PacketVisitorImpl(KeyValuePair<FlowKey, PacketUnit> packetMetadata)
        {
            m_packetMetadata = packetMetadata;

        }

        public override void VisitWakeOnLanPacket(WakeOnLanPacket packet)
        {
        }

        public override void VisitUdpPacket(UdpPacket udp)
        {
            m_packetMetadata.Value.Transport.Bytes.Offset = udp.BytesHighPerformance.Offset;
            m_packetMetadata.Value.Transport.Bytes.Length = udp.BytesHighPerformance.Length;
            m_packetMetadata.Key.Protocol = Ndx.Model.IpProtocolType.Udp;
            m_packetMetadata.Key.DestinationPort = udp.DestinationPort;
            m_packetMetadata.Key.SourcePort = udp.SourcePort;
        }

        public override void VisitTcpPacket(TcpPacket tcp)
        {
            m_packetMetadata.Value.Transport.Bytes.Offset = tcp.BytesHighPerformance.Offset;
            m_packetMetadata.Value.Transport.Bytes.Length = tcp.BytesHighPerformance.Length;
            m_packetMetadata.Key.Protocol = Ndx.Model.IpProtocolType.Tcp;
            m_packetMetadata.Key.DestinationPort = tcp.DestinationPort;
            m_packetMetadata.Key.SourcePort = tcp.SourcePort;
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
            m_packetMetadata.Value.Datalink.Bytes.Offset = eth.BytesHighPerformance.Offset;
            m_packetMetadata.Value.Datalink.Bytes.Length = eth.BytesHighPerformance.Length;
            m_packetMetadata.Key.Protocol = Ndx.Model.IpProtocolType.None;
            m_packetMetadata.Key.AddressFamily = Ndx.Model.AddressFamily.Unspecified;
            m_packetMetadata.Key.SourceIpAddress = System.Net.IPAddress.None;
            m_packetMetadata.Key.DestinationIpAddress = System.Net.IPAddress.None;
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
            m_packetMetadata.Value.Network.Bytes.Offset = ipv4.BytesHighPerformance.Offset;
            m_packetMetadata.Value.Network.Bytes.Length = ipv4.BytesHighPerformance.Length;
            m_packetMetadata.Key.AddressFamily = Ndx.Model.AddressFamily.InterNetwork;
            m_packetMetadata.Key.Protocol = (Ndx.Model.IpProtocolType)ipv4.Protocol;
            m_packetMetadata.Key.SourceIpAddress = ipv4.SourceAddress;
            m_packetMetadata.Key.DestinationIpAddress = ipv4.DestinationAddress;
        }

        public override void VisitIPv6Packet(IPv6Packet ipv6)
        {
            m_packetMetadata.Value.Network.Bytes.Offset = ipv6.BytesHighPerformance.Offset;
            m_packetMetadata.Value.Network.Bytes.Length = ipv6.BytesHighPerformance.Length;
            m_packetMetadata.Key.AddressFamily = Ndx.Model.AddressFamily.InterNetworkV6;
            m_packetMetadata.Key.Protocol = (Ndx.Model.IpProtocolType)ipv6.Protocol;
            m_packetMetadata.Key.SourceIpAddress = ipv6.SourceAddress;
            m_packetMetadata.Key.DestinationIpAddress = ipv6.DestinationAddress;
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
            m_packetMetadata.Value.Application.Bytes.Offset = packet.BytesHighPerformance.Offset;
            m_packetMetadata.Value.Application.Bytes.Length = packet.BytesHighPerformance.Length;
        }
    }
}
