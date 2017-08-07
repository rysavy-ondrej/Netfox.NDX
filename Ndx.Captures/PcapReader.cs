//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Ndx.Model;
using Ndx.Utils;
using Google.Protobuf;
using System.Threading.Tasks.Dataflow;

namespace Ndx.Captures
{

    /// <summary>
    /// Provides a unified access to packet captures of supported formats.
    /// </summary>
    public static class PcapReader
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
        private static PcapFileFormat DetectPcapFileFormat(byte[] buf)
        {
            Debug.Assert(buf.Length >= 4);
            if (buf[0] == 0xa1 && buf[1] == 0xb2 && buf[2] == 0xc3 && buf[3] == 0xd4) return PcapFileFormat.Libpcap;
            if (buf[0] == 0xd4 && buf[1] == 0xc3 && buf[2] == 0xb2 && buf[3] == 0xa1) return PcapFileFormat.Libpcap;
            if (buf[0] == 0x47 && buf[1] == 0x4d && buf[2] == 0x42 && buf[3] == 0x55) return PcapFileFormat.Netmon;
            if (buf[0] == 0x0a && buf[1] == 0x0d && buf[2] == 0x0d && buf[3] == 0x0a) return PcapFileFormat.Pcapng;
            return PcapFileFormat.UnknownFormat;
        }
        private static LinkLayers GetLinkType(MediaType media)
        {
            switch (media)
            {
                case MediaType.Ethernet: return LinkLayers.Ethernet;
                case MediaType.ATM: return LinkLayers.AtmRfc1483;
                case MediaType.Wifi: return LinkLayers.Ieee80211;
                default:
                    return LinkLayers.Null;                    
            }
        }

        /// <summary>
        /// Reads the capture file at the specified path. It automatically analyzes type of capture file and applies to corresponding reader. 
        /// This method is implemented by using deferred execution.
        /// </summary>
        /// <param name="path">Path to the capture file.</param>
        /// <returns>Enumerable collection of <c>CapturedFrame</c> object.</returns>
        /// <remarks>
        /// Three types of capture files are currently supported: i)PcapLib, ii)PcapNg and iii) NetMon 3 cap file.
        /// As convenient for Wireshar and Network Monitor, frames are numbered from 1.
        /// </remarks>
        public static IEnumerable<RawFrame> ReadFile(string path)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
            {
                throw new ArgumentException($"Specified file '{path}' cannot be found.");
            }

            using (var stream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan))
            {
                var magicNumber = new byte[4];
                var len = stream.Read(magicNumber, 0, 4);
                if (len < 4)
                {
                    throw new ArgumentException($"Specified file '{path}' is corrupted, cannot identify its type.");
                }
                stream.Seek(0, SeekOrigin.Begin);
                switch (DetectPcapFileFormat(magicNumber))
                {
                    case PcapFileFormat.Netmon:
                        {
                            foreach (var frameRecord in PcapNetmon.ReadForward(stream))
                            {
                                yield return new RawFrame()
                                {
                                    Data = ByteString.CopyFrom(frameRecord.Data),
                                    TimeStamp = frameRecord.Timestamp.Ticks,
                                    LinkType = (DataLinkType)GetLinkType(frameRecord.MediaType),
                                    FrameNumber = frameRecord.FrameNumber,
                                    FrameLength = frameRecord.Data.Length,
                                    FrameOffset = frameRecord.DataOffset,
                                    ProcessId = frameRecord.Pid,
                                    ProcessName = frameRecord.ProcessName
                                };
                            }
                            break;
                        }
                    case PcapFileFormat.Libpcap:
                        {
                            // this iteration is needed because we have "yield return" inside "using" so 
                            // we have to avoid disposing the stream before we read all frames.
                            foreach (var frame in LibPcapFile.ReadForward(stream))
                            {
                                yield return frame;
                            }
                            break;
                        }
                    case PcapFileFormat.Pcapng:
                        {
                            throw new NotSupportedException("PCAP-NG format is not supported yet.");
                        }
                    default:
                        throw new NotImplementedException("Unknown packet type.");
                }
            }
        }
    }
}