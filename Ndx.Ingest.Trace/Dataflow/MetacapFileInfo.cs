﻿using System.IO;

namespace Ndx.Ingest.Trace
{
    /// <summary>
    /// Represents configuration of Metacap file.
    /// </summary>
    public static class MetacapFileInfo
    {
        public const string PacketBlockFolder = "blocks";
        public const string FlowRecordFolder = "flows";
        public const string FlowKeyTableFile = "keytable";
        public const string ConversationTableFile = "conversations";
        public const string FrameFolder = "frames";

        public static string GetPacketBlockPath(int index)
        {
            return Path.Combine(PacketBlockFolder, index.ToString().PadLeft(8, '0'));
        }

        public static string GetFlowRecordPath(int index)
        {
            return Path.Combine(FlowRecordFolder, index.ToString().PadLeft(8, '0'));
        }

        public static string GetFramePath(int index)
        {
            return Path.Combine(FrameFolder, index.ToString().PadLeft(8, '0'));
        }
    }
}
