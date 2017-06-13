//  
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
    [DebuggerDisplay("[PacketBlock:LinkType={LinkType}, Count={Count}]")]
    public partial class PacketBlock
    { 
        /// <summary>
        /// Sync object for the following method: <see cref="PacketBlock.Add(PacketMetadata)"/>.
        /// </summary>
        private object m_sync = new object();
        internal static readonly int Capacity = 64;

        /// <summary>
        /// Creates a new <see cref="PacketBlock"/> for the parameters provided.
        /// </summary>
        /// <param name="flowKey">Flow key.</param>
        /// <param name="blockIndex">Packet block index within the flow.</param>
        /// <param name="metadata">An array of metadata.</param>
        public PacketBlock(params PacketUnit[] packets)
        {
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
