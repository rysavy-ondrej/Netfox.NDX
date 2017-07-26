using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Ingest;
using Ndx.Model;
using Ndx.Captures;
using SharpPcap;
using SharpPcap.LibPcap;

public static class Capture
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
    
    static public IEnumerable<RawFrame> Filter(Predicate<FlowKey> filter, params string[] captures)
	{
		foreach(var capture in captures)
		{
        	foreach (var frame in PcapReader.ReadFile(capture))
        	{       
        		var flowKey = PacketAnalyzer.GetFlowKey(frame);
        		if (flowKey != null && filter(flowKey))
					yield return frame;
        	}			
		}
	}
    

	static public void Write(CaptureFileWriterDevice device, RawFrame frame)
	{
		var capture = new RawCapture(LinkLayers.Ethernet, new PosixTimeval(frame.Seconds, frame.Microseconds),frame.Bytes );
		device.Write(capture);
	}

    public static void PrintConversations(IEnumerable<Conversation> conversations)
    {
        foreach (var conv in conversations)
        {
            Console.WriteLine("{0}#{1}@{2}:{3}<->{4}:{5}", conv.ConversationId, conv.ConversationKey.IpProtocol, conv.ConversationKey.SourceIpAddress,conv.ConversationKey.SourcePort,
            conv.ConversationKey.DestinationIpAddress,conv.ConversationKey.DestinationPort);
        }
    }
}

public class Filter
{
	static public Predicate<FlowKey> Address(string address)
	{
		var ip = IPAddress.Parse(address);
		return (FlowKey  f) => ip.Equals(f.SourceIpAddress) || ip.Equals(f.DestinationIpAddress); 		
	}
}

