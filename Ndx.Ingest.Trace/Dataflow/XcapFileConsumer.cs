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

namespace Ndx.Ingest.Trace
{

    /// <summary>
    /// This consumer writes information in the metacap file, which is a zipped filed 
    /// containting all index files.
    /// </summary>
    /// <remarks>
    /// This class offers three consumers that can be linked to dataflow pipeline.
    /// 
    /// </remarks>
    public class XcapFileConsumer : IDisposable
    {
        private object m_sync = new object();

        private ZipArchive m_archive;
        
        Task m_completitionTask;

        int m_packetBlockCount;
        ActionBlock<PacketBlock> m_packetBlockTarget;

        int m_flowRecordCount;
        ActionBlock<FlowRecord> m_flowRecordTarget;

        int m_rawframeCount;
        ActionBlock<RawFrame> m_rawFrameTarget;

        int m_conversationId;

        private Dictionary<FlowKey, IndexRecord> m_flowDictionary;
        private Dictionary<FlowKey, int> m_conversationDictionary;

        private PacketBlock.BinaryConverter m_packetBlockConverter = new PacketBlock.BinaryConverter();
        FlowRecord.BinaryConverter m_flowRecordConverter = new FlowRecord.BinaryConverter();

        /// <summary>
        /// Creates new Consumer that produced xcap file as its output.
        /// </summary>
        /// <param name="xcapfile">Name of the xcap file to be created.</param>
        public XcapFileConsumer(string xcapfile) : this()
        {
            var mcapPath = Path.GetFullPath(xcapfile);

            // delete if exists:
            if (File.Exists(mcapPath))
            {
                File.Delete(mcapPath);
            }

            m_archive = ZipFile.Open(mcapPath, ZipArchiveMode.Create);
            m_flowDictionary = new Dictionary<FlowKey, IndexRecord>(1024);
            m_conversationDictionary = new Dictionary<FlowKey, int>(1024);
        }


        XcapFileConsumer()
        {
            m_packetBlockTarget = new ActionBlock<PacketBlock>(x => { WritePacketBlock(x, Interlocked.Increment(ref m_packetBlockCount)); });
            m_flowRecordTarget = new ActionBlock<FlowRecord>(x => { WriteFlowRecord(x, Interlocked.Increment(ref m_flowRecordCount)); });
            m_rawFrameTarget = new ActionBlock<RawFrame>(x => { WriteFrame(x, Interlocked.Increment(ref m_rawframeCount)); });
            m_completitionTask = Task.WhenAll(m_packetBlockTarget.Completion, m_flowRecordTarget.Completion, m_rawFrameTarget.Completion).ContinueWith((t) => FinishWriting());
        }

        private void WriteFrame(RawFrame x, int v)
        {
            lock (m_sync)
            {
                var path = MetacapFileInfo.GetFramePath(x.Meta.FrameNumber);
                var blockEntry = m_archive.CreateEntry(path, CompressionLevel.Fastest);
                using (var writer = new BinaryWriter(blockEntry.Open()))
                {
                    writer.Write(x.RawFrameData);
                }
            }
        }

        void WritePacketBlock(PacketBlock block, int index)
        {
            lock (m_sync)
            {
                if (!m_flowDictionary.TryGetValue(block.Key, out IndexRecord value))
                {
                    m_flowDictionary[block.Key] = value = new IndexRecord();
                }
                value.PacketBlockList.Add(index);

                var path = MetacapFileInfo.GetPacketBlockPath(index);
                var blockEntry = m_archive.CreateEntry(path, CompressionLevel.Fastest);
                using (var writer = new BinaryWriter(blockEntry.Open()))
                {
                    m_packetBlockConverter.WriteObject(writer, block);
                }
            }
        }

        void WriteFlowRecord(FlowRecord flow, int index)
        {
            lock (m_sync)
            {
                if (!m_flowDictionary.TryGetValue(flow.Key, out IndexRecord value))
                {
                    m_flowDictionary[flow.Key] = value = new IndexRecord();

                };
                value.FlowRecordIndex = index;

                if (flow.EndpointType == FlowEndpointType.Originator)
                {
                    m_conversationDictionary.Add(flow.Key, ++m_conversationId);
                }

                var path = MetacapFileInfo.GetFlowRecordPath(index);
                var entry = m_archive.CreateEntry(path, CompressionLevel.Fastest);
                using (var writer = new BinaryWriter(entry.Open()))
                {
                    m_flowRecordConverter.WriteObject(writer, flow);
                }
            }
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
            WriteKeyTable();
            WriteConversationTable();
            // to write all entries, we need to call this explicitly here:
            m_archive.Dispose();
            return Task.FromResult(0);
        }

        private void WriteKeyTable()
        {
            lock (m_sync)
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
        }

        private void WriteConversationTable()
        {
            lock (m_sync)
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
        /// If this dataflow block is not connected then you must call <see cref="IDataflowBlock.Complete()"/> method 
        /// otherwise completion task never finishes.
        /// </summary>
        public ITargetBlock<PacketBlock> PacketBlockTarget => m_packetBlockTarget;

        /// <summary>
        /// Gets target dataflow block that represents a consumer of <see cref="FlowRecord"/> objects.
        /// If this dataflow block is not connected then you must call <see cref="IDataflowBlock.Complete()"/> method 
        /// otherwise completion task never finishes.
        /// </summary>
        public ITargetBlock<FlowRecord> FlowRecordTarget => m_flowRecordTarget;

        /// <summary>
        /// Gets target dataflow block that represents a consumer of <see cref="RawFrame"/> objects.
        /// If this dataflow block is not connected then you must call <see cref="IDataflowBlock.Complete()"/> method 
        /// otherwise completion task never finishes.
        /// </summary>
        public ITargetBlock<RawFrame> RawFrameTarget => m_rawFrameTarget;

        /// <summary>
        /// Gets the number of <see cref="PacketBlock"/> objects processed by the current dataflow blocks.
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
