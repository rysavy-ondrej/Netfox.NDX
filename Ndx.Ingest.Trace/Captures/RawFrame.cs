//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using PacketDotNet;
using System;
namespace Ndx.Network
{
    /// <summary>
    /// Represents a single captured packet and its metadata.
    /// </summary>
    public class RawFrame
    {       
        /// <summary>
        /// Gets or sets the raw frame content.
        /// </summary>
        public byte[] RawFrameData;
        /// <summary>
        /// Gets or sets frame metadata.
        /// </summary>
        public FrameMetadata Meta { get; internal set; }

        public uint ProcessId { get; internal set; }

        public string ProcessName { get; internal set; }
    }
}