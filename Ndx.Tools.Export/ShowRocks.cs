using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Ingest.Trace;
using Ndx.Shell.Commands;
using Newtonsoft.Json;
using RocksDbSharp;

namespace Ndx.Tools.Export
{
    /// <summary>
    /// Shows content of RocksDb.
    /// </summary>
    /// <remarks>
    /// Usage: -r bb7de71e185a2a7818fff92d3ec0dc05.rdb Show-Rocks
    /// </remarks>
    [Command(VerbsCommon.Show, "Rocks")]
    class ShowRocks : Command
    {

        /// <summary>
        /// Path to the folder with RocksDB generated for the input Metacap file.
        /// </summary>
        string m_rocksDbFolder;

        /// <summary>
        /// An instance of <see cref="RocksDb"/> class that is to be used for writing exported data.
        /// </summary>
        RocksDb m_rocksDb;
        private ColumnFamilyHandle m_flowsCollection;
        private ColumnFamilyHandle m_packetsCollection;

        /// <summary>
        /// Gets or sets the path to the output RocksDB root folder.
        /// </summary>
        [Parameter(Mandatory = true)]
        public string RocksDbFolder { get => m_rocksDbFolder; set => m_rocksDbFolder = value; }

        protected override void BeginProcessing()
        {
            try
            {
                var options = new DbOptions();
                var columnFamilies = new ColumnFamilies();
                columnFamilies.Add("flows", new ColumnFamilyOptions());
                columnFamilies.Add("packets", new ColumnFamilyOptions());
                m_rocksDb = RocksDb.Open(options, m_rocksDbFolder, columnFamilies);                
                m_flowsCollection = m_rocksDb.GetColumnFamily("flows");
                m_packetsCollection = m_rocksDb.GetColumnFamily("packets");
            }
            catch (Exception e)
            {
                WriteError(e, "Cannot process inout file.");
            }
        }


        protected override void EndProcessing()
        {
            m_rocksDb?.Dispose();
        }


        protected override void ProcessRecord()
        {
            if (m_rocksDb==null)
            {
                WriteDebug("Non-existing Rocks database!");
                return;
            }

            WriteObject("{");
            WriteObject("\"flows\": [");
            using (var iter = m_rocksDb.NewIterator(m_flowsCollection))
            {
                iter.SeekToFirst();
                while (iter.Valid())
                {
                    var flowKey = new FlowKey(iter.Key());
                    var value = new FlowRecord(iter.Value());                                        
                    iter.Next();
                    var eol = iter.Valid() ? "," : "";
                    WriteObject($"{{ \"key\":\"{flowKey}\", \"value\" : {JsonConvert.SerializeObject(value, FlowRecordSerializer.Instance)} }} {eol}");
                }
            }
            WriteObject("],");
            WriteObject("\"packets\": [");
            using (var iter = m_rocksDb.NewIterator(m_packetsCollection))
            {
                iter.SeekToFirst();
                while (iter.Valid())
                {
                    var flowKey = new FlowKey(iter.Key());
                    var value = iter.Value();

                    WriteObject($"{{ \"key\":\"{flowKey}\" , \"items\" : [");
                    var count = BitConverter.ToInt32(value, 0);
                    for (int i=0; i<count; i++)
                    {
                        var pm = new PacketMetadata(value, sizeof(int) + i * PacketMetadata.MetadataSize);
                        var valstr = JsonConvert.SerializeObject(pm, PacketMetadataSerializer.Instance, ByteRangeSerializer.Instance, FrameMetadataSerializer.Instance);
                        var eol1 = i<count-1 ? "," : "";
                        WriteObject($"{valstr}{eol1}");

                    }
                    iter.Next();
                    var eol = iter.Valid() ? "," : "";
                    WriteObject($"] }} {eol}");                                         
                }
            }
            WriteObject("]");
            WriteObject("}");
        }
    }

