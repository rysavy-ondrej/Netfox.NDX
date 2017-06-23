using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Ingest;
using Ndx.Model;
using Ndx.Captures;

static class LoadFile
{
    public static IEnumerable<Conversation> GetConversations(string path)
    {
        var conversations = new HashSet<Conversation>(new Conversation.ReferenceComparer());
        var frameCount = 0;
        var tracker = new ConversationTracker();
        tracker.Output.LinkTo(new ActionBlock<KeyValuePair<Conversation, MetaFrame>>(x => { frameCount++; conversations.Add(x.Key); }), new DataflowLinkOptions());

        foreach (var f in PcapReader.ReadFile(path))
        {
            tracker.Input.Post(f);
        }
        tracker.Input.Complete();
        Task.WaitAll(tracker.Completion);
        return conversations;
    }

    public static void PrintConversations(IEnumerable<Conversation> conversations)
    {
        foreach (var conv in conversations)
        {
            Console.WriteLine("{0}#{1}:{2}<->{3}:{4}", conv.ConversationId,conv.ConversationKey.SourceIpAddress,conv.ConversationKey.SourcePort,
            conv.ConversationKey.DestinationIpAddress,conv.ConversationKey.DestinationPort);
        }
    }
}

