//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Ndx.Metacap
{
    public class PcapFileIngestor
    {
        private readonly CancellationTokenSource m_cancellationTokenSource;
        private readonly ITargetBlock<ConversationElement<PacketBlock>> m_blockConsumer;
        private readonly ITargetBlock<ConversationElement<FlowRecord>> m_flowConsumer;
        private readonly ITargetBlock<RawFrame> m_frameConsumer;
        private readonly ConversationCollector m_collector;
        private readonly MetadataExtractor m_extractor;
        private readonly ISourceBlock<RawFrame> m_frameProvider;

        /// <summary>
        /// Creates a new instances of PcapIngestor based on the provided Dataflow blocks.
        /// </summary>
        /// <param name="frameProvider">Dataflow source block of <see cref="RawFrame"/> objects.</param>
        /// <param name="frameConsumer">Dataflow target block of <see cref="RawFrame"/> objects.</param>
        /// <param name="packetBlockConsumer">Dataflow target block of <see cref="PacketBlock"/> objects.</param>
        /// <param name="flowConsumer">Dataflow target block of <see cref="FlowRecord"/> objects.</param>
        /// <param name="opt">Ingest options.</param>
        public PcapFileIngestor(ISourceBlock<RawFrame> frameProvider, ITargetBlock<RawFrame> frameConsumer, ITargetBlock<ConversationElement<PacketBlock>> packetBlockConsumer, ITargetBlock<ConversationElement<FlowRecord>> flowConsumer, IngestOptions opt)
        {
            m_frameProvider = frameProvider;
            m_blockConsumer = packetBlockConsumer;
            m_flowConsumer = flowConsumer;
            m_frameConsumer = frameConsumer;

            m_cancellationTokenSource = new CancellationTokenSource();
            m_collector = new ConversationCollector(opt.CollectorCapacity, m_cancellationTokenSource.Token);
            m_extractor = new MetadataExtractor(opt.ExtractorCapacity, opt.FlowFilter, m_cancellationTokenSource.Token);
            // setup dataflow pipeline
            //
            //            RawFrame                        PacketMetadata               PacketBlock
            //    INPUT =============[ ]=====> extractor ==============> collector |==============> packetBlockConsumer
            //               (1)      |  (2)                   (4)                 |      (5)     
            //                        |                                            |
            //                        |                                            |   FlowRecord
            //                        |                                            |==============> flowConsumer
            //                        |                                                   (6)
            //                        |
            //                        |                                                RawFrame
            //                        |===========================================================> frameConsumer
            //                                (3)

            // linking blocks
            var propagationOption = new DataflowLinkOptions { PropagateCompletion = true };

            if (m_frameConsumer != null)
            {
                var broadcastBlock = new BroadcastBlock<RawFrame>(x => x);
                // L(1)
                m_frameProvider.LinkTo(broadcastBlock, propagationOption);
                // L(2)
                broadcastBlock.LinkTo(m_extractor.RawFrameTarget, propagationOption);
                // L(3)
                broadcastBlock.LinkTo(m_frameConsumer, propagationOption);
            }
            else
            {
                // L(1) --[]--> L(2)
                m_frameProvider.LinkTo(m_extractor.RawFrameTarget, propagationOption);
            }
            // L(4)
            m_extractor.PacketMetadataSource.LinkTo(m_collector.PacketMetadataTarget, propagationOption, x => x != null);
            m_extractor.PacketMetadataSource.LinkTo(DataflowBlock.NullTarget<PacketMetadata>());
            // L(5)
            m_collector.PacketBlockSource.LinkTo(m_blockConsumer, propagationOption);
            // L(6)
            m_collector.FlowRecordSource.LinkTo(m_flowConsumer, propagationOption);
        }

        public void Cancel()
        {
            m_cancellationTokenSource.Cancel();
        }

        public void Complete()
        {
            m_extractor.RawFrameTarget.Complete();
        }

        public Task Completion => m_collector.Completion;
    }
}
