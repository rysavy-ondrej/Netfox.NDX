// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
// Forked from Microsoft.Tx project.


using System;

using System.Collections.Generic;

using System.IO;



// The Pcap format is: https://wiki.wireshark.org/Development/LibpcapFileFormat

// The C# implementation below reads files in .pcap format



namespace Ndx.Network
{
    public class Pcap
    {
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
                int pos = 0;
                int length = (int)reader.BaseStream.Length;
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
    }



    public class PcapRecord

    {
        public DateTimeOffset Timestamp;

        public uint NetworkId;

        public byte[] Data;

        /// <summary>
        /// Ofsets of the frame's data within the capture file.
        /// </summary>
        public long DataOffset;
    }

}