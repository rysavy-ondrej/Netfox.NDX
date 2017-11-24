using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using Newtonsoft.Json.Linq;

namespace Ndx.Captures
{

    public class JsonPacket
    {
        long m_timestamp;
        JToken m_layers;
        string[] m_protocols;

        internal JsonPacket(JToken rootToken)
        {
            m_layers = rootToken["layers"];
            m_timestamp = ((long)rootToken["timestamp"]);
            m_protocols = m_layers.Value<JToken>("frame")?.Value<string>("frame_frame_protocols")?.Split(':') ?? new String[0];
        }

        public long Timestamp => m_timestamp;
        public long TimestampTicks => DateTimeOffset.FromUnixTimeMilliseconds(m_timestamp).Ticks;
        public DateTimeOffset TimestampDateTime => DateTimeOffset.FromUnixTimeMilliseconds(m_timestamp);

        public string[] Protocols => m_protocols;

        public JToken GetProtocol(string name)
        {
            return m_layers[name];
        }
    }
    /// <summary>
    /// This class represents pcap file produced by TSHARK -T ek command. This is New-line delimited JSON file. 
    /// </summary>
    public class PcapJsonStream : IDisposable
    {
        StreamReader m_reader;
        public PcapJsonStream(StreamReader reader)
        {
            m_reader = reader;
        }

        /// <summary>
        /// Reads next available packet and provides it as a single line string.
        /// </summary>
        /// <returns></returns>
        public string ReadPacketLine()
        {
            return ReadInternal<string>(x => x);
        }

        /// <summary>
        /// Reads next available packet from JSON source and provides it as <see cref="JsonPacket"/> object.
        /// </summary>
        /// <returns>New <see cref="JsonPacket"/> object or null if no more packets are available. </returns>
        public JsonPacket ReadPacket()
        {
            return ReadInternal(line => new JsonPacket(JToken.Parse(line)));
        }

        protected T ReadInternal<T>(Func<string,T> decoder)
        {
            while (true)
            {
                var line = m_reader.ReadLine();
                if (line == null) return default(T);
                if (line.StartsWith("{\"timestamp\""))
                {
                    return decoder(line);
                }
            }
        }

        /// <summary>
        /// Reads the single packet from the source stream.
        /// </summary>
        /// <returns>Next <see cref="DecodedFrame"/> object or null if the end of source stream was reached.</returns>
        public DecodedFrame Read(Func<String, String, String,Tuple<String,Variant>> customDecoder = null)
        {
            return ReadInternal<DecodedFrame>(line => DecodeJsonLine(line, customDecoder));
        }

        /// <summary>
        /// Decodes JSON line produced by TSHARK to <see cref="DecodedFrame"/> object.
        /// </summary>
        /// <param name="line">An input line to decode.</param>
        /// <param name="customDecoder">Function that is called for each field. This function is used to filter and format the fields.</param>
        /// <returns>A single <see cref="DecodedFrame"/> consisting of fields that passes <paramref name="customDecoder"/> function.</returns>
        public static DecodedFrame DecodeJsonLine(string line, Func<String,String,String,Tuple<String,Variant>> customDecoder = null)
        {
            var jsonObject = JToken.Parse(line);
            var layers = jsonObject["layers"];
            var frame = layers["frame"];

            var timestamp = ((long)jsonObject["timestamp"]);
            var framenumber = (int)frame["frame_frame_number"];
            var frameprotocols = (string)frame["frame_frame_protocols"];
            var frameLength = (int)frame["frame_frame_len"];
            var result = new DecodedFrame()
            {
                Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).Ticks,
                FrameNumber = framenumber,
                FrameProtocols = frameprotocols
            };

            result.Fields["timestamp"] = new Variant(timestamp);
            result.Fields["frame_number"] = new Variant(framenumber);
            result.Fields["frame_len"] = new Variant(frameLength);
            var protocols = result.FrameProtocols.Split(':');
            foreach (var proto in protocols)
            {
                var fields = layers[proto];
                if (fields != null)
                {
                    foreach (var _field in fields)
                    {
                        var field = (JProperty)_field;
                        if (field?.Value.Type == JTokenType.String)
                        {

                            if (customDecoder != null)
                            {
                                var nameValueTuple = customDecoder(proto, field.Name, (string)field.Value);
                                if (nameValueTuple != null)
                                {
                                    result.Fields[nameValueTuple.Item1.Replace('.','_')] = nameValueTuple.Item2;
                                }
                            }
                            else
                            {

                                // field name has a prefix, for instance:
                                // dns_dns_count_answers 
                                // dns_flags_dns_flags_opcode
                                // the following tries to remove this prefix:
                                var fieldNameParts = field.Name.Split('_');
                                var fieldNameCore = fieldNameParts.Skip(1).SkipWhile(s => !s.Equals(proto)).ToArray();
                                if (fieldNameCore.Count() > 0)
                                {
                                    var fieldName = String.Join("_", fieldNameCore);

                                    result.Fields[fieldName] = new Variant((string)field.Value);

                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public void Dispose()
        {
            ((IDisposable)m_reader).Dispose();
        }
    }
}
