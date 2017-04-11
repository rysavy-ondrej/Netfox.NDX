using System.IO;

namespace Ndx.Ingest.Trace
{
    /// <summary>
    /// Represents configuration of Metacap file.
    /// </summary>
    public static class MetacapFileInfo
    {
        public const string PacketBlockFolder = "map";
        public const string FlowRecordFolder = "fix";
        public const string KeyFile = "key";

        public static string GetPacketBlockPath(int index)
        {
            return Path.Combine(PacketBlockFolder, index.ToString().PadLeft(6, '0'));
        }

        public static string GetFlowRecordPath(int index)
        {
            return Path.Combine(FlowRecordFolder, index.ToString().PadLeft(6, '0'));
        }
    }
}
