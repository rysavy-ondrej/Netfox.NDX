using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Google.Protobuf;
using Ndx.Ingest;
using Ndx.Model;
using NFX.ApplicationModel.Pile;
 
namespace Ndx.Shell.Console
{
    using IConversationTable = System.Collections.Generic.IDictionary<int, Ndx.Model.Conversation>;
    using IFrameTable = System.Collections.Generic.IDictionary<int, Ndx.Model.Frame>;
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
        /// <param name="pile">Pile interface used for storing conversations and frames.</param>
        /// <param name="ctable">Dictionary that keeps pile pointers of conversations. Conversation Id is used as the key.</param>
        /// <param name="ftable">Dictionary that keeps pile pointers of frames. Frame number is used as the key.</param>
        /// <returns>Dictionary containing all conversations for the input collection of frames.</returns>
        public static void TrackConversations(IEnumerable<Frame> frames, IPile pile, IDictionary<int, PilePointer> ctable, IDictionary<int, PilePointer> ftable)
        {
            var tracker = new ConversationTracker();
            void AcceptConversation(Frame item)
            {
                try
                {
                    
                    // TODO: Implement storing frames in PILE
                }
                catch (Exception e)
                {
                    System.Console.Error.WriteLine($"Capture.AcceptConversation: Error when processing item {item}: {e}. ");
                }
            }
            var sink = new ActionBlock<Frame>((Action<Frame>)AcceptConversation);
            var filter = new TransformBlock<Frame,Frame>(x => tracker.ProcessFrame(x));
            filter.LinkTo(sink, new DataflowLinkOptions() { PropagateCompletion = true });

            foreach (var frame in frames)
            {
                filter.Post(frame);
            }
            filter.Complete();

            Task.WaitAll(sink.Completion);

            System.Console.WriteLine($"Tracking completed: ctable={ctable.Count}, ftable={ftable.Count}, pile bytes={pile.AllocatedMemoryBytes}.");
        }

        /// <summary>
        /// Tracks conversation for the given collection of <paramref name="frames"/>.
        /// </summary>
        /// <param name="frames">A collection of frames to be processed.</param>
        /// <returns>A collection of conversations. Each conversation is represented as group with conversationas a key and related frames as values.</returns>
        public static IEnumerable<Frame> TrackConversations(IEnumerable<Frame> frames, out IConversationTable conversations)
        {
            var tracker = new ConversationTracker();
            var _conversations = new Dictionary<int, Conversation>();
            var observer = new Ndx.Utils.Observer<Conversation>(x => _conversations.Add(x.ConversationId, x));
            using (var t = tracker.Subscribe(observer))
            {
                var labeledFrames = frames.Select(x => tracker.ProcessFrame(x)).Where(x => x != null).ToList();
                tracker.Complete();
                conversations = _conversations;
                return labeledFrames;
            }
        }

        /// <summary>
        /// Saves the conversation dictionary to the specified file. 
        /// </summary>
        /// <param name="dictionary">Conversation dictionary.</param>
        /// <param name="path">Path to the output file. The file must not exists otherwise an exception is thrown.</param>
        public static void WriteTo(IConversationTable ctable, IFrameTable ftable, string path)
        {
            using (var archive = ZipFile.Open(path, ZipArchiveMode.Create))
            {
                var ctableEntry = archive.CreateEntry("ctable", CompressionLevel.Fastest);
                using (var ctableOutput = ctableEntry.Open())
                {
                    foreach (var item in ctable)
                    {
                        item.Value.WriteDelimitedTo(ctableOutput);
                    }
                }
                var ftableEntry = archive.CreateEntry("ftable", CompressionLevel.Fastest);
                using (var ftableOutput = ftableEntry.Open())
                {
                    foreach(var item in ftable)
                    {
                        item.Value.WriteDelimitedTo(ftableOutput);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conversation"></param>
        /// <param name="ftable"></param>
        /// <returns></returns>
        public static IEnumerable<Frame> Frames(this Conversation conversation, IFrameTable ftable)
        {
            return Frames(conversation, x => ftable);
        }
        /// <summary>
        /// Gets a collection of <see cref="MetaFrame"/> objects for the given set of conversations.
        /// </summary>
        /// <param name="conversations"></param>
        /// <param name="ftable"></param>
        /// <returns></returns>
        public static IEnumerable<Frame> Frames(this IEnumerable<Conversation> conversations, IFrameTable ftable)
        {
            return Frames(conversations, x => ftable);
        }
        /// <summary>
        /// Gets a collection of <see cref="MetaFrame"/> objects for the given set of conversations.
        /// </summary>
        /// <param name="conversations">A collection of conversations.</param>
        /// <param name="ftableProvider">Function that provides <see cref="FrameTable"/>.</param>
        /// <returns></returns>
        public static IEnumerable<Frame> Frames(this IEnumerable<Conversation> conversations, Func<int, IFrameTable> ftableProvider)
        {
            return conversations.SelectMany(c =>
            {
                return c.Packets.Select(p => ftableProvider(Conversation.PacketSource(p))[Conversation.PacketNumber(p)]);
            });
        }

        public static IEnumerable<Frame> Frames(Conversation conversation, Func<int, IFrameTable> ftableProvider)
        {
            return conversation.Packets.Select(p => ftableProvider(Conversation.PacketSource(p))[Conversation.PacketNumber(p)]);
        }

        public static void MergeFrom(string path, IDictionary<int,Conversation> ctable, IFrameTable ftable)
        {
            using (var archive = ZipFile.OpenRead(path))
            {
                if (ctable != null)
                {
                    var ctableEntry = archive.GetEntry("ctable");
                    using (var ctableInput = ctableEntry.Open())
                    {
                        while (true)
                        {
                            var conversation = Conversation.Parser.ParseDelimitedFrom(ctableInput);
                            if (conversation != null)
                            {
                                ctable[conversation.ConversationId] = conversation;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                if (ftable != null)
                {
                    var ftableEntry = archive.GetEntry("ftable");
                    using (var ftableInput = ftableEntry.Open())
                    {
                        while (true)
                        {

                            var frame = Frame.Parser.ParseDelimitedFrom(ftableInput);
                            if (frame != null)
                            {
                                ftable[frame.FrameNumber] = frame;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
