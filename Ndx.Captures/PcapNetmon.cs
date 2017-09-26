//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
// The Microsoft Network Monitor capture format is based on the implementation by Jiri Formacek: https://code.msdn.microsoft.com/How-to-parse-Network-f6162019
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ndx.Model;
using PacketDotNet;

namespace Ndx.Captures
{
    public class PcapNetmonStream
    {
        BinaryReader m_reader;
        int m_frameNumber;
        LinkType m_network;
        uint[] m_frameTableOffsets;
        CapFileHeader m_header;
        ProcessInfo[] m_processInfoTable;

        public PcapNetmonStream(Stream stream)
        {
            m_reader = new BinaryReader(stream);
            m_header = CapFileHeader.ReadFrom(m_reader);
            m_processInfoTable = ReadProcessInfoTable(m_header, m_reader);
            m_network = (LinkType)m_header.MacType;
            m_frameTableOffsets = ReadFrameTable(m_reader, m_header);
        }

        /// <summary>
        /// Reads a stream and returns the raw blocks in the order they were written
        /// </summary>
        /// <param name="stream">Stream in pcap-next-generation format</param>
        /// <returns></returns>
        public Frame Read()
        {   
            m_reader.BaseStream.Seek((long)m_frameTableOffsets[m_frameNumber], SeekOrigin.Begin);
            var frameStruct = FrameLayout.Read(m_reader);
            return new Frame()
            {                
                TimeStamp = (m_header.TimeStamp.ToDateTime() + TimeSpan.FromMilliseconds(((double)frameStruct.TimeOffsetLocal) / 1000)).ToBinary(),
                LinkType = GetLinkLayers((MediaType)frameStruct.MediaType),
                FrameOffset = frameStruct.FrameDataOffset,
                Bytes = frameStruct.FrameData,
                ProcessId = frameStruct.ProcessInfoIndex < m_processInfoTable.Length ? m_processInfoTable[frameStruct.ProcessInfoIndex].PID : 0,
                ProcessName = frameStruct.ProcessInfoIndex < m_processInfoTable.Length ? m_processInfoTable[frameStruct.ProcessInfoIndex].ProcessName : String.Empty
            };
        }

        public static DataLinkType GetLinkLayers(MediaType media)
        {
            switch (media)
            {
                case MediaType.Ethernet: return DataLinkType.Ethernet;
                case MediaType.ATM: return DataLinkType.AtmRfc1483;
                case MediaType.Wifi: return DataLinkType.Ieee80211;
                default:
                    return DataLinkType.Null;
            }
        }

        /// <summary>
        /// Reads the complete frame table using the specified BinaryReader. 
        /// </summary>
        /// <param name="reader">A BinaryReader object used to read frame table.</param>
        /// <param name="capHdr">Capture header structure.</param>
        /// <returns>An array of uint offsets from the start of the file to the frame info for each frame in the file.</returns>
        private static uint[] ReadFrameTable(BinaryReader reader, CapFileHeader capHdr)
        {
            var frameTableOffsets = new uint[capHdr.FrameTableLength / sizeof(uint)];
            reader.BaseStream.Seek(capHdr.FrameTableOffset, SeekOrigin.Begin);
            for (int i = 0; i < frameTableOffsets.Length; i++)
            {
                frameTableOffsets[i] = reader.ReadUInt32();
            }

            return frameTableOffsets;
        }

        /// <summary>
        /// Reads the comment infor table using the specified BinaryReader.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="capHdr"></param>
        /// <returns></returns>
        private static IEnumerable<CommentInfo> ReadCommentInfoTable(BinaryReader reader, CapFileHeader capHdr)
        {
            reader.BaseStream.Seek(capHdr.CommentDataOffset, SeekOrigin.Begin);
            var cdEnd = capHdr.CommentDataOffset + capHdr.CommentDataLength;
            while(reader.BaseStream.Position < cdEnd)
            {
                 yield return CommentInfo.Read(reader);
            }
        }

        /// <summary>
        /// Reads process info table and returns it as an array.
        /// </summary>
        /// <param name="capHdr">Capture header structure.</param>
        /// <param name="reader">Reader that can be used to read data from capture file.</param>
        /// <returns>An array of ProcessInfo objects.</returns>
        private static ProcessInfo[] ReadProcessInfoTable(CapFileHeader capHdr, BinaryReader reader)
        {
            // read frame table:
            reader.BaseStream.Seek(capHdr.ProcessInfoTableOffset, SeekOrigin.Begin);
            var processInfoTable = new ProcessInfo[capHdr.ProcessInfoTableCount];
            var version = reader.ReadUInt16();
            for (int i = 0; i < capHdr.ProcessInfoTableCount; i++)
            {
                processInfoTable[i] = ProcessInfo.Read(reader);
            }
            return processInfoTable;

        }    
    }

