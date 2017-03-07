using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Ndx.Ingest.Trace;
using System.Collections;
using static Ndx.Ingest.Trace.DataConsumers.ZipFileConsumer;

namespace Ndx.Ingest
{
    /// <summary>
    /// Representation of "index.json" file.
    /// </summary>
    class McapIndex
    {
        /// <summary>
        /// Specifies a single entry in the Mcap file.
        /// </summary>
        public class McapIndexEntry
        {
            /// <summary>
            /// Id of the capture entry.
            /// </summary>
            public Guid Id;
            /// <summary>
            /// Info file that contains summry informaiton about the capture file.
            /// </summary>
            public string InfoFile;
            /// <summary>
            /// Key file name. 
            /// </summary>
            public string KeyFile;
            /// <summary>
            /// Flow record file name.
            /// </summary>
            public string FlowRecordFolder;
            /// <summary>
            /// PacketBlock file name.
            /// </summary>
            public string PacketBlockFolder;

            /// <summary>
            /// Gets a relative path to capture file.
            /// </summary>
            public string CaptureFile;
        }

        /// <summary>
        /// Lists all capture entries indexed by the <see cref="McapDataset"/>.
        /// </summary>
        public Dictionary<Guid,McapIndexEntry> CaptureEntries { get; private set; }

        public McapIndex()
        {
            CaptureEntries = new Dictionary<Guid, McapIndexEntry>();
        }

        public McapIndexEntry New()
        {
            var newentry = new McapIndexEntry() { Id = Guid.NewGuid() };
            return newentry;
        }

        object _sync = new object();
        public void Add(McapIndexEntry entry)
        {
            lock (_sync)
            {
                CaptureEntries.Add(entry.Id, entry);
            }
        }
    }

    /// <summary>
    /// This class represents a single McapFile.
    /// </summary>
    public class McapFile
    {
        private ZipArchive m_archive;
        private McapIndex m_index;
        private string m_rootPath;

        public IEnumerable<Guid> Captures
        {
            get
            {
                return m_index.CaptureEntries.Select(x=>x.Key);
            }
        }

        McapFile(ZipArchive archive)
        {
            this.m_archive = archive;
            LoadIndex();
        }

