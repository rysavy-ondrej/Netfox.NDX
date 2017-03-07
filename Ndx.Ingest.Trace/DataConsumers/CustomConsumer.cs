using Ndx.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Ndx.Ingest.Trace.DataConsumers
{
    class CustomConsumer
    {
        int m_packetBlockCount;
        ActionBlock<PacketBlock> m_packetBlockTarget;

        int m_flowRecordCount;
        ActionBlock<FlowRecord> m_flowRecordTarget;

        int m_rawframeCount;
        ActionBlock<RawFrame> m_rawFrameTarget;


        public event EventHandler<PacketBlock> PacketBlockReceived;
        public event EventHandler<FlowRecord> FlowRecordReceived;
        public event EventHandler<RawFrame> RawFrameReceived;

        public CustomConsumer()
        {
            m_packetBlockTarget = new ActionBlock<PacketBlock>(x =>
            {
                m_packetBlockCount++;
                PacketBlockReceived?.Invoke(this, x);
            });
            m_flowRecordTarget = new ActionBlock<FlowRecord>(x => 
            {
                m_flowRecordCount++;
                FlowRecordReceived?.Invoke(this, x);
            });
            m_rawFrameTarget = new ActionBlock<RawFrame>(x =>
            {
                m_rawframeCount++;
                RawFrameReceived?.Invoke(this, x);
            });
        }

        public ITargetBlock<PacketBlock> PacketBlockTarget => m_packetBlockTarget;

        public ITargetBlock<FlowRecord> FlowRecordTarget => m_flowRecordTarget;

        public ITargetBlock<RawFrame> RawFrameTarget => m_rawFrameTarget;

        public int PacketBlockCount => m_packetBlockCount;

        public int FlowRecordCount => m_flowRecordCount;

        public int RawFrameCount => m_rawframeCount;
    }
}
