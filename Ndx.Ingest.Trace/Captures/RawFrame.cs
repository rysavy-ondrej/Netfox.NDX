//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using PacketDotNet;
using System;
using ProtoBuf;

namespace Ndx.Metacap
{
    /// <summary>
    /// Represents a single captured packet and its metadata.
    /// </summary>
    [ProtoContract]
    public class RawFrame
    {
        /// <summary>
        /// Gets or sets the raw frame content.
        /// </summary>
        [ProtoMember(1)]
        public byte[] RawFrameData;
        /// <summary>
        /// Gets or sets frame metadata.
        /// </summary>
        [ProtoMember(2)]     
        public FrameMetadata Meta { get; internal set; }
        [ProtoMember(3)]
        public uint ProcessId { get; internal set; }
        [ProtoMember(4)]
        public string ProcessName { get; internal set; }
    }
}