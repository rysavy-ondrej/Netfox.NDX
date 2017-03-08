using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Ingest.Trace
{
    public enum FlowDirection { Upflow, Downflow }
    [DebuggerDisplay("[Segment: S={S}, R={R}, Len={Packet.BytesHighPerformance.Length}]")]
    public struct TcpSegment
    {
        public FlowDirection Direction;
        public TcpPacket Packet;

        public TcpSegment(FlowDirection direction, TcpPacket packet) : this()
        {
            this.Direction = direction;
            this.Packet = packet;
        }

        public uint S => Direction == FlowDirection.Upflow ? Packet.SequenceNumber : Packet.AcknowledgmentNumber;
        public uint R => Direction == FlowDirection.Downflow ? Packet.SequenceNumber : Packet.AcknowledgmentNumber;
    }
}
