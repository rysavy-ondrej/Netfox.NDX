//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using PacketDotNet;
using PacketDotNet.Utils;

namespace Ndx.Metacap
{

    /// <summary>
    /// Contains a collection of extension methods for accessing packets in <see cref="McapFile"/>.
    /// </summary>
    public static class McapFileFlowExtension
    {
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
        /// Gets the collection of packets and their content for the specified Flow.
        /// </summary>
        /// <param name="mcap"></param>
        /// <param name="flow"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<PacketMetadata, byte[]>> GetPacketsBytes(this McapFile mcap, FlowKeyTableEntry flow, Func<PacketMetadata, Tuple<long, int, PacketMetadata>> selector)
        {
            throw new NotImplementedException();
            /*
                foreach (int index in flow.IndexRecord.PacketBlockList)
                {
                    var block = mcap.GetPacketBlock(index);
                    if (block != null)      // if block is null we silently ignore this situation.
                    {
                        for (int i = 0; i < block.Count; i++)
                        {
                            var meta = block[i];

                            var access = selector(meta);
                            var bytes = new byte[access.Item2];
                            mcap.GetRawData(access.Item1, bytes, 0, access.Item2);
                            yield return new Tuple<PacketMetadata, byte[]>(access.Item3, bytes);
                        }
                    }
                }
                */
        }


        public static IEnumerable<TcpPacket> GetTcpPackets(this McapFile mcap, FlowKeyTableEntry entry)
        {
            return mcap.GetPacketsBytes(entry, McapFileFlowExtension.TransportContent).Select(x => new TcpPacket(new ByteArraySegment(x.Item2)));
        }

        /// <summary>
        /// This method writes the content of a TcpFlow to the specified Stream.
        /// </summary>
        /// <param name="mcap"></param>
        /// <param name="flow"></param>
        /// <returns></returns>
        public static void WriteTcpStream(this McapFile mcap, FlowKeyTableEntry flow, Stream stream)
        {
            var packets = mcap.GetTcpPackets(flow).ToObservable();
            var tcpstream = new TcpStream(stream, 128, new CancellationToken());
            packets.Subscribe(tcpstream.PacketsTarget.AsObserver());
            Task.WaitAll(tcpstream.Completion);
        }
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
