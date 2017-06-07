﻿//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Ndx.Metacap;
using Ndx.Utils;

namespace Ndx.Model
{
    /// <summary>
    /// This class implements a wrapper around <see cref="_PacketBlock"/> data structure.
    /// </summary>
    [DebuggerDisplay("[PacketBlock: FlowKey={m_flowKey}, Index={m_blockIndex}, Count={Count}]")]
    public partial class PacketBlock
    {
        /// <summary>
        /// Index in the array of <see cref="PacketBlock"/> of the flow.
        /// </summary>
        private int m_blockIndex;
 
        /// <summary>
        /// Sync object for the following method: <see cref="PacketBlock.Add(PacketMetadata)"/>.
        /// </summary>
        private object m_sync = new object();
        internal static readonly int Capacity = 64;

        /// <summary>
        /// Gets the block index of the current <see cref="PacketBlock"/>.
        /// </summary>
        public int BlockIndex => m_blockIndex;

        /// <summary>
        /// Creates a new <see cref="PacketBlock"/> for the parameters provided.
        /// </summary>
        /// <param name="flowKey">Flow key.</param>
        /// <param name="blockIndex">Packet block index within the flow.</param>
        /// <param name="metadata">An array of metadata.</param>
        public PacketBlock(int blockIndex, params PacketUnit[] packets)
        {
            m_blockIndex = blockIndex;
            packets_.AddRange(packets);
        }

        public PacketBlock(byte[] bytes, int offset = 0)
        {
            using (var ms = new MemoryStream(bytes, offset, bytes.Length - offset))
            using (var cis = new Google.Protobuf.CodedInputStream(ms))
            {
                this.MergeFrom(cis);
            }
        }
    }
}
