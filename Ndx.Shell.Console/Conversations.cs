using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Google.Protobuf;
using Ndx.Ingest;
using Ndx.Model;
using Ndx.Utils;
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

        }


        /// <summary>
        /// Saves the conversation dictionary to the specified file. 
        /// </summary>
        /// <param name="dictionary">Conversation dictionary.</param>
        /// <param name="path">Path to the output file. The file must not exists otherwise an exception is thrown.</param>
        public static void WriteTo(IConversationTable ctable, IFrameTable ftable, string path)
        {

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
                    archive.MergeFrom("conversations", ctable, Conversation.Parser, x => x.ConversationId, x => x);
                }
                if (ftable != null)
                {
                    archive.MergeFrom("frames", ftable, Frame.Parser, x => x.ConversationId, x => x);
                }
            }
        }
    }
}
