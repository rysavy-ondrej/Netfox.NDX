using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using PacketDotNet;

namespace Ndx.Model
{
    public partial class Frame
    {
        //  January 1, 1970
        static public readonly long UnixBaseTicks = new DateTime(1970, 1, 1).Ticks;
        static public readonly PhysicalAddress PhysicalAddressEmpty = new PhysicalAddress(new byte[6]);
        public const long TicksPerSecond = 10000000;
        public const long TicksPerMicrosecond = 10;

        public uint Seconds => (uint)((TimeStamp - UnixBaseTicks) / TicksPerSecond);

        public uint Microseconds => (uint)(((TimeStamp - UnixBaseTicks) % TicksPerSecond)/ TicksPerMicrosecond);

        public DateTime DateTime => new DateTime(TimeStamp);

        public byte[] Bytes { get => Data.ToByteArray(); set => data_ = ByteString.CopyFrom(value); }


        public Packet Parse()
        {
            return Packet.ParsePacket((LinkLayers)LinkType, Bytes);
        }

        static EthernetPacket ConvertToEthernetPacket(Packet packet, PhysicalAddress src =null, PhysicalAddress dst = null)
        {
            src = src ?? PhysicalAddressEmpty;
            dst = dst ?? PhysicalAddressEmpty;
            var ipv4 = packet.Extract(typeof(IPv4Packet));
            if (ipv4 != null)
            { return new EthernetPacket(src, dst, PacketDotNet.EthernetPacketType.IpV4) { PayloadPacket = ipv4 }; }
            var ipv6 = packet.Extract(typeof(IPv4Packet));
            if (ipv6 != null)
            { return new EthernetPacket(src, dst, PacketDotNet.EthernetPacketType.IpV6) { PayloadPacket = ipv6 }; }
            return new EthernetPacket(src, dst, PacketDotNet.EthernetPacketType.None);
        }

        public static Frame EthernetRaw(Packet p, int frameNumber, int frameOffset, long timestamp, PhysicalAddress src = null, PhysicalAddress dst = null)
        {
            var eth = p.Extract(typeof(EthernetPacket)) ?? ConvertToEthernetPacket(p, src, dst);
            var bytes = eth.Bytes;
            return new Frame { FrameLength = bytes.Length, FrameNumber = frameNumber, FrameOffset = frameOffset, LinkType = DataLinkType.Ethernet, TimeStamp = timestamp, Bytes = bytes };
        }


        public bool HasBytes => !data_.IsEmpty;

        /// <summary>
        /// Loads the frame bytes from the given stream. It requires an input stream and this 
        /// stream must be seekable and be for an exclusive use of this method. 
        /// </summary>
        /// <param name="frame">The meta frame that contains information about the packet.</param>
        /// <param name="stream">A stream to read data from.</param>
        /// <returns>True or false depending on the reslt of this operation.</returns>
        public bool LoadFrameBytes(Stream stream)
        {
            try
            {
                stream.Position = this.FrameOffset;
                var buffer = new byte[this.FrameLength];
                var result = stream.Read(buffer, 0, this.FrameLength);
                if (result == this.FrameLength)
                {
                    this.Bytes = buffer;
                    return true;
                }
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine($"[ERROR] Capture.GetFrameBytes: {e}");
            }
            return false;
        }
    }
}
