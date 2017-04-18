//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using Ndx.Utils;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Runtime.InteropServices;
namespace Ndx.Ingest.Trace
{

    /// <summary>
    /// Represents a structure that contains basic characterization of each captured frame.
    /// This structure is suitable for direct binary serialization as it specifies explicit 
    /// field layout. 
    /// 
    /// </summary>
    /// <remarks>
    /// <see cref="_FrameMetadata"/> contains necessary information about the capture frame:
    /// <list type="table">
    /// <listheader>
    /// <term>Field</term><description>Description</description>
    /// </listheader>
    /// <item>
    /// <term><see cref="frameNumber"/></term><description>An index of the frame in its source capture.</description>
    /// <term><see cref="frameLength"/></term><description>Total Lenght of the frame.</description>
    /// <term><see cref="frameOffset"/></see></term><description>An absolute offset of the frame within the capture file.</description>
    /// <term><see cref="timestamp"/></term><description>Timestamp of the frame in Unix time format.</description>
    /// </item>
    /// </list>
    /// </remarks>
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public unsafe struct _FrameMetadata
    {
        /// <summary>
        /// The size of this structure in 32-bit words.
        /// </summary>
        internal const int __size = 24;

        /// <summary>
        /// An index of the frame in its source capture. Noite that the first frame should have index 1.
        /// </summary>
        [FieldOffset(0)] public int frameNumber;
        /// <summary>
        /// Total Lenght of the frame.
        /// </summary>
        [FieldOffset(4)] public int frameLength;
        /// <summary>
        /// An absolute offset of the frame within the capture file. 
        /// </summary>
        [FieldOffset(8)] public long frameOffset;
        /// <summary>
        /// Timestamp of the frame in Unix time format.
        /// </summary>
        [FieldOffset(16)] public long timestamp;

        /// <summary>
        /// Creates a new <see cref="_FrameMetadata"/> structure from the passed byte array.
        /// </summary>
        /// <param name="bytes">Byte array containing data for initialization of new <see cref="_FrameMetadata"/> strcuture.</param>
        /// <param name="offset">First byte in the passed byte array to be used for initialization of the current struct.</param>
        /// <exception cref="ArgumentException">Provided byte array is shorter that expected.</exception>
        public _FrameMetadata(byte[] bytes, int offset=0)
        {
            if (bytes.Length - offset < __size)
            {
                throw new ArgumentException("Not enough data to create the object.", nameof(bytes));
            }

            fixed (byte* pdata = bytes)
            {
                this = *(_FrameMetadata*)(pdata+offset);
            }
        }

        /// <summary>
        /// Gets byte array that represents the current object.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            fixed (void* ptr = &this) return ExplicitStruct.FastGetBytes(ptr, __size);
        }

        /// <summary>
        /// Copies the content of the current object to the specified byte array.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        public void CopyTo(byte[]buffer, int offset)
        {
            if (buffer.Length - offset < __size)
            {
                throw new ArgumentOutOfRangeException("Cannot copy, target buffer has not enough capacity");
            }

            fixed (void *ptr = &this) ExplicitStruct.FastCopyTo(ptr, __size, buffer, offset);
        }

        public override bool Equals(object obj)
        {
            if (obj is _FrameMetadata)
            {
                return Equals(this, (_FrameMetadata)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return GetHashCode(this);

        }

        private static bool Equals(_FrameMetadata objX, _FrameMetadata objY)
        {
            var fkX = (int*)&objX;
            var fkY = (int*)&objY;
            for (int i = 0; i < __size/sizeof(int); i++)
            {
                if (fkX[i] != fkY[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static int GetHashCode(_FrameMetadata obj)
        {
            var x = (int*)&obj;
            var hash = 0;
            for (int i = 0; i < __size / sizeof(int); i++)
            {
                hash ^= x[i];
            }
            return hash;
        }

    }



    /// <summary>
    /// This class contains metadata associated with captured frames. 
    /// It implements a wrapper around <see cref="_FrameMetadata"/> structure.
    /// </summary>
    public class FrameMetadata
    {
        /// <summary>
        /// <see cref="_FrameMetadata"/> structure that stores data of this object.
        /// </summary>
        _FrameMetadata m_data;
        /// <summary>
        /// Gets the underlying data structure that backs the current instance.
        /// </summary>
        internal _FrameMetadata Data => m_data;

        /// <summary>
        /// Specifies a timestamp when the packet was captured.
        /// </summary>
        public DateTimeOffset Timestamp {
            get => DateTimeOffsetExt.FromUnixTimeMilliseconds(m_data.timestamp); set => m_data.timestamp = value.ToUnixTimeMilliseconds();
        }
        /// <summary>
        /// Determines link type.
        /// </summary>
        public PacketDotNet.LinkLayers LinkLayer;

        /// <summary>
        /// Specifies the length of the frame.
        /// </summary>
        public int FrameLength { get => m_data.frameLength; set => m_data.frameLength = value; }
        /// <summary>
        /// Gets the position of the frame in a capture file as the offset from the start of the file.
        /// </summary>
        public long FrameOffset { get => m_data.frameOffset; set => m_data.frameOffset = value; }
        /// <summary>
        /// Gets the index number of the current Frame within the capture file.
        /// </summary>
        public int FrameNumber { get => m_data.frameNumber; set => m_data.frameNumber = value; }

        /// <summary>
        /// Creates a new <see cref="FrameMetadata"/> object.
        /// </summary>
        public FrameMetadata()
        {
            m_data = new _FrameMetadata();
        }

        /// <summary>
        /// Creates a new <see cref="FrameMetadata"/> object.
        /// </summary>
        /// <param name="buffer">Byte array used for initialization.</param>
        /// <param name="offset">Offset of the first byte in the passed byte array.</param>
        public FrameMetadata(byte[] buffer, int offset=0)
        {
            m_data = new _FrameMetadata(buffer, offset);
        }

        /// <summary>
        /// Creates a new <see cref="FrameMetadata"/> object for the passed <see cref="_FrameMetadata"/> structure.
        /// </summary>
        /// <param name="frame">Data strcuture used to initialize newly created object.</param>
        public FrameMetadata(_FrameMetadata frame)
        {
            m_data = frame;
        }

        public override string ToString()
        {
            return $"FrameMetadata {{ Timestamp : {Timestamp}, LinkLayer : {LinkLayer}, FrameLength : {FrameLength}, FrameOffset : {FrameOffset} }}";
        }
    }
}
