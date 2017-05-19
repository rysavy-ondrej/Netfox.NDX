using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Ndx.Ingest.Trace
{
    /// <summary>
    /// This class represents a consumer that drops any data consumed.
    /// It only counts the statistic about the consumed items.
    /// </summary>
    public class NullConsumer
    {
        int m_packetBlockCount;
        ActionBlock<Tuple<Guid, PacketBlock>> m_packetBlockTarget;

        int m_flowRecordCount;
        ActionBlock<Tuple<Guid, FlowRecord>> m_flowRecordTarget;

        int m_rawframeCount;
        ActionBlock<RawFrame> m_rawFrameTarget;

        public NullConsumer()
        {
            m_packetBlockTarget = new ActionBlock<Tuple<Guid, PacketBlock>>(x => m_packetBlockCount++);
            m_flowRecordTarget = new ActionBlock<Tuple<Guid, FlowRecord>>(x => m_flowRecordCount++);
            m_rawFrameTarget = new ActionBlock<RawFrame>(x => m_rawframeCount++);
        }

        public ITargetBlock<Tuple<Guid, PacketBlock>> PacketBlockTarget => m_packetBlockTarget;

        public ITargetBlock<Tuple<Guid, FlowRecord>> FlowRecordTarget => m_flowRecordTarget;

        public ITargetBlock<RawFrame> RawFrameTarget => m_rawFrameTarget;

        public int PacketBlockCount => m_packetBlockCount;

        public int FlowRecordCount => m_flowRecordCount;

        public int RawFrameCount => m_rawframeCount;

        public Task Completion
        {
            get
            {
                return Task.WhenAll(m_packetBlockTarget.Completion, m_flowRecordTarget.Completion);
            }
        }
    }
}
