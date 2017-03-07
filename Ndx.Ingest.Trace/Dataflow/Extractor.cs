using Ndx.Network;
using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Ndx.Ingest.Trace.Dataflow
{
    public sealed class Extractor 
    {
        TransformBlock<RawFrame, PacketMetadata> m_transformBlock;

        public Extractor(int boundedCapacity, CancellationToken cancellationToken)
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