    /// <summary>
    /// The FrameLayout is the structure that holds for the raw bytes captured on the network including some header information.
    /// </summary>
    public class FrameLayout
    {
        /// <summary>
        /// The relative time offset of the frame, in microseconds. This is the local time offset from the timestamp in the capture file header.
        /// </summary>
        public ulong TimeOffsetLocal;
        /// <summary>
        /// Original length of the frame as captured on the wire. This value must be greater than zero.
        /// </summary>
        public uint FrameLengthWire;
        /// <summary>
        /// Number of actual bytes found in Frame Data and copied. This can be smaller than FrameLengthWire if incomplete frames are captured.
        /// </summary>
        public uint FrameLength;
        /// <summary>
        /// Pointer to the beginning of the raw frame data.
        /// </summary>
        public byte[] FrameData;
        /// <summary>
        /// Media type of the computer on which the frame data was captured.
        /// </summary>
        public ushort MediaType;
        /// <summary>
        /// Index of the Process Info array, or 0xFFFFFFFF if no related process exists. 
        /// </summary>
        public uint ProcessInfoIndex;
        /// <summary>
        /// FILETIME structure UTC timestamp. This is used if and only if ExtendedInfoOffset in the capture file header is not 0.
        /// </summary>
        public ulong TimeStamp;
        /// <summary>
        /// Index of the time zone information table. This is used if and only if ExtendedInfoOffset in the capture file header is not 0.
        /// </summary>
        public byte TimeZoneIndex;

        /// <summary>
        /// Stores an absolute location of frame's data in the capture file. 
        /// </summary>
        public long FrameDataOffset;

        internal static FrameLayout Read(BinaryReader reader)
        {
            var fs = new FrameLayout()
            {
                TimeOffsetLocal = reader.ReadUInt64(),
                FrameLengthWire = reader.ReadUInt32(),
                FrameLength = reader.ReadUInt32(),
                FrameDataOffset = reader.BaseStream.Position
            };
            fs.FrameData = reader.ReadBytes((int)fs.FrameLength);
            fs.MediaType = reader.ReadUInt16();
            fs.ProcessInfoIndex = reader.ReadUInt32();
            fs.TimeStamp = reader.ReadUInt64();
            fs.TimeZoneIndex = reader.ReadByte();
            return fs;
        }
    }

    public class FrameRecord
    {
        public MediaType MediaType;
        public DateTimeOffset Timestamp;
        public string ProcessName;
        public uint Pid;
        public byte[] Data;
        public long DataOffset;

        public int FrameNumber { get; internal set; }
    }


    public class SystemTime
    {
        public ushort Year;
        public ushort Month;
        public ushort DayOfWeek;
        public ushort Day;
        public ushort Hour;
        public ushort Minute;
        public ushort Second;
        public ushort Milliseconds;

        public static SystemTime ReadFrom(BinaryReader reader)
        {
            return new SystemTime()
            {
                Year = reader.ReadUInt16(),
                Month = reader.ReadUInt16(),
                DayOfWeek = reader.ReadUInt16(),
                Day = reader.ReadUInt16(),
                Hour = reader.ReadUInt16(),
                Minute = reader.ReadUInt16(),
                Second = reader.ReadUInt16(),
                Milliseconds = reader.ReadUInt16()
            };
        }
        /// <summary>
        /// Gets the time representation of the current SystemTime object as DateTime instance.
        /// </summary>
        /// <returns>A DateTime object representing time the current SystemTime object.</returns>
        public DateTime ToDateTime() => new DateTime(Year, Month, Day, Hour, Minute, Second, Milliseconds);        
    }

    internal class CapFileHeader
    {
        public uint Signature;
        public byte BCDVerMinor;
        public byte BCDVerMajor;
        public ushort MacType;
        public SystemTime TimeStamp;
        public uint FrameTableOffset;
        public uint FrameTableLength;
        public uint UserDataOffset;
        public uint UserDataLength;
        public uint CommentDataOffset;
        public uint CommentDataLength;
        public uint ProcessInfoTableOffset;
        public uint ProcessInfoTableCount;
        public uint ExtendedInfoOffset;
        public uint ExtendedInfoLength;
        public uint ConversationStatsOffset;
        public uint ConversationStatsLength;

