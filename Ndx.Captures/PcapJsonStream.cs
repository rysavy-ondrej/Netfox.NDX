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
            var result = new PacketFields()
            {
                Timestamp = (long)jsonObject["timestamp"],
                FrameNumber = (int)frame["frame_frame_number"],
                FrameProtocols = (string)frame["frame_frame_protocols"]
            };

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
                            result.Fields.Add(field.Name, (string)field.Value);
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
