using System;
using System.IO;

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
        public const string Conversations = "conversations";
        public const string FrameFolder = "frames";

        public static string GetPacketBlockPath(Guid convId, int index)
        {
            return Path.Combine(Conversations, convId.ToString(), index.ToString().PadLeft(8, '0'));
        }

        public static string GetFlowRecordPath(Guid convId, FlowEndpointType enpointType)
        {
            return Path.Combine(Conversations, convId.ToString(), enpointType.ToString());
        }

        public static string GetFramePath(int index)
        {
            return Path.Combine(FrameFolder, index.ToString().PadLeft(8, '0'));
        }
    }
}
