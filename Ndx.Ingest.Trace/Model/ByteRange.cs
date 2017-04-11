//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using Ndx.Utils;
using PacketDotNet.Utils;
using System;
using System.Runtime.InteropServices;

namespace Ndx.Ingest.Trace
{

    /// <summary>
    /// Byte range structure represents a tuple of offset nad length value
    /// that can be used to point to the byte array.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = __size)]
    public unsafe struct _ByteRange
    {
        /// <summary>
        /// Size of the structure.
        /// </summary>
        internal const int __size = 8;
        [FieldOffset(0)] private int m_start;
        [FieldOffset(sizeof(int))] private int m_count;

        /// <summary>
        /// Creates a new <see cref="_ByteRange"/> and initializes it with from <see cref="ByteArraySegment"/>
        /// object.
        /// </summary>
        /// <param name="bas">Object used to initialize newly created instance.</param>
        public _ByteRange(ByteArraySegment bas) : this()
        {
            Start = bas.Offset;
            Count = bas.Length;
        }

        /// <summary>
        /// Creates a new <see cref="_ByteRange"/> and initializes it with from passed arguments.
        /// </summary>
        /// <param name="start">Value used to initialize <see cref="_ByteRange.Start"/> field.</param>
        /// <param name="count">Value used to initialize <see cref="_ByteRange.Count"/> field.</param>
        public _ByteRange(int start, int count)
        {
            this.m_start = start;
            this.m_count = count;
        }

        /// <summary>
        /// Deserializes the <see cref="_ByteRange"/> from the provided byte array.
        /// </summary>
        /// <param name="bytes">Byte array that contains data for initializing the new <see cref="_ByteRange"/>.</param>
        /// <param name="offset">Offset to the <see cref="bytes"/> byte array.</param>
        public _ByteRange(byte[] bytes, int offset=0)
        {
            if (bytes.Length - offset < __size)
            {
                throw new ArgumentException("Not enough data to create the object.", nameof(bytes));
            }

            fixed (byte* pdata = bytes)
            {
                this = *(_ByteRange*)(pdata+offset);
            }
        }

        /// <summary>
        /// Test equality of two instance of <see cref="_ByteRange"/>.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equals(_ByteRange left, _ByteRange right)
        {
            return left.m_start == right.m_start && left.m_count == right.m_count;
        }

        public override bool Equals(object obj)
        {
            
            if (obj is _ByteRange)
            {
                return Equals(this, (_ByteRange)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return m_count.GetHashCode() ^ m_start.GetHashCode();
        }

        /// <summary>
        /// Gets byte representation of the current object.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return ExplicitStruct.GetBytes<_ByteRange>(this);
        }

        /// <summary>
        /// Tests if the current object is null, that is start and count are both equal to 0.
        /// </summary>
        public bool IsNull => Start == 0 && Count == 0;
        /// <summary>
        /// Creates a new null <see cref="_ByteRange"/> structure.
        /// </summary>
        public static _ByteRange Null => new _ByteRange(0, 0);

        /// <summary>
        /// Gets or sets an index of the first byte of the byte array referenced by the current object. 
        /// </summary>
        public int Start { get => m_start; set => m_start = value; }
        /// <summary>
        /// Gets or sets a number of bytes of the byte array referenced by tge current object.
        /// </summary>
        public int Count { get => m_count; set => m_count = value; }

        public override string ToString()
        {
            return $"[ByteRange: Start={Start},Count={Count}]";
        }
    }
}
