using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using PacketDotNet;

namespace Ndx.Model
{
    public partial class RawFrame
    {
        //  January 1, 1970
        static readonly long UnixBaseTicks = new DateTime(1970, 1, 1).Ticks;
        const long TicksPerSecond = 10000000;
        const long TicksPerMicrosecond = 10;

        public RawFrame(MetaFrame metaframe, byte[] bytes)
        {
            FrameLength = metaframe.FrameLength;
            FrameNumber = metaframe.FrameNumber;
            FrameOffset = metaframe.FrameOffset;
            LinkType = metaframe.LinkType;
            TimeStamp = metaframe.TimeStamp;
            Data = Google.Protobuf.ByteString.CopyFrom(bytes);
        }

        public uint Seconds => (uint)((TimeStamp - UnixBaseTicks) / TicksPerSecond);

        public uint Microseconds => (uint)(((TimeStamp - UnixBaseTicks) % TicksPerSecond)/ TicksPerMicrosecond);

        public DateTime DateTime => new DateTime(TimeStamp);

        public byte[] Bytes => Data.ToByteArray();


        public Packet Parse()
        {
            return Packet.ParsePacket((LinkLayers)LinkType, Bytes);
        }


        static EthernetPacket ConvertToEthernetPacket(Packet packet, PhysicalAddress src =null, PhysicalAddress dst = null)
        {
            src = src ?? PhysicalAddress.None;
            dst = dst ?? PhysicalAddress.None;
            var ipv4 = packet.Extract(typeof(IPv4Packet));
            if (ipv4 != null)
            { return new EthernetPacket(src, dst, PacketDotNet.EthernetPacketType.IpV4) { PayloadPacket = ipv4 }; }
            var ipv6 = packet.Extract(typeof(IPv4Packet));
            if (ipv6 != null)
            { return new EthernetPacket(src, dst, PacketDotNet.EthernetPacketType.IpV6) { PayloadPacket = ipv6 }; }
            return new EthernetPacket(src, dst, PacketDotNet.EthernetPacketType.None);
        }

        public static RawFrame EthernetRaw(Packet p, int frameNumber, int frameOffset, long timestamp, PhysicalAddress src = null, PhysicalAddress dst = null)
        {
            var eth = p.Extract(typeof(EthernetPacket)) ?? ConvertToEthernetPacket(p, src, dst);
            var bytes = eth.Bytes;
            return new RawFrame(new MetaFrame() { FrameLength = bytes.Length, FrameNumber = frameNumber, FrameOffset = frameOffset, LinkType = DataLinkType.Ethernet, TimeStamp = timestamp }, bytes);
        }
    }
}
