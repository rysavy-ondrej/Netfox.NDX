//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using Ndx.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Ndx.Metacap;

namespace Ndx.Model
{
    /// <summary>
    /// This class collects information about a single flow.
    /// </summary>
    [DebuggerDisplay("[FlowRecord: packets={Packets}, octets={Octets}, first={FirstSeen}, last={LastSeen}, appid={ApplicationId}]")]
    public partial class FlowRecord
    {
        /// <summary>
        /// Synchronization object.
        /// </summary>
        private Object m_sync = new Object();

        /// <summary>
        /// Gets the underlying data as byte array.
        /// </summary>
        public byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var cos = new Google.Protobuf.CodedOutputStream(ms))
                {
                    WriteTo(cos);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Creates a new <see cref="FlowRecord"/> and initializes it by deserializing its <see cref="_FlowRecord"/>
        /// data with the provided bytes.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        public FlowRecord(byte[] bytes, int offset = 0)
        {
            using (var ms = new MemoryStream(bytes, offset, bytes.Length - offset))
            using (var cis = new Google.Protobuf.CodedInputStream(ms))
            {
                this.MergeFrom(cis);
            }
        }


        /// <summary>
        /// Updates the current flow record with information from <see cref="PacketMetadata"/>.
        /// </summary>
        /// <param name="packetMetadata"><see cref="PacketMetadata"/> object representing 
        /// a single frame of the flow.</param>
        public void UpdateWith(PacketUnit packet)
        {
            lock (m_sync)
            {
                packets_++;
                octets_ += (long)(packet.Frame.FrameLength);
                long ts = packet.Frame.TimeStamp;

                if (firstSeen_ == 0 || firstSeen_ > ts)
                {
                    firstSeen_ = ts;
                }

                if (lastSeen_ == 0 || lastSeen_ < ts)
                {
                    lastSeen_ = ts;
                }
            }
        }
    }
}
