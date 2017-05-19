using System;
using System.IO;
using System.Linq;
using Ndx.Ingest.Trace;
using Ndx.Shell.Commands;

namespace Ndx.Tools.Metacap
{

    /// <summary>
    /// This command verifies the integrity of the metacap file.
    /// It enumerates all flows and checks that the corresponding flow records
    /// and packet blocks are available.
    /// </summary>
    [Command(VerbsDiagnostic.Verify, "Index")]
    public class VerifyIndex : Command
    {

        string m_capfile;
        McapFile m_mcap;

        [Parameter(Mandatory = true)]
        public string Capfile { get => m_capfile; set => m_capfile = value; }

        protected override void BeginProcessing()
        {
            try
            {
                var mcapfile = Path.ChangeExtension(m_capfile, "mcap");
                m_mcap = McapFile.Open(mcapfile, m_capfile);
                if (m_capfile == null)
                {
                    throw new FileNotFoundException($"File '{mcapfile}' cannot be found.");
                }
            }
            catch(Exception e)
            {
                WriteError(e, "Cannot process inout file.");    
            }   
        }

        protected override void ProcessRecord()
        {
            if (m_mcap == null)
            {
                WriteDebug("Empty file!");
                return;
            }
            /*
            var flowTable = m_mcap.FlowKeyTable.Entries.ToArray();
            WriteDebug($"Start processing flow table, {flowTable.Count()} entries.");
            foreach (var entry in flowTable)
            {
                var flowRecordIdx = entry.IndexRecord.FlowRecordIndex;

                var flowRecord = m_mcap.GetFlowRecord(flowRecordIdx);
                if (flowRecord == null)
                {
                    WriteObject($"{entry.Key}: FlowRecord {flowRecordIdx} not found in the metacap file.");
                }

                var packetCount = 0;
                foreach(var packetBlockIdx in entry.IndexRecord.PacketBlockList)
                {
                    var packetBlock = m_mcap.GetPacketBlock(packetBlockIdx);
                    if (packetBlock == null)
                    {
                        WriteObject($"{entry.Key}: PacketBlock {packetBlockIdx} not found in the metacap file.");
                    }
                    else
                    {
                        packetCount += packetBlock.Count;
                    }
                } 
                if (flowRecord?.Packets != packetCount)
                {
                    WriteObject($"{entry.Key}: flow packets number ({flowRecord?.Packets}) and block packets number ({packetCount}) differ.");
                }
            }
            */
        }
    }
}
