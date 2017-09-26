//
// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
// Forked from Microsoft.Tx project.


using System;

using System.Collections.Generic;

using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Google.Protobuf;
using Ndx.Model;



// The Pcap format is: https://wiki.wireshark.org/Development/LibpcapFileFormat

// The C# implementation below reads files in .pcap format



namespace Ndx.Captures
{
    /// <summary>
    /// Implements the read access to Libpcap file format. 
    /// </summary>
    public class LibPcapStream : IDisposable
    {
        const long UnixBaseTicks = 621355968000000000; // new DateTime(1970, 1, 1).Ticks;
        const long TickPerMicroseconds = 10; // TimeSpan.TicksPerMillisecond / 1000)

        BinaryReader m_reader;
        int m_frameNumber;
        DataLinkType m_network;
        public LibPcapStream(FileStream stream)
        {
            m_reader = new BinaryReader(stream);
            ReadHeader();
        }

        public void ReadHeader()
        {
            var magicNumber = m_reader.ReadUInt32();
            var version_major = m_reader.ReadUInt16();
            var version_minor = m_reader.ReadUInt16();
            var thiszone = m_reader.ReadInt32();
            var sigfigs = m_reader.ReadUInt32();
            var snaplen = m_reader.ReadUInt32();
            m_network = (DataLinkType)m_reader.ReadUInt32();
        }

        public Frame Read()
        {
            if (m_reader.BaseStream.Position + 16 <= m_reader.BaseStream.Length)
            {
                var tsSeconds = m_reader.ReadUInt32();
                var tsMicroseconds = m_reader.ReadUInt32();
                var ticks = UnixBaseTicks + (tsSeconds * TimeSpan.TicksPerSecond) + (tsMicroseconds * TickPerMicroseconds);
                var includedLength = m_reader.ReadUInt32();
                var originalLength = m_reader.ReadUInt32();

                if ((m_reader.BaseStream.Position + includedLength) <= m_reader.BaseStream.Length)
                {

                    var frameOffset = m_reader.BaseStream.Position;
                    var frameBytes = m_reader.ReadBytes((int)includedLength);

                    return new Frame
                    {
                        Data = ByteString.CopyFrom(frameBytes, 0, frameBytes.Length),
                        FrameOffset = frameOffset,
                        FrameLength = frameBytes.Length,
                        FrameNumber = ++m_frameNumber,
                        LinkType = m_network,
                        TimeStamp = ticks,
                    };
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        public const uint MagicNumber = 0xa1b2c3d4;
        public const ushort VersionMajor = 0x0002;
        public const ushort VersionMinor = 0x0004;
        public const uint ThisZone = 0;
        public const uint Sigfigs = 0;
        public const uint Snaplen = UInt16.MaxValue;

        /// <summary>
        /// Creates a new file, write the specified frame array to the file, and then closes the file.
        /// </summary>
        /// <param name="path">The file to write to.</param>
        /// <param name="network">The link type.</param>
        /// <param name="contents">The raw frame array to write to the file.</param>
        public static async Task WriteAllFramesAsync(string path, DataLinkType network, IObservable<Frame> frames)
        {

            using (var stream = File.Create(path))
            {
                async Task writeAsync(byte[] buffer)
                {
                    await stream.WriteAsync(buffer, 0, buffer.Length);
                }
                // WRITE HEADER:
                await writeAsync(BitConverter.GetBytes(MagicNumber));
                await writeAsync(BitConverter.GetBytes(VersionMajor));
                await writeAsync(BitConverter.GetBytes(VersionMinor));
                await writeAsync(BitConverter.GetBytes(ThisZone));
                await writeAsync(BitConverter.GetBytes(Sigfigs));
                await writeAsync(BitConverter.GetBytes(Snaplen));
                await writeAsync(BitConverter.GetBytes((uint)network));

                // WRITE RECORDS
                await frames.ForEachAsync(async frame =>
                {
                    uint ts_sec = (uint)frame.Seconds;
                    uint ts_usec = (uint)frame.Microseconds;
                    uint incl_len = (uint)frame.Data.Length;
                    uint orig_len = (uint)frame.Data.Length;
                    await writeAsync(BitConverter.GetBytes(ts_sec));
                    await writeAsync(BitConverter.GetBytes(ts_usec));
                    await writeAsync(BitConverter.GetBytes(incl_len));
                    await writeAsync(BitConverter.GetBytes(orig_len));
                    await writeAsync(frame.Bytes);
                });
            }
        }

        public void Dispose()
        {
            ((IDisposable)m_reader).Dispose();
        }
    }
}