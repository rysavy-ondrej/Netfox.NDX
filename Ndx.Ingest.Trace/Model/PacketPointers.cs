//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//

using Ndx.Utils;
using System;
using System.Runtime.InteropServices;

namespace Ndx.Ingest.Trace
{
    [StructLayout(LayoutKind.Explicit, Size = __size)]
    public unsafe struct _PacketPointers
    {
        internal const int __size = _ByteRange.__size * 4;
        [FieldOffset(0)] public _ByteRange link;
        [FieldOffset(_ByteRange.__size)] public _ByteRange network;
        [FieldOffset(_ByteRange.__size*2)] public _ByteRange transport;
        [FieldOffset(_ByteRange.__size*3)] public _ByteRange payload;


        public _PacketPointers(byte[] bytes, int offset=0)
        {
            if (bytes.Length - offset< __size) throw new ArgumentException("Not enough data to create the object.", nameof(bytes));
            fixed (byte* pdata = bytes)
            {
                this = *(_PacketPointers*)(pdata+offset);
            }
        }

        public byte[] GetBytes()
        {
            return ExplicitStruct.GetBytes(this);
        }
    }
}