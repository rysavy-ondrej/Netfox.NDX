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
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public unsafe struct _FrameMetadata
    {
        /// <summary>
        /// The size of this structure in 32-bit words.
        /// </summary>
        internal const int __size = 24;

        /// <summary>
        /// An index of the frame in its source capture.
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

        public _FrameMetadata(byte[] bytes, int offset=0)
        {
            if (bytes.Length - offset < __size) throw new ArgumentException("Not enough data to create the object.", nameof(bytes));
            fixed (byte* pdata = bytes)
            {
                this = *(_FrameMetadata*)(pdata+offset);
            }
        }

        public byte[] GetBytes()
        {
            fixed (void* ptr = &this) return ExplicitStruct.FastGetBytes(ptr, __size);
        }

        public void CopyTo(byte[]buffer, int offset)
        {
            if (buffer.Length - offset < __size) throw new ArgumentOutOfRangeException("Cannot copy, target buffer has not enough capacity");
            fixed(void *ptr = &this) ExplicitStruct.FastCopyTo(ptr, __size, buffer, offset);
        }

        public override bool Equals(object obj)
        {
            if (obj is _FrameMetadata)
            {
                return equals(this, (_FrameMetadata)obj);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return getHashCode(this);

        }

        private static bool equals(_FrameMetadata objX, _FrameMetadata objY)
        {
            var fkX = (int*)&objX;
            var fkY = (int*)&objY;
            for (int i = 0; i < __size/sizeof(int); i++)
            {
                if (fkX[i] != fkY[i]) return false;
            }
            return true;
        }

        private static int getHashCode(_FrameMetadata obj)
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
    [JsonConverter(typeof(FrameMetadataConvert))]
    public class FrameMetadata
    {
        /// <summary>
        /// <see cref="_FrameMetadata"/> structure that stores data of this object.
        /// </summary>
        _FrameMetadata m_data;


        internal _FrameMetadata Data => m_data;

        /// <summary>
        /// Specifies a timestamp when the packet was captured.
        /// </summary>
        public DateTimeOffset Timestamp { 
            get {return DateTimeOffsetExt.FromUnixTimeMilliseconds(m_data.timestamp); } 
            set { m_data.timestamp = value.ToUnixTimeMilliseconds(); } 
        }
        /// <summary>
        /// Determines link type.
        /// </summary>
        public PacketDotNet.LinkLayers LinkLayer;

        /// <summary>
        /// Specifies the length of the frame.
        /// </summary>
        public int FrameLength { get {return m_data.frameLength; } set { m_data.frameLength = value; } }
        /// <summary>
        /// Gets the position of the frame in a capture file as the offset from the start of the file.
        /// </summary>
        public long FrameOffset { get {return m_data.frameOffset; } set { m_data.frameOffset = value; } }
        /// <summary>
        /// Gets the index number of the current Frame within the capture file.
        /// </summary>
        public int FrameNumber { get {return m_data.frameNumber; } set { m_data.frameNumber = value; } }

        public FrameMetadata()
        {
            m_data = new _FrameMetadata();
        }

        public FrameMetadata(byte[] buffer, int offset=0)
        {
            m_data = new _FrameMetadata(buffer, offset);
        }

        public FrameMetadata(_FrameMetadata frame)
        {
            m_data = frame;
        }

        public override string ToString()
        {
            return $"FrameMetadata {{ Timestamp : {Timestamp}, LinkLayer : {LinkLayer}, FrameLength : {FrameLength}, FrameOffset : {FrameOffset} }}";
        }

        private class FrameMetadataConvert : JsonConverter
        {

            public static readonly string FrameNumber = "frame.no";
            public static readonly string FrameOffset = "frame.offset";
            public static readonly string FrameTimestamp = "frame.ts";
            public static readonly string FrameLength = "frame.len";

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(FrameMetadata);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                Newtonsoft.Json.Linq.JObject jsonObject = Newtonsoft.Json.Linq.JObject.Load(reader);
                var properties = jsonObject.Properties().ToDictionary(x => x.Name);

                return new FrameMetadata()
                {
                    FrameNumber = (int)properties[FrameNumber].Value,
                    FrameOffset = (int)properties[FrameOffset].Value,
                    Timestamp = DateTimeOffsetExt.FromUnixTimeMilliseconds((long)properties[FrameTimestamp].Value),
                    FrameLength = (int)properties[FrameLength].Value
                };
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var data = value as FrameMetadata;
                writer.WriteStartObject();
                writer.WritePropertyName(FrameNumber);
                writer.WriteValue(data.FrameNumber);

                writer.WritePropertyName(FrameOffset);
                writer.WriteValue(data.FrameOffset);

                writer.WritePropertyName(FrameTimestamp);
                writer.WriteValue(data.Timestamp.ToUnixTimeMilliseconds());

                writer.WritePropertyName(FrameLength);
                writer.WriteValue(data.FrameLength);

                writer.WriteEndObject();
            }
        }
    }
}
