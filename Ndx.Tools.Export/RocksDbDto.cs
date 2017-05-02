using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Ndx.Utils;
using Newtonsoft.Json;

namespace Ndx.Tools.Export
{

    public abstract class RocksBinarySerializer<T>
    {
        public abstract byte[] GetBytes(T obj);
        public abstract T FromBytes(byte[] bytes, int startIndex);
    }


    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class RocksPcapId
    {
        internal static readonly int __size = sizeof(ushort);
        ushort uid;
        public ushort Uid { get => uid; set => uid = value; }


        public class BinarySerializer : RocksBinarySerializer<RocksPcapId>
        {
            public override RocksPcapId FromBytes(byte[] bytes, int startIndex)
            {
                return new RocksPcapId()
                {
                    uid = BitConverter.ToUInt16(bytes, startIndex)
                };
            }

            public override byte[] GetBytes(RocksPcapId obj)
            {
                return BitConverter.GetBytes(obj.uid);
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class RocksPcapFile
    {
        ushort pcapType;
        Uri uri;
        byte[] md5signature = new byte[16];
        byte[] shasignature = new byte[20];
        DateTimeOffset ingestedOn;

        public ushort PcapType { get => pcapType; set => pcapType = value; }
        public Uri Uri { get => uri; set => uri = value; }
        public byte[] Md5signature { get => md5signature; set => md5signature = value; }
        public byte[] Shasignature { get => shasignature; set => shasignature = value; }
        public DateTimeOffset IngestedOn { get => ingestedOn; set => ingestedOn = value; }

        public class BinarySerializer : RocksBinarySerializer<RocksPcapFile>
        {
            public override RocksPcapFile FromBytes(byte[] bytes, int startIndex)
            {
                var uriLen = BitConverter.ToUInt16(bytes, startIndex + 2);
                var uriBytes = new byte[uriLen];
                Array.Copy(bytes, startIndex + 4, uriBytes, 0, uriLen);
                var md5Bytes = new byte[16];
                Array.Copy(bytes, startIndex + 4 + uriLen, md5Bytes, 0, 16);
                var shaBytes = new byte[20];
                Array.Copy(bytes, startIndex + 4 + uriLen+16, shaBytes, 0, 20);
                var ingestedOnBytes = BitConverter.ToUInt64(bytes, startIndex + 2 + 2 + uriLen + 16 + 20);
                return new RocksPcapFile()
                {
                    pcapType = BitConverter.ToUInt16(bytes, startIndex),
                    uri = new Uri(Encoding.ASCII.GetString(uriBytes)),
                    ingestedOn = DateTimeOffsetExt.FromUnixTimeMilliseconds((long)ingestedOnBytes)
                };
            }

            public override byte[] GetBytes(RocksPcapFile obj)
            {
                var pcapTypeBytes = BitConverter.GetBytes(obj.pcapType);
                var uriBytes = Encoding.ASCII.GetBytes(obj.uri.ToString());
                var uriLength = BitConverter.GetBytes((ushort)uriBytes.Length);
                var ingestOnBytes = BitConverter.GetBytes(obj.ingestedOn.ToUnixTimeMilliseconds());
                return pcapTypeBytes.Concat(uriLength).Concat(uriBytes).Concat(obj.md5signature).Concat(obj.shasignature).Concat(ingestOnBytes).ToArray();
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class RocksFlowId
    {
        uint uid;

        public uint Uid { get => uid; set => uid = value; }

        public class BinarySerializer : RocksBinarySerializer<RocksFlowId>
        {
            public override RocksFlowId FromBytes(byte[] bytes, int startIndex)
            {
                return new RocksFlowId()
                {
                    uid = BitConverter.ToUInt32(bytes, startIndex)
                };
            }

            public override byte[] GetBytes(RocksFlowId obj)
            {
                return BitConverter.GetBytes(obj.uid);
            }
        }

    }

    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class RocksFlowKey
    {
        ushort protocol;
        IPAddress sourceAddress;
        IPAddress destinationAddress;
        ushort sourcePort;
        ushort destinationPort;

        public ushort Protocol { get => protocol; set => protocol = value; }
        public IPAddress SourceAddress { get => sourceAddress; set => sourceAddress = value; }
        public IPAddress DestinationAddress { get => destinationAddress; set => destinationAddress = value; }
        public ushort SourcePort { get => sourcePort; set => sourcePort = value; }
        public ushort DestinationPort { get => destinationPort; set => destinationPort = value; }

        public class BinarySerializer : RocksBinarySerializer<RocksFlowKey>
        {
            public override RocksFlowKey FromBytes(byte[] bytes, int startIndex)
            {
                var protocol = BitConverter.ToUInt16(bytes, startIndex);
                var srcAddressBytes = new byte[16];
                Array.Copy(bytes, startIndex + 2, srcAddressBytes, 0, 16);
                var dstAddressBytes = new byte[16];
                Array.Copy(bytes, startIndex + 2+16, dstAddressBytes, 0, 16);
                
                if (protocol >> 8 == (int)AddressFamily.InterNetwork)
                {
                    Array.Resize(ref srcAddressBytes, 4);
                    Array.Resize(ref dstAddressBytes, 4);
                }

                return new RocksFlowKey()
                {
                    protocol = protocol,
                    sourceAddress = new IPAddress(srcAddressBytes),
                    destinationAddress = new IPAddress(dstAddressBytes),
                    sourcePort = BitConverter.ToUInt16(bytes, startIndex + 2 + 16 + 16),
                    destinationPort = BitConverter.ToUInt16(bytes, startIndex + 2 + 16 + 16 + 2),
                };
            }

            public override byte[] GetBytes(RocksFlowKey obj)
            {
                var protocolBytes = BitConverter.GetBytes(obj.protocol);
                var sourceAddressBytes = obj.sourceAddress.GetAddressBytes();
                var destinAddressBytes = obj.destinationAddress.GetAddressBytes();
                Array.Resize(ref sourceAddressBytes, 16);
                Array.Resize(ref destinAddressBytes, 16);
                var sourcePortBytes = BitConverter.GetBytes(obj.sourcePort);                
                var destinationPortBytes = BitConverter.GetBytes(obj.destinationPort);
                
                return protocolBytes.Concat(sourceAddressBytes).Concat(destinAddressBytes).Concat(sourcePortBytes).Concat(destinationPortBytes).ToArray();
            }
   
        }
    }

    [JsonObject(MemberSerialization=MemberSerialization.Fields)]
    public class RocksFlowRecord
    {
        ulong octets;
        uint packets;
        ulong first;
        ulong last;
        uint blocks;
        uint application;

        public ulong Octets { get => octets; set => octets = value; }
        public uint Packets { get => packets; set => packets = value; }
        public ulong First { get => first; set => first = value; }
        public ulong Last { get => last; set => last = value; }
        public uint Blocks { get => blocks; set => blocks = value; }
        public uint Application { get => application; set => application = value; }

        public class BinarySerializer : RocksBinarySerializer<RocksFlowRecord>
        {
            public override RocksFlowRecord FromBytes(byte[] bytes, int startIndex)
            {
                return new RocksFlowRecord()
                {
                    octets = BitConverter.ToUInt64(bytes, startIndex),
                    packets = BitConverter.ToUInt32(bytes, startIndex + sizeof(ulong)),
                    first = BitConverter.ToUInt64(bytes, startIndex + sizeof(ulong) + sizeof(uint)),
                    last = BitConverter.ToUInt64(bytes, startIndex + sizeof(ulong) + sizeof(uint) + sizeof(ulong)),
                    blocks = BitConverter.ToUInt32(bytes, startIndex + sizeof(ulong) + sizeof(uint) + sizeof(ulong) + sizeof(ulong)),
                    application = BitConverter.ToUInt32(bytes, startIndex + sizeof(ulong) + sizeof(uint) + sizeof(ulong) + sizeof(ulong) + sizeof(uint))
                };
            }

            public override byte[] GetBytes(RocksFlowRecord obj)
            {
                var octestsBytes = BitConverter.GetBytes(obj.Octets);
                var packetsBytes = BitConverter.GetBytes(obj.Packets);
                var firstBytes = BitConverter.GetBytes(obj.First);
                var lastBytes = BitConverter.GetBytes(obj.Last);
                var blockstBytes = BitConverter.GetBytes(obj.Blocks);
                var applicationBytes = BitConverter.GetBytes(obj.Application);
                return octestsBytes.Concat(packetsBytes).Concat(firstBytes).Concat(lastBytes).Concat(blockstBytes).Concat(applicationBytes).ToArray();
            }
        }

    }

    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class RocksPacketBlockId
    {
        uint flowId;
        uint blockId;

        public uint FlowId { get => flowId; set => flowId = value; }
        public uint BlockId { get => blockId; set => blockId = value; }

        public class BinarySerializer : RocksBinarySerializer<RocksPacketBlockId>
        {
            public override RocksPacketBlockId FromBytes(byte[] bytes, int startIndex)
            {
                return new RocksPacketBlockId()
                {
                    flowId = BitConverter.ToUInt32(bytes, startIndex),
                    blockId = BitConverter.ToUInt32(bytes, startIndex + 4),
                };
            }

            public override byte[] GetBytes(RocksPacketBlockId obj)
            {
                var flowIdBytes = BitConverter.GetBytes(obj.FlowId);
                var blockIdBytes = BitConverter.GetBytes(obj.BlockId);
                return flowIdBytes.Concat(blockIdBytes).ToArray();
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class RocksFrameData
    {
        internal const int __size = sizeof(uint) + sizeof(uint) + sizeof(ulong) + sizeof(ulong);
        uint frameNumber;
        uint frameLength;
        ulong frameOffset;
        ulong timestamp;

        public uint FrameNumber { get => frameNumber; set => frameNumber = value; }
        public uint FrameLength { get => frameLength; set => frameLength = value; }
        public ulong FrameOffset { get => frameOffset; set => frameOffset = value; }
        public ulong Timestamp { get => timestamp; set => timestamp = value; }

        public class BinarySerializer : RocksBinarySerializer<RocksFrameData>
        {
            public override RocksFrameData FromBytes(byte[] bytes, int startIndex)
            {
                return new RocksFrameData()
                {
                    frameNumber = BitConverter.ToUInt32(bytes, startIndex),
                    frameLength = BitConverter.ToUInt32(bytes, startIndex + 4),
                    frameOffset = BitConverter.ToUInt32(bytes, startIndex + 4 + 4),
                    timestamp = BitConverter.ToUInt64(bytes, startIndex + 4 + 4 + 8),
                };
            }

            public override byte[] GetBytes(RocksFrameData obj)
            {
                var frameNumberBytes = BitConverter.GetBytes(obj.FrameNumber);
                var frameLengthBytes = BitConverter.GetBytes(obj.FrameLength);
                var frameOffsetBytes = BitConverter.GetBytes(obj.FrameOffset);
                var timestampBytes = BitConverter.GetBytes(obj.Timestamp);
                return frameNumberBytes.Concat(frameLengthBytes).Concat(frameOffsetBytes).Concat(timestampBytes).ToArray();
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class RocksByteRange
    {
        internal static readonly int __size = sizeof(int) + sizeof(int);
        int start;
        int count;

        public int Start { get => start; set => start = value; }
        public int Count { get => count; set => count = value; }

        public class BinarySerializer : RocksBinarySerializer<RocksByteRange>
        {
            public override RocksByteRange FromBytes(byte[] bytes, int startIndex)
            {
                return new RocksByteRange()
                {
                    start = BitConverter.ToInt32(bytes, startIndex),
                    count = BitConverter.ToInt32(bytes, startIndex + 4),
                };
            }

            public override byte[] GetBytes(RocksByteRange obj)
            {
                var startBytes = BitConverter.GetBytes(obj.Start);
                var countBytes = BitConverter.GetBytes(obj.Count);
                return startBytes.Concat(countBytes).ToArray();
            }
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class RocksPacketMetadata
    {
        internal static int __size = RocksFrameData.__size + 4 * RocksByteRange.__size;
        RocksFrameData frameMetadata;
        RocksByteRange link;
        RocksByteRange network;
        RocksByteRange transport;
        RocksByteRange payload;

        public RocksFrameData FrameMetadata { get => frameMetadata; set => frameMetadata = value; }
        public RocksByteRange Link { get => link; set => link = value; }
        public RocksByteRange Network { get => network; set => network = value; }
        public RocksByteRange Transport { get => transport; set => transport = value; }
        public RocksByteRange Payload { get => payload; set => payload = value; }

        public class BinarySerializer : RocksBinarySerializer<RocksPacketMetadata>
        {
            public override RocksPacketMetadata FromBytes(byte[] bytes, int startIndex)
            {
                return new RocksPacketMetadata()
                {
                    frameMetadata = RocksSerializer.ToFrameData(bytes, startIndex),
                    link = RocksSerializer.ToByteRange(bytes, startIndex + RocksFrameData.__size),
                    network = RocksSerializer.ToByteRange(bytes, startIndex + RocksFrameData.__size + RocksByteRange.__size),
                    transport = RocksSerializer.ToByteRange(bytes, startIndex + RocksFrameData.__size + RocksByteRange.__size + RocksByteRange.__size),
                    payload = RocksSerializer.ToByteRange(bytes, startIndex + RocksFrameData.__size + RocksByteRange.__size + RocksByteRange.__size + RocksByteRange.__size)
                };
            }        

            public override byte[] GetBytes(RocksPacketMetadata obj)
            {
                var frameMetadataBytes = RocksSerializer.GetBytes(obj.FrameMetadata);
                var linkBytes = RocksSerializer.GetBytes(obj.Link);
                var networkBytes = RocksSerializer.GetBytes(obj.Network);
                var transportBytes = RocksSerializer.GetBytes(obj.Transport);
                var payloadBytes = RocksSerializer.GetBytes(obj.Payload);
                return frameMetadataBytes.Concat(linkBytes).Concat(networkBytes).Concat(transportBytes).Concat(payloadBytes).ToArray();
            }
        }
    }

    public class RocksSerializer
    {
        static RocksByteRange.BinarySerializer byteRangeSerializer = new RocksByteRange.BinarySerializer();
        static RocksFrameData.BinarySerializer frameDataSerializer = new RocksFrameData.BinarySerializer();
        static RocksPcapId.BinarySerializer pcapIdSerializer = new RocksPcapId.BinarySerializer();
        static RocksPacketMetadata.BinarySerializer packetMetadataSerializer = new RocksPacketMetadata.BinarySerializer();
        static RocksFlowId.BinarySerializer flowIdSerializer = new RocksFlowId.BinarySerializer();
        static RocksFlowKey.BinarySerializer flowKeySerializer = new RocksFlowKey.BinarySerializer();
        static RocksFlowRecord.BinarySerializer flowRecordSerializer = new RocksFlowRecord.BinarySerializer();
        static RocksPacketBlock.BinarySerializer packetBlockSerializer = new RocksPacketBlock.BinarySerializer();
        static RocksPacketBlockId.BinarySerializer packetBlockIdSerializer = new RocksPacketBlockId.BinarySerializer();

        public static byte[] GetBytes(RocksFlowKey flowKey)
        {
            return flowKeySerializer.GetBytes(flowKey);
        }

        public static byte[] GetBytes(RocksFrameData frameMetadata)
        {
            return frameDataSerializer.GetBytes(frameMetadata);
        }

        public static byte[] GetBytes(RocksByteRange link)
        {
            return byteRangeSerializer.GetBytes(link);
        }

        public static byte[] GetBytes(RocksPcapId pcapRef)
        {
            return pcapIdSerializer.GetBytes(pcapRef);
        }

        public static byte[] GetBytes(RocksPacketMetadata item)
        {
            return packetMetadataSerializer.GetBytes(item);
        }

        public static byte[] GetBytes(RocksFlowId flowId)
        {
            return flowIdSerializer.GetBytes(flowId);
        }

        public static byte[] GetBytes(RocksFlowRecord flowKey)
        {
            return flowRecordSerializer.GetBytes(flowKey);
        }

        public static byte[] GetBytes(RocksPacketBlockId pbId)
        {
            return packetBlockIdSerializer.GetBytes(pbId);
        }

        public static byte[] GetBytes(RocksPacketBlock packetBlock)
        {
            return packetBlockSerializer.GetBytes(packetBlock);
        }

        public static RocksByteRange ToByteRange(byte[] bytes, int startIndex)
        {
            return byteRangeSerializer.FromBytes(bytes, startIndex);
        }

        public static RocksFrameData ToFrameData(byte[] bytes, int startIndex)
        {
            return frameDataSerializer.FromBytes(bytes, startIndex);
        }

        public static RocksPcapId ToPcapId(byte[] bytes, int startIndex)
        {
            return pcapIdSerializer.FromBytes(bytes, startIndex);
        }

        public static RocksPacketMetadata ToPacketMetadata(byte[] bytes, int startIndex)
        {
            return packetMetadataSerializer.FromBytes(bytes, startIndex);
        }

        public static RocksFlowRecord ToFlowRecord(byte[] bytes, int startIndex)
        {
            return flowRecordSerializer.FromBytes(bytes, startIndex);
        }

        public static RocksFlowKey ToFlowKey(byte[] bytes, int startIndex)
        {
            return flowKeySerializer.FromBytes(bytes, startIndex);
        }

        public static RocksFlowId ToFlowId(byte[] bytes, int startIndex)
        {
            return flowIdSerializer.FromBytes(bytes, startIndex);
        }

        public static RocksPacketBlockId ToPacketBlockId(byte[] bytes, int startIndex)
        {
            return packetBlockIdSerializer.FromBytes(bytes, startIndex);
        }

        public static RocksPacketBlock ToPacketBlock(byte[] bytes, int startIndex)
        {
            return packetBlockSerializer.FromBytes(bytes, startIndex);
        }
    }

    [JsonObject(MemberSerialization = MemberSerialization.Fields)]
    public class RocksPacketBlock
    {
        RocksPcapId pcapRef;
        int count;
        RocksPacketMetadata[] items;

        public RocksPcapId PcapRef { get => pcapRef; set => pcapRef = value; }
        public RocksPacketMetadata[] Items { get => items; set { items = value; count = value.Length; } }

        public class BinarySerializer : RocksBinarySerializer<RocksPacketBlock>
        {
            public override RocksPacketBlock FromBytes(byte[] bytes, int startIndex)
            {
                var pcapref = RocksSerializer.ToPcapId(bytes, startIndex);
                var count = BitConverter.ToInt32(bytes, startIndex + RocksPcapId.__size);
                var items = new RocksPacketMetadata[count];
                for(int i=0; i < count; i++)
                {
                    items[i] = RocksSerializer.ToPacketMetadata(bytes, startIndex + RocksPcapId.__size + 4 + (i * RocksPacketMetadata.__size));
                }
                return new RocksPacketBlock()
                {
                    PcapRef = pcapref,
                    Items = items
                };
            }

            public override byte[] GetBytes(RocksPacketBlock obj)
            {
                var pcapRefBytes = RocksSerializer.GetBytes(obj.PcapRef);
                var countBytes = BitConverter.GetBytes(obj.count);
                var bytes = pcapRefBytes.Concat(countBytes);
                foreach(var item in obj.Items)
                {
                    var itemBytes = RocksSerializer.GetBytes(item);
                    bytes = bytes.Concat(itemBytes);
                }
                return bytes.ToArray();
            }
        }
    }
}
