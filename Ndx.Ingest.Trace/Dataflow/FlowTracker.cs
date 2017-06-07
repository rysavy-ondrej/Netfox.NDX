//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Model;
namespace Ndx.Metacap
{
    /// <summary>
    /// An instance of this class tracks a single flow object. It collects flow metadata 
    /// as well as Packet blocks to be later propagated to respective consumers. 
    /// </summary>
    class FlowTracker
    {
        static IPropagatorBlock<PacketUnit, ConversationElement<KeyValuePair<FlowKey, PacketBlock>>> CreateDataflowBlock(FlowKey flowKey, Guid conversationId, FlowOrientation orientation, Func<int> getIndex)
        {
            var target = new BatchBlock<PacketUnit>(PacketBlock.Capacity); ;
            var source = new TransformBlock<PacketUnit[], ConversationElement<KeyValuePair<FlowKey, PacketBlock>>>(metadata =>
                new ConversationElement<KeyValuePair<FlowKey, PacketBlock>>(conversationId, orientation, new KeyValuePair<FlowKey, PacketBlock>(flowKey, new PacketBlock(getIndex(), metadata))));
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
        IPropagatorBlock<PacketUnit, ConversationElement<KeyValuePair<FlowKey, PacketBlock>>> m_dataflowBlock;

        /// <summary>
        /// Gets the <see cref="Trace.FlowRecord"/> object.
        /// </summary>
        internal FlowRecord FlowRecord => m_flowRecord;
        /// <summary>
        /// Gets the target dataflow block that consumes <see cref="PacketMetadata"/>.
        /// </summary>
        internal ITargetBlock<PacketUnit> PacketMetadataTarget => m_dataflowBlock;
        /// <summary>
        /// Gets the source dataflow block that produces <see cref="PacketBlock"/>. Link this source to
        /// process generated <see cref="PacketBlock"/> objects.
        /// </summary>
        internal ISourceBlock<ConversationElement<KeyValuePair<FlowKey, PacketBlock>>> PacketBlockSource => m_dataflowBlock;

        internal FlowTracker(FlowKey flowKey, Guid conversationId, FlowOrientation orientation)
        {
            m_conversationId = conversationId;
            m_flowRecord = new FlowRecord();
            m_dataflowBlock = CreateDataflowBlock(flowKey, m_conversationId, orientation, () => m_flowRecord.Packets / PacketBlock.Capacity);
        }

        public FlowTracker(FlowKey flowKey)
        {
            m_flowRecord = new FlowRecord();
            m_conversationId = Guid.Empty;
            m_dataflowBlock = CreateDataflowBlock(flowKey, m_conversationId, FlowOrientation.Upflow, () => m_flowRecord.Packets / PacketBlock.Capacity);
        }

        internal Task Completion => m_dataflowBlock.Completion;

        internal Guid ConversationId { get => m_conversationId; set => m_conversationId = value; }
    }
}