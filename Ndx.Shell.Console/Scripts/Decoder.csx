using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Ingest;
using Ndx.Model;
using Ndx.Captures;
using Ndx.TShark;

public static class Decoder
{
	public static void LoadAndSendToTShark(Ndx.TShark.WiresharkSender sender, IEnumerable<RawFrame> frames)
	{
	    foreach (var f in frames)
       	{       
        	sender.Send(f);	    
        }
	}
}