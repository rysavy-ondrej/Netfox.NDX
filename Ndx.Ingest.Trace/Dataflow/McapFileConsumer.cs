//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Linq;

namespace Ndx.Ingest.Trace
{

    public class ConversationElement<T>
    {
        Guid m_conversationId;
        FlowOrientation m_orientation;
        T m_data;

        public Guid ConversationId { get => m_conversationId; set => m_conversationId = value; }
        public FlowOrientation Orientation { get => m_orientation; set => m_orientation = value; }
        public T Data { get => m_data; set => m_data = value; }

        public ConversationElement(Guid id, FlowOrientation orientation, T data)
        {
            m_conversationId = id;
            m_orientation = orientation;
            m_data = data;
        }
    }

    /// <summary>
    /// This consumer writes information in the metacap file, which is a zipped filed 
    /// containting all index files.
    /// </summary>
    /// <remarks>
    /// This class offers three consumers that can be linked to dataflow pipeline.
    /// 
    /// </remarks>
    public class McapFileConsumer : IDisposable
    {
        private object m_sync = new object();

        private ZipArchive m_archive;
        public Task m_completitionTask;

        ActionBlock<ConversationElement<PacketBlock>> m_packetBlockTarget;

        ActionBlock<ConversationElement<FlowRecord>> m_flowRecordTarget;

        int m_rawframeCount;
        ActionBlock<RawFrame> m_rawFrameTarget;

        private PacketBlock.BinaryConverter m_packetBlockConverter = new PacketBlock.BinaryConverter();
        FlowRecord.BinaryConverter m_flowRecordConverter = new FlowRecord.BinaryConverter();
        FlowKey.BinaryConverter m_flowKeyConverter = new FlowKey.BinaryConverter();
        private int m_packetBlockCount;
        private int m_flowRecordCount;

        /// <summary>
        /// Creates new Consumer that produced metacap file as its output.
        /// </summary>
        /// <param name="mcapfile">Name of the mcap file to be created.</param>
        public McapFileConsumer(string mcapfile) : this()
        {
            var mcapPath = Path.GetFullPath(mcapfile);

            // delete if exists:
            if (File.Exists(mcapPath))
            {
                File.Delete(mcapPath);
            }

            m_archive = ZipFile.Open(mcapPath, ZipArchiveMode.Create);
        }

        void WritePacketBlock(ConversationElement<PacketBlock> block)
        {
            lock (m_sync)
            {
                m_packetBlockCount++;
                var path = MetacapFileInfo.GetPacketBlockPath(block.ConversationId, block.Orientation, block.Data.BlockIndex);
                var blockEntry = m_archive.CreateEntry(path, CompressionLevel.Fastest);
                using (var writer = new BinaryWriter(blockEntry.Open()))
                {
                    m_packetBlockConverter.WriteObject(writer, block.Data);
                }
            }
        }

        void WriteFlowRecord(ConversationElement<FlowRecord> flow)
        {
            lock (m_sync)
            {
                m_flowRecordCount++;
                var pathRecord = MetacapFileInfo.GetFlowRecordPath(flow.ConversationId, flow.Orientation);
                var entryRecord = m_archive.CreateEntry(pathRecord, CompressionLevel.Fastest);
                using (var writer = new BinaryWriter(entryRecord.Open()))
                {
                    m_flowRecordConverter.WriteObject(writer, flow.Data);
                }

                var pathKey = MetacapFileInfo.GetFlowKeyPath(flow.ConversationId, flow.Orientation);
                var entryKey = m_archive.CreateEntry(pathKey, CompressionLevel.Fastest);
                using (var writer = new BinaryWriter(entryKey.Open()))
                {
                    m_flowKeyConverter.WriteObject(writer, flow.Data.Key);
                }
            }
        }

        McapFileConsumer()
        {
            m_packetBlockTarget = new ActionBlock<ConversationElement<PacketBlock>>(x=>WritePacketBlock(x));
            m_flowRecordTarget = new ActionBlock<ConversationElement<FlowRecord>>(x=>WriteFlowRecord(x));
            m_rawFrameTarget = new ActionBlock<RawFrame>(x => m_rawframeCount++);
            m_completitionTask = Task.WhenAll(m_packetBlockTarget.Completion, m_flowRecordTarget.Completion, m_rawFrameTarget.Completion).ContinueWith((t) => FinishWriting());
        }

        /// <summary>
        /// Gets the <see cref="Task"/> that completes when the current object finishes all writing operations to the metacap file.
        /// Do not forget to call <see cref="IDataflowBlock.Complete()"/> for all disconnected dataflow targets, otherwise completition <see cref="Task"/>
        /// never completes.
        /// </summary>
        public Task Completion => m_completitionTask;

        /// <summary>
        /// Called when all consumers completes. Key and conversation tables are written in this method.
        /// </summary>
        /// <returns></returns>
        private Task FinishWriting()
        {
            m_archive.Dispose();
            return Task.FromResult(0);
        }

        /// <summary>
        /// Releases all resources owned by the current object.
        /// </summary>
        public void Dispose()
        {
            ((IDisposable)m_archive).Dispose();
        }


        /// <summary>
        /// Gets target dataflow block that represents a consumer of <see cref="PacketBlock"/> objects.
        /// If this dataflow block is not csee cref="IDataflowBlock.Complete()"/> method 
        /// otherwise completion task never finishes.
        /// </summary>
        public ITargetBlock<ConversationElement<PacketBlock>> PacketBlockTarget => m_packetBlockTarget;

        /// <summary>
        /// Gets target dataflow block that represents a consumer of <see cref="FlowRecord"/> objects.
        /// If this dataflow block is not connected then you must call <see cref=" => ataf => wBlock.Complete()"/> method 
        /// otherwise completion task never finishes.
        /// </summary>
        public ITargetBlock<ConversationElement<FlowRecord>> FlowRecordTarget => m_flowRecordTarget;

        /// <summary>
        /// Gets target dataflow block that represents a consumer of <see cref="RawFrame"/> objects.
        /// If this dataflow block is not connected then you must call <see cref="IDataflowBlock.Complete()"/> method 
        /// otherwise completion task never finishes.
        /// </summary>
        public ITargetBlock<RawFrame> RawFrameTarget => m_rawFrameTarget;

        /// <summary>
        /// Gets the number of <see cref="PacketBlock" =>  o => ects processed by the current dataflow blocks.
        /// </summary>
        public int PacketBlockCount => m_packetBlockCount;

        /// <summary>
        /// Gets the number of <see cref="FlowRecord"/> objects processed by the current dataflow blocks.
        /// </summary>
        public int FlowRecordCount => m_flowRecordCount;

        /// <summary>
        /// Gets the number of <see cref="RawFrame"/> objects processed by the current dataflow blocks.
        /// </summary>
        public int RawFrameCount => m_rawframeCount;
    }
}
