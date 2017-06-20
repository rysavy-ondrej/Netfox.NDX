using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Model;
using Ndx.Shell.Commands;
using RocksDbSharp;

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
        /// Path to the folder with RocksDB generated for the input Metacap file.
        /// </summary>
        string m_rocksDbFolder;

        /// <summary>
        /// An instance of <see cref="RocksDb"/> class that is to be used for writing exported data.
        /// </summary>
        RocksDb m_rocksDb;
        private IDictionary<int, Conversation> m_conversations;

        private IDictionary<int, MetaFrame> m_frames;

        /// <summary>
        /// Gets or sets the path to the input PCAP file.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string PcapFile { get => m_capfile; set => m_capfile = value; }

        /// <summary>
        /// Gets or sets the path to the output RocksDB root folder.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string RocksDbFolder { get => m_rocksDbFolder; set => m_rocksDbFolder = value; }


        


        protected override void BeginProcessing()
        {
            try
            {
                var fullpath = Path.GetFullPath(m_capfile);
                // open pcap file and process it with metacap
                if (!File.Exists(fullpath)) { throw new FileNotFoundException("${fullpath}"); }

                m_conversations = new Dictionary<int, Conversation>();
                m_frames = new Dictionary<int, MetaFrame>();
               

               

                // create DB
                var options = new DbOptions().SetCreateIfMissing(true).SetCreateMissingColumnFamilies(true);
                var columnFamilies = new ColumnFamilies
                {
                    { "pcaps", new ColumnFamilyOptions() },
                    { "flows", new ColumnFamilyOptions() },
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
            if (m_capfile == null || m_rocksDb==null)
            {
                WriteDebug("Non-existing input file or uninitialized database!");
                return;
            }

            // insert information about pcap file:
            var pcapsCollection = m_rocksDb.GetColumnFamily("pcaps");
            var flowsCollection = m_rocksDb.GetColumnFamily("flows");
            var packetsCollection = m_rocksDb.GetColumnFamily("packets");

            var pcapId = new RocksPcapId()
            {
                Uid = 0
            };
            var rdbPcapFile = new RocksPcapFile()
            {
                PcapType = (ushort)PcapFileFormat.Libpcap,
                IngestedOn = DateTimeOffset.Now,
                Uri = new Uri(Path.GetFullPath(m_capfile))
            };
            m_rocksDb.Put(RocksSerializer.GetBytes(pcapId), RocksSerializer.GetBytes(rdbPcapFile), pcapsCollection);


            var frames = Captures.PcapReader.ReadFile(m_capfile);

            var tracker = new Ingest.ConversationTracker();
            tracker.Output.LinkTo(new ActionBlock<KeyValuePair<Conversation, MetaFrame>>((Action<KeyValuePair<Conversation, MetaFrame>>)ConsumeLabeledFrames));

            foreach (var frame in frames)
            {
                tracker.Input.Post(frame);
            }

            Task.WaitAll(tracker.Completion);

            foreach (var conversation in m_conversations.Values)
            {
                var upflowKey = conversation.ConversationKey;
                var downflowKey = conversation.ConversationKey.Swap();

                var upflowRecord = conversation.Upflow;
                var downflowRecord = conversation.Downflow;

                var upflowPackets = conversation.UpflowPackets;
                var downflowPackets = conversation.DownflowPackets;

                // Note that conversation may consists only of a single flow:
                if (upflowKey != null && upflowRecord != null)
                {
                    WriteFlowRecord(upflowKey, upflowRecord);
                    WritePacketBlock(upflowKey, upflowPackets.Select(x => m_frames[x]));

                }
                // Note that conversation may consists only of a single flow, it should be upflow, but for regularity...
                if (downflowKey != null && downflowRecord != null)
                {
                    WriteFlowRecord(downflowKey, downflowRecord);
                    WritePacketBlock(downflowKey, downflowPackets.Select(x => m_frames[x]));
                }
            }

            void WriteFlowRecord(FlowKey flowKey, FlowAttributes flowRecord)
            {
                var flowKeyValue = new RocksFlowKey()
                {
                    Protocol = (ushort)((int)flowKey.SourceIpAddress.AddressFamily << 8 | (int)flowKey.IpProtocol),
                    SourceAddress = flowKey.SourceIpAddress,
                    DestinationAddress = flowKey.DestinationIpAddress,
                    SourcePort = (ushort)flowKey.SourcePort,
                    DestinationPort = (ushort)flowKey.DestinationPort,
                    FlowCounter = 1
                };
                var flowRecordValue = new RocksFlowRecord()
                {
                    Octets = (ulong)flowRecord.Octets + (ulong)flowRecord.Octets,
                    Packets = (uint)flowRecord.Packets + (uint)flowRecord.Packets,
                    First = Math.Min((ulong)flowRecord.FirstSeen, (ulong)flowRecord.FirstSeen),
                    Last = Math.Max((ulong)flowRecord.LastSeen, (ulong)flowRecord.LastSeen),
                    Blocks = (uint)1,   // we always create only one block here
                    Application = (uint)0,
                };
                m_rocksDb.Put(RocksSerializer.GetBytes(flowKeyValue), RocksSerializer.GetBytes(flowRecordValue), flowsCollection);
            }
            void WritePacketBlock(FlowKey flowKey, IEnumerable<MetaFrame> packets)
            {
                var rdbPacketBlockId = new RocksPacketBlockId()
                {
                    FlowKey = new RocksFlowKey()
                    {
                        Protocol = (ushort)((int)flowKey.SourceIpAddress.AddressFamily << 8 | (int)flowKey.IpProtocol),
                        SourceAddress = flowKey.SourceIpAddress,
                        DestinationAddress = flowKey.DestinationIpAddress,
                        SourcePort = (ushort)flowKey.SourcePort,
                        DestinationPort = (ushort)flowKey.DestinationPort,
                        FlowCounter = 1
                    },
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
                                FrameLength = (uint)x.FrameLength,
                                FrameNumber = (uint)x.FrameNumber,
                                FrameOffset = (ulong)x.FrameOffset,
                                Timestamp = (ulong)x.TimeStamp
                            },
                            Link = new RocksByteRange() { Start = x.Datalink.Bytes.Offset, Count = x.Datalink.Bytes.Length },
                            Network = new RocksByteRange() { Start = x.Network.Bytes.Offset, Count = x.Network.Bytes.Length },
                            Transport = new RocksByteRange() { Start = x.Transport.Bytes.Offset, Count = x.Transport.Bytes.Length },
                            Payload = new RocksByteRange() { Start = x.Application.Bytes.Offset, Count = x.Application.Bytes.Length },
                        }
                    ).ToArray()
                };
                m_rocksDb.Put(RocksSerializer.GetBytes(rdbPacketBlockId), RocksSerializer.GetBytes(rdbPacketBlock), packetsCollection);
            }
        }

        private void ConsumeLabeledFrames(KeyValuePair<Conversation, MetaFrame> obj)
        {
            if (!m_conversations.TryGetValue(obj.Key.ConversationId, out var value))
            {
                m_conversations.Add(obj.Key.ConversationId, obj.Key);
            }
            m_frames.Add(obj.Value.FrameNumber, obj.Value);
        }
    }
}
