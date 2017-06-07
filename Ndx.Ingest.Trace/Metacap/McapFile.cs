//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
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
    /// This class provides read-only access to metacap and pcap files.
    /// It provides the basic set of operations.
    /// </summary>
    public class McapFile
    {
        private static NLog.Logger m_logger = NLog.LogManager.GetCurrentClassLogger();
        private ZipArchive m_mcapArchive;
        private Stream m_pcapStream;

        /// <summary>
        /// Opens MCAP file provided <see cref="ZipArchive"/> object.
        /// This is private constructor. To create this object use <see cref="McapFile.Open(string)"/>.
        /// </summary>
        /// <param name="archive"></param>
        private McapFile(ZipArchive archive, Stream stream)
        {
            this.m_mcapArchive = archive;
            this.m_pcapStream = stream;
        }

        /// <summary>
        /// Opens MCAP file for reading.
        /// </summary>
        /// <param name="mcapFile">A path to the mcap file.</param>
        /// <param name="pcapFile">A path to the PCAP file.</param>
        /// <returns></returns>
        public static McapFile Open(string mcapFile, string pcapFile)
        {
            if (File.Exists(mcapFile) && File.Exists(pcapFile))
            {
                try
                {
                    var archive = ZipFile.Open(mcapFile, ZipArchiveMode.Read);
                    var stream = new FileStream(pcapFile, FileMode.Open, FileAccess.Read);
                    var mcap = new McapFile(archive, stream);
                    return mcap;
                }
                catch (Exception e)
                {
                    m_logger.Error(e, $"Cannot open MCAP file {Path.GetFullPath(mcapFile)}.");
                    return null;
                }
            }
            else
            {
                m_logger.Error($"MCAP file {Path.GetFullPath(mcapFile)} or PCAP file {Path.GetFullPath(pcapFile)} do not exist.");
                return null;
            }
        }

        IEnumerable<Guid> m_conversations;
        /// <summary>
        /// Gets the collection of all conversation identifiers.
        /// </summary>
        public IEnumerable<Guid> Conversations
        {
            get
            {
                /// gets the id of conversation from the conversation path
                /// conversation path is: conversations/GUID/...
                string getConversationDirectory(string path)
                {
                    var components = path.Split(Path.DirectorySeparatorChar);
                    return components[1];
                }

                if (m_conversations == null)
                {
                    m_conversations = from entry in m_mcapArchive.Entries
                                        where entry.FullName.EndsWith(@"upflow\key")
                                        select Guid.Parse(getConversationDirectory(entry.FullName));
                }
                return m_conversations;
            }
        }

        /// <summary>
        /// Gets the collection of packet blocks for the conversation.
        /// </summary>
        /// <returns>The enumerable collection of packet blocks.</returns>
        /// <param name="convId">Conversation identifier.</param>
        public IEnumerable<PacketBlock> GetPacketBlocks(Guid convId, FlowOrientation orientation)
        {
            int index = 0;
            PacketBlock packetBlock = null;
            while ((packetBlock = GetPacketBlock(convId, orientation, index)) != null)
            {
                index++;
                yield return packetBlock;
            }
        }


        public FlowKey GetFlowKey(Guid conversation, FlowOrientation orientation)
        {
            var path = MetacapFileInfo.GetFlowKeyPath(conversation, orientation);
            var entry = m_mcapArchive.GetEntry(path);

            if (entry == null)
            {
                m_logger.Error($"Flow key[{path}] cannot be find. Corrupted file?");
                return null;
            }

            using (var cis = new Google.Protobuf.CodedInputStream(entry.Open()))
            {
                return FlowKey.Parser.ParseFrom(cis);
            }
        }

        /// <summary>
        /// Gets the packet metadata collection for the conversation.
        /// </summary>
        /// <returns>The packet metadata collection.</returns>
        /// <param name="convId">Conversation identifier.</param>
        public IEnumerable<PacketUnit> GetPacketMetadataCollection(Guid convId, FlowOrientation orientation)
        {
            foreach (var packetBlock in GetPacketBlocks(convId, orientation))
            {
                if (packetBlock != null)
                {
                    foreach (var packet in packetBlock.Packets)
                    {
                        yield return packet;
                    }
                }
                else
                {
                    m_logger.Warn($"PacketBlock={packetBlock} is not found in metacap.");
                }
            }
        }

        /// <summary>
        /// Gets the i-th <see cref="PacketBlock"/> for the specified capture.
        /// </summary>
        /// <param name="index">Index of the <see cref="PacketBlock"/> object to retrieve.</param>
        /// <returns>The <see cref="PacketBlock"/> for the conversation.</returns>
        private PacketBlock GetPacketBlock(Guid convId, FlowOrientation orientation, int index)
        {
            var path = MetacapFileInfo.GetPacketBlockPath(convId, orientation, index);
            var entry = m_mcapArchive.GetEntry(path);

            if (entry == null)
            {
                m_logger.Error($"PacketBlock[{path}] cannot be find. Corrupted file?");
                return null;
            }

            using (var cis = new Google.Protobuf.CodedInputStream(entry.Open()))
            {
                return PacketBlock.Parser.ParseFrom(cis);
            }
        }
                                    

        /// <summary>
        /// Gets the i-th <see cref="FlowRecord"/> for the specified capture.
        /// </summary>
        /// <param name="convId">Identifier of the <see cref="FlowRecord"/> object to retrieve.</param>
        /// <returns><see cref="FlowRecord"/> for the specified conversation endpoint.</returns>
        public FlowRecord GetFlowRecord(Guid convId, FlowOrientation endpoint)
        {
            var path = MetacapFileInfo.GetFlowRecordPath(convId, endpoint);
            var entry = m_mcapArchive.GetEntry(path);
            if (entry == null) return null;
            using (var cis = new CodedInputStream(entry.Open()))
            {
                return FlowRecord.Parser.ParseFrom(cis);
            }
        }

        object _sync = new object();
        public int GetRawData(long streamOffset, byte[] buffer, int bufferOffset, int count)
        {
            lock (_sync)
            {
                m_pcapStream.Seek(streamOffset, SeekOrigin.Begin);
                return m_pcapStream.Read(buffer, bufferOffset, count);
            }
        }

        /// <summary>
        /// Gets the <see cref="Stream"/> of the capture referenced by <paramref name="capId"/>.
        /// Each time this method is called a new <see cref="Stream"/> is created.
        /// </summary>
        /// <param name="capId"></param>
        /// <returns></returns>
        public Stream CaptureStream => m_pcapStream;

    }

    /*

            /// <summary>
            /// Gets the collection of <see cref="TcpSegment"/> objects that represents conversation stream. 
            /// If conversation cannot be composed for the given <paramref name="biflow"/> object, <see cref="null"/> is returned.
            /// </summary>
            /// <param name="capId">Id of the capture.</param>
            /// <param name="biflow">An array of <see cref="FlowKeyTableEntry"/> that specify flows for which the conversation should be composed. </param>
            /// <param name="flowKey"><see cref="FlowKey"/> object or null is provided by the function.</param>
            /// <returns>The collection of <see cref="TcpSegment"/> objects or <see cref="null"/>.</returns>
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
                    return 0;
                    //throw new ArgumentException($"Cannot compare ({x.S},{x.R}) and ({y.S},{y.R}).");
                }
            }

            /// <summary>
            /// Gets the dataflow block for accessing <see cref="PacketMetadata"/> by <see cref="FlowKey"/>
            /// within capture file identified by <paramref name="capId"/>.
            /// </summary>
            /// <param name="capId"></param>
            /// <returns></returns>
            public IPropagatorBlock<FlowKey, PacketMetadata> GetPacketMetadataBlock(Guid capId)
            {
                var buffer = new BufferBlock<PacketMetadata>();
                var flowTable = GetFlowTableDictionary(capId);
                async Task GetMetadata(FlowKey key)
                {
                    if (flowTable.TryGetValue(key, out FlowKeyTableEntry entry))
                    {
                        foreach (var index in entry.IndexRecord.PacketBlockList)
                        {
                            var blocks = GetPacketBlocks(capId, entry.IndexRecord.PacketBlockList);
                            foreach (var meta in GetPacketMetadata(blocks))
                            {
                                await buffer.SendAsync(meta);
                            }
                        }
                    }
                    buffer.Complete();
                }
                var action = new ActionBlock<FlowKey>(GetMetadata);
                return DataflowBlock.Encapsulate(action, buffer);
            }


            /// <summary>
            /// Creates dataflow block that calls <paramref name="selector"/> on each <see cref="PacketMetadata"/> and the content of packet.
            /// </summary>
            /// <param name="captureId"></param>
            /// <param name="selector"></param>
            /// <returns></returns>
            public IPropagatorBlock<PacketMetadata, T> GetPacketsBlock<T>(Guid captureId, Func<PacketMetadata, byte[], T> selector)
            {
                var stream = GetCaptureStream(captureId);
                async Task<T> getBytes(PacketMetadata meta)
                {                                              
                    var bytes = new byte[meta.Frame.FrameLength];
                    stream.Seek(meta.Frame.FrameOffset, SeekOrigin.Begin);
                    await stream.ReadAsync(bytes, 0, bytes.Length);
                    return selector(meta, bytes);
                }
                var block = new TransformBlock<PacketMetadata, T>((Func<PacketMetadata, Task<T>>)getBytes);            
                block.Completion.ContinueWith((t) => stream.Close());
                return block;
            }

            public ISourceBlock<TcpSegment> GetConversationStream2(Guid capId, FlowKeyTableEntry[] biflow, out FlowKey flowKey)
            {
                flowKey = null;
                if (biflow.Length != 2) return null;
                // Implement sliding window:
                //
                //
                //    ---- M [s1,r1]------>
                //
                //    <----N [r2,s2]-------  if s2 = s1 + M.len  =====> push(M)
                //
                //  That is, when ack is send then we can deliver M
                //


                //
                //     tcp0source ---------> tcp0transform 
                //
                //
                //     tcp1source ---------> tcp1transform 
                var tcp0source = GetPacketMetadataBlock(capId);
                var tcp1source = GetPacketMetadataBlock(capId);

                TcpPacket getPacket(PacketMetadata meta, byte[] bytes)
                {
                    var bas = new ByteArraySegment(bytes, meta.Transport.Start, meta.Transport.Count);
                    return new TcpPacket(bas);
                }

                var tcp0transform = GetPacketsBlock(capId, getPacket);
                var tcp1transform = GetPacketsBlock(capId, getPacket);
                var composer = new TcpComposer();
                tcp0source.LinkTo(tcp0transform, new DataflowLinkOptions() { PropagateCompletion = true });
                tcp1source.LinkTo(tcp1transform, new DataflowLinkOptions() { PropagateCompletion = true });

                tcp0transform.LinkTo(composer.UpFlowPacketsTarget, new DataflowLinkOptions() { PropagateCompletion = true });
                tcp1transform.LinkTo(composer.DownFlowPacketsTarget, new DataflowLinkOptions() { PropagateCompletion = true });

                // run pipeline
                tcp0source.Post(biflow[0].Key);
                tcp1source.Post(biflow[1].Key);

                // complete the input to the pipeline
                tcp0source.Complete();
                tcp1source.Complete();

                return null;

            }

            public class TcpComposer
            {
                public ITargetBlock<TcpPacket> UpFlowPacketsTarget => m_upFlowAction;
                public ITargetBlock<TcpPacket> DownFlowPacketsTarget => m_downFlowAction;
                public ISourceBlock<TcpSegment> ConversationSegmentsSource => m_segmentBuffer;

                ActionBlock<TcpPacket> m_upFlowAction;
                ActionBlock<TcpPacket> m_downFlowAction;
                BufferBlock<TcpSegment> m_segmentBuffer;

                Queue<TcpPacket> m_upFlowQueue;
                Queue<TcpPacket> m_downFlowQueue;

                Func<TcpPacket,Task> EnqueueAndTestActionTask(FlowDirection direction, Queue<TcpPacket> thisQueue, Queue<TcpPacket> thatQueue)
                {
                    async Task actionAsync(TcpPacket packet)
                    {
                        while (thatQueue.Count > 0 && m_upFlowQueue.Peek().SequenceNumber + m_upFlowQueue.Peek().GetPayloadLength() < packet.AcknowledgmentNumber)
                        {
                            var packetToDeliver = thatQueue.Dequeue();
                            var segment = new TcpSegment(direction, packetToDeliver);
                            await m_segmentBuffer.SendAsync(segment);
                            thisQueue.Enqueue(packet);
                        }
                    }
                    return actionAsync;
                }

                public TcpComposer()
                {
                    m_upFlowQueue = new Queue<TcpPacket>();
                    m_downFlowQueue = new Queue<TcpPacket>();
                    m_segmentBuffer = new BufferBlock<TcpSegment>();
                    m_upFlowAction = new ActionBlock<TcpPacket>(EnqueueAndTestActionTask(FlowDirection.Upflow, m_upFlowQueue, m_downFlowQueue));
                    m_downFlowAction = new ActionBlock<TcpPacket>(EnqueueAndTestActionTask(FlowDirection.Downflow, m_downFlowQueue, m_upFlowQueue));
                }
            }       
        }

        public static class TcpPacketExt
        {
            /// <summary>
            /// Gets the length of the <see cref="TcpPacket"/> payload.
            /// </summary>
            /// <param name="packet"><see cref="TcpPacket"/> object.</param>
            /// <returns>The lenght of the packet payload.</returns>
            public static int GetPayloadLength(this TcpPacket packet)
            {
                return packet.PayloadPacket?.BytesHighPerformance.Length ?? packet.PayloadData?.Length ?? 0;
            }
        }
        */
}
