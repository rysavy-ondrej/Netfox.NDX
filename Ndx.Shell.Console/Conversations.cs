using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Google.Protobuf;
using Ndx.Ingest;
using Ndx.Model;
using NFX.ApplicationModel.Pile;
namespace Ndx.Shell.Console
{
    /// <summary>
    /// This class provides various operations that can be applied on conversations.
    /// </summary>
    public static class Conversations
    {
        public static object sync = new object();
        public const int FrameTableMaxItems = 1024;
        /// <summary>
        /// Computes all conversations for the given collection of frames.
        /// </summary>
        /// <param name="frames">The input collection of frames.</param>
        /// <returns>Dictionary containing all conversations for the input collection of frames.</returns>
        public static void TrackConversations(IEnumerable<RawFrame> frames, IPile pile, IDictionary<int, PilePointer> ctable, IDictionary<int, PilePointer> ftable)
        {
            var tracker = new ConversationTracker();
            void AcceptConversation(KeyValuePair<Conversation, MetaFrame> item)
            {
                try
                {
                    lock (sync)
                    {
                        var conversationId = item.Key.ConversationId;
                        if (ctable.TryGetValue(conversationId, out PilePointer ptr))
                        {
                            pile.Put(ptr, item.Key.ToByteArray());
                        }
                        else
                        {
                            var cptr = pile.Put(item.Key.ToByteArray());
                            ctable[conversationId] = cptr;
                        }
                        var fptr = pile.Put(item.Value.ToByteArray());
                        ftable[item.Value.FrameNumber] = fptr;
                    }
                }
                catch (Exception e)
                {
                    System.Console.Error.WriteLine($"Capture.AcceptConversation: Error when processing item {item}: {e}. ");
                }
            }
            var actionBlock = new ActionBlock<KeyValuePair<Conversation, MetaFrame>>((Action<KeyValuePair<Conversation, MetaFrame>>)AcceptConversation);
            tracker.Output.LinkTo(actionBlock);

            foreach (var frame in frames)
            {
                tracker.Input.Post(frame);
            }
            tracker.Input.Complete();

            Task.WaitAll(tracker.Completion);

            System.Console.WriteLine($"Tracking completed: ctable={ctable.Count}, ftable={ftable.Count}, pile bytes={pile.AllocatedMemoryBytes}.");
        }

        /// <summary>
        /// Tracks conversation for the given collection of <paramref name="frames"/>.
        /// </summary>
        /// <param name="frames">A collection of frames to be processed.</param>
        public static void TrackConversations(IEnumerable<RawFrame> frames)
        {
            FlowKey getKeySelector(RawFrame frame)
            {
                return PacketAnalyzer.GetFlowKey(frame);
            }
            var preConversations = frames.Select(rawFrame => new {RawFrame = rawFrame, FlowKey = getKeySelector(rawFrame)}).GroupBy(item => item.FlowKey);
        }

        /// <summary>
        /// Saves the conversation dictionary to the specified file. 
        /// </summary>
        /// <param name="dictionary">Conversation dictionary.</param>
        /// <param name="path">Path to the output file. The file must not exists otherwise exception is thrown.</param>
        public static void WriteTo(ConversationTable ctable, FrameTable ftable, string path)
        {
            using (var archive = ZipFile.Open(path, ZipArchiveMode.Create))
            {
                var ctableEntry = archive.CreateEntry("ctable", CompressionLevel.Fastest);
                using (var ctableOutput = new CodedOutputStream(ctableEntry.Open()))
                {
                    ctable.WriteTo(ctableOutput);
                }
                var ftableEntry = archive.CreateEntry("ftable", CompressionLevel.Fastest);
                using (var ftableOutput = new CodedOutputStream(ftableEntry.Open()))
                {
                    ftable.WriteTo(ftableOutput);
                }
            }
        }

        public static IEnumerable<MetaFrame> Frames(this Conversation conversation, FrameTable ftable)
        {
            return Frames(conversation, x => ftable);
        }
        /// <summary>
        /// Gets a collection of <see cref="MetaFrame"/> objects for the given set of conversations.
        /// </summary>
        /// <param name="conversations"></param>
        /// <param name="ftable"></param>
        /// <returns></returns>
        public static IEnumerable<MetaFrame> Frames(this IEnumerable<Conversation> conversations, FrameTable ftable)
        {
            return Frames(conversations, x => ftable);
        }
        /// <summary>
        /// Gets a collection of <see cref="MetaFrame"/> objects for the given set of conversations.
        /// </summary>
        /// <param name="conversations">A collection of conversations.</param>
        /// <param name="ftableProvider">Function that provides <see cref="FrameTable"/>.</param>
        /// <returns></returns>
        public static IEnumerable<MetaFrame> Frames(this IEnumerable<Conversation> conversations, Func<int, FrameTable> ftableProvider)
        {
            return conversations.SelectMany(c =>
            {
                return c.Packets.Select(p => ftableProvider(Conversation.PacketSource(p)).Items[Conversation.PacketNumber(p)]);
            });
        }

        public static IEnumerable<MetaFrame> Frames(Conversation conversation, Func<int, FrameTable> ftableProvider)
        {
            return conversation.Packets.Select(p => ftableProvider(Conversation.PacketSource(p)).Items[Conversation.PacketNumber(p)]);
        }

        public static void MergeFrom(string path, ConversationTable ctable, FrameTable ftable)
        {
            using (var archive = ZipFile.OpenRead(path))
            {
                if (ctable != null)
                {
                    var ctableEntry = archive.GetEntry("ctable");
                    using (var ctableInput = CodedInputStream.CreateWithLimits(ctableEntry.Open(), Int32.MaxValue, 32))
                    {
                        ctable.MergeFrom(ctableInput);
                    }
                }
                if (ftable != null)
                {
                    var ftableEntry = archive.GetEntry("ftable");
                    using (var ftableInput = CodedInputStream.CreateWithLimits(ftableEntry.Open(), Int32.MaxValue, 32))
                    {
                        ftable.MergeFrom(ftableInput);
                    }
                }
            }
        }
    }
}
