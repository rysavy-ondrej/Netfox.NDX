using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using PacketDotNet;
using PacketDotNet.Utils;

namespace Ndx.Metacap
{
    public class TcpStream
    {
        IPropagatorBlock<TcpPacket, ByteArraySegment> m_extractSegmentBlock;
        ITargetBlock<ByteArraySegment> m_writer;
        Stream m_stream;


        uint? m_initialSequenceNumber;
        uint? m_expectedSequenceNumber;
        ByteArraySegment Transform(TcpPacket packet)
        {
            var payload = packet.PayloadPacket.BytesHighPerformance;
            if (packet.Syn || m_initialSequenceNumber == null)
            {
                m_initialSequenceNumber = m_expectedSequenceNumber = packet.SequenceNumber;
            }

            //TODO: Implement a correct tcp reassembling code here:
            // see network monitor implementation

            m_expectedSequenceNumber += (uint)payload.Length;
            return payload;
        }


        void WriteSegment(ByteArraySegment segment)
        {
            m_stream.Write(segment.Bytes, segment.Offset, segment.Length);
        }

        public TcpStream(Stream stream, int boundedCapacity, CancellationToken cancellationToken)
        {
            m_stream = stream;
            var opt = new ExecutionDataflowBlockOptions()
            {
                BoundedCapacity = boundedCapacity,
                CancellationToken = cancellationToken,
            };
            m_extractSegmentBlock = new TransformBlock<TcpPacket, ByteArraySegment>((Func<TcpPacket, ByteArraySegment>)Transform, opt);
            m_writer = new ActionBlock<ByteArraySegment>((Action<ByteArraySegment>)WriteSegment);
            m_extractSegmentBlock.LinkTo(m_writer, new DataflowLinkOptions() { PropagateCompletion = true });
        }


        public ITargetBlock<TcpPacket> PacketsTarget => m_extractSegmentBlock;

        public Task Completion
        {
            get
            {
                return Task.WhenAll(m_extractSegmentBlock.Completion, m_writer.Completion);
            }
        }
    }
}
