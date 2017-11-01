using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using PacketDotNet;

namespace Ndx.Captures
{
    public enum PcapFileFormat { Libpcap, Netmon, Pcapng, Json, UnknownFormat }
    /// <summary>
    /// Provides static methods for the creation and opening of Pcap files and provides IObservable interface to these files.
    /// </summary>
    public static class PcapFile
    {
        /// <summary>
        /// Tries to detect file type from the magic number bytes.
        /// </summary>       
        /// <param name="buf">Al least first four bytes of the file.</param>
        /// <returns><see cref="PcapType"/> detected from the provided bytes.</returns>
        /// <remarks>
        /// The magic numbers for three recognized and supported file formats are:
        /// pcap:   0xa1b2c3d4 or 0xd4c3b2a1 (swapped) 
        /// pcapng: 0x0A0D0D0A 
        /// netmon: 47 4D 42 55 
        /// </remarks>
        public static PcapFileFormat DetectFileFormat(string path)
        {
            byte[] buf = new byte[4];
            using (var stream = File.OpenRead(path))
            {
                if (stream.Read(buf, 0, 4) == 4)
                {
                    if (buf[0] == 0xa1 && buf[1] == 0xb2 && buf[2] == 0xc3 && buf[3] == 0xd4) return PcapFileFormat.Libpcap;
                    if (buf[0] == 0xd4 && buf[1] == 0xc3 && buf[2] == 0xb2 && buf[3] == 0xa1) return PcapFileFormat.Libpcap;
                    if (buf[0] == 0x47 && buf[1] == 0x4d && buf[2] == 0x42 && buf[3] == 0x55) return PcapFileFormat.Netmon;
                    if (buf[0] == 0x0a && buf[1] == 0x0d && buf[2] == 0x0d && buf[3] == 0x0a) return PcapFileFormat.Pcapng;
                    return PcapFileFormat.UnknownFormat;
                }
                else
                {
                    return PcapFileFormat.UnknownFormat;
                }
            }
        }


        public static IObservable<DecodedFrame> ReadJson(string path, Func<String, String, String, Tuple<String, Variant>> customDecoder = null)
        {
            return Observable.Using(() => File.OpenText(path), stream =>
            {
                var source = new PcapJsonStream(stream);
                var observable = Observable.Create<DecodedFrame>(obs =>
                {

                    var frame = source.Read();
                    while (frame != null)
                    {
                        obs.OnNext(frame);
                        frame = source.Read(customDecoder);
                    }
                    obs.OnCompleted();
                    return Disposable.Create(() => { });
                });
                return observable;
            });
        }

        public static IObservable<Frame> ReadLibpcap(string path)
        {
            return Observable.Using(() => File.OpenRead(path), stream =>
            {
                var source = new LibPcapStream(stream);
                var observable = Observable.Create<Frame>(obs =>
                {

                    var frame = source.Read();
                    while (frame != null)
                    {
                        obs.OnNext(frame);
                        frame = source.Read();
                    }
                    obs.OnCompleted();
                    return Disposable.Create(() => { });
                });
                return observable;
            });
        }

        public static IObservable<Frame> ReadNetmon(string path)
        {
            return Observable.Using(() => File.OpenRead(path), stream =>
            {
                var source = new PcapNetmonStream(stream);
                var observable = Observable.Create<Frame>(obs =>
                {

                    var frame = source.Read();
                    while (frame != null)
                    {
                        obs.OnNext(frame);
                        frame = source.Read();
                    }
                    obs.OnCompleted();
                    return Disposable.Create(() => { });
                });
                return observable;
            });
        }

        public static IObservable<Frame> ReadFile(string path)
        {
            var fileFormat = DetectFileFormat(path);
            switch (fileFormat)
            {
                case PcapFileFormat.Libpcap: return ReadLibpcap(path);
                case PcapFileFormat.Netmon: return ReadNetmon(path);
                case PcapFileFormat.Pcapng: throw new NotImplementedException();
                default: throw new NotSupportedException("The specified file is not any of supported formats.");
            }
        }
    }
}
