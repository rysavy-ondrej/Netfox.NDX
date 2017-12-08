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
// frames observable is COLD because it is activated during subscription.
var frames = PcapFile.ReadFile(captureFile);

// on the other hand the tracker is in principle HOT
var tracker = new ConversationTracker<Frame>(new FrameFlowHelper());

// thus it is necessary to subscribe the observers before we start the processing of the input data
var framesCountTask = tracker.Sum(x => 1).ToTask();
var conversationCountTask = tracker.ClosedConversations.Sum(x => 1).ToTask();

// now the tracker can subscribe to frames observable  
using (frames.Subscribe(tracker))
{
    // we need to wait for the results:
    var framesCount = await framesCountTask;
    var conversationCount = await conversationCountTask;
                
    Console.WriteLine($"Done, conversations = {conversationCount}, frames = {framesCount}");
}
```