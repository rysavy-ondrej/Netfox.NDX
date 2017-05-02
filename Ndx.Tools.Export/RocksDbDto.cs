using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ndx.Utils;
namespace Ndx.Tools.Export
{

    public abstract class RocksBinarySerializer<T>
    {
        public abstract byte[] GetBytes(T obj);
        public abstract T FromBytes(byte[] bytes, int startIndex);
    }


    public class RocksPcapId
    {
        ushort m_uid;
        public ushort Uid { get => m_uid; set => m_uid = value; }


        public class BinarySerializer : RocksBinarySerializer<RocksPcapId>
        {
            public override RocksPcapId FromBytes(byte[] bytes, int startIndex)
            {
                return new RocksPcapId()
                {
                    m_uid = BitConverter.ToUInt16(bytes, startIndex)
                };
            }

            public override byte[] GetBytes(RocksPcapId obj)
            {
                return BitConverter.GetBytes(obj.m_uid);
            }
        }
    }

    public class RocksPcapFile
    {
        ushort m_pcapType;
        Uri m_uri;
        byte[] m_md5signature;
        byte[] m_shasignature;
        DateTimeOffset m_ingestedOn;

        public ushort PcapType { get => m_pcapType; set => m_pcapType = value; }
        public Uri Uri { get => m_uri; set => m_uri = value; }
        public byte[] Md5signature { get => m_md5signature; set => m_md5signature = value; }
        public byte[] Shasignature { get => m_shasignature; set => m_shasignature = value; }
        public DateTimeOffset IngestedOn { get => m_ingestedOn; set => m_ingestedOn = value; }

        public class BinarySerializer : RocksBinarySerializer<RocksPcapFile>
        {
            public override RocksPcapFile FromBytes(byte[] bytes, int startIndex)
            {
                throw new NotImplementedException();
            }

            public override byte[] GetBytes(RocksPcapFile obj)
            {
                var pcapTypeBytes = BitConverter.GetBytes(obj.m_pcapType);
                var uriBytes = Encoding.ASCII.GetBytes(obj.m_uri.ToString());
                var uriLength = BitConverter.GetBytes(uriBytes.Length);
                var ingestOnBytes = BitConverter.GetBytes(obj.m_ingestedOn.ToUnixTimeMilliseconds());
                return pcapTypeBytes.Concat(uriLength).Concat(uriBytes).Concat(obj.m_md5signature).Concat(obj.m_shasignature).Concat(ingestOnBytes).ToArray();
            }
        }
    }

    public class RocksFlowId
    {
        uint m_uid;

        public uint Uid { get => m_uid; set => m_uid = value; }

        public class BinarySerializer : RocksBinarySerializer<RocksFlowId>
        {
            public override RocksFlowId FromBytes(byte[] bytes, int startIndex)
            {
                return new RocksFlowId()
                {
                    m_uid = BitConverter.ToUInt32(bytes, startIndex)
                };
            }

            public override byte[] GetBytes(RocksFlowId obj)
            {
                return BitConverter.GetBytes(obj.m_uid);
            }
        }

    }

    public class RocksFlowKey
    {
        ushort m_protocol;
        byte[] sourceAddress;
        byte[] destinationAddress;
        ushort sourcePort;
        ushort destinationPort;

        public ushort Protocol { get => m_protocol; set => m_protocol = value; }
        public IPAddress SourceAddress { get => new IPAddress(sourceAddress); set => sourceAddress = value.GetAddressBytes(); }
        public IPAddress DestinationAddress { get => new IPAddress(destinationAddress); set => destinationAddress = value.GetAddressBytes(); }
        public ushort SourcePort { get => sourcePort; set => sourcePort = value; }
        public ushort DestinationPort { get => destinationPort; set => destinationPort = value; }

        public class BinarySerializer : RocksBinarySerializer<RocksFlowKey>
        {
            public override RocksFlowKey FromBytes(byte[] bytes, int startIndex)
            {
                throw new NotImplementedException();
            }

