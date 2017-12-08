# Ndx.Ipflow

Library for manipulation of packet trace files. In comparison to other similar libraries the main idea here is to 
create a list of conversations first and then apply additional processing to selected conversation only.

In particular, this library provides the following features:

* Identification of conversations and labeling frames with conversations identifier
* Infrastruture for implementing various methods of protocol identification
* Simple filter language based on flow key 

## Usage for source file
The basic usage of conversation tracker is to label frames with the identified conversation id. The following
snippet demonstrates the pattern of conversation tracker use. The program count number of frames and conversations to 
and prints this information to the standard output.

```csharp
// first we prepare tracker:
var tracker = new ConversationTracker<Frame>(new FrameFlowHelper());

// we simply count number of records produces by the tracker:
var frameTask = tracker.Count().ForEachAsync(x => { Assert.AreEqual(43, x); Console.WriteLine($" f={x}"); });
var convTask = tracker.ClosedConversations.Count().ForEachAsync(x => { Assert.AreEqual(3, x); Console.WriteLine($" c={x}"); });

// frames is a COLD observable, so it waits for subscription
var frames = PcapFile.ReadFile(captureFile);
using (frames.Subscribe(tracker))
{   // at this moment both task should be completed, because work is scheduled on the current thread
    Console.WriteLine("!");
    await frameTask;
    await convTask;
    Console.WriteLine("@");
}
// Output will be:
// f=43
// c=3
// !
// @
```
More examples can be found at Ndx.Test\Ipflow\ConversationsTest.cs.
