using PacketDotNet;
using System;
using System.Threading;
using System.Threading.Tasks.Dataflow;

namespace Ndx.Ingest.Trace
{
    public sealed class MetadataExtractor 
    {
        TransformBlock<RawFrame, PacketMetadata> m_transformBlock;

        public MetadataExtractor(int boundedCapacity, CancellationToken cancellationToken)
        {
            var opt = new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = boundedCapacity,
                CancellationToken = cancellationToken,
            };

            m_transformBlock = new TransformBlock<RawFrame, PacketMetadata>((Func<RawFrame, PacketMetadata>)Transform, opt);            
        }
           
        PacketMetadata Transform(RawFrame frame)
        {
            try
            {
                var packet = Packet.ParsePacket(frame.Meta.LinkLayer, frame.RawFrameData);
                var visitor = new PacketVisitorImpl(frame.Meta.Data);

                packet?.Accept(visitor);
                return new PacketMetadata(visitor.FlowKey, visitor.Metadata);

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