        internal static CapFileHeader ReadFrom(BinaryReader reader)
        {
            return new CapFileHeader()
            {
                Signature = reader.ReadUInt32(),
                BCDVerMinor = reader.ReadByte(),
                BCDVerMajor = reader.ReadByte(),
                MacType = reader.ReadUInt16(),
                TimeStamp = SystemTime.ReadFrom(reader),
                FrameTableOffset = reader.ReadUInt32(),
                FrameTableLength = reader.ReadUInt32(),
                UserDataOffset = reader.ReadUInt32(),
                UserDataLength = reader.ReadUInt32(),
                CommentDataOffset = reader.ReadUInt32(),
                CommentDataLength = reader.ReadUInt32(),
                ProcessInfoTableOffset = reader.ReadUInt32(),
                ProcessInfoTableCount = reader.ReadUInt32(),
                ExtendedInfoOffset = reader.ReadUInt32(),
                ExtendedInfoLength = reader.ReadUInt32(),
                ConversationStatsOffset = reader.ReadUInt32(),
                ConversationStatsLength = reader.ReadUInt32()
            };
        }
    }

    internal class FrameHeader
    {
        public ulong TimeStamp;
        public uint FrameLength;
        public uint BytesAvailable;

        internal static FrameHeader Read(BinaryReader reader)
        {
            return new FrameHeader()
            {
                TimeStamp = reader.ReadUInt64(),
                FrameLength = reader.ReadUInt32(),
                BytesAvailable = reader.ReadUInt32()
            };
        }        
    }

    public class ProcessInfo
    {
        public uint PathSize;
        public byte[] _UnicodePathToApp;
        public uint IconSize;
        public byte[] IconData;
        public uint PID;
        public ushort LocalPort;
        public ushort RemotePort;
        public uint _IsIPv6;
        public byte[] _LocalAddr;
        public byte[] _RemoteAddr;


        public string ProcessName
        {
            get
            {
                var path = (_UnicodePathToApp?.Length > 0) ? Encoding.Unicode.GetString(_UnicodePathToApp) : String.Empty;
                var pname = path.Substring(0, path.IndexOf((char)0)); 
                return pname;
            }
        }
        internal static ProcessInfo Read(BinaryReader reader)
        {
            var pi = new ProcessInfo();
            pi.PathSize = reader.ReadUInt32();
            pi._UnicodePathToApp = reader.ReadBytes((int)pi.PathSize);    /// Unicode encoding means that 2 bytes are per character.
            pi.IconSize = reader.ReadUInt32();
            pi.IconData = reader.ReadBytes((int)pi.IconSize);
            pi.PID = reader.ReadUInt32();
            pi.LocalPort = reader.ReadUInt16();
            reader.ReadUInt16();
            pi.RemotePort = reader.ReadUInt16();
            reader.ReadUInt16();
            pi._IsIPv6 = reader.ReadUInt32();
            pi._LocalAddr= reader.ReadBytes(16);
            pi._RemoteAddr = reader.ReadBytes(16);
            return pi;
        }
    }
    public enum MediaType
    {
        Ethernet = 1,
        Tokenring = 2,
        FDDI = 3,
        ATM = 4,
        P1394 = 5,
        Wifi = 6,
        TunnelingInterfaces = 7,
        WirelessWAN = 8,
        RawIP = 9,
        Unsupported = 0xe000,
        LinuxCookedMode = 0xe071,
        NetEvent = 0xffe0,
        NetmonInfoEx = 0xfffb,
        PayloadHeader = 0xfffc,
        NetmonInfo = 0xfffd,
        NetmonDnsCache = 0xffe,
        NetmonFilter = 0xffff
    }


    /// <summary>
    /// Each comment is represented by identification of frames, title and description.
    /// </summary>
    public class CommentInfo
    {
        /// <summary>
        /// Number of frames to which this comment is attached. Currently, each comment is attached to only one frame.
        /// </summary>
        public uint NumberOfFramesPerComment;
        /// <summary>
        /// Offset in the capture file table that indicates the beginning of the frame.
        /// </summary>
        public uint FrameOffset;
        /// <summary>
        /// Number of bytes in the comment title. Must be greater than zero.
        /// </summary>
        public uint TitleByteLength;
        /// <summary>
        /// UTF16 title string as a byte sequence.
        /// </summary>
        public byte[] Title;
        /// <summary>
        /// Number of bytes in the comment description. Must be at least zero.
        /// </summary>
        public uint DescriptionByteLength;
        /// <summary>
        /// RTF sequence of characters represented as an ASCII set of bytes.
        /// </summary>
        public byte[] Description;
        internal static CommentInfo Read(BinaryReader reader)
        {
            var ci = new CommentInfo();
            ci.NumberOfFramesPerComment = reader.ReadUInt32();
            ci.FrameOffset = reader.ReadUInt32();
            ci.TitleByteLength = reader.ReadUInt32();
            ci.Title = reader.ReadBytes((int)ci.TitleByteLength);
            ci.DescriptionByteLength = reader.ReadUInt32();
            ci.Description = reader.ReadBytes((int)ci.DescriptionByteLength);
            return ci;
        }
    }

}
