# Ndx.Ingest.Trace

Library for manipulation of packet trace files. In comparison to other similar libraries the main idea here is to 
create a list of conversations first and then apply additional processing to selected conversation only.

In particular, this library provides the following features:

* Identification of conversations and labeling frames with conversations identifier
* Infrastruture for implementing various methods of protocol identification
* Simple filter language based on flow key 

## Usage
The basic usage of conversation tracker is to label frames with the identified conversation id. The following
snippet demonstrates the pattern of conversation tracker use. The program prints frames and conversations to 
standard output.

```csharp
var frames = PcapFile.ReadFile("input.pcap");
var tracker = new ConversationTracker<Frame>(frame => { var key = frame.GetFlowKey(out bool create); return (key, create); }, ConversationTracker<Frame>.UpdateConversation);
var observer = new Ndx.Utils.Observer<Conversation>(Console.WriteLine);
using (tracker.Conversations.Subscribe(observer))
{
    frames.Select(frame => { var conversation = tracker.ProcessRecord(frame); frame.ConversationId = conversation.ConversationId; return frame; }).ForEach(Console.WriteLine);
    tracker.Complete();
}
```
It is important that `ConversationTracker.Complete()` is called after processing all input frames.
In this example, `ForEach` guarantees that all elements in the observable were processed.