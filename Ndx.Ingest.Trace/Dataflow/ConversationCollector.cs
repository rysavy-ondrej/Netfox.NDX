﻿//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Collections.Concurrent;
using NLog;
using System.Threading;

namespace Ndx.Ingest.Trace
{
    /// <summary>
    /// The class implements a conversation collector.It consumes <see cref="PacketMetadata"/> by <see cref="ConversationCollector.PacketMetadataTarget"/>
    /// and generates <see cref="PacketBlock"/> objects available from <see cref="ConversationCollector.PacketBlockSource"/>
    /// and <see cref="FlowRecord"/> objects each for both conversation directions available from <see cref="ConversationCollector.FlowRecordSource"/>.
    /// </summary>
    public sealed class ConversationCollector
    {
        private static readonly NLog.Logger m_logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// This <see cref="ActionBlock{TInput}"/> performs Collect action.
        /// </summary>
        private ActionBlock<PacketMetadata> m_actionBlock;

        /// <summary>
        /// Manages a collection of <see cref="FlowTracker"/> objects. Each tracker 
        /// collects information about a single flow.
        /// </summary>
        private ConcurrentDictionary<FlowKey, FlowTracker> m_flowDictionary;

        /// <summary>
        /// Output buffer that stores <see cref="PacketBlock"/> objects.
        /// </summary>
        private BufferBlock<ConversationElement<PacketBlock>> m_packetBlockBuffer;
        /// <summary>
        /// Output buffer that stores <see cref="FlowRecord"/> objects.
        /// </summary>
        private BufferBlock<ConversationElement<FlowRecord>> m_flowRecordBuffer;

        /// <summary>
        /// Creates a new instance of <see cref="FlowCollector"/> block.
        /// </summary>
        /// <param name="boundedCapacity">Maximum number of messages that can be buffered in the block.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> for monitoring cancellation request.</param>
        public ConversationCollector(int boundedCapacity, CancellationToken cancellationToken)
        {
            var opt = new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = boundedCapacity,
                CancellationToken = cancellationToken
            };

            m_packetBlockBuffer = new BufferBlock<ConversationElement<PacketBlock>>(opt);

            m_flowRecordBuffer = new BufferBlock<ConversationElement<FlowRecord>>(opt);

            m_actionBlock = new ActionBlock<PacketMetadata>(CollectAsync, opt);

            m_actionBlock.Completion.ContinueWith(async delegate
            {
                foreach (var item in m_flowDictionary)
                {
                    await m_flowRecordBuffer.SendAsync(new ConversationElement<FlowRecord>(item.Value.ConversationId, item.Value.FlowRecord.Orientation, item.Value.FlowRecord));

                    item.Value.PacketMetadataTarget.Complete();

                    await item.Value.Completion;
                }

                m_flowRecordBuffer.Complete();
                m_packetBlockBuffer.Complete();
            });

            m_flowDictionary = new ConcurrentDictionary<FlowKey, FlowTracker>();

        }
        private object m_sync = new object();

        //
        // 
        //     m_actionBlock  ----------> update flow record
        //
        //                    ----------> 
        //
        //
        async Task CollectAsync(PacketMetadata metadata)
        {
            try
            {
                var flowKey = metadata.Flow;
                if (!m_flowDictionary.TryGetValue(flowKey, out FlowTracker value))
                {
                    // This is a very simple way of composing conversations...
                    if (m_flowDictionary.TryGetValue(SwapFlowKey(flowKey), out FlowTracker complementaryFlow))
                    {
                        m_flowDictionary[flowKey] = value = new FlowTracker(flowKey, complementaryFlow.ConversationId, FlowOrientation.Downflow);
                        value.FlowRecord.Orientation = FlowOrientation.Downflow;
                    }
                    else
                    {
                        m_flowDictionary[flowKey] = value = new FlowTracker(flowKey, Guid.NewGuid(), FlowOrientation.Upflow);
                        value.FlowRecord.Orientation = FlowOrientation.Upflow;
                    }

                    value.PacketBlockSource.LinkTo(m_packetBlockBuffer);
                }
                value.FlowRecord.UpdateWith(metadata);
                await value.PacketMetadataTarget.SendAsync(metadata);
            }
            catch (Exception e)
            {
                m_logger.Error(e, "Collect Async cannot process input packet metadata.");
            }
        }

        internal static FlowKey SwapFlowKey(FlowKey flowKey)
        {
            return new FlowKey(flowKey.AddressFamily, flowKey.Protocol, flowKey.DestinationAddress, flowKey.DestinationPort, flowKey.SourceAddress, flowKey.SourcePort);
        }