            public override byte[] GetBytes(RocksFlowKey obj)
            {
                var protocolBytes = BitConverter.GetBytes(obj.m_protocol);
                var sourcePortBytes = BitConverter.GetBytes(obj.sourcePort);
                var destinationPortBytes = BitConverter.GetBytes(obj.destinationPort);
                return protocolBytes.Concat(obj.sourceAddress).Concat(GetIpAddressBytes(obj.destinationAddress)).Concat(sourcePortBytes).Concat(destinationPortBytes).ToArray();

            }

            private IEnumerable<byte> GetIpAddressBytes(byte[] address)
            {
                Array.Resize(ref address, 16);
                return address;
            }
        }
    }

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
                throw new NotImplementedException();
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
                throw new NotImplementedException();
            }

            public override byte[] GetBytes(RocksPacketBlockId obj)
            {
                var flowIdBytes = BitConverter.GetBytes(obj.FlowId);
                var blockIdBytes = BitConverter.GetBytes(obj.BlockId);
                return flowIdBytes.Concat(blockIdBytes).ToArray();
            }
        }
    }

    public class RocksFrameData
    {
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
                throw new NotImplementedException();
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

    public class RocksByteRange
    {
        int start;
        int count;

        public int Start { get => start; set => start = value; }
        public int Count { get => count; set => count = value; }

        public class BinarySerializer : RocksBinarySerializer<RocksByteRange>
        {
            public override RocksByteRange FromBytes(byte[] bytes, int startIndex)
            {
                throw new NotImplementedException();
            }

            public override byte[] GetBytes(RocksByteRange obj)
            {
                var startBytes = BitConverter.GetBytes(obj.Start);
                var countBytes = BitConverter.GetBytes(obj.Count);
                return startBytes.Concat(countBytes).ToArray();
            }
        }
    }

    public class RocksPacketMetadata
    {
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
                throw new NotImplementedException();
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
        static RocksFrameData.BinarySerializer framedataSerializer = new RocksFrameData.BinarySerializer();
        static RocksPcapId.BinarySerializer pcapidSerializer = new RocksPcapId.BinarySerializer();
        static RocksPacketMetadata.BinarySerializer packetSerializer = new RocksPacketMetadata.BinarySerializer();
        static RocksFlowId.BinarySerializer flowIdSerializer = new RocksFlowId.BinarySerializer();
        static RocksFlowKey.BinarySerializer flowKeySerializer = new RocksFlowKey.BinarySerializer();
        static RocksFlowRecord.BinarySerializer flowRecordSerializer = new RocksFlowRecord.BinarySerializer();
        static RocksPacketBlock.BinarySerializer packetBlockSerializer = new RocksPacketBlock.BinarySerializer();
        static RocksPacketBlockId.BinarySerializer blockIdSerializer = new RocksPacketBlockId.BinarySerializer();
        public static byte[] GetBytes(RocksFrameData frameMetadata)
        {
            return framedataSerializer.GetBytes(frameMetadata);
        }

        internal static byte[] GetBytes(RocksByteRange link)
        {
            return byteRangeSerializer.GetBytes(link);
        }

        internal static byte[] GetBytes(RocksPcapId pcapRef)
        {
            return pcapidSerializer.GetBytes(pcapRef);
        }

        internal static byte[] GetBytes(RocksPacketMetadata item)
        {
            return packetSerializer.GetBytes(item);
        }

        internal static byte[] GetBytes(RocksFlowId flowId)
        {
            return flowIdSerializer.GetBytes(flowId);
        }

        internal static byte[] GetBytes(RocksFlowKey flowKey)
        {
            return flowKeySerializer.GetBytes(flowKey);
        }
        internal static byte[] GetBytes(RocksFlowRecord flowKey)
        {
            return flowRecordSerializer.GetBytes(flowKey);
        }

        internal static byte[] GetBytes(RocksPacketBlockId pbId)
        {
            return blockIdSerializer.GetBytes(pbId);
        }

        internal static byte[] GetBytes(RocksPacketBlock packetBlock)
        {
            return packetBlockSerializer.GetBytes(packetBlock);
        }
    }

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
                throw new NotImplementedException();
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
