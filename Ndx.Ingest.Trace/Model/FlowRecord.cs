//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using Ndx.Utils;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Ndx.Ingest.Trace
{
    [StructLayout(LayoutKind.Explicit, Size = __size)]
    public unsafe struct _FlowRecord
    {
        public const int __size = 40;
        [FieldOffset(0)] public long octets;
        [FieldOffset(8)] public int packets;
        [FieldOffset(12)] public long first;
        [FieldOffset(20)] public long last;
        [FieldOffset(28)] public int blocks;
        [FieldOffset(32)] public uint application;
        [FieldOffset(36)] public uint reserved;

        public _FlowRecord(byte[] bytes)
        {
            fixed (byte* pdata = bytes)
            {
                this = *(_FlowRecord*)pdata;
            }
        }

        public byte[] GetBytes()
        {
            return ExplicitStruct.GetBytes<_FlowRecord>(this);
        }

        public override bool Equals(object obj)
        {
            if (obj is _FlowRecord other)
            {
                fixed (_FlowRecord* x = &this)
                {
                    return equals(x, &other);
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            fixed (_FlowRecord* ptr = &this)
            {
                return ExplicitStruct.GetHash((int*)ptr, __size / sizeof(int));
            }
        }

        private static bool equals(_FlowRecord* x, _FlowRecord* y)
        {
            return ExplicitStruct.CmpInt((int*)x, (int*)y, __size / sizeof(int));
        }
    }



    /// <summary>
    /// This class collects information about a single flow.
    /// </summary>
    [JsonConverter(typeof(FlowRecordSerializer))]
    public class FlowRecord 
    {
        public static readonly string FlowKey = "key";
        public static readonly string FlowOctets = "octets";
        public static readonly string FlowPackets = "packets";
        public static readonly string FlowFirst = "first";
        public static readonly string FlowLast = "last";
        public static readonly string FlowBlockCount = "block_count";
        public static readonly string FlowRecognizedProtocol = "recog_proto";

        /// <summary>
        /// Stores the <see cref="_FlowRecord"/> for the current <see cref="FlowRecord"/>.
        /// </summary>
        _FlowRecord m_data;
        /// <summary>
        /// Stores <see cref="_FlowKey"/> for the current object.
        /// </summary>
        FlowKey m_flowkey;

        internal int PacketBlockCount;

        /// <summary>
        /// Gets the underlying data (<see cref="_PacketBlock"/>) as byte array.
        /// </summary>
        public byte[] DataBytes => m_data.GetBytes();

        /// <summary>
        /// Key of the flow.
        /// </summary>
        public FlowKey Key => m_flowkey;
        /// <summary>
        /// Number of flow octets.
        /// </summary>
        public long Octets { get { return m_data.octets; } private set { m_data.octets = value; } }
        /// <summary>
        /// UNIX time represented as long of the first frame in the flow.
        /// </summary>
        public long FirstSeen { get { return m_data.first; } private set { m_data.first = value; } }
        /// <summary>
        /// UNIX time represented as long of the last frame in the flow.
        /// </summary>
        public long LastSeen { get { return m_data.last; } private set { m_data.last = value; } }
        /// <summary>
        /// Number of flow packets.
        /// </summary>
        public int Packets { get { return m_data.packets; } private set { m_data.packets = value; } }

        /// <summary>
        /// Recognized application protocol of the flow. 
        /// </summary>
        public ApplicationProtocol RecognizedProtocol { get { return (ApplicationProtocol)m_data.application; } internal set { m_data.application = (uint)value; } }

        /// <summary>
        /// Creates a new <see cref="FlowRecord"/>.
        /// </summary>
        private FlowRecord()
        {
        }

        /// <summary>
        /// Creates a new <see cref="FlowRecord"/> for the specified <see cref="Ndx.Ingest.Trace.Trace.FlowKey"/>.
        /// </summary>
        /// <param name="packetMetadata"><see cref="PacketMetadata"/> object representing a single frame of the flow.</param>
        public FlowRecord(FlowKey flowkey) : this()
        {
            m_flowkey = flowkey;
        }

        public FlowRecord(FlowKey flowkey, _FlowRecord data) : this(flowkey)
        {
            m_data = data;
        }

        private Object _sync = new Object();
        private _FlowRecord data;

        /// <summary>
        /// Updates the current flow record with information from <see cref="PacketMetadata"/>.
        /// </summary>
        /// <param name="packetMetadata"><see cref="PacketMetadata"/> object representing 
        /// a single frame of the flow.</param>
        public void UpdateWith(PacketMetadata packetMetadata)
        {
            lock (_sync)
            {
                Packets++;
                Octets += (long)(packetMetadata.Frame.FrameLength);
                long ts = packetMetadata.Frame.Timestamp.ToUnixTimeMilliseconds();
                if (FirstSeen == 0 || FirstSeen > ts) FirstSeen = ts;
                if (LastSeen == 0 || LastSeen < ts) LastSeen = ts;
            }
        }


        public class BinaryConverter : IBinaryConverter<FlowRecord>
        {
            public bool CanRead => true;

            public bool CanWrite => true;

            public FlowRecord ReadObject(BinaryReader reader)
            {
                var buf = reader.ReadBytes(_FlowRecord.__size);
                if (buf.Length < _FlowRecord.__size)
                    return null;
                else
                    return FromBytes(buf);
            }

            public void WriteObject(BinaryWriter writer, FlowRecord value)
            {
                writer.Write(value.DataBytes);                
            }
        }

        public static IBinaryConverter<FlowRecord> Converter => new BinaryConverter();


        private static FlowRecord FromBytes(byte[] bytes)
        {
            var data = new _FlowRecord(bytes);
            return new FlowRecord(null, data);
        }

        public class FlowRecordSerializer : JsonConverter
        {

            T ParseEnum<T>(string value, T defaultResult) where T : struct
            {
                var result = defaultResult;
                Enum.TryParse<T>(value, out result);
                return result;
            }

            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(FlowRecord));
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                Newtonsoft.Json.Linq.JObject jsonObject = Newtonsoft.Json.Linq.JObject.Load(reader);
                var properties = jsonObject.Properties().ToDictionary(x => x.Name);

                var newObj = new FlowRecord()
                {
                    FirstSeen = (long)properties[FlowFirst].Value,

                    LastSeen = (long)properties[FlowLast].Value,

                    Octets = (long)properties[FlowOctets].Value,

                    Packets = (int)properties[FlowPackets].Value,

                    RecognizedProtocol = ParseEnum<ApplicationProtocol>(properties[FlowRecognizedProtocol].Value.ToString(), ApplicationProtocol.NULL),

                    PacketBlockCount = (int)properties[FlowBlockCount].Value
                };
                return newObj;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var data = value as FlowRecord;
                writer.WriteStartObject();

                writer.WritePropertyName(FlowKey);
                serializer.Serialize(writer, data.Key);

                writer.WritePropertyName(FlowFirst);
                writer.WriteValue(data.FirstSeen);

                writer.WritePropertyName(FlowLast);
                writer.WriteValue(data.LastSeen);

                writer.WritePropertyName(FlowOctets);
                writer.WriteValue(data.Octets);

                writer.WritePropertyName(FlowPackets);
                writer.WriteValue(data.Packets);

                writer.WritePropertyName(FlowRecognizedProtocol);
                writer.WriteValue(data.RecognizedProtocol.ToString());

                writer.WritePropertyName(FlowBlockCount);
                writer.WriteValue(data.PacketBlockCount);

                writer.WriteEndObject();
            }
        }
    }
}
