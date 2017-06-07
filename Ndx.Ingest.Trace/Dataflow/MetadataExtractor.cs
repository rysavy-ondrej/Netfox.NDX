using PacketDotNet;
using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;
using Ndx.Model;
using System.Collections.Generic;

namespace Ndx.Metacap
{
    public sealed class MetadataExtractor 
    {
        IPropagatorBlock<RawFrame, KeyValuePair<FlowKey,PacketUnit>> m_transformBlock;
        private readonly Func<FlowKey, bool> m_filter;

        public MetadataExtractor(int boundedCapacity, CancellationToken cancellationToken)
        {
            var opt = new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = boundedCapacity,
                CancellationToken = cancellationToken,
            };
            m_filter = null;
            m_transformBlock = new TransformBlock<RawFrame, KeyValuePair<FlowKey,PacketUnit>>((Func<RawFrame, KeyValuePair<FlowKey,PacketUnit>>)Transform, opt);            
        }

        public MetadataExtractor(int boundedCapacity, Func<FlowKey,bool> filter, CancellationToken cancellationToken)
        {
            var opt = new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = boundedCapacity,
                CancellationToken = cancellationToken,
            };
            m_filter = filter;

            KeyValuePair<FlowKey,PacketUnit> transformAndFilter(RawFrame arg)
            {
                var val = Transform(arg);
                if (val.Key == null || val.Value == null) return new KeyValuePair<FlowKey, PacketUnit>(null, null);
                if (m_filter != null && m_filter(val.Key) == false) return new KeyValuePair<FlowKey,PacketUnit>(null,null);                   
                return val;
            }

            m_transformBlock = new TransformBlock<RawFrame, KeyValuePair<FlowKey,PacketUnit>>((Func<RawFrame, KeyValuePair<FlowKey,PacketUnit>>)transformAndFilter, opt);
        }
        KeyValuePair<FlowKey,PacketUnit> Transform(RawFrame frame)
        {
            try
            {
                var packet = Packet.ParsePacket(frame.Meta.LinkLayer, frame.RawFrameData);
                var unit = new KeyValuePair<FlowKey,PacketUnit>(new FlowKey(), new PacketUnit());
                var visitor = new PacketVisitorImpl(unit);
                
                packet?.Accept(visitor);
                
                return unit;

            }
            catch (Exception)
            {
                return new KeyValuePair<FlowKey, PacketUnit>(null, null);
            }                        
        }

        public ITargetBlock<RawFrame> RawFrameTarget => m_transformBlock;
        public ISourceBlock<KeyValuePair<FlowKey,PacketUnit>> PacketSource => m_transformBlock;
    }
}
