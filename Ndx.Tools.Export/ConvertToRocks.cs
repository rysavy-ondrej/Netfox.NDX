using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Ingest.Trace;
using Ndx.Shell.Commands;
using RocksDbSharp;
using Ndx.Utils;
namespace Ndx.Tools.Export
{
    /// <summary>
    /// Export metacap to RocksDb.
    /// </summary>
    /// <remarks>
    /// Usage: -r bb7de71e185a2a7818fff92d3ec0dc05.mcap -w bb7de71e185a2a7818fff92d3ec0dc05.rdb ConvertTo-Rocks
    /// </remarks>
    [Command(VerbsData.Export, "Rocks")]
    class ConvertToRocks : Command
    {

        /// <summary>
        /// Path to the source pcap file.
        /// </summary>
        string m_capfile;
        /// <summary>
        /// An instance of Metacap file for the <see cref="m_capfile"/> file.
        /// </summary>
        McapFile m_mcap;

        /// <summary>
        /// Path to the folder with RocksDB generated for the input Metacap file.
        /// </summary>
        string m_rocksDbFolder;

        /// <summary>
        /// An instance of <see cref="RocksDb"/> class that is to be used for writing exported data.
        /// </summary>
        RocksDb m_rocksDb;

        /// <summary>
        /// Gets or sets the path to the input PCAP file.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Metacap { get => m_capfile; set => m_capfile = value; }

        /// <summary>
        /// Gets or sets the path to the output RocksDB root folder.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string RocksDbFolder { get => m_rocksDbFolder; set => m_rocksDbFolder = value; }



        protected override void BeginProcessing()
        {
            try
            {
                var mcapfile = Path.ChangeExtension(m_capfile, "mcap");
                if (m_capfile == null)
                {
                    throw new FileNotFoundException($"File '{mcapfile}' cannot be found.");
                }

                m_mcap = McapFile.Open(mcapfile, m_capfile);

                var options = new DbOptions().SetCreateIfMissing(true).SetCreateMissingColumnFamilies(true);
                var columnFamilies = new ColumnFamilies
                {
                    { "pcaps", new ColumnFamilyOptions() },
                    { "flows.key", new ColumnFamilyOptions() },
                    { "flows.record", new ColumnFamilyOptions() },
                    { "flows.features", new ColumnFamilyOptions() },
                    { "packets", new ColumnFamilyOptions() }
                };
                m_rocksDb = RocksDb.Open(options, m_rocksDbFolder,columnFamilies);
            }
            catch (Exception e)
            {
                WriteError(e, "Cannot process inout file.");
            }
        }


        protected override void EndProcessing()
        {
            m_rocksDb?.Dispose();
        }


