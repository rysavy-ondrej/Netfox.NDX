#r "..\bin\debug\Ndx.Captures.dll"
#r "..\bin\debug\Ndx.Metacap.dll"
#r "..\bin\debug\Ndx.Utils.dll"
#r "..\bin\debug\Ndx.Ingest.Trace.dll"
using System.Net;
using System.IO;
using System.Threading.Tasks.Dataflow;
using Ndx.Ingest;
using Ndx.Model;
using Ndx.Captures;
var conversations = new HashSet<Conversation>(new Conversation.ReferenceComparer());
var frameCount = 0;
var input = @"C:\Users\Ondrej\Documents\Network Monitor 3\Captures\dd687644e0c87cc63934f9ff7053f235.cap";
var sink = new ActionBlock<KeyValuePair<Conversation, MetaFrame>>(x => { frameCount++; conversations.Add(x.Key); });
var ct = new ConversationTracker();
ct.Output.LinkTo(sink, new DataflowLinkOptions() { PropagateCompletion = true });

foreach (var f in PcapReader.ReadFile(input))
{
    ct.Input.Post(f);
}
ct.Input.Complete();
Task.WaitAll(sink.Completion);

foreach (var conv in conversations)
{

    Console.WriteLine($"{conv.ConversationId}#{conv.ConversationKey.SourceIpAddress}:{conv.ConversationKey.SourcePort}<->{conv.ConversationKey.DestinationIpAddress}:{conv.ConversationKey.DestinationPort}");
}