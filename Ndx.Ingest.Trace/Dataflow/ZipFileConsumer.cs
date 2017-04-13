using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Linq;

namespace Ndx.Ingest.Trace
{

    /// <summary>
    /// This consumer writes information in the index files.
    /// </summary>
    /// <remarks>
    /// Three index files are generated:
    /// * Key - contains all flow keys and offsets pointing to Map and Fix files.
    /// * Map - contains a collection of serialized _PacketBlock objects that can be used to access packets.
    /// * Fix - contains a collection of flow records.
    /// </remarks>
    /// <todo>Use ZIP64 to compress generated file and create a single zip file.</todo>
    public class ZipFileConsumer : IDisposable
    {       
        private ZipArchive m_archive;
        private string m_captureFile;
        private int m_blockCount;
        private int m_flowCount;

        private object m_sync = new object();

        public ZipFileConsumer(string pcapfile, string mcapfile) : this()
        {

            var pcapPath = Path.GetFullPath(pcapfile);
            var capFilename = Path.GetFileName(pcapfile);
            var mcapPath = Path.GetFullPath(mcapfile);

            // delete if exists:
            if (File.Exists(mcapPath))
            {
                File.Delete(mcapPath);
            }

            m_archive = ZipFile.Open(mcapPath, ZipArchiveMode.Update);
            m_captureFile = capFilename;
            m_flowDictionary = new Dictionary<FlowKey, IndexRecord>(1024);
            m_conversationDictionary = new Dictionary<FlowKey, int>(1024);
        }

        int m_packetBlockCount;
        ActionBlock<PacketBlock> m_packetBlockTarget;

        int m_flowRecordCount;
        ActionBlock<FlowRecord> m_flowRecordTarget;

        int m_rawframeCount;
        ActionBlock<RawFrame> m_rawFrameTarget;

        private Dictionary<FlowKey, IndexRecord> m_flowDictionary;
        private Dictionary<FlowKey, int> m_conversationDictionary;

        private PacketBlock.BinaryConverter m_packetBlockConverter = new PacketBlock.BinaryConverter();
        void WritePacketBlock(PacketBlock block)
        {
            var index = 0;
            lock (m_sync)
            {
                index = m_blockCount++;
                if (!m_flowDictionary.TryGetValue(block.Key, out IndexRecord value))
                {
                    m_flowDictionary[block.Key] = value = new IndexRecord();
                }
                value.PacketBlockList.Add(index);
            }
            var path = MetacapFileInfo.GetPacketBlockPath(index);
            var blockEntry = m_archive.CreateEntry(path, CompressionLevel.Fastest);
            using (var writer = new BinaryWriter(blockEntry.Open()))
            {
                m_packetBlockConverter.WriteObject(writer, block);
            }
        }

        
        FlowRecord.BinaryConverter m_flowRecordConverter = new FlowRecord.BinaryConverter();
        void WriteFlowRecord(FlowRecord flow)
        {
            var index = 0;
            var convid = 0;
            lock (m_sync)
            {
                index = m_flowCount++;
                m_flowDictionary[flow.Key] = new IndexRecord()
                {
                    FlowRecordIndex = index
                };

                if (flow.EndpointType == FlowEndpointType.Originator)
                {
                    m_conversationDictionary.Add(flow.Key, ++convid);
                }                
            }
            var path = MetacapFileInfo.GetFlowRecordPath(index);
            var entry = m_archive.CreateEntry(path, CompressionLevel.Fastest);
            using (var writer = new BinaryWriter(entry.Open()))
            {
                m_flowRecordConverter.WriteObject(writer, flow);
            }                
        }

        ZipFileConsumer()
        {
            m_packetBlockTarget = new ActionBlock<PacketBlock>(x => { m_packetBlockCount++; WritePacketBlock(x); });
            m_flowRecordTarget = new ActionBlock<FlowRecord>(x => { m_flowRecordCount++; WriteFlowRecord(x); });
            m_rawFrameTarget = new ActionBlock<RawFrame>(x => m_rawframeCount++);
        }

        public Task Completion => Task.WhenAll(m_packetBlockTarget.Completion, m_flowRecordTarget.Completion);


        public void Close()
        {
            WriteKeyTable();
            WriteConversationTable();
            m_archive.Dispose();
        }

        private void WriteKeyTable()
        {
            var entry = m_archive.CreateEntry(MetacapFileInfo.FlowKeyTableFile, CompressionLevel.Fastest);
            using (var writer = new BinaryWriter(entry.Open()))
            {
                foreach (var item in m_flowDictionary)
                {
                    var keyTableEntry = new FlowKeyTableEntry(item.Key, item.Value);
                    FlowKeyTableEntry.Converter.WriteObject(writer, keyTableEntry);
                }
            }
        }


        private void WriteConversationTable()
        {
            var entry = m_archive.CreateEntry(MetacapFileInfo.ConversationTableFile, CompressionLevel.Fastest);
            using (var writer = new BinaryWriter(entry.Open()))
            {
                foreach (var item in m_conversationDictionary)
                {
                    var conversationTableEntry = new ConversationTableEntry(item.Value, item.Key, item.Key.Swap());
                    ConversationTableEntry.Converter.WriteObject(writer, conversationTableEntry);
                }
            }
        }

        public void Dispose()
        {
            m_archive.Dispose();
        }

        public ITargetBlock<PacketBlock> PacketBlockTarget => m_packetBlockTarget;

        public ITargetBlock<FlowRecord> FlowRecordTarget => m_flowRecordTarget;

        public ITargetBlock<RawFrame> RawFrameTarget => m_rawFrameTarget;

        public int PacketBlockCount => m_packetBlockCount;

        public int FlowRecordCount => m_flowRecordCount;

        public int RawFrameCount => m_rawframeCount;      
    }
}
