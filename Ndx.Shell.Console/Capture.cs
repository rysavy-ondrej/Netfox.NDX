using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Captures;
using Ndx.Ingest;
using Ndx.Model;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

namespace Ndx.Shell.Console
{
    public static class Capture
    {
        /// <summary>
        /// Computes all conversations for the given collection of frames.
        /// </summary>
        /// <param name="frames">The input collection of frames.</param>
        /// <returns>Dictionary containing all conversations for the input collection of frames.</returns>
        public static IDictionary<Conversation, IList<MetaFrame>> Conversations(IEnumerable<RawFrame> frames)
        {
            var conversations = new Dictionary<Conversation,IList<MetaFrame>>(new Conversation.ReferenceComparer());
            var frameCount = 0;
            var tracker = new ConversationTracker();


            void AcceptConversation(KeyValuePair<Conversation, MetaFrame> item)
            {
                try
                {
                    frameCount++;
                    if (conversations.TryGetValue(item.Key, out IList<MetaFrame> clist))
                    {
                        clist.Add(item.Value);
                    }
                    else
                    {
                        conversations[item.Key] = new List<MetaFrame>(new[] { item.Value });
                    }
                }
                catch (Exception e)
                {
                    System.Console.Error.WriteLine($"Capture.AcceptConversation: Error when processing item {item}: {e}. ");
                }
            }

            tracker.Output.LinkTo(new ActionBlock<KeyValuePair<Conversation, MetaFrame>>((Action<KeyValuePair<Conversation, MetaFrame>>)AcceptConversation));

            foreach (var frame in frames)
            {
                tracker.Input.Post(frame);
            }
            tracker.Input.Complete();            
            Task.WaitAll(tracker.Completion);
            return conversations;
        }

        /// <summary>
        /// Gets packet specified by <paramref name="meta"/> by reading it from the provided <paramref name="stream"/>.
        /// </summary>
        /// <param name="stream">The source stream.</param>
        /// <param name="meta">Meta information decribing the packet to read from the source stream.</param>
        /// <returns><see cref="Packet"/> or null if some error occured.</returns>
        public static async Task<Packet> GetPacketAsync(LinkLayers linkType, Stream stream, MetaFrame meta)
        {
            try
            {
                stream.Seek(meta.FrameOffset, SeekOrigin.Begin);
                var buffer = new byte[meta.FrameLength];
                var result = await stream.ReadAsync(buffer, 0, meta.FrameLength);
                if (result == meta.FrameLength)
                {
                    return Packet.ParsePacket(linkType, buffer);
                }
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine($"[ERROR] Capture.GetPacketAsync: {e}");
            }
                return null;
        }


        /// <summary>
        /// Reads frames for the specified collection of capture files.
        /// </summary>
        /// <param name="captures">Collection of capture files.</param>
        /// <returns>A collection of frames read sequentially from the specified capture files.</returns>
        public static IEnumerable<RawFrame> Select(IEnumerable<string> captures, Action<string,int> progressCallback = null)
        {
            foreach (var capture in captures)
            {
                int frameCount = 0;
                foreach (var frame in PcapReader.ReadFile(capture))
                {
                    frameCount++;
                    yield return frame;                    
                }
                progressCallback?.Invoke(capture, frameCount);
            }
        }

        public static IEnumerable<RawFrame> Select(string capture)
        {
                foreach (var frame in PcapReader.ReadFile(capture))
                {
                    yield return frame;
                }
        }

        public static void WriteAllFrames(string capturefile, IEnumerable<RawFrame> frames)
        {
            var device = new CaptureFileWriterDevice(capturefile);
            WriteFrame(device, frames);
            device.Close();
        }

        public static void WriteFrame(CaptureFileWriterDevice device, RawFrame frame)
        {
            var capture = new RawCapture(LinkLayers.Ethernet, new PosixTimeval(frame.Seconds, frame.Microseconds), frame.Bytes);
            device.Write(capture);
        }


        public static void WriteFrame(CaptureFileWriterDevice device, IEnumerable<RawFrame> frames)
        {
            foreach(var frame in frames)
            {
                WriteFrame(device, frame);
            }
        }

        public static void PrintConversations(IEnumerable<Conversation> conversations)
        {
            foreach (var conv in conversations)
            {
                System.Console.WriteLine("{0}#{1}@{2}:{3}<->{4}:{5}", conv.ConversationId, conv.ConversationKey.IpProtocol, conv.ConversationKey.SourceIpAddress, conv.ConversationKey.SourcePort,
                conv.ConversationKey.DestinationIpAddress, conv.ConversationKey.DestinationPort);
            }
        }
    }
}
