using Ndx.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Ingest.Trace
{
    //[JsonConverter(typeof(PacketBlockSerializer))]
    [StructLayout(LayoutKind.Explicit, Size = __size)]
    unsafe public struct _PacketBlock
    {
        internal const int __size = sizeof(int) + _PacketMetadata.__size * __count;
        internal const int __count = 64;
        [FieldOffset(0)] public int count;
        [FieldOffset(4)] public fixed byte packets[_PacketMetadata.__size * __count];

        public _PacketBlock(byte[] bytes)
        {
            fixed (byte* pdata = bytes)
            {
                this = *(_PacketBlock*)pdata;
            }
        }

        public byte[] GetBytes()
        {
            return ExplicitStruct.GetBytes<_PacketBlock>(this);
        }

        public _PacketMetadata this[int index]
        {
            get
            {
                if (index < 0 || index > __count) throw new IndexOutOfRangeException();
                fixed (byte* ptr = packets)
                {
                    return *(_PacketMetadata*)(ptr + index * _PacketMetadata.__size);
                }
            }
            set
            {
                if (index < 0 || index > __count) throw new IndexOutOfRangeException();
                fixed (byte* ptr = packets)
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
            var maxcount = Math.Min(__count,Math.Min(source.Length, count));
            for (int i = 0; i < maxcount; i++)
                this[i] = source[i + start];
            return maxcount;
        }

        public int CopyTo(_PacketMetadata[] target, int start, int count)
        {
            var maxcount = Math.Min(__count, Math.Min(target.Length, count));
            for (int i = 0; i < maxcount; i++)
                target[i + start] = this[i];
            return maxcount;
        }
    }
   
    public class PacketBlock
    {
        /// <summary>
        /// <see cref="FlowKey"/> associated with this <see cref="PacketBlock"/>.
        /// </summary>
        FlowKey m_flowKey;
        /// <summary>
        /// Index in the array of <see cref="PacketBlock"/> of the flow.
        /// </summary>
        int m_blockIndex;
        /// <summary>
        /// Data storage for the current <see cref="PacketBlock"/>.
        /// </summary>
        _PacketBlock m_data;

        /// <summary>
        /// Gets the underlying data (<see cref="_PacketBlock"/>) as byte array.
        /// </summary>
        public byte[] DataBytes => m_data.GetBytes();

        /// <summary>
        /// Gets or sets the Key string of this <see cref="PacketBlock"/>.
        /// </summary>
        public FlowKey Key => m_flowKey;

        public int BlockIndex => m_blockIndex;

        public PacketBlock(FlowKey flowKey, int blockIndex, PacketMetadata[] metadata)
        {
            m_flowKey = flowKey;
            m_blockIndex = blockIndex;
            var source = metadata.Select(x => x.Data).ToArray();
            m_data.count = m_data.CopyFrom(source, 0, Math.Min(source.Length, _PacketBlock.__count));            
        }

        PacketBlock()
        { }

        /// <summary>
        /// Gets the number of <see cref="PacketMetadata"/> in the current <see cref="PacketBlock"/>.
        /// </summary>
        public int Count => m_data.count;

        public bool IsFull => !(m_data.count < _PacketBlock.__count);

        object _sync = new object();

        /// <summary>
        /// Adds new item to the current <see cref="PacketBlock"/>.
        /// </summary>
        /// <param name="packetMetadata"></param>
        /// <returns>true if item was added or false if the current block is full.</returns>
        public bool Add(PacketMetadata packetMetadata)
        {
            lock (_sync)
            {
                if (m_data.count < _PacketBlock.__count)
                {
                    m_data[m_data.count] = packetMetadata.Data;
                    m_data.count++;
                    return true;
                }
                else
                    return false;
            }
        }

        public PacketMetadata[] Content
        {
            get
            {
                var result = new _PacketMetadata[m_data.count];
                m_data.CopyTo(result, 0, m_data.count);
                return result.Select(x => new PacketMetadata(Key, x)).ToArray();
            }
        }

        public static IBinaryConverter<PacketBlock> Converter = new BinaryConverter();

        public class BinaryConverter : IBinaryConverter<PacketBlock>
        {
            public bool CanRead => true;

            public bool CanWrite => true;

            public PacketBlock ReadObject(BinaryReader reader)
            {
                var buf = reader.ReadBytes(_PacketBlock.__size);
                if (buf.Length < _PacketBlock.__size)
                    return null;
                else
                    return PacketBlock.FromBytes(buf);
            }

            public void WriteObject(BinaryWriter writer, PacketBlock value)
            {
                writer.Write(value.DataBytes);
            }
        }

        private static PacketBlock FromBytes(byte[] bytes)
        {
            var packetBlock = new PacketBlock()
            {
                m_data = new _PacketBlock(bytes)
            };
            return packetBlock;            
        }
    }


  /*
    class PacketBlockSerializer : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(PacketBlock));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Newtonsoft.Json.Linq.JObject jsonObject = Newtonsoft.Json.Linq.JObject.Load(reader);
            var properties = jsonObject.Properties().ToDictionary(x => x.Name);
            var key = Newtonsoft.Json.JsonConvert.DeserializeObject<FlowKey>(properties[PacketBlockKey].Value.ToString());
            IEnumerable<PacketMetadata> items = properties[PacketBlockItems].Values<PacketMetadata>();
            return new PacketBlock()
            {
                Key = key,
                m_packetFeatures = items.ToList()
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //serializer.Converters.Add(new PacketFeaturesSerializer());
            var data = value as PacketBlock;
            writer.WriteStartObject();

            writer.WritePropertyName(PacketBlockKey);
            serializer.Serialize(writer, data.Key);

            writer.WritePropertyName(PacketBlockItems);
            writer.WriteStartArray();
            foreach (var x in data.m_packetFeatures)
            {
                serializer.Serialize(writer, x);
            }
            writer.WriteEndArray();

            writer.WriteEndObject();
        }
    }
    */
}
