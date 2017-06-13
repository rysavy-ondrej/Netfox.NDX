using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Google.Protobuf;
using Ndx.Model;
namespace Ndx.Metacap
{
    /// <summary>
    /// This is the main class of the Metacap Infrastructure.
    /// </summary>
    public class Metacap
    {
        List<PcapFile> m_captureFiles;
        List<Conversation> m_conversations;
        List<PacketBlock> m_packetBlocks;
        MetacapIndex m_index;

        /// <summary>
        /// Gets the dictionary of capture files. 
        /// </summary>
        public List<PcapFile> CaptureFiles { get => m_captureFiles;  }
        /// <summary>
        /// Gets the dictionary of the conversations. 
        /// </summary>
        public List<Conversation> Conversations { get => m_conversations; }
        /// <summary>
        /// Gets the dictionary of packet blocks.
        /// </summary>
        public List<PacketBlock> PacketBlocks { get => m_packetBlocks; }
        /// <summary>
        /// Gets the index object. 
        /// </summary>
        public MetacapIndex Index { get => m_index; }

        /// <summary>
        /// Creates a new instance of <see cref="Metacap"/> class.
        /// </summary>
        public Metacap(bool init=true)
        {
            if (init)
            {
                m_index = new MetacapIndex();
                m_captureFiles = new List<PcapFile>();
                m_conversations = new List<Conversation>();
                m_packetBlocks = new List<PacketBlock>();
            }
        }

        /// <summary>
        /// Loads <see cref="Metacap"/> objects from the ZIP archive.
        /// </summary>
        /// <param name="path">The path to Zip archive that contains the metacap data.</param>
        /// <returns>A new isntance of <see cref="Metacap"/> or null if this methods failed.</returns>
        public static Metacap LoadFrom(string path)
        {
            var metacap = new Metacap(false);
            using (var archive = ZipFile.OpenRead(path))
            {
                var indexEntry = archive.GetEntry("index");
                using (var cis = new CodedInputStream(indexEntry.Open()))
                {
                    metacap.m_index = MetacapIndex.Parser.ParseFrom(cis);
                }

                metacap.m_captureFiles =
                    ReadEntries(archive, "pcaps", cis => PcapFile.Parser.ParseFrom(cis)).ToList();
                metacap.m_conversations =
                    ReadEntries(archive, "convs", cis => Conversation.Parser.ParseFrom(cis)).ToList();
                metacap.m_packetBlocks =
                    ReadEntries(archive, "blcks", cis => PacketBlock.Parser.ParseFrom(cis)).ToList();
            }
            return metacap;
        }

        /// <summary>
        /// Save metacap to the archive on the specified path.
        /// </summary>
        /// <param name="path">A path where the new archive containing metacap data will be created.</param>
        public void SaveTo(string path)
        {
            using (var archive = ZipFile.Open(path, ZipArchiveMode.Create))
            {
                var indexEntry = archive.CreateEntry("index", CompressionLevel.Fastest);
                using (var cos = new CodedOutputStream(indexEntry.Open()))
                {
                    this.Index.WriteTo(cos);
                }
                
                for(var index = 0; index < m_captureFiles.Count; index++)
                {
                    var capfileEntry = archive.CreateEntry(GetEntryName("pcaps", index), CompressionLevel.Fastest);
                    using (var cos = new CodedOutputStream(capfileEntry.Open()))
                    {
                        m_captureFiles[index].WriteTo(cos);
                    }
                }

                for (var index = 0; index < m_conversations.Count; index++)
                {
                    var convEntry = archive.CreateEntry(GetEntryName("convs", index), CompressionLevel.Fastest);
                    using (var cos = new CodedOutputStream(convEntry.Open()))
                    {
                        m_conversations[index].WriteTo(cos);
                    }
                }

                for (var index = 0; index < m_packetBlocks.Count; index++)
                {
                    var blckEntry = archive.CreateEntry(GetEntryName("blcks", index), CompressionLevel.Fastest);
                    using (var cos = new CodedOutputStream(blckEntry.Open()))
                    {
                        m_packetBlocks[index].WriteTo(cos);
                    }
                }
            }
        }

        static string GetEntryName(string collectionName, int key)
        {
            return Path.Combine(collectionName, key.ToString().PadLeft(8, '0'));
        }

        static IEnumerable<T> ReadEntries<T>(ZipArchive archive, string collectionName, Func<CodedInputStream,T> reader)
        {
            for (int i = 1; ; i++)
            {
                var capfileEntry = archive.GetEntry(GetEntryName(collectionName, i));
                if (capfileEntry == null)
                {
                    yield break;
                }
                using (var cis = new CodedInputStream(capfileEntry.Open()))
                {
                    yield return reader(cis);
                }
            }
        }
    }
}
