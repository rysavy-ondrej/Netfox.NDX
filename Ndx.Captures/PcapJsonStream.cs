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
        /// Reads the single packet from the source stream.
        /// </summary>
        /// <returns>Next <see cref="DecodedFrame"/> object or null if the end of source stream was reached.</returns>
        public DecodedFrame Read(Func<String, String, String,Tuple<String,Variant>> customDecoder = null)
        {
            while (true)
            {
                var line = m_reader.ReadLine();
                if (line == null) return null;
                if (line.StartsWith("{\"timestamp\""))
                {
                    return DecodeJsonLine(line, customDecoder);
                }
            }
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
                Timestamp = EpochMsToTick(timestamp),
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

        private static long EpochMsToTick(long epochms)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(epochms).Ticks;
        }

        public void Dispose()
        {
            ((IDisposable)m_reader).Dispose();
        }
    }
}
