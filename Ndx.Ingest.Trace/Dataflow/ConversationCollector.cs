//  
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
        private static readonly NLog.Logger m_logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// This <see cref="ActionBlock{TInput}"/> performs Collect action.
        /// </summary>
        private ActionBlock<KeyValuePair<FlowKey,PacketUnit>> m_actionBlock;

        /// <summary>
        /// Manages a collection of <see cref="FlowTracker"/> objects. Each tracker 
        /// collects information about a single flow.
        /// </summary>
        private ConcurrentDictionary<FlowKey, FlowTracker> m_flowDictionary;

        /// <summary>
        /// Output buffer that stores <see cref="PacketBlock"/> objects.
        /// </summary>
        private BufferBlock<ConversationElement<KeyValuePair<FlowKey,PacketBlock>>> m_packetBlockBuffer;
        /// <summary>
        /// Output buffer that stores <see cref="FlowRecord"/> objects.
        /// </summary>
        private BufferBlock<ConversationElement<KeyValuePair<FlowKey, FlowRecord>>> m_flowRecordBuffer;

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

            m_packetBlockBuffer = new BufferBlock<ConversationElement<KeyValuePair<FlowKey,PacketBlock>>>(opt);

            m_flowRecordBuffer = new BufferBlock<ConversationElement<KeyValuePair<FlowKey,FlowRecord>>>(opt);

            m_actionBlock = new ActionBlock<KeyValuePair<FlowKey, PacketUnit>>(CollectAsync, opt);

            m_actionBlock.Completion.ContinueWith(async delegate
            {
                foreach (var item in m_flowDictionary)
                {
                    await m_flowRecordBuffer.SendAsync(new ConversationElement<KeyValuePair<FlowKey, FlowRecord>>(item.Value.ConversationId, item.Value.FlowRecord.Orientation, new KeyValuePair<FlowKey, FlowRecord>(item.Key, item.Value.FlowRecord)));

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
        async Task CollectAsync(KeyValuePair<FlowKey, PacketUnit> metadata)
        {
            try
            {
                var flowKey = metadata.Key;
                if (!m_flowDictionary.TryGetValue(flowKey, out FlowTracker value))
                {
                    // This is a very simple way of composing conversations...
                    if (m_flowDictionary.TryGetValue(FlowCollector.SwapFlowKey(flowKey), out FlowTracker complementaryFlow))
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
                value.FlowRecord.UpdateWith(metadata.Value);
                await value.PacketMetadataTarget.SendAsync(metadata.Value);
            }
            catch (Exception e)
            {
                m_logger.Error(e, "Collect Async cannot process input packet metadata.");
            }
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
        public IEnumerable<FlowKey> FlowKeys => m_flowDictionary.Keys;    
    }
}