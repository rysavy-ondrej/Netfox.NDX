//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Model;
namespace Ndx.Metacap
{
    /// <summary>
    /// The class implements a conversation collector.It consumes <see cref="PacketMetadata"/> by <see cref="ConversationCollector.PacketUnitTarget"/>
    /// and generates <see cref="PacketBlock"/> objects available from <see cref="ConversationCollector.PacketBlockSource"/>
    /// and <see cref="FlowRecord"/> objects each for both conversation directions available from <see cref="ConversationCollector.FlowRecordSource"/>.
    /// </summary>
    public sealed class ConversationCollector
    {
        /// <summary>
        /// This <see cref="ActionBlock{RawFrame}"/> performs Collect action.
        /// </summary>
        private ActionBlock<RawFrame> m_actionBlock;

        /// <summary>
        /// Manages a collection of <see cref="FlowTracker"/> objects. Each tracker 
        /// collects information about a single flow.
        /// </summary>
        private ConcurrentDictionary<FlowKey, FlowTracker> m_conversationDictionary;

        /// <summary>
        /// Output buffer that stores <see cref="PacketBlock"/> objects.
        /// </summary>
        private BufferBlock<KeyValuePair<int,PacketBlock>> m_packetBlockBuffer;
        /// <summary>
        /// Output buffer that stores <see cref="FlowRecord"/> objects.
        /// </summary>
        private BufferBlock<KeyValuePair<int, Conversation>> m_conversationBuffer;

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

            m_packetBlockBuffer = new BufferBlock<KeyValuePair<int,PacketBlock>>(opt);

            m_conversationBuffer = new BufferBlock<KeyValuePair<int,Conversation>>(opt);

            m_actionBlock = new ActionBlock<RawFrame>(CollectAsync, opt);

            m_actionBlock.Completion.ContinueWith(async delegate
            {
                foreach (var item in m_conversationDictionary)
                {
                    await m_conversationBuffer.SendAsync(item);

                    item.Value.PacketMetadataTarget.Complete();

                    await item.Value.Completion;
                }

                m_flowRecordBuffer.Complete();
                m_packetBlockBuffer.Complete();
            });

            m_conversationDictionary = new ConcurrentDictionary<FlowKey, FlowTracker>();

        }
        private object m_sync = new object();

        //
        // 
        //     m_actionBlock  ----------> update flow record
        //
        //                    ----------> 
        //
        //
        async Task CollectAsync(RawFrame frame)
        {
            /*
            try
            {
                var flowKey = metadata.Key;
                if (!m_conversationDictionary.TryGetValue(flowKey, out FlowTracker value))
                {
                    // This is a very simple way of composing conversations...
                    if (m_conversationDictionary.TryGetValue(FlowCollector.SwapFlowKey(flowKey), out FlowTracker complementaryFlow))
                    {
                        m_conversationDictionary[flowKey] = value = new FlowTracker(flowKey, complementaryFlow.ConversationId, FlowOrientation.Downflow);
                        value.FlowRecord.Orientation = FlowOrientation.Downflow;
                    }
                    else
                    {
                        m_conversationDictionary[flowKey] = value = new FlowTracker(flowKey, Guid.NewGuid(), FlowOrientation.Upflow);
                        value.FlowRecord.Orientation = FlowOrientation.Upflow;
                    }

                    value.PacketBlockSource.LinkTo(m_packetBlockBuffer);
                }
                value.FlowRecord.UpdateWith(metadata.Value);
                await value.PacketMetadataTarget.SendAsync(metadata.Value);
            }
            catch (Exception e)
            {
                m_logger.Error(e, "Collect Async cannot process input packet metadata.");
            }
            */
        }

        /// <summary>
        /// Gets a <see cref="Task"/> object that represents an asynchronous operation and completition of the <see cref="FlowCollector"/> block.
        /// </summary>
        public Task Completion => Task.WhenAll(m_actionBlock.Completion, m_flowRecordBuffer.Completion, m_packetBlockBuffer.Completion);

        /// <summary>
        /// Use this target dataflow block to send <see cref="PacketMetadata"/> objects 
        /// for their processing in the collector.
        /// </summary>
        public ITargetBlock<KeyValuePair<FlowKey, PacketUnit>> PacketUnitTarget => m_actionBlock;

        /// <summary>
        /// Use this source dataflow block to acquire <see cref="PacketBlock"/> objects
        /// produced by the collector.
        /// </summary>
        public ISourceBlock<ConversationElement<KeyValuePair<FlowKey,PacketBlock>>> PacketBlockSource => m_packetBlockBuffer;

        /// <summary>
        /// Use this source dataflow block to acquire <see cref="FlowRecord"/> objects
        /// produced by the collector.
        /// </summary>
        public ISourceBlock<ConversationElement<KeyValuePair<FlowKey, FlowRecord>>> FlowRecordSource => m_flowRecordBuffer;

        /// <summary>
        /// Gets an enumerable collection of all <see cref="FlowKey"/> items
        /// stored with the current collector.
        /// </summary>
        public IEnumerable<FlowKey> FlowKeys => m_conversationDictionary.Keys;    
    }
}