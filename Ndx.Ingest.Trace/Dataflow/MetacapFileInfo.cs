using System;
using System.IO;

namespace Ndx.Metacap
{
    /// <summary>
    /// Represents configuration of Metacap file.
    /// </summary>
    public static class MetacapFileInfo
    {
        public const string PacketBlockFolder = "blocks";
        public const string FlowRecordFolder = "flows";
        public const string FlowKeyTableFile = "keytable";
        public const string Conversations = "conversations";
        public const string FrameFolder = "frames";

        public static string GetPacketBlockPath(Guid convId, FlowOrientation orientation, int index)
        {
            return Path.Combine(Conversations, convId.ToString(), orientation.ToString().ToLowerInvariant(), index.ToString().PadLeft(8, '0'));
        }

        public static string GetFlowRecordPath(Guid convId, FlowOrientation orientation)
        {
            return Path.Combine(Conversations, convId.ToString(), orientation.ToString().ToLower(), "record");
        }

        public static string GetFlowKeyPath(Guid convId, FlowOrientation orientation)
        {
            return Path.Combine(Conversations, convId.ToString(), orientation.ToString().ToLower(), "key");
        }

        public static string GetFramePath(int index)
        {
            return Path.Combine(FrameFolder, index.ToString().PadLeft(8, '0'));
        }
    }
}
