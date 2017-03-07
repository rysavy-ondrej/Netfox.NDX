using Ndx.Network;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class FileConsumer : IDisposable
    {
        /// <summary>
        /// | 0 1 2 3 | 4 5 6 7  |
        /// | P M A P | reserved |
        /// </summary>
        public byte[] BlockHeader = new byte[] { 0x50 , 0x4d , 0x41, 0x50, 0x0, 0x0, 0x0, 0x0 };

        /// <summary>
        /// | 0 1 2 3 | 4 5 6 7  |
        /// | P F I X | reserved |
        /// </summary>
        public byte[] FlowHeader = new byte[] { 0x50, 0x46, 0x49, 0x58, 0x0, 0x0, 0x0, 0x0 };

        /// <summary>
        /// | 0 1 2 3 | 4 5 6 7  |
        /// | P K E Y | reserved |
        /// </summary>
        public byte[] KeyHeader = new byte[] { 0x50, 0x4b, 0x45, 0x59, 0x0, 0x0, 0x0, 0x0 };

        BinaryWriter m_blockWriter;
        BinaryWriter m_flowWriter;
        BinaryWriter m_keyWriter;
        public FileConsumer(string fileName) : this ()
        {
            var fullname = Path.GetFullPath(fileName);
            var blockName = Path.ChangeExtension(fullname, "pmap");
            var flowName = Path.ChangeExtension(fullname, "pfix");
            var keyName = Path.ChangeExtension(fullname, "pkey");

            m_blockWriter = new BinaryWriter(File.Open(blockName, FileMode.Create));
            m_flowWriter = new BinaryWriter(File.Open(flowName, FileMode.Create));
            m_keyWriter = new BinaryWriter(File.Open(keyName, FileMode.Create));

            m_blockWriter.Write(BlockHeader);
            m_flowWriter.Write(FlowHeader);
            m_keyWriter.Write(KeyHeader);

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

        void WritePacketBlock(PacketBlock block)
        {
            lock (_sync)
            {
                if (!m_flowDictionary.TryGetValue(block.Key, out IndexRecord value))
                {
                    m_flowDictionary[block.Key] = value = new IndexRecord();
                }
                var pos = m_blockWriter.BaseStream.Position;
                value.PmapOffsetList.Add((int)pos);
            }
            m_blockWriter.Write(block.DataBytes);
        }
        void WriteFlowRecord(FlowRecord flow)
        {
            lock (_sync)
            {
                if (!m_flowDictionary.TryGetValue(flow.Key, out IndexRecord value))
                {
                    m_flowDictionary[flow.Key] = value = new IndexRecord();
                }
                var pos = m_blockWriter.BaseStream.Position;
                value.PfixOffset = (int)pos;
            }
            m_flowWriter.Write(flow.DataBytes);
        }

        FileConsumer()
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
            m_keyWriter.Close();
            m_blockWriter.Close();
            m_flowWriter.Close();
        }

        private void WriteKeyTable()
        {
            lock (_sync)
            {
                foreach (var item in m_flowDictionary)
                {
                    m_keyWriter.Write(item.Key.GetBytes());
                    var contentBytes = item.Value.GetBytes();
                    m_keyWriter.Write(contentBytes.Length);
                    m_keyWriter.Write(contentBytes);
                }
            }
        }

        public void Dispose()
        {
            m_keyWriter.Dispose();
            m_blockWriter.Dispose();
            m_flowWriter.Dispose();
        }

        public ITargetBlock<PacketBlock> PacketBlockTarget => m_packetBlockTarget;

        public ITargetBlock<FlowRecord> FlowRecordTarget => m_flowRecordTarget;

        public ITargetBlock<RawFrame> RawFrameTarget => m_rawFrameTarget;

        public int PacketBlockCount => m_packetBlockCount;

        public int FlowRecordCount => m_flowRecordCount;

        public int RawFrameCount => m_rawframeCount;

        private unsafe class IndexRecord
        {
            public int PfixOffset;
            public List<int> PmapOffsetList;
            internal IndexRecord()
            {
                PmapOffsetList = new List<int>();
            }
            public byte[] GetBytes()
            {
                var buffer = new byte[sizeof(int)*(PmapOffsetList.Count + 1)];
                fixed(byte *ptr = buffer)
                {
                    int* intPtr = (int*)ptr;
                    intPtr[0] = PfixOffset;                    
                    for(int i=0; i<PmapOffsetList.Count; i++)
                    {
                        intPtr[i + 1] = PmapOffsetList[i];
                    }
                }
                return buffer;
            }
        }
    }
}
