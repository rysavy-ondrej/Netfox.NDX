//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Runtime.InteropServices;

namespace Ndx.Utils
{
    public unsafe static class ExplicitStruct
    {
        
        public static byte[] GetBytes<T>(T value)
        {    
            byte[] destination = new byte[Marshal.SizeOf(typeof(T))];
            CopyTo(value, destination, 0);
            return destination;
        }

        public static void CopyTo<T>(T value, byte[] destination, int offset)
        {

            GCHandle handle = GCHandle.Alloc(value, GCHandleType.Pinned);
            try
            {
                var size = Marshal.SizeOf(typeof(T));
                IntPtr pointer = handle.AddrOfPinnedObject();
                Marshal.Copy(pointer, destination, offset, size);
            }
            finally
            {
                if (handle.IsAllocated)
                    handle.Free();
            }
        }

        public static void FastCopyTo(void* valuePtr, int length, byte[] destination, int offset)
        {
            var ptr = new IntPtr(valuePtr);
            Marshal.Copy(ptr, destination, offset, length);
        }

        public static byte[] FastGetBytes(void* valuePtr, int size)
        {
            byte[] destination = new byte[size];
            var ptr = new IntPtr(valuePtr);
            Marshal.Copy(ptr, destination, 0, size);
            return destination;
        }


        /// <summary>
        /// Test equality of two integer arrays of the given length.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public unsafe static bool CmpInt(int* x, int *y, int len)
        {
            for (int i = 0; i < len; i++)
            {
                if (x[i] != y[i]) return false;
            }
            return true;
        }

        public unsafe static bool CmpByte(byte* x, byte* y, int len)
        {
            for (int i = 0; i < len; i++)
            {
                if (x[i] != y[i]) return false;
            }
            return true;
        }

        public static unsafe int GetHash(int* x, int len)
        {
            var hash = 0;
            for (int i = 0; i < len; i++)
            {
                hash ^= x[i];
            }
            return hash;
        }
    }
}