        /// <summary>
        /// Gets a <see cref="Task"/> object that represents an asynchronous operation and completition of the <see cref="FlowCollector"/> block.
        /// </summary>
        public Task Completion => Task.WhenAll(m_actionBlock.Completion, m_flowRecordBuffer.Completion, m_packetBlockBuffer.Completion);

        /// <summary>
        /// Use this target dataflow block to send <see cref="PacketMetadata"/> objects 
        /// for their processing in the collector.
        /// </summary>
        public ITargetBlock<PacketMetadata> PacketMetadataTarget => m_actionBlock;

        /// <summary>
        /// Use this source dataflow block to acquire <see cref="PacketBlock"/> objects
        /// produced by the collector.
        /// </summary>
        public ISourceBlock<ConversationElement<PacketBlock>> PacketBlockSource => m_packetBlockBuffer;

        /// <summary>
        /// Use this source dataflow block to acquire <see cref="FlowRecord"/> objects
        /// produced by the collector.
        /// </summary>
        public ISourceBlock<ConversationElement<FlowRecord>> FlowRecordSource => m_flowRecordBuffer;

        /// <summary>
        /// Gets an enumerable collection of all <see cref="FlowKey"/> items
        /// stored with the current collector.
        /// </summary>
        public IEnumerable<FlowKey> FlowKeys => m_flowDictionary.Keys;

        /// <summary>
        /// An instance of this class tracks a single flow object. It collects flow metadata 
        /// as well as Packet blocks. 
        /// </summary>
        class FlowTracker
        {
            static IPropagatorBlock<PacketMetadata, ConversationElement<PacketBlock>> CreateDataflowBlock(FlowKey flowKey, Guid conversationId, FlowOrientation orientation, Func<int> getIndex)
            {
                var target = new BatchBlock<PacketMetadata>(PacketBlock.Capacity); ;
                var source = new TransformBlock<PacketMetadata[], ConversationElement<PacketBlock>>(metadata => new ConversationElement<PacketBlock>(conversationId, orientation, new PacketBlock(flowKey, getIndex(), metadata)));
                target.LinkTo(source);
                target.Completion.ContinueWith(completion =>
                {
                    if (completion.IsFaulted)
                    {
                        ((IDataflowBlock)source).Fault(completion.Exception);
                    }
                    else
                    {
                        source.Complete();
                    }
                });

                // TransformBlock.Complete: After Complete has been called on a dataflow block, 
                // that block will complete, and its Completion task will enter a final state after 
                // it has processed all previously available data. 
                return DataflowBlock.Encapsulate(target, source);
            }

            /// <summary>
            /// The unique conversation identifier.
            /// </summary>
            Guid m_conversationId;

            /// <summary>
            /// Flow record associated with the current object.
            /// </summary>
            FlowRecord m_flowRecord;
            /// <summary>
            /// This dataflow block groups <see cref="PacketMetadata"/> objects and produces <see cref="PacketBlock"/>. 
            /// Each <see cref="PacketBlock"/> contains at most <see cref="PacketBlock.Capacity"/> <see cref="PacketMetadata"/> objects.
            /// </summary>
            IPropagatorBlock<PacketMetadata, ConversationElement<PacketBlock>> m_dataflowBlock;

            /// <summary>
            /// Gets the <see cref="Trace.FlowRecord"/> object.
            /// </summary>
            internal FlowRecord FlowRecord => m_flowRecord;
            /// <summary>
            /// Gets the target dataflow block that consumes <see cref="PacketMetadata"/>.
            /// </summary>
            internal ITargetBlock<PacketMetadata> PacketMetadataTarget => m_dataflowBlock;
            /// <summary>
            /// Gets the source dataflow block that produces <see cref="PacketBlock"/>. Link this source to
            /// process generated <see cref="PacketBlock"/> objects.
            /// </summary>
            internal ISourceBlock<ConversationElement<PacketBlock>> PacketBlockSource => m_dataflowBlock;

            internal FlowTracker(FlowKey flowKey, Guid conversationId, FlowOrientation orientation)
            {
                m_conversationId = conversationId;
                m_flowRecord = new FlowRecord(flowKey);
                m_dataflowBlock = CreateDataflowBlock(flowKey, m_conversationId, orientation, () => m_flowRecord.Packets / PacketBlock.Capacity);
            }

            internal Task Completion => m_dataflowBlock.Completion;

            internal Guid ConversationId { get => m_conversationId; set => m_conversationId = value; }
        }
    }
}
