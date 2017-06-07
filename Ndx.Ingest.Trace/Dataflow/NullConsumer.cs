using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Model;
namespace Ndx.Metacap
{
    /// <summary>
    /// This class represents a consumer that drops any data consumed.
    /// It only counts the statistic about the consumed items.
    /// </summary>
    public class NullConsumer
    {
        int m_packetBlockCount;
        ActionBlock<ConversationElement<KeyValuePair<FlowKey,PacketBlock>>> m_packetBlockTarget;

        int m_flowRecordCount;
        ActionBlock<ConversationElement<KeyValuePair<FlowKey,FlowRecord>>> m_flowRecordTarget;

        int m_rawframeCount;
        ActionBlock<RawFrame> m_rawFrameTarget;

        public NullConsumer()
        {
            m_packetBlockTarget = new ActionBlock<ConversationElement<KeyValuePair<FlowKey,PacketBlock>>>(x => m_packetBlockCount++);
            m_flowRecordTarget = new ActionBlock<ConversationElement<KeyValuePair<FlowKey,FlowRecord>>>(x => m_flowRecordCount++);
            m_rawFrameTarget = new ActionBlock<RawFrame>(x => m_rawframeCount++);
        }

        public ITargetBlock<ConversationElement<KeyValuePair<FlowKey,PacketBlock>>> PacketBlockTarget => m_packetBlockTarget;

        public ITargetBlock<ConversationElement<KeyValuePair<FlowKey, FlowRecord>>> FlowRecordTarget => m_flowRecordTarget;

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
