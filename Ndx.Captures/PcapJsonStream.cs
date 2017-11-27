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
        /// <returns>String representation of packet.</returns>
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

        /// <summary>
        /// Reads the next packet from the source stream and applies provided decoder function.
        /// </summary>
        /// <typeparam name="T">The result type.</typeparam>
        /// <param name="decoder">Decoder function.</param>
        /// <returns>An object of type <typeparamref name="T"/>.</returns>
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

        public void Dispose()
        {
            ((IDisposable)m_reader).Dispose();
        }
    }
}
