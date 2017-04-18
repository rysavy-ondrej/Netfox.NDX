using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Ingest.Trace;
using Ndx.Shell.Commands;
using Newtonsoft.Json;
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
        private ColumnFamilyHandle m_flowsCollection;
        private ColumnFamilyHandle m_packetsCollection;

        /// <summary>
        /// Gets or sets the path to the output RocksDB root folder.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string RocksDbFolder { get => m_rocksDbFolder; set => m_rocksDbFolder = value; }

        protected override void BeginProcessing()
        {
            try
            {
                var options = new DbOptions();
                var columnFamilies = new ColumnFamilies();
                columnFamilies.Add("flows", new ColumnFamilyOptions());
                columnFamilies.Add("packets", new ColumnFamilyOptions());
                m_rocksDb = RocksDb.Open(options, m_rocksDbFolder, columnFamilies);                
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
            if (m_rocksDb==null)
            {
                WriteDebug("Non-existing Rocks database!");
                return;
            }

            WriteObject("{");
            WriteObject("\"flows\": [");
            using (var iter = m_rocksDb.NewIterator(m_flowsCollection))
            {
                iter.SeekToFirst();
                while (iter.Valid())
                {
                    var flowKey = new FlowKey(iter.Key());
                    var value = new FlowRecord(iter.Value());                                        
                    iter.Next();
                    var eol = iter.Valid() ? "," : "";
                    WriteObject($"{{ \"key\":\"{flowKey}\", \"value\" : {JsonConvert.SerializeObject(value)} }} {eol}");
                }
            }
            WriteObject("],");
            WriteObject("\"packets\": [");
            using (var iter = m_rocksDb.NewIterator(m_packetsCollection))
            {
                iter.SeekToFirst();
                while (iter.Valid())
                {
                    var flowKey = new FlowKey(iter.Key());
                    var value = iter.Value();

                    WriteObject($"{{ \"key\":\"{flowKey}\" , \"items\" : [");
                    var count = BitConverter.ToInt32(value, 0);
                    for (int i=0; i<count; i++)
                    {
                        var pm = new PacketMetadata(value, sizeof(int) + i * PacketMetadata.MetadataSize);
                        var valstr = JsonConvert.SerializeObject(pm);
                        var eol1 = i<count-1 ? "," : "";
                        WriteObject($"{valstr}{eol1}");

                    }
                    iter.Next();
                    var eol = iter.Valid() ? "," : "";
                    WriteObject($"] }} {eol}");                                         
                }
            }
            WriteObject("]");
            WriteObject("}");
        }
    }
}
