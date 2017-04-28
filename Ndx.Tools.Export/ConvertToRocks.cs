using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Ingest.Trace;
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
        private ColumnFamilyHandle m_flowsCollection;
        private ColumnFamilyHandle m_packetsCollection;

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
                    { "flows", new ColumnFamilyOptions() },
                    { "packets", new ColumnFamilyOptions() }
                };
                m_rocksDb = RocksDb.Open(options, m_rocksDbFolder,columnFamilies);
                m_flowsCollection = m_rocksDb.GetColumnFamily("flows");
                m_packetsCollection = m_rocksDb.GetColumnFamily("packets");
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
            var flowTable = m_mcap.FlowKeyTable.Entries.ToArray();
            WriteDebug($"Start processing flow table, {flowTable.Count()} entries.");
            foreach (var entry in flowTable)
            {
                var flowRecordIdx = entry.IndexRecord.FlowRecordIndex;

                var flowRecord = m_mcap.GetFlowRecord(flowRecordIdx);
                if (flowRecord != null)
                {
                    m_rocksDb.Put(entry.Key.GetBytes(), flowRecord.GetBytes(), m_flowsCollection);
                }
                else
                {
                    WriteWarning($"{entry.Key}: FlowRecord {flowRecordIdx} not found in the metacap file.");
                }
                
                var metadata = m_mcap.GetPacketMetadataCollection(entry).ToArray();
                var value = new byte[sizeof(int) + PacketMetadata.MetadataSize * metadata.Length];
                Array.Copy(BitConverter.GetBytes(metadata.Length), value, sizeof(int));
                for(int i = 0; i < metadata.Length; i++)
                {
                    var srcArr = metadata[i].GetBytes();
                    Array.Copy(srcArr, 0, value, sizeof(int) + i * PacketMetadata.MetadataSize, srcArr.Length); 
                }
                m_rocksDb.Put(entry.Key.GetBytes(), value, m_packetsCollection);
            }
        }
    }
}
