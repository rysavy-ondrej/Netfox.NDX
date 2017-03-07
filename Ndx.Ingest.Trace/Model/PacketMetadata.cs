//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using Newtonsoft.Json;
using Ndx.Network;
using PacketDotNet;
using PacketDotNet.Ieee80211;
using System.Runtime.InteropServices;


namespace Ndx.Ingest.Trace
{
    using AddressFamily = PacketDotNet.LLDP.AddressFamily;

    [StructLayout(LayoutKind.Explicit, Size = __size)]
    public struct _PacketMetadata
    {
        internal const int __size = _FrameMetadata.__size + _PacketPointers.__size;
        [FieldOffset(0)] public _FrameMetadata frame;
        [FieldOffset(_FrameMetadata.__size)] public _PacketPointers pointers;
    }

    /// <summary>
    /// Represents a collection of extracted fields of the parsed packet.
    /// </summary>
    public class PacketMetadata
    {        
        FlowKey m_flowkey;
        _PacketMetadata m_metadata;

        internal _PacketMetadata Data => m_metadata;
        /// <summary>
        /// Gets a flow key for the current <see cref="PacketMetadata"/>.
        /// </summary>
        public FlowKey Flow => m_flowkey;

        public FrameMetadata Frame => new FrameMetadata(m_metadata.frame);


        public _ByteRange Network => m_metadata.pointers.network;
        public _ByteRange Transport => m_metadata.pointers.transport;
        public _ByteRange Payload => m_metadata.pointers.payload;

        PacketMetadata()
        {
            m_flowkey = new FlowKey();
            m_metadata = new _PacketMetadata();
        }

        public PacketMetadata(_FlowKey flowKey, _PacketMetadata metadata)
        {
            this.m_flowkey = new FlowKey(flowKey);
            this.m_metadata = metadata;
        }

        public PacketMetadata(FlowKey flowKey, _PacketMetadata metadata)
        {
            this.m_flowkey = flowKey;
            this.m_metadata = metadata;
        }
    }

    internal class PacketVisitorImpl: PacketVisitor
    {
        _FlowKey m_flowKey;
        _PacketMetadata m_packetMetadata;
        public PacketVisitorImpl(_FrameMetadata frameMetadata)
        {
            m_flowKey = new _FlowKey();
            m_packetMetadata = new _PacketMetadata() { frame = frameMetadata };
        }

        public _FlowKey FlowKey => m_flowKey;
        public _PacketMetadata Metadata  => m_packetMetadata;

        public override void VisitWakeOnLanPacket(WakeOnLanPacket packet)
        {
        }

        public override void VisitUdpPacket(UdpPacket udp)
        {
            m_packetMetadata.pointers.transport.start = udp.BytesHighPerformance.Offset;
            m_packetMetadata.pointers.transport.count = udp.BytesHighPerformance.Length;
            m_flowKey.protocol = (byte)(IPProtocolType.UDP);
            m_flowKey.destinationPort = udp.DestinationPort;
            m_flowKey.sourcePort = udp.SourcePort;
        }

        public override void VisitTcpPacket(TcpPacket tcp)
        {
            m_packetMetadata.pointers.transport.start = tcp.BytesHighPerformance.Offset;
            m_packetMetadata.pointers.transport.count = tcp.BytesHighPerformance.Length;
            m_flowKey.protocol = (byte)(IPProtocolType.TCP);
            m_flowKey.destinationPort = tcp.DestinationPort;
            m_flowKey.sourcePort = tcp.SourcePort;
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
            m_packetMetadata.pointers.link.start = eth.BytesHighPerformance.Offset;
            m_packetMetadata.pointers.link.count = eth.BytesHighPerformance.Length;
            m_flowKey.protocol = (ushort)IPProtocolType.NONE;
            m_flowKey.family = (ushort)AddressFamily.Eth802;
            m_flowKey.SetSourceAddress(eth.SourceHwAddress.GetAddressBytes(), 0, 6);
            m_flowKey.SetDestinationAddress(eth.DestinationHwAddress.GetAddressBytes(), 0, 6);
        }

        public override void VisitLLDPPacket(LLDPPacket packet)
        {
            m_flowKey.protocol = (ushort)(EthernetPacketType.LLDP);
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
            m_packetMetadata.pointers.network.start = ipv4.BytesHighPerformance.Offset;
            m_packetMetadata.pointers.network.count = ipv4.BytesHighPerformance.Length;
            m_flowKey.family = (ushort)AddressFamily.IPv4;
            m_flowKey.protocol = (byte)(ipv4.Protocol);
            m_flowKey.SetSourceAddress(ipv4.SourceAddress.GetAddressBytes(), 0,4);
            m_flowKey.SetDestinationAddress(ipv4.DestinationAddress.GetAddressBytes(),0,4);
        }

        public override void VisitIPv6Packet(IPv6Packet ipv6)
        {
            m_packetMetadata.pointers.network.start = ipv6.BytesHighPerformance.Offset;
            m_packetMetadata.pointers.network.count = ipv6.BytesHighPerformance.Length;
            m_flowKey.family = (ushort)AddressFamily.IPv4;
            m_flowKey.protocol = (byte)(ipv6.Protocol);
            m_flowKey.SetSourceAddress(ipv6.SourceAddress.GetAddressBytes(), 0, 16);
            m_flowKey.SetDestinationAddress(ipv6.DestinationAddress.GetAddressBytes(), 0, 16);
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
            m_packetMetadata.pointers.payload.start = packet.BytesHighPerformance.Offset;
            m_packetMetadata.pointers.payload.count = packet.BytesHighPerformance.Length;
        }
    }
}
