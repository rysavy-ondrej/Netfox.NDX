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
using Ndx.Utils;

namespace Ndx.Ingest.Trace
{
    /// <summary>
    /// This class implements a wrapper around <see cref="_PacketBlock"/> data structure.
    /// </summary>
    [DebuggerDisplay("[PacketBlock: FlowKey={m_flowKey}, Index={m_blockIndex}, Count={Count}]")]
    public class PacketBlock
    {
        /// <summary>
        /// <see cref="FlowKey"/> associated with this <see cref="PacketBlock"/>.
        /// </summary>
        private FlowKey m_flowKey;
        /// <summary>
        /// Index in the array of <see cref="PacketBlock"/> of the flow.
        /// </summary>
        private int m_blockIndex;
        /// <summary>
        /// Data storage for the current <see cref="PacketBlock"/>.
        /// </summary>
        private _PacketBlock m_data;

        /// <summary>
        /// Sync object for the following method: <see cref="PacketBlock.Add(PacketMetadata)"/>.
        /// </summary>
        private object m_sync = new object();

        /// <summary>
        /// Gets the underlying data (<see cref="_PacketBlock"/>) as byte array.
        /// </summary>
        public byte[] DataBytes => m_data.GetBytes();

        /// <summary>
        /// Gets the capacity of <see cref="PacketBlock"/>.
        /// </summary>
        public static int Capacity => _PacketBlock.__count;

        /// <summary>
        /// Gets or sets the Key string of this <see cref="PacketBlock"/>.
        /// </summary>
        public FlowKey Key { get => m_flowKey; set => m_flowKey = value; }

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
        public PacketBlock(FlowKey flowKey, int blockIndex, PacketMetadata[] metadata)
        {
            m_flowKey = flowKey;
            m_blockIndex = blockIndex;
            var source = metadata.Select(x => x.Data).ToArray();
            m_data.Count = m_data.CopyFrom(source, 0, Math.Min(source.Length, _PacketBlock.__count));
        }

        public PacketBlock(byte[] bytes, int offset = 0)
        {
            m_data = new _PacketBlock(bytes, 0);
        }


        /// <summary>
        /// Gets the number of <see cref="PacketMetadata"/> in the current <see cref="PacketBlock"/>.
        /// </summary>
        public int Count => m_data.Count;

        /// <summary>
        /// Checks if all slots in this packet block are occupied. 
        /// </summary>
        public bool IsFull => !(m_data.Count < _PacketBlock.__count);

        /// <summary>
        /// Adds new item to the current <see cref="PacketBlock"/>.
        /// </summary>
        /// <param name="packetMetadata"></param>
        /// <returns>true if item was added or false if the current block is full.</returns>
        public bool Add(PacketMetadata packetMetadata)
        {
            lock (m_sync)
            {
                if (m_data.Count < _PacketBlock.__count)
                {
                    m_data[m_data.Count] = packetMetadata.Data;
                    m_data.Count++;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        /// <summary>
        /// Gets or sets <see cref="PacketMetadata"/> value at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PacketMetadata this[int index]
        {
            get => new PacketMetadata(m_flowKey, m_data[index]);
            set => m_data[index] = value.Data;
        }

        /// <summary>
        /// Gets all <see cref="PacketMetadata"/> of the current <see cref="PacketBlock"/>.
        /// </summary>
        public IEnumerable<PacketMetadata> Packets
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                {
                    yield return this[i];
                }
            }
        }

        /// <summary>
        /// <see cref="IBinaryConverter{T}"/> for <see cref="PacketBlock"/> class.
        /// </summary>
        public static IBinaryConverter<PacketBlock> Converter = new BinaryConverter();

        public class BinaryConverter : IBinaryConverter<PacketBlock>
        {
            public bool CanRead => true;

            public bool CanWrite => true;

            public PacketBlock ReadObject(BinaryReader reader)
            {
                var buf = reader.ReadBytes(_PacketBlock.__size);
                if (buf.Length < _PacketBlock.__size)
                {
                    return null;
                }
                else
                {
                    return new PacketBlock(buf);
                }
            }

            public void WriteObject(BinaryWriter writer, PacketBlock value)
            {
                writer.Write(value.DataBytes);
            }
        }
       
        /// <summary>
        /// This strcuture is a compact representation of packet block.
        /// Packet block is a collection of packet references.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size = __size)]
        private unsafe struct _PacketBlock
        {
            internal const int __size = sizeof(int) + _PacketMetadata.__size * __count;
            internal const int __count = 64;

            [FieldOffset(0)] public int Count;
            [FieldOffset(4)] public fixed byte Packets[_PacketMetadata.__size * __count];

            /// <summary>
            /// Creates a new object from the passed bytes.
            /// </summary>
            /// <param name="bytes">Byte array to used for object initialization.</param>
            public _PacketBlock(byte[] bytes, int offset = 0)
            {
                if (bytes.Length + offset < __size)
                {
                    throw new ArgumentException($"Not enough data for creating {nameof(_PacketBlock)}.");
                }

                fixed (byte* pdata = bytes)
                {
                    this = *(_PacketBlock*)pdata;
                }
            }

            /// <summary>
            /// Gets a byte representation of the current object.
            /// </summary>
            /// <returns></returns>
            public byte[] GetBytes()
            {
                return ExplicitStruct.GetBytes<_PacketBlock>(this);
            }


            /// <summary>
            /// Gets or sets <see cref="_PacketMetadata"/> at the specified index.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public _PacketMetadata this[int index]
            {
                get
                {
                    if (index < 0 || index > __count)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    fixed (byte* ptr = Packets)
                    {
                        return *(_PacketMetadata*)(ptr + index * _PacketMetadata.__size);
                    }
                }
                set
                {
                    if (index < 0 || index > __count)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    fixed (byte* ptr = Packets)
                    {
                        *(_PacketMetadata*)(ptr + index * _PacketMetadata.__size) = value;
                    }
                }
            }

            /// <summary>
            /// Copies <see cref="_PacketMetadata"/> object from the source array.
            /// </summary>
            /// <param name="source"></param>
            /// <param name="start"></param>
            /// <param name="count"></param>
            /// <returns>Number of items copied.</returns>
            public int CopyFrom(_PacketMetadata[] source, int start, int count)
            {
                var maxcount = Math.Min(__count, Math.Min(source.Length, count));
                for (int i = 0; i < maxcount; i++)
                {
                    this[i] = source[i + start];
                }

                return maxcount;
            }

            /// <summary>
            /// Copies the <see cref="_PacketMetadata"/> objects to the provided array.
            /// </summary>
            /// <param name="target"></param>
            /// <param name="start"></param>
            /// <param name="count"></param>
            /// <returns></returns>
            public int CopyTo(_PacketMetadata[] target, int start, int count)
            {
                var maxcount = Math.Min(__count, Math.Min(target.Length, count));
                for (int i = 0; i < maxcount; i++)
                {
                    target[i + start] = this[i];
                }

                return maxcount;
            }
        }
    }
}
