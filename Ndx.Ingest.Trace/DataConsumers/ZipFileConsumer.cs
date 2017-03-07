using Ndx.Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Ndx.Ingest.Trace.DataConsumers
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
    public partial class ZipFileConsumer : IDisposable
    {       
        ZipArchive m_archive;
        McapIndex.McapIndexEntry m_entry;

        public ZipFileConsumer(string fileName) : this()
        {

            var fullname = Path.GetFullPath(fileName);
            var localname = Path.GetFileName(fileName);
            var archName = Path.ChangeExtension(fullname, "mcap");

            // delete if exists:
            if (File.Exists(archName)) File.Delete(archName);

            m_archive = ZipFile.Open(archName, ZipArchiveMode.Update);
            m_entry = new McapIndex.McapIndexEntry()
            {
                CaptureFile = localname,
                PacketBlockFolder = Path.ChangeExtension(localname, "map"),
                FlowRecordFolder = Path.ChangeExtension(localname, "fix"),
                KeyFile = Path.ChangeExtension(localname, "key"),
            };
            m_flowDictionary = new Dictionary<FlowKey, IndexRecord>(1024);
        }

        int m_packetBlockCount;
        ActionBlock<PacketBlock> m_packetBlockTarget;

        int m_flowRecordCount;
        ActionBlock<FlowRecord> m_flowRecordTarget;

        int m_rawframeCount;
        ActionBlock<RawFrame> m_rawFrameTarget;

        Dictionary<FlowKey, IndexRecord> m_flowDictionary;


        object _sync = new object();

        int blockCount;
        PacketBlock.BinaryConverter PacketBlockConverter = new PacketBlock.BinaryConverter();
        void WritePacketBlock(PacketBlock block)
        {
            var index = 0;
            lock (_sync)
            {
                index = blockCount++;
                if (!m_flowDictionary.TryGetValue(block.Key, out IndexRecord value))
                {
                    m_flowDictionary[block.Key] = value = new IndexRecord();
                }
                value.PacketBlockList.Add(index);
            }
            var path = Path.Combine(m_entry.PacketBlockFolder, index.ToString().PadLeft(6, '0'));
            var blockEntry = m_archive.CreateEntry(path, CompressionLevel.Fastest);
            using (var writer = new BinaryWriter(blockEntry.Open()))
            {
                PacketBlockConverter.WriteObject(writer, block);
            }
        }

        int flowCount;
        FlowRecord.BinaryConverter FlowRecordConverter = new FlowRecord.BinaryConverter();
        void WriteFlowRecord(FlowRecord flow)
        {
            var index = 0;
            lock (_sync)
            {
                index = flowCount++;
                if (!m_flowDictionary.TryGetValue(flow.Key, out IndexRecord value))
                {
                    m_flowDictionary[flow.Key] = value = new IndexRecord();
                }
                value.FlowRecordOffset = index;
            }
            var path = Path.Combine(m_entry.FlowRecordFolder, index.ToString().PadLeft(6, '0'));
            var entry = m_archive.CreateEntry(path, CompressionLevel.Fastest);
            using (var writer = new BinaryWriter(entry.Open()))
            {
                FlowRecordConverter.WriteObject(writer, flow);
            }
                
        }

        ZipFileConsumer()
        {
            m_packetBlockTarget = new ActionBlock<PacketBlock>(x => { m_packetBlockCount++; WritePacketBlock(x); });
            m_flowRecordTarget = new ActionBlock<FlowRecord>(x => { m_flowRecordCount++; WriteFlowRecord(x); });
            m_rawFrameTarget = new ActionBlock<RawFrame>(x => m_rawframeCount++);
        }

        public Task Completion
        {
            get
            {
                return Task.WhenAll(m_packetBlockTarget.Completion, m_flowRecordTarget.Completion);
            }
        }


        public void Close()
        {
            WriteKeyTable();
            WriteIndexFile();
            m_archive.Dispose();
        }

        private void WriteIndexFile()
        {
            var index = new McapIndex();
            index.Add(m_entry);

            var indexJson = JsonConvert.SerializeObject(index);
            var entry = m_archive.CreateEntry("index.json", CompressionLevel.Fastest);
            using (var writer = new StreamWriter(entry.Open()))
            {
                writer.Write(indexJson);
            }
        }

        private void WriteKeyTable()
        {
            var entry = m_archive.CreateEntry(m_entry.KeyFile, CompressionLevel.Fastest);
            using (var writer = new BinaryWriter(entry.Open()))
            {
                foreach (var item in m_flowDictionary)
                {
                    var keyTableEntry = new KeyTableEntry(item.Key, item.Value);
                    KeyTableEntry.Converter.WriteObject(writer, keyTableEntry);
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