        private void LoadIndex()
        {
            var indexFile = m_archive.GetEntry("index.json");
            using (var tr = new StreamReader(indexFile.Open()))
            {
                m_index = JsonConvert.DeserializeObject<McapIndex>(tr.ReadToEnd());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">A path to the *.pmfx file.</param>
        /// <returns></returns>
        public static McapFile Open(string path)
        {
            path = Path.GetFullPath(path);
            var archive = ZipFile.Open(path, ZipArchiveMode.Read);
            var reader = new McapFile(archive)
            {
                m_rootPath = Path.GetDirectoryName(path)
            };            
            return reader;
        }

        /// <summary>
        /// Gets KeyTable for the specified capture id.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> of the capture.</param>
        /// <returns>A collection of <see cref="KeyTableEntry"/> object for the specified capture.</returns>
        public IEnumerable<KeyTableEntry> GetKeyTable(Guid id)
        {
            IEnumerable<KeyTableEntry> GetEntries(Stream stream)
            {
                using (var reader = new BinaryReader(stream))
                {
                    do
                    {
                        var obj = KeyTableEntry.Converter.ReadObject(reader);
                        if (obj == null) break;
                        yield return obj;
                    }
                    while (true);                    
                }
            }
            var entry = m_archive.GetEntry(m_index.CaptureEntries[id].KeyFile);
            return GetEntries(entry.Open());
        }

        public IEnumerable<KeyTableEntry[]> GetConversations(Guid id)
        {
            string keySelector(KeyTableEntry entry)
            {
                var k0 = $"{entry.Key.Protocol}";
                var k1 = $"{entry.Key.SourceAddress}.{entry.Key.SourcePort}";
                var k2 = $"{entry.Key.DestinationAddress}.{entry.Key.DestinationPort}";
                return k0 + (String.Compare(k1,k2) < 0 ? k1 + k2 : k2 + k1);
            }

            var flows = GetKeyTable(id);
            return flows.GroupBy(keySelector).Select(g => g.ToArray());
        }

        /// <summary>
        /// Gets the i-th <see cref="FlowRecord"/> for the specified capture.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> of the capture.</param>
        /// <param name="index">Index of the flow record to retrieve.</param>
        /// <returns></returns>
        public FlowRecord GetFlowRecord(Guid id, int index)
        {
            var path = Path.Combine(m_index.CaptureEntries[id].KeyFile, index.ToString().PadLeft(6, '0'));
            var entry = m_archive.GetEntry(path);
            using (var reader = new BinaryReader(entry.Open()))
            {
                return FlowRecord.Converter.ReadObject(reader);
            }
        }


        public static Func<PacketMetadata, Tuple<long, int, PacketMetadata>> FrameContent = (x) => Tuple.Create(x.Frame.FrameOffset, x.Frame.FrameLength, x);
        public static Func<PacketMetadata, Tuple<long, int, PacketMetadata>> NetworkContent = (x) => Tuple.Create(x.Frame.FrameOffset + x.Network.Start, x.Network.Count, x);
        public static Func<PacketMetadata, Tuple<long, int, PacketMetadata>> TransportContent = (x) => Tuple.Create(x.Frame.FrameOffset + x.Transport.Start, x.Transport.Count, x);
        public static Func<PacketMetadata, Tuple<long, int, PacketMetadata>> PayloadContent = (x) => Tuple.Create(x.Frame.FrameOffset + x.Payload.Start, x.Payload.Count, x);
        public IEnumerable<byte[]> GetPacketsContent(Guid capId, KeyTableEntry flow, Func<PacketMetadata, Tuple<long,int>> selector)
        {
            var blocks = GetPacketBlocks(capId, flow.IndexRecord.PacketBlockList);
            var meta = GetPacketMetadata(blocks);
            return GetPacketsContent(capId, meta, (x) =>
            {
                var r = selector(x);
                return Tuple.Create(r.Item1, r.Item2, x);
            }).Select(x=>x.Item1);
        }


        public IEnumerable<Tuple<byte[], T>> GetPacketsContent<T>(Guid capId, KeyTableEntry flow, Func<PacketMetadata, Tuple<long, int, T>> selector)
        {
            var blocks = GetPacketBlocks(capId, flow.IndexRecord.PacketBlockList);
            var meta = GetPacketMetadata(blocks);
            return GetPacketsContent(capId, meta, selector);
        }

        /// <summary>
        /// Gets the i-th <see cref="PacketBlock"/> for the specified capture.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> of the capture.</param>
        /// <param name="index">Index of the <see cref="PacketBlock"/> object to retrieve.</param>
        /// <returns></returns>
        public PacketBlock GetPacketBlock(Guid id, int index)
        {
            var path = Path.Combine(m_index.CaptureEntries[id].PacketBlockFolder, index.ToString().PadLeft(6, '0'));
            var entry = m_archive.GetEntry(path);
            using (var reader = new BinaryReader(entry.Open()))
            {
                return PacketBlock.Converter.ReadObject(reader);
            }
        }
 
        public Stream GetCaptureStream(Guid capId)
        {
            var capEntry = m_index.CaptureEntries[capId];
            var path = Path.Combine(m_rootPath, capEntry.CaptureFile);
            return File.OpenRead(path);
        }

        public IEnumerable<PacketBlock> GetPacketBlocks(Guid id, IEnumerable<int> indexes)
        {
            foreach(var index in indexes)
            {
                yield return GetPacketBlock(id, index);
            }
        }

        public IEnumerable<PacketMetadata> GetPacketMetadata(IEnumerable<PacketBlock> blocks)
        {
            return blocks.SelectMany(x => x.Content);
        }

        public IEnumerable<Tuple<byte[], T>> GetPacketsContent<T>(Guid id, IEnumerable<PacketMetadata> source, Func<PacketMetadata, Tuple<long,int,T>> selector)
        {
            using (var stream = GetCaptureStream(id))
            {
                foreach(var item in source)
                {
                    var access = selector(item);
                    var bytes = new byte[access.Item2];
                    stream.Seek(access.Item1, SeekOrigin.Begin);
                    stream.Read(bytes, 0, access.Item2);
                    yield return Tuple.Create(bytes, access.Item3);
                }
            }            
        }
    }
}
