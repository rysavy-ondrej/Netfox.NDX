//
// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
// Forked from Microsoft.Tx project.


using System;

using System.Collections.Generic;

using System.IO;
using Google.Protobuf;
using Ndx.Model;



// The Pcap format is: https://wiki.wireshark.org/Development/LibpcapFileFormat

// The C# implementation below reads files in .pcap format



namespace Ndx.Captures
{
    public static class LibPcapFile
    {
        const long UnixBaseTicks = 621355968000000000; // new DateTime(1970, 1, 1).Ticks;
        const long TickPerMicroseconds = 10; // TimeSpan.TicksPerMillisecond / 1000)
        /// <summary>
        /// Reads network capture file and returns the raw blocks in the order they were written
        /// </summary>
        /// <param name="filename">Path to the file in pcap-next-generation (.pcapng) format</param>
        /// <returns></returns>
        public static IEnumerable<RawFrame> ReadFile(string filename)
        {
            var stream = File.OpenRead(filename);
            return ReadForward(stream);
        }

        public static IEnumerable<RawFrame> ReadForward(Stream stream)
        {
            using (var reader = new BinaryReader(stream))
            {
                long length = stream.Length;
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
                var frameNumber = 0;
                while ((stream.Position + 16) < length)
                {
                    var tsSeconds = reader.ReadUInt32();
                    var tsMicroseconds = reader.ReadUInt32();
                    var ticks = UnixBaseTicks + (tsSeconds * TimeSpan.TicksPerSecond) + (tsMicroseconds * TickPerMicroseconds);
                    var includedLength = reader.ReadUInt32();
                    var originalLength = reader.ReadUInt32();
                    
                    if ((stream.Position + includedLength) > length)
                    {   // not enough data to read packet
                        yield break;
                    }
                    var frameOffset = stream.Position;
                    var frameBytes = reader.ReadBytes((int)includedLength);
                    
                    yield return new RawFrame
                    {
                        Data = ByteString.CopyFrom(frameBytes, 0, frameBytes.Length),
                        FrameOffset = frameOffset,
                        FrameLength = frameBytes.Length,
                        FrameNumber = ++frameNumber,
                        LinkType = (DataLinkType)network,
                        TimeStamp = ticks, 
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