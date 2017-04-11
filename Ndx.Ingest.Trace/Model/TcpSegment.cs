//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System.Diagnostics;
using PacketDotNet;

namespace Ndx.Ingest.Trace
{
    public enum FlowDirection { Upflow, Downflow }
    /// <summary>
    /// Represents a single TCP segment. 
    /// </summary>
    [DebuggerDisplay("[Segment: S={S}, R={R}, Len={Packet.BytesHighPerformance.Length}]")]
    public struct TcpSegment
    {
        /// <summary>
        /// Direction of the segment.
        /// </summary>
        public FlowDirection Direction;
        /// <summary>
        /// <see cref="TcpPacket"/> that represents data of the segment.
        /// </summary>
        public TcpPacket Packet;

        public TcpSegment(FlowDirection direction, TcpPacket packet) : this()
        {
            Direction = direction;
            Packet = packet;
        }

        public uint S => Direction == FlowDirection.Upflow ? Packet.SequenceNumber : Packet.AcknowledgmentNumber;
        public uint R => Direction == FlowDirection.Downflow ? Packet.SequenceNumber : Packet.AcknowledgmentNumber;
    }
}
