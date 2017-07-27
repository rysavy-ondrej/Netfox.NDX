using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Google.Protobuf;
using Ndx.Ingest;
using Ndx.Model;

namespace Ndx.Shell.Console
{
    /// <summary>
    /// This class provides various operations that can be applied on conversations.
    /// </summary>
    public static class Conversations
    {
        /// <summary>
        /// Computes all conversations for the given collection of frames.
        /// </summary>
        /// <param name="frames">The input collection of frames.</param>
        /// <returns>Dictionary containing all conversations for the input collection of frames.</returns>
        public static Tuple<ConversationTable, FrameTable> TrackConversations(IEnumerable<RawFrame> frames)
        {
            var ctable = new ConversationTable();
            var ftable = new FrameTable();
            var frameCount = 0;
            var tracker = new ConversationTracker();


            void AcceptConversation(KeyValuePair<Conversation, MetaFrame> item)
            {
                try
                {
                    frameCount++;
                    ctable.Items[item.Key.ConversationId] = item.Key;
                    ftable.Items[item.Value.FrameNumber] = item.Value;
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
            return Tuple.Create(ctable, ftable);
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