    public class FlowRecordSerializer : JsonConverter
    {
        public static readonly string FlowOctets = "octets";
        public static readonly string FlowPackets = "packets";
        public static readonly string FlowFirst = "first";
        public static readonly string FlowLast = "last";
        public static readonly string FlowApplication = "application";



        static Lazy<FlowRecordSerializer> m_instance = new Lazy<FlowRecordSerializer>(() => new FlowRecordSerializer());
        public static JsonConverter Instance => m_instance.Value;


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

                RecognizedProtocol = ParseEnum<ApplicationProtocol>(properties[FlowApplication].Value.ToString(), ApplicationProtocol.NULL),

            };
            return newObj;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var data = value as FlowRecord;
            writer.WriteStartObject();

            writer.WritePropertyName(FlowFirst);
            writer.WriteValue(data.FirstSeen);

            writer.WritePropertyName(FlowLast);
            writer.WriteValue(data.LastSeen);

            writer.WritePropertyName(FlowOctets);
            writer.WriteValue(data.Octets);

            writer.WritePropertyName(FlowPackets);
            writer.WriteValue(data.Packets);

            writer.WritePropertyName(FlowApplication);
            writer.WriteValue(data.RecognizedProtocol.ToString());

            writer.WriteEndObject();
        }
    }

    public class PacketMetadataSerializer : JsonConverter
    {
        static Lazy<PacketMetadataSerializer> m_instance = new Lazy<PacketMetadataSerializer>(() => new PacketMetadataSerializer());
        public static JsonConverter Instance => m_instance.Value;

        public override bool CanConvert(Type objectType)
        {
            return typeof(PacketMetadata).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var data = value as PacketMetadata;
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(data.Frame).ToLower());

            serializer.Serialize(writer, data.Frame);

            writer.WritePropertyName(nameof(data.Link).ToLower());
            serializer.Serialize(writer, data.Link);

            writer.WritePropertyName(nameof(data.Network).ToLower());
            serializer.Serialize(writer, data.Network);

            writer.WritePropertyName(nameof(data.Transport).ToLower());
            serializer.Serialize(writer, data.Transport);

            writer.WritePropertyName(nameof(data.Payload).ToLower());
            serializer.Serialize(writer, data.Payload);

            writer.WriteEndObject();
        }
    }

    public class ByteRangeSerializer : JsonConverter
    {
        static Lazy<ByteRangeSerializer> m_instance = new Lazy<ByteRangeSerializer>(() => new ByteRangeSerializer());
        public static JsonConverter Instance => m_instance.Value;

        public override bool CanConvert(Type objectType)
        {
            return typeof(_ByteRange).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var data = (_ByteRange)value;
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(data.Start).ToLower());
            writer.WriteValue(data.Start);

            writer.WritePropertyName(nameof(data.Count).ToLower());
            writer.WriteValue(data.Count);

            writer.WriteEndObject();
        }
    }

    public class FrameMetadataSerializer : JsonConverter
    {
        static Lazy<FrameMetadataSerializer> m_instance = new Lazy<FrameMetadataSerializer>(() => new FrameMetadataSerializer());
        public static JsonConverter Instance => m_instance.Value;

        public override bool CanConvert(Type objectType)
        {
            return typeof(FrameMetadata).Equals(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var data = (FrameMetadata)value;
            writer.WriteStartObject();

            writer.WritePropertyName(nameof(data.FrameNumber).ToLower());
            writer.WriteValue(data.FrameNumber);

            writer.WritePropertyName(nameof(data.FrameLength).ToLower());
            writer.WriteValue(data.FrameLength);

            writer.WritePropertyName(nameof(data.FrameOffset).ToLower());
            writer.WriteValue(data.FrameOffset);

            writer.WritePropertyName(nameof(data.Timestamp).ToLower());
            writer.WriteValue(data.Timestamp);

            writer.WriteEndObject();
        }
    }

}
