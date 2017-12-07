# Ndx.Ipflow

Library for manipulation of packet trace files. In comparison to other similar libraries the main idea here is to 
create a list of conversations first and then apply additional processing to selected conversation only.

In particular, this library provides the following features:

* Identification of conversations and labeling frames with conversations identifier
* Infrastruture for implementing various methods of protocol identification
* Simple filter language based on flow key 

## Usage
The basic usage of conversation tracker is to label frames with the identified conversation id. The following
snippet demonstrates the pattern of conversation tracker use. The program count number of frames and conversations to 
and prints this information to the standard output.

```csharp
var frameAnalyzer = new FrameFlowHelper();
var frames = PcapFile.ReadFile(captureFile).AsObservable();            
var tracker = new ConversationTracker<Frame>(frameAnalyzer);

var conversationCount = 0;
var framesCount = 0;

var observer1 = new Ndx.Utils.Observer<Conversation>((_) => conversationCount++);
var observer2 = new Ndx.Utils.Observer<(int,Frame)>((_) => framesCount++);
using (tracker.Conversations.Subscribe(observer1))
using (tracker.Packets.Subscribe(observer2))
using (frames.Subscribe(tracker))
{ }
Console.WriteLine($"Done, conversations = {conversationCount}, frames = {framesCount}");
```