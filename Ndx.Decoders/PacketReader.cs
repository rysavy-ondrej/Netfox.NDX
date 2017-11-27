using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using Newtonsoft.Json.Linq;
using System.Reactive.Linq;
using System.Reactive.Disposables;

namespace Ndx.Decoders
{
    /// <summary>
    /// This class represents pcap file produced by TSHARK -T ek command. This is New-line delimited JSON file. 
    /// </summary>
    public class PacketReader : IDisposable
    {
        Stream m_stream;
        public PacketReader(Stream stream)
        {
            m_stream = stream;
        }

        /// <summary>
        /// Reads next available packet from JSON source and provides it as <see cref="JsonPacket"/> object.
        /// </summary>
        /// <returns>New <see cref="JsonPacket"/> object or null if no more packets are available. </returns>
        public Packet ReadPacket()
        {
            if (m_stream.Position < m_stream.Length)
            {
                return Packet.Parser.ParseDelimitedFrom(m_stream);
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
            ((IDisposable)m_stream).Dispose();
        }

        public static IEnumerable<Packet> ReadAllPackets(string path)
        {
            return ReadPackets(path).ToEnumerable();
        }

        public static IObservable<Packet> ReadPackets(string path)
        {
            return Observable.Using(() => File.OpenRead(path), stream =>
            {
                var reader = new PacketReader(stream);                         
                var observable = Observable.Create<Packet>(obs =>
                {

                    var frame = reader.ReadPacket();
                    while (frame != null)
                    {
                        obs.OnNext(frame);
                        frame = reader.ReadPacket();
                    }
                    obs.OnCompleted();
                    return Disposable.Create(() => { });
                });
                return observable;
            });
        }


    }
}
