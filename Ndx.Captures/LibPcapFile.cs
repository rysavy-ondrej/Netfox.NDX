//
// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
// Forked from Microsoft.Tx project.


using System;

using System.Collections.Generic;

using System.IO;
using Ndx.Model;



// The Pcap format is: https://wiki.wireshark.org/Development/LibpcapFileFormat

// The C# implementation below reads files in .pcap format



namespace Ndx.Captures
{
    public static class LibPcapFile
    {

        /// <summary>
        /// Represents a single record in Libpcap file. 
        /// </summary>
        public class PcapRecord
        {
            /// <summary>
            /// Timestamp of the record.
            /// </summary>
            public DateTimeOffset Timestamp;

            /// <summary>
            /// Network type.
            /// </summary>
            public uint NetworkId;

            /// <summary>
            /// Byte array containing packet data.
            /// </summary>
            public byte[] Data;

            /// <summary>
            /// Ofsets of the frame's data within the capture file.
            /// </summary>
            public long DataOffset;
        }
        /// <summary>
        /// Reads network capture file and returns the raw blocks in the order they were written
        /// </summary>
        /// <param name="filename">Path to the file in pcap-next-generation (.pcapng) format</param>
        /// <returns></returns>
        public static IEnumerable<PcapRecord> ReadFile(string filename)
        {
            var stream = File.OpenRead(filename);
            return ReadForward(stream);
        }

        public static IEnumerable<PcapRecord> ReadForward(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                long pos = 0;
                long length = reader.BaseStream.Length;
                if (length <= (24 + 16))
                {
                    yield break;
                }
                var magicNumber = reader.ReadUInt32();
                var version_major = reader.ReadUInt16();
                var version_minor = reader.ReadUInt16();
                var thiszone = reader.ReadInt32();
                var sigfigs = reader.ReadUInt32();
                var snaplen = reader.ReadUInt32();
                var network = reader.ReadUInt32();

                pos += 24;

                while ((pos + 16) < length)
                {
                    var ts_sec = reader.ReadUInt32();
                    var ts_usec = reader.ReadUInt32();
                    var incl_len = reader.ReadUInt32();
                    var orig_len = reader.ReadUInt32();
                    pos += 16;
                    if ((pos + incl_len) > length)
                    {
                        yield break;
                    }
                    var offset = reader.BaseStream.Position;
                    var data = reader.ReadBytes((int)incl_len);
                    pos += (int)incl_len;

                    yield return new PcapRecord
                    {
                        Data = data,
                        DataOffset = offset,
                        NetworkId = network,
                        Timestamp = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)
                            .AddSeconds(ts_sec + thiszone)
                            .AddMilliseconds((ts_usec == 0 || ts_usec >= 1000000) ? ts_usec : ts_usec / 1000)
                    };
                }
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
        public static void WriteAllFrames(string path, DataLinkType network, IEnumerable<RawFrame> frames)
        {
            var stream = File.Create(path);
            using (var writer = new BinaryWriter(stream))
            {
                // WRITE HEADER:
                writer.Write(MagicNumber);
                writer.Write(VersionMajor);
                writer.Write(VersionMinor);
                writer.Write(ThisZone);
                writer.Write(Sigfigs);
                writer.Write(Snaplen);
                writer.Write((uint)network);

                // WRITE RECORDS
                foreach (var frame in frames)
                {
                    uint ts_sec = (uint)frame.Seconds;
                    uint ts_usec = (uint)frame.Microseconds;
                    uint incl_len = (uint)frame.Data.Length;
                    uint orig_len = (uint)frame.Data.Length;
                    writer.Write(ts_sec);
                    writer.Write(ts_usec);
                    writer.Write(incl_len);
                    writer.Write(orig_len);
                    writer.Write(frame.Bytes);
                }
            }
            stream.Close();
        }
    }
}