        protected override void ProcessRecord()
        {
            if (m_mcap == null || m_rocksDb==null)
            {
                WriteDebug("Non-existing input file or uninitialized database!");
                return;
            }

            // insert information about pcap file:
            var pcapsCollection = m_rocksDb.GetColumnFamily("pcaps");
            var flowsKeyCollection = m_rocksDb.GetColumnFamily("flows.key");
            var flowsRecordCollection = m_rocksDb.GetColumnFamily("flows.record");
            var packetsCollection = m_rocksDb.GetColumnFamily("packets");

            var pcapId = new RocksPcapId()
            {
                Uid = 0
            };
/*
            var convTable = m_mcap.ConversationTable.Entries.ToArray();
            var flowTable = m_mcap.FlowKeyTable.Entries.ToArray();
            WriteDebug($"Start processing conversation table, {convTable.Count()} entries.");
            foreach (var entry in convTable)
            {
                var originatorId = entry.OriginatorFlowId;
                var responderId = entry.ResponderFlowId;

                var originatorFlow = flowTable[originatorId];
                var responderFlow = flowTable[responderId];


                // this id will be also id of the conversation
                var originatorFlowRecordIdx = new RocksFlowId() { Uid = (uint)originatorFlow.IndexRecord.FlowRecordIndex };

                var originatorFlowRecord = m_mcap.GetFlowRecord(originatorFlow.IndexRecord.FlowRecordIndex);
                var responderFlowRecord = m_mcap.GetFlowRecord(responderFlow.IndexRecord.FlowRecordIndex);

                var originatorBlocks = originatorFlow.IndexRecord.PacketBlockList.Select(pbIdx => m_mcap.GetPacketBlock(pbIdx));
                var responderBlocks = responderFlow.IndexRecord.PacketBlockList.Select(pbIdx => m_mcap.GetPacketBlock(pbIdx));
                // list of all packets
                var packets = originatorBlocks.SelectMany(x => x.Packets).Concat(responderBlocks.SelectMany(x => x.Packets)).OrderBy(x => x.Frame.Timestamp);


                if (originatorFlowRecord != null && responderFlowRecord != null)
                {

                    var flowKeyItem = new RocksFlowKey()
                    {
                        Protocol = (ushort)((int)originatorFlow.Key.AddressFamily << 8 | (int)originatorFlow.Key.Protocol),
                        SourceAddress = originatorFlow.Key.SourceAddress,
                        DestinationAddress = originatorFlow.Key.DestinationAddress,
                        SourcePort = originatorFlow.Key.SourcePort,
                        DestinationPort = originatorFlow.Key.DestinationPort
                    };

                    m_rocksDb.Put(RocksSerializer.GetBytes(originatorFlowRecordIdx), RocksSerializer.GetBytes(flowKeyItem), flowsKeyCollection);

                    var flowRecordItem = new RocksFlowRecord()
                    {
                        Octets = (ulong)originatorFlowRecord.Octets + (ulong)responderFlowRecord.Octets,
                        Packets = (uint)originatorFlowRecord.Packets + (uint)responderFlowRecord.Packets,
                        First = Math.Min((ulong)originatorFlowRecord.FirstSeen, (ulong)responderFlowRecord.FirstSeen),
                        Last = Math.Max((ulong)originatorFlowRecord.LastSeen, (ulong)responderFlowRecord.LastSeen),
                        Blocks = (uint)1,   // we always create only one block
                        Application = (uint)originatorFlowRecord.RecognizedProtocol
                    };

                    m_rocksDb.Put(RocksSerializer.GetBytes(originatorFlowRecordIdx), RocksSerializer.GetBytes(flowRecordItem), flowsRecordCollection);
                }
                else
                {
                    WriteWarning($"{originatorFlow.Key}: FlowRecord {originatorFlowRecordIdx} not found in the metacap file.");
                }


                var pbId = new RocksPacketBlockId()
                {
                    FlowId = originatorFlowRecordIdx.Uid,
                    BlockId = 0
                };

                var rdbPacketBlock = new RocksPacketBlock()
                {
                    PcapRef = pcapId,
                    Items = packets.Select(x =>
                        new RocksPacketMetadata()
                        {
                            FrameMetadata = new RocksFrameData()
                            {
                                FrameLength = (uint)x.Frame.FrameLength,
                                FrameNumber = (uint)x.Frame.FrameNumber,
                                FrameOffset = (ulong)x.Frame.FrameOffset,
                                Timestamp = (ulong)x.Frame.Timestamp.ToUnixTimeMilliseconds()
                            },
                            Link = new RocksByteRange() { Start = x.Link.Start, Count = x.Link.Count },
                            Network = new RocksByteRange() { Start = x.Network.Start, Count = x.Network.Count },
                            Transport = new RocksByteRange() { Start = x.Transport.Start, Count = x.Transport.Count },
                            Payload = new RocksByteRange() { Start = x.Payload.Start, Count = x.Payload.Count },
                        }
                    ).ToArray()
                };
                m_rocksDb.Put(RocksSerializer.GetBytes(pbId), RocksSerializer.GetBytes(rdbPacketBlock), packetsCollection);

            }
            */
        }
    }
}
