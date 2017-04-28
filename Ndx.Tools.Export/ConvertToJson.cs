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
    /// Export metacap to RocksDb.
    /// </summary>
    /// <remarks>
    /// Usage: -r file.mcap -w file.json ConvertTo-Json
    /// </remarks>
    [Command(VerbsData.Export, "Json")]
    class ConvertToJson : Command
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
        /// Gets or sets the path to the input PCAP file.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string Metacap { get => m_capfile; set => m_capfile = value; }

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

                
            }
            catch (Exception e)
            {
                WriteError(e, "Cannot process inout file.");
            }
        }


        protected override void EndProcessing()
        {
        }


        protected override void ProcessRecord()
        {
            if (m_mcap == null)
            {
                WriteDebug("Non-existing or incorrect input file!");
                return;
            }

            WriteObject("{");
            WriteObject("\"flows\": [");

            var flowTable = m_mcap.FlowKeyTable.Entries.ToArray();
            for(int flowTableIndex=0; flowTableIndex< flowTable.Length; flowTableIndex++)
            {
                var entry = flowTable[flowTableIndex];
                var flowRecordIdx = entry.IndexRecord.FlowRecordIndex;

                var flowRecord = m_mcap.GetFlowRecord(flowRecordIdx);
                if (flowRecord != null)
                {
                    var flowKey = entry.Key;
                    var value = flowRecord;
                    WriteObject($"{{ \"key\":\"{flowKey}\", \"record\" : {JsonConvert.SerializeObject(value, FlowRecordSerializer.Instance)}, ");
                }
                else
                {
                    WriteWarning($"{entry.Key}: FlowRecord {flowRecordIdx} not found in the metacap file.");
                }
                var metadata = m_mcap.GetPacketMetadataCollection(entry).ToArray();
                WriteObject("\"packets\": [");
                for(int metadataIndex =0; metadataIndex< metadata.Length; metadataIndex++)
                {
                    var value = metadata[metadataIndex];
                    var valstr = JsonConvert.SerializeObject(value, PacketMetadataSerializer.Instance, ByteRangeSerializer.Instance, FrameMetadataSerializer.Instance);
                    WriteObject($"{valstr}{GetEol(metadataIndex, metadata.Length)}");
                }
                WriteObject("]");
                WriteObject($"}}{GetEol(flowTableIndex, flowTable.Length)}");
            }

            WriteObject("]");
            WriteObject("}");
        }

        private string GetEol(int index, int length)
        {
            return index < length - 1 ? "," : "";
        }
    }
}
