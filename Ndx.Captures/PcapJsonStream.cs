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
        /// <returns>Next <see cref="PacketFields"/> object or null if the end of source stream was reached.</returns>
        public PacketFields Read()
        {
            while (true)
            {
                var line = m_reader.ReadLine();
                if (line == null) return null;
                if (line.StartsWith("{\"timestamp\""))
                {
                    return DecodeJsonLine(line);
                }
            }
        }

        /// <summary>
        /// Decodes JSON line produced by TSHARK to <see cref="PacketFields"/> object.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static PacketFields DecodeJsonLine(string line)
        {
            var jsonObject = JToken.Parse(line);
            var layers = jsonObject["layers"];
            var frame = layers["frame"];

            var timestamp = ((long)jsonObject["timestamp"]);
            var framenumber = (int)frame["frame_frame_number"];
            var frameprotocols = (string)frame["frame_frame_protocols"];

            var result = new PacketFields()
            {
                Timestamp = EpochMsToTick(timestamp),
                FrameNumber = framenumber,
                FrameProtocols = frameprotocols
            };

            result.Fields["timestamp"] = timestamp.ToString();
            result.Fields["frame_number"] = framenumber.ToString();
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
                            // field name has a prefix, for instance:
                            // dns_dns_count_answers 
                            // dns_flags_dns_flags_opcode
                            // the following tries to remove this prefix:
                            var fieldNameParts = field.Name.Split('_');
                            var fieldNameCore = fieldNameParts.Skip(1).SkipWhile(s => !s.Equals(proto)).ToArray();
                            if (fieldNameCore.Count() > 0)
                            {
                                var fieldName = String.Join("_", fieldNameCore);
                                result.Fields[fieldName] = (string)field.Value;
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
