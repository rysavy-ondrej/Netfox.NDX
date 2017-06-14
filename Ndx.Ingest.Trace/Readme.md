# Ndx.Ingest.Trace

Library for manipulation of packet trace files. In comparison to other similar libraries the main idea here is to 
create a list of conversations first and then apply additional processing to selected conversation only.

In particular, this library provides the following features:

* Computation of PCAP index to improve efficiency of PCAP manipulation
* Sequential access to PCAP files
* Access to packets which are part of a specific conversation
* Stream export (similar to Follow Stream function of Wireshark)
* Efficient access to packet related to conversations.

Processing input PCAP files are done in the following steps:

* RawFrames are parsed by PacketDotNet parsers. 
* FrameAttributes are extracted.
* Each frame is assigned a sequence number which is unique within its data source.
* For each packet its conversations are identified.
  There may be different conversations for a single packet as
  we recognize conversations at different level.
* All related conversations are updated with packet attributes.

## Load file
The simples usage is demonstrated in the following example. 
Input file is processed and the list of conversations is printed.
```CSharp
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
cf.ReadFrom(new FileInfo(@"input.pcap"));
```