using PacketDotNet;
using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Ndx.Ingest.Trace
{
    public sealed class MetadataExtractor 
    {
        IPropagatorBlock<RawFrame, PacketMetadata> m_transformBlock;
        private readonly Func<FlowKey, bool> m_filter;

        public MetadataExtractor(int boundedCapacity, CancellationToken cancellationToken)
        {
            var opt = new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = boundedCapacity,
                CancellationToken = cancellationToken,
            };
            m_filter = null;
            m_transformBlock = new TransformBlock<RawFrame, PacketMetadata>((Func<RawFrame, PacketMetadata>)Transform, opt);            
        }

        public MetadataExtractor(int boundedCapacity, Func<FlowKey,bool> filter, CancellationToken cancellationToken)
        {
            var opt = new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = boundedCapacity,
                CancellationToken = cancellationToken,
            };
            m_filter = filter;

            PacketMetadata transformAndFilter(RawFrame arg)
            {
                var val = Transform(arg);
                if (val == null) return null;
                if (m_filter != null && m_filter(val.Flow) == false) return null;                   
                return val;
            }

            m_transformBlock = new TransformBlock<RawFrame, PacketMetadata>((Func<RawFrame, PacketMetadata>)transformAndFilter, opt);
        }
        PacketMetadata Transform(RawFrame frame)
        {
            try
            {
                var packet = Packet.ParsePacket(frame.Meta.LinkLayer, frame.RawFrameData);
                var packetMetadata = new PacketMetadata(frame.Meta);
                var visitor = new PacketVisitorImpl(packetMetadata);
                
                packet?.Accept(visitor);
                
                return packetMetadata;

            }
            catch (Exception)
            {
                return null;
            }                        
        }

        public ITargetBlock<RawFrame> RawFrameTarget => m_transformBlock;
        public ISourceBlock<PacketMetadata> PacketMetadataSource => m_transformBlock;
    }
}
