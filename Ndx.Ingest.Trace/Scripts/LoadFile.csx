#r "Ndx.Captures.dll"
#r "Ndx.Metacap.dll"
#r "Ndx.Utils.dll"
#r "Ndx.Ingest.Trace.dll"
using Ndx.Ingest;
using Ndx.Model;
using System.Net;
using System.IO;
using System.Threading.Tasks.Dataflow;

var sink = new ActionBlock<Conversation>(c => Console.WriteLine($"{c.ConversationId} # {c.ConversationKey.IpProtocol}@{c.ConversationKey.SourceIpAddress}:{c.ConversationKey.SourcePort}<->{c.ConversationKey.DestinationIpAddress}:{c.ConversationKey.DestinationPort}"));
var frameSink = new ActionBlock<MetaFrame>(x => Console.Write("."));
var ct = new ConversationTracker();
var cf = new CaptureReader(1024, 512, new System.Threading.CancellationToken());
cf.RawFrameSource.LinkTo(ct.FrameAnalyzer, new DataflowLinkOptions() { PropagateCompletion = true });
ct.ConversationBuffer.LinkTo(sink, new DataflowLinkOptions() { PropagateCompletion = true });
ct.MetaframeBuffer.LinkTo(frameSink, new DataflowLinkOptions() { PropagateCompletion = true });
cf.ReadFrom(new FileInfo(@"C:\Users\Ondrej\Documents\Network Monitor 3\Captures\2adc3aaa83b46ef8d86457e0209e0aa9.cap"));