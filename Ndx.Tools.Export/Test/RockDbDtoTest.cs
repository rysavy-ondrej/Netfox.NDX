using Ndx.Metacap;
using PacketDotNet;
using System.Net;
using System.Net.Sockets;
using Ndx.Tools.Export;
using System.Diagnostics;
using Ndx.Utils;
using System;

namespace Ndx.Tools.Export.Test
{
    public class RockDbDtoTest
    {
        public void RocksFlowKeySerializationTest()
        {
            var flowKey = new RocksFlowKey()
            {
                Protocol = (ushort)((int)AddressFamily.InterNetwork << 8 | (int)IPProtocolType.UDP),
                SourceAddress = IPAddress.Parse("192.168.1.1"),
                DestinationAddress = IPAddress.Parse("10.10.10.1"),
                SourcePort = 12345,
                DestinationPort = 53
            };

            var bytes = RocksSerializer.GetBytes(flowKey);

            var flowKey2 = RocksSerializer.ToFlowKey(bytes, 0);
            Debug.Assert(flowKey.Protocol == flowKey2.Protocol);
            Debug.Assert(flowKey.SourceAddress.Equals(flowKey2.SourceAddress));
            Debug.Assert(flowKey.DestinationAddress.Equals(flowKey2.DestinationAddress));
            Debug.Assert(flowKey.SourcePort == flowKey2.SourcePort);
            Debug.Assert(flowKey.DestinationPort == flowKey2.DestinationPort);
        }

        public void RocksFlowRecordSerializationTest()
        {
            var flowRecord = new RocksFlowRecord()
            {
                Application = 1232,
                Blocks = 13,
                First = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                Last = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds()+ 10000,
                Octets = 123255,
                Packets = 124
            };
            var bytes = RocksSerializer.GetBytes(flowRecord);
            var flowRecord2 = RocksSerializer.ToFlowRecord(bytes, 0);
            Debug.Assert(flowRecord.Application == flowRecord2.Application);
            Debug.Assert(flowRecord.Blocks == flowRecord2.Blocks);
            Debug.Assert(flowRecord.First == flowRecord2.First);
            Debug.Assert(flowRecord.Last == flowRecord2.Last);
            Debug.Assert(flowRecord.Octets == flowRecord2.Octets);
            Debug.Assert(flowRecord.Packets == flowRecord2.Packets);
        }

        public void RocksPacketMetadataSerializationTest()
        {
            var pm = new RocksPacketMetadata()
            {
                FrameMetadata = new RocksFrameData() { FrameLength = 1024, FrameNumber = 1, FrameOffset = 47876, Timestamp = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds() },
                Link = new RocksByteRange() { Start = 0, Count = 1024 },
                Network = new RocksByteRange() { Start = 40, Count = 984 },
                Transport = new RocksByteRange() { Start = 80, Count = 944 },
                Payload = new RocksByteRange() { Start = 100, Count = 924 }
            };
            var bytes = RocksSerializer.GetBytes(pm);
            var pm2 = RocksSerializer.ToPacketMetadata(bytes, 0);


        }


        public void RocksPacketBlockSerializationTest()
        {
            var pms = new RocksPacketMetadata[10];
            for (int i =0; i < 10; i++)
            {
                pms[i] = new RocksPacketMetadata()
                {
                    FrameMetadata = new RocksFrameData() { FrameLength = 1024, FrameNumber = 1, FrameOffset = 47876, Timestamp = (ulong)DateTimeOffset.Now.ToUnixTimeMilliseconds() },
                    Link = new RocksByteRange() { Start = 0, Count = 1024 },
                    Network = new RocksByteRange() { Start = 40, Count = 984 },
                    Transport = new RocksByteRange() { Start = 80, Count = 944 },
                    Payload = new RocksByteRange() { Start = 100, Count = 924 }
                };
            }


            var pb = new RocksPacketBlock()
            {
                Items = pms,
                PcapRef = new RocksPcapId() { Uid = 1245 }
            };

            var bytes = RocksSerializer.GetBytes(pb);
            var pb2 = RocksSerializer.ToPacketBlock(bytes, 0);



        }



    }
}
