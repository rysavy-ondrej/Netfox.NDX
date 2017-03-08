//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using Newtonsoft.Json;
using PacketDotNet;
using PacketDotNet.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Ndx.Ingest.Trace
{

    /// <summary>
    /// This class represents a single MCAP file. The MCAP file is 
    /// "metadata packet capture" file and thus contains metadata for 
    /// single or more pcap files.
    /// </summary>
    public class McapFile
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private ZipArchive m_archive;
        private McapIndex m_index;
        private string m_rootPath;

        /// <summary>
        /// Gets the collection of capture file IDs.
        /// </summary>
        public IEnumerable<Guid> Captures
        {
            get
            {
                return m_index.CaptureEntries.Select(x=>x.Key);
            }
        }

        /// <summary>
        /// Opens MCAP file provided <see cref="ZipArchive"/> object.
        /// This is private constructor. To create this object use <see cref="McapFile.Open(string)"/>.
        /// </summary>
        /// <param name="archive"></param>
        private McapFile(ZipArchive archive)
        {
            this.m_archive = archive;           
        }

        /// <summary>
        /// Attempts to load index file.
        /// </summary>
        private void LoadIndex()
        {
            var indexFile = m_archive.GetEntry("index.json");
            using (var tr = new StreamReader(indexFile.Open()))
            {
                m_index = JsonConvert.DeserializeObject<McapIndex>(tr.ReadToEnd());
            }
        }

        /// <summary>
        /// Opens MCAP file for reading.
        /// </summary>
        /// <param name="path">A path to the MCAP file.</param>
        /// <returns></returns>
        public static McapFile Open(string path)
        {
            try
            {
                path = Path.GetFullPath(path);
                var archive = ZipFile.Open(path, ZipArchiveMode.Read);
                var mcap = new McapFile(archive)
                {
                    m_rootPath = Path.GetDirectoryName(path)
                };
                mcap.LoadIndex();
                return mcap;
            }
            catch(Exception e)
            {
                _logger.Error(e, $"Cannot open MCAP file '{path}'.");
                return null;
            }
        }

        /// <summary>
        /// Gets KeyTable for the specified capture id.
        /// </summary>
        /// <param name="id"><see cref="Guid"/> of the capture.</param>
        /// <returns>A collection of <see cref="FlowKeyTableEntry"/> object for the specified capture.</returns>
        public IEnumerable<FlowKeyTableEntry> GetKeyTable(Guid id)
        {
            IEnumerable<FlowKeyTableEntry> GetEntries(Stream stream)
            {
                using (var reader = new BinaryReader(stream))
                {
                    do
                    {
                        var obj = FlowKeyTableEntry.Converter.ReadObject(reader);
                        if (obj == null) break;
                        yield return obj;
                    }
                    while (true);                    
                }
            }
            var entry = m_archive.GetEntry(m_index.CaptureEntries[id].KeyFile);
            return GetEntries(entry.Open());
        }

        /// <summary>
        /// Gets all conversations for the specified capture.
        /// </summary>
        /// <param name="id">Capture Id.</param>
        /// <returns>Collection of <see cref="FlowKeyTableEntry"/> pairs for all collections.</returns>
        public IEnumerable<FlowKeyTableEntry[]> GetConversations(Guid id)
        {
            string keySelector(FlowKeyTableEntry entry)
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

        /// <summary>
        /// Gets selector for retrieving frame bytes.
        /// </summary>
        public static Func<PacketMetadata, Tuple<long, int, PacketMetadata>> FrameContent = (x) => Tuple.Create(x.Frame.FrameOffset, x.Frame.FrameLength, x);
        /// <summary>
        /// Gets selector for retrieving network packet bytes.
        /// </summary>
        public static Func<PacketMetadata, Tuple<long, int, PacketMetadata>> NetworkContent = (x) => Tuple.Create(x.Frame.FrameOffset + x.Network.Start, x.Network.Count, x);
        /// <summary>
        /// Gets selector for retrieving transport pdu bytes.
        /// </summary>
        public static Func<PacketMetadata, Tuple<long, int, PacketMetadata>> TransportContent = (x) => Tuple.Create(x.Frame.FrameOffset + x.Transport.Start, x.Transport.Count, x);
        /// <summary>
        /// Gets selector for retrieving payload bytes.
        /// </summary>
        public static Func<PacketMetadata, Tuple<long, int, PacketMetadata>> PayloadContent = (x) => Tuple.Create(x.Frame.FrameOffset + x.Payload.Start, x.Payload.Count, x);

        /// <summary>
        /// Gets a collection of bytes for packets belonging to the specified <paramref name="flow"/>.
        /// </summary>
        /// <param name="capId"></param>
        /// <param name="flow"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IEnumerable<byte[]> GetPacketsBytes(Guid capId, FlowKeyTableEntry flow, Func<PacketMetadata, Tuple<long,int>> selector)
        {
            var blocks = GetPacketBlocks(capId, flow.IndexRecord.PacketBlockList);
            var meta = GetPacketMetadata(blocks);
            return GetPacketsBytes(capId, meta, (x) =>
            {
                var r = selector(x);
                return Tuple.Create(r.Item1, r.Item2, x);
            }).Select(x=>x.Item1);
        }


        /// <summary>
        /// Gets a collection of bytes for packets belonging to the specified <paramref name="flow"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="capId"></param>
        /// <param name="flow"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<byte[], T>> GetPacketsBytes<T>(Guid capId, FlowKeyTableEntry flow, Func<PacketMetadata, Tuple<long, int, T>> selector)
        {
            var blocks = GetPacketBlocks(capId, flow.IndexRecord.PacketBlockList);
            var meta = GetPacketMetadata(blocks);
            return GetPacketsBytes(capId, meta, selector);
        }

        /// <summary>
        /// Gets a collection of bytes for packets belonging to the specified <paramref name="flow"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="source"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<byte[], T>> GetPacketsBytes<T>(Guid id, IEnumerable<PacketMetadata> source, Func<PacketMetadata, Tuple<long, int, T>> selector)
        {
            using (var stream = GetCaptureStream(id))
            {
                foreach (var item in source)
                {
                    var access = selector(item);
                    var bytes = new byte[access.Item2];
                    stream.Seek(access.Item1, SeekOrigin.Begin);
                    stream.Read(bytes, 0, access.Item2);
                    yield return Tuple.Create(bytes, access.Item3);
                }
            }
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
 
        /// <summary>
        /// Gets the <see cref="Stream"/> of the capture referenced by <paramref name="capId"/>.
        /// Each time this method is called a new <see cref="Stream"/> is created.
        /// </summary>
        /// <param name="capId"></param>
        /// <returns></returns>
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

        public IEnumerable<TcpSegment> GetConversationStream(Guid capId, FlowKeyTableEntry[] biflow, out FlowKey flowKey)
        {
            if (biflow.Length == 1)
            { // unidirectional conversation...ignore now but implement later.
                flowKey = null;
                return null;
            }

            var flow0 = biflow[0];
            var flow1 = biflow[1];

            var tcp0 = GetPacketsBytes(capId, flow0, McapFile.TransportContent).Select(x => new TcpPacket(new ByteArraySegment(x.Item1))).ToList();
            var tcp1 = GetPacketsBytes(capId, flow1, McapFile.TransportContent).Select(x => new TcpPacket(new ByteArraySegment(x.Item1))).ToList();

            IList<TcpPacket> clientFlow = null;
            IList<TcpPacket> serverFlow = null;
            uint clientIsn = 0;
            uint serverIsn = 0;
            // find who initiated conversation:
            var tcp0syn = tcp0.FirstOrDefault(x => x.Syn);
            var tcp1syn = tcp1.FirstOrDefault(x => x.Syn);

            // currently, we do not support incomplete conversations...but this will be implemented in future.
            if (tcp0syn == null || tcp1syn == null)
            {
                flowKey = null;
                return null;
            }

            // Note: SYN and FIN flags are treated as representing 1-byte payload
            if (tcp1syn.Ack && tcp1syn.AcknowledgmentNumber == tcp0syn.SequenceNumber + 1)
            {
                clientFlow = tcp0;
                serverFlow = tcp1;
                clientIsn = tcp0syn.SequenceNumber;
                serverIsn = tcp1syn.SequenceNumber;
                flowKey = flow0.Key;
            }
            else
            {
                clientFlow = tcp1;
                serverFlow = tcp0;
                clientIsn = tcp1syn.SequenceNumber;
                serverIsn = tcp0syn.SequenceNumber;
                flowKey = flow1.Key;
            }


            // compute a total order on the packets:
            //
            // s ... sequence # of client
            // r ... sequence # of server
            //
            // considering that each message can contain seq and ack numbers
            // then each TCP segment is associated with (s,r) pair:
            //
            // client->server message:  s = seq, r = ack
            // server->client message:  s = ack, r = seq
            //
            // It holds that:
            // for all (s,r),(s',r'): s < s' ==> r <= r'
            // and
            // for all (s,r),(s',r'): r < r' ==> s <= s' .
            // 
            // in other words a sequence {(si,ri)} is monotonic
            // for total ordering of TCP segments that we are looking for.
            var preconversation = Enumerable.Union(
                clientFlow.Select(x => new TcpSegment(FlowDirection.Upflow, x)),
                serverFlow.Select(x => new TcpSegment(FlowDirection.Downflow, x))).OrderBy(x => x, new RSComparer());
            // removing empty segments and duplicities:
            uint ExpS = 0;
            uint ExpR = 0;
            var conversation = preconversation.Where((x) =>
            {
                var fresh = x.S >= ExpS || x.R >= ExpR;
                var len = x.Packet.PayloadPacket.BytesHighPerformance.Length;
                var usefull = len > 0;
                ExpS = x.S + (uint)(x.Direction == FlowDirection.Upflow ? len : 0);
                ExpR = x.R + (uint)(x.Direction == FlowDirection.Downflow ? len : 0);
                return fresh && usefull;
            });

            return conversation;
        }


        class RSComparer : IComparer<TcpSegment>
        {
            public int Compare(TcpSegment x, TcpSegment y)
            {
                if (x.S == y.S && x.R == y.R) return 0;
                if (x.S <= y.S && x.R <= y.R) return -1;
                if (x.S >= y.S && x.R >= y.R) return 1;
                throw new ArgumentException($"Cannot compare ({x.S},{x.R}) and ({y.S},{y.R}).");
            }
        }
    }
}
