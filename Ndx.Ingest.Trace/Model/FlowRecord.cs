﻿//  
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
    /// <summary>
    /// This strcuture is a compact representation of the Flow Record. 
    /// </summary>
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

        /// <summary>
        /// Creates a new <see cref="_FlowRecord"/> from the specified bytes.
        /// </summary>
        /// <param name="bytes">Byte array used to initialize the flow record.</param>
        public _FlowRecord(byte[] bytes, int offset = 0)
        {
            if (bytes.Length + offset < __size)
            {
                throw new ArgumentException($"Not enough bytes for intialization of {nameof(_FlowRecord)} instance.");
            }

            fixed (byte* pdata = bytes)
            {
                this = *(_FlowRecord*)(pdata+offset);
            }
        }
        /// <summary>
        /// Gets bytes that represents the current instance.
        /// </summary>
        /// <returns>byte array representing data of the current object.</returns>
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
                    return Equals(x, &other);
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

        public static bool Equals(_FlowRecord* x, _FlowRecord* y)
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
        /// <summary>
        /// Stores the <see cref="_FlowRecord"/> for the current <see cref="FlowRecord"/>.
        /// </summary>
        private _FlowRecord m_data;
        /// <summary>
        /// Stores <see cref="_FlowKey"/> for the current object.
        /// </summary>
        private FlowKey m_flowkey;
            
        /// <summary>
        /// Synchronization object.
        /// </summary>
        private Object m_sync = new Object();

        /// <summary>
        /// Gets the underlying data as byte array.
        /// </summary>
        public byte[] DataBytes => m_data.GetBytes();

        /// <summary>
        /// Gets the Key of the flow.
        /// </summary>
        public FlowKey Key => m_flowkey;
        /// <summary>
        /// Gets or sets a number of flow octets.
        /// </summary>
        public long Octets { get => m_data.octets; private set => m_data.octets = value; }
        /// <summary>
        /// Gets or sets a UNIX time represented as long of the first frame in the flow.
        /// </summary>
        public long FirstSeen { get => m_data.first; private set => m_data.first = value; }
        /// <summary>
        /// Gets or sets a UNIX time represented as long of the last frame in the flow.
        /// </summary>
        public long LastSeen { get => m_data.last; private set => m_data.last = value; }
        /// <summary>
        /// Gets or sets a number of flow packets.
        /// </summary>
        public int Packets { get => m_data.packets; private set => m_data.packets = value; }

        /// <summary>
        /// Recognized application protocol of the flow. 
        /// </summary>
        public ApplicationProtocol RecognizedProtocol { get => (ApplicationProtocol)m_data.application; internal set => m_data.application = (uint)value; }

        /// <summary>
        /// Creates a new object that contains an empty <see cref="_FlowRecord"/> instance and null <see cref="FlowKey"/>.
        /// </summary>
        public FlowRecord()
        {
            m_data = new _FlowRecord();
        }

        /// <summary>
        /// Creates a new <see cref="FlowRecord"/> for the specified <see cref="Ndx.Ingest.Trace.Trace.FlowKey"/>.
        /// </summary>
        /// <param name="packetMetadata"><see cref="PacketMetadata"/> object representing a single frame of the flow.</param>
        public FlowRecord(FlowKey flowkey) 
        {
            m_flowkey = flowkey;
            m_data = new _FlowRecord();
        }

        /// <summary>
        /// Creates a new <see cref="FlowRecord"/> and initializes it with the provided parameters.
        /// </summary>
        /// <param name="flowkey"></param>
        /// <param name="data"></param>
        public FlowRecord(FlowKey flowkey, _FlowRecord data) 
        {
            m_flowkey = flowkey;
            m_data = data;
        }

        /// <summary>
        /// Creates a new <see cref="FlowRecord"/> and initializes it by deserializing its <see cref="_FlowRecord"/>
        /// data with the provided bytes.
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        public FlowRecord(byte[] buf, int offset=0)
        {
            m_data = new _FlowRecord(buf, offset);
        }

        /// <summary>
        /// Creates a new <see cref="FlowRecord"/> for the passed <paramref name="flowkey"/> and initializes it by deserializing its <see cref="_FlowRecord"/>
        /// data with the provided bytes. 
        /// </summary>
        /// <param name="flowkey"></param>
        /// <param name="buf"></param>
        /// <param name="offset"></param>
        public FlowRecord(FlowKey flowkey, byte[] buf, int offset = 0)
        {
            m_flowkey = flowkey;
            m_data = new _FlowRecord(buf, offset);
        }

        /// <summary>
        /// Updates the current flow record with information from <see cref="PacketMetadata"/>.
        /// </summary>
        /// <param name="packetMetadata"><see cref="PacketMetadata"/> object representing 
        /// a single frame of the flow.</param>
        public void UpdateWith(PacketMetadata packetMetadata)
        {
            lock (m_sync)
            {
                Packets++;
                Octets += (long)(packetMetadata.Frame.FrameLength);
                long ts = packetMetadata.Frame.Timestamp.ToUnixTimeMilliseconds();

                if (FirstSeen == 0 || FirstSeen > ts)
                {
                    FirstSeen = ts;
                }

                if (LastSeen == 0 || LastSeen < ts)
                {
                    LastSeen = ts;
                }
            }
        }

        /// <summary>
        /// Implementation of <see cref="IBinaryConverter{T}"/> for <see cref="FlowRecord"/>.
        /// </summary>
        public class BinaryConverter : IBinaryConverter<FlowRecord>
        {
            public bool CanRead => true;

            public bool CanWrite => true;

            public FlowRecord ReadObject(BinaryReader reader)
            {
                var buf = reader.ReadBytes(_FlowRecord.__size);
                if (buf.Length < _FlowRecord.__size)
                {
                    return null;
                }
                else
                {
                    return new FlowRecord(buf);
                }
            }

            public void WriteObject(BinaryWriter writer, FlowRecord value)
            {
                writer.Write(value.DataBytes);                
            }
        }

        public static IBinaryConverter<FlowRecord> Converter => new BinaryConverter();

        public class FlowRecordSerializer : JsonConverter
        {
            public static readonly string FlowKey = "key";
            public static readonly string FlowOctets = "octets";
            public static readonly string FlowPackets = "packets";
            public static readonly string FlowFirst = "first";
            public static readonly string FlowLast = "last";
            public static readonly string FlowRecognizedProtocol = "recog_proto";


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
                var jsonObject = Newtonsoft.Json.Linq.JObject.Load(reader);
                var properties = jsonObject.Properties().ToDictionary(x => x.Name);

                var newObj = new FlowRecord()
                {
                    FirstSeen = (long)properties[FlowFirst].Value,

                    LastSeen = (long)properties[FlowLast].Value,

                    Octets = (long)properties[FlowOctets].Value,

                    Packets = (int)properties[FlowPackets].Value,

                    RecognizedProtocol = ParseEnum<ApplicationProtocol>(properties[FlowRecognizedProtocol].Value.ToString(), ApplicationProtocol.NULL),

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

                writer.WriteEndObject();
            }
        }
    }
}
