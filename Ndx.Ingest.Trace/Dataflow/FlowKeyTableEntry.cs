//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Ndx.Model;

namespace Ndx.Metacap
{
    /// <summary>
    /// An item of the Flow Key Table. Each <see cref="FlowKeyTableEntry"/> 
    /// corresponds to exactly one Flow Key and provides information about 
    /// associated <see cref="FlowRecord"/> and <see cref="PacketBlock"/> 
    /// through <see cref="FlowKeyTableEntry.IndexRecord"/> property.
    /// </summary>
    public partial class FlowKeyTableEntry
    {               
        private FlowKey m_key;
        private IndexRecord m_indexRecord;
        public FlowKeyTableEntry(FlowKey key, IndexRecord value)
        {
            this.m_key = key;
            this.m_indexRecord = value;
        }

        public FlowKeyTableEntry(byte[] buffer, int offset = 0)
        {
            
        }

        public FlowKey Key => m_key;
        public IndexRecord IndexRecord => m_indexRecord;
                    
    }


    public unsafe class IndexRecord
    {
        private int m_flowRecordIndex;
        private List<int> m_packetBlockList;
        public IndexRecord()
        {
            m_packetBlockList = new List<int>(16);
        }
        public byte[] GetBytes()
            {
            var buffer = new byte[sizeof(int) * (PacketBlockList.Count + 1)];
            fixed (byte* ptr = buffer)
            {
                int* intPtr = (int*)ptr;
                intPtr[0] = FlowRecordIndex;
                for (int i = 0; i < PacketBlockList.Count; i++)
                {
                    intPtr[i + 1] = PacketBlockList[i];
                }
            }
            return buffer;
        }
        public static IndexRecord FromBytes(byte[] bytes)
        {
            var obj = new IndexRecord();
            fixed (byte* ptr = bytes)
            {
                int* intPtr = (int*)ptr;
                obj.FlowRecordIndex = intPtr[0];
                for (int i = 1; i < bytes.Length / sizeof(int); i++)
                {
                    obj.PacketBlockList.Add(intPtr[i]);
                }
            }
            return obj;
        }
                                
        /// <summary>
        /// Gets or sets the flow record index.
        /// </summary>
        public int FlowRecordIndex { get => m_flowRecordIndex; set => m_flowRecordIndex = value; }

        /// <summary>
        /// Gets the list of packet block indexes.
        /// </summary>
        public IList<int> PacketBlockList => m_packetBlockList; 
    }
}
