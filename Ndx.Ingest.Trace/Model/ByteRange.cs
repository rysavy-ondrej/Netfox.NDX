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

    [StructLayout(LayoutKind.Explicit, Size = __size)]
    public unsafe struct _ByteRange
    {
        internal const int __size = 8;
        [FieldOffset(0)] internal int start;
        [FieldOffset(sizeof(int))] internal int count;

        public _ByteRange(ByteArraySegment bytesHighPerformance) : this()
        {
            start = bytesHighPerformance.Offset;
            count = bytesHighPerformance.Length;
        }

        public _ByteRange(int start, int count)
        {
            this.start = start;
            this.count = count;
        }

        public _ByteRange(byte[] bytes, int offset=0)
        {
            if (bytes.Length - offset < __size) throw new ArgumentException("Not enough data to create the object.", nameof(bytes));
            fixed (byte* pdata = bytes)
            {
                this = *(_ByteRange*)(pdata+offset);
            }
        }

        public byte[] GetBytes()
        {
            return ExplicitStruct.GetBytes<_ByteRange>(this);
        }

        public bool IsNull { get { return start == 0 && count == 0; } }

        public static _ByteRange Null => new _ByteRange(0, 0);

        public int Count => count;
        public int Start => start;

        public override string ToString()
        {
            return $"[ByteRange: Start={Start},Count={Count}]";
        }
    }
}
