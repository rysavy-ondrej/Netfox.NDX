    using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ndx.Metacap;
using Ndx.Shell.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RocksDbSharp;

namespace Ndx.Tools.Export
{
    /// <summary>
    /// Shows content of RocksDb.
    /// </summary>
    /// <remarks>
    /// Usage: -r bb7de71e185a2a7818fff92d3ec0dc05.rdb Show-Rocks
    /// </remarks>
    [Command(VerbsCommon.Show, "Rocks")]
    class ShowRocks : Command
    {

        /// <summary>
        /// Path to the folder with RocksDB generated for the input Metacap file.
        /// </summary>
        string m_rocksDbFolder;

        /// <summary>
        /// An instance of <see cref="RocksDb"/> class that is to be used for writing exported data.
        /// </summary>
        RocksDb m_rocksDb;

        /// <summary>
        /// Output JSON file.
        /// </summary>
        string m_outputFile;

        /// <summary>
        /// Gets or sets the path to the output RocksDB root folder.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string RocksDbFolder { get => m_rocksDbFolder; set => m_rocksDbFolder = value; }

        [Parameter(Mandatory = true)]
        public string OutputFile { get => m_outputFile; set => m_outputFile = value; }

        protected override void BeginProcessing()
        {
            try
            {
                var options = new DbOptions();
                var columnFamilies = new ColumnFamilies
                {
                    { "pcaps", new ColumnFamilyOptions() },
                    { "flows", new ColumnFamilyOptions() },
                    { "packets", new ColumnFamilyOptions() }
                };
                m_rocksDb = RocksDb.Open(options, m_rocksDbFolder, columnFamilies);
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
            if (m_rocksDb==null)
            {
                WriteDebug("Non-existing Rocks database!");
                return;
            }

            var pcapsCollection = m_rocksDb.GetColumnFamily("pcaps");
            var flowsCollection = m_rocksDb.GetColumnFamily("flows");
            var packetsCollection = m_rocksDb.GetColumnFamily("packets");

            var pcaps = new JArray();
            using (var iter = m_rocksDb.NewIterator(pcapsCollection))
            {
                iter.SeekToFirst();
                while (iter.Valid())
                {
                    var pcapId = RocksSerializer.ToPcapId(iter.Key(), 0);
                    var pcapFile = RocksSerializer.ToPcapFile(iter.Value(), 0);

                    var item = new JObject()
                    {
                        ["key"] = new JObject()
                        {
                            ["uid"] = pcapId.Uid
                        },
                        ["value"] = new JObject()
                        {
                            ["pcapType"] = pcapFile.PcapType,
                            ["uriLength"] = pcapFile.UriLength,
                            ["uri"] = pcapFile.Uri.ToString(),
                            ["md5signature"] = pcapFile.Md5signature,
                            ["shasignature"] = pcapFile.Shasignature,
                        }
                    };
                    pcaps.Add(item);
                    iter.Next();
                }
            }


            var flows = new JArray();
            using (var iter = m_rocksDb.NewIterator(flowsCollection))
            {
                iter.SeekToFirst();
                while (iter.Valid())
                {
                    var flowKey = RocksSerializer.ToFlowKey(iter.Key(),0);
                    var flowRecord = RocksSerializer.ToFlowRecord(iter.Value(),0);
                    var item = new JObject
                    {
                        ["key"] = new JObject
                        {
                            ["protocol"] = flowKey.Protocol,
                            ["sourceAddress"] = flowKey.SourceAddress.ToString(),
                            ["destinationAddress"] = flowKey.DestinationAddress.ToString(),
                            ["sourcePort"] = flowKey.SourcePort,
                            ["destinationPort"] = flowKey.DestinationPort,
                            ["flowCounter"] = flowKey.FlowCounter
                        },
                        ["value"] = new JObject
                        {
                            ["octets"] = flowRecord.Octets,
                            ["packets"] = flowRecord.Packets,
                            ["first"] = flowRecord.First,
                            ["last"] = flowRecord.Last,
                            ["blocks"] = flowRecord.Blocks,
                            ["application"] = flowRecord.Application
                        }
                    };
                    flows.Add(item);                
                    iter.Next();
                }
            }

            var packets = new JArray();
            using (var iter = m_rocksDb.NewIterator(packetsCollection))
            {
                iter.SeekToFirst();
                while (iter.Valid())
                {
                    var packetBlockId = RocksSerializer.ToPacketBlockId(iter.Key(),0);
                    var packetBlock = RocksSerializer.ToPacketBlock(iter.Value(), 0);

                    var item = new JObject
                    {
                        ["key"] = new JObject
                        {
                            ["fkey"] = new JObject
                            {
                                ["protocol"] = packetBlockId.FlowKey.Protocol,
                                ["sourceAddress"] = packetBlockId.FlowKey.SourceAddress.ToString(),
                                ["destinationAddress"] = packetBlockId.FlowKey.DestinationAddress.ToString(),
                                ["sourcePort"] = packetBlockId.FlowKey.SourcePort,
                                ["destinationPort"] = packetBlockId.FlowKey.DestinationPort,
                                ["flowCounter"] = packetBlockId.FlowKey.FlowCounter
                            },
                            ["blockId"] = packetBlockId.BlockId
                        },
                        ["value"] = new JObject
                        {
                                ["pcapRef"] = packetBlock.PcapRef.Uid,
                                ["count"] = packetBlock.Items.Count(),
                                ["items"] = new JArray(packetBlock.Items.Select(x=>
                                    new JObject()
                                    {
                                        ["frame"] = new JObject()
                                        {
                                            ["frameNumber"] = x.FrameMetadata.FrameNumber,
                                            ["frameLength"] = x.FrameMetadata.FrameLength,
                                            ["frameOffset"] = x.FrameMetadata.FrameOffset,
                                            ["timestamp"] = x.FrameMetadata.Timestamp
                                        },
                                        ["link"] = x.Link.ToJObject(),
                                        ["network"] = x.Network.ToJObject(),
                                        ["transport"] = x.Transport.ToJObject(),
                                        ["payload"] = x.Payload.ToJObject(),
                                    }                                                                        
                                    ).ToArray()),
                        }
                    };
                    packets.Add(item);


                    iter.Next();
                }
            }
            var root = new JObject()
            {
                ["pcaps"] = pcaps,
                ["flows"] = flows,
                ["packets"] = packets
            };
            WriteObject(root);
        }
    }       
}
