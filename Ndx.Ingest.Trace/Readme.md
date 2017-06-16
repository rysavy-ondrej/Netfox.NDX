# Ndx.Metacap

Library for manipulation of packet trace files. In comparison to other similar libraries the main idea here is to 
create a list of conversations first and then apply additional processing to selected conversation only.

In particular, this library provides the following features:

* Computation of PCAP index to improve efficiency of PCAP manipulation
* Sequential access to PCAP files
* Access to packets which are part of a specific conversation
* Stream export (similar to Follow Stream function of Wireshark)
* Efficient access to packet related to conversations.

## Data Model

## Processing

* RawFrames are parsed by PacketDotNet parsers. 
* FrameAttributes are extracted.
* Each frame is assigned a sequence number which is unique within its data source.
* For each packet its conversations are identified.
  There may be different conversations for a single packet as
  we recognize conversations at different level.
* All related conversations are updated with packet attributes.
* Frame content is sent along its key to packet content writer
* Frame attributes are sent along its key to packet attribute writer
* 

The packet header is decoded to extract key fields. A hash function is computed over the keys in order to look up the flow record in the flow cache. If an existing record is found, its values are updated, otherwise a record is created for the new flow. Records are flushed from the cache based on protocol information (e.g. if a FIN flag is seen in a TCP packet), a timeout, inactivity, or when the cache is full. The flushed records are finally sent to the traffic analysis application.



## Indexing PCAP
The following snippet generates the MCAP file for the given PCAP file. 
The MCAP file contains metadata information that is required for 
most of operations implemented in Ndx.Ingest.Trace library.
```CSharp
// Parameters:
// string inputPath - path to the source PCAP file.

var cts = new CancellationTokenSource();
var reader = new PcapReaderProvider(32768, 1000, cts.Token);
var consumer = new ZipFileConsumer(inputPath);

var ingestOptions = new IngestOptions() { FlowFilter = filterFun };
var ingest = new PcapFileIngestor(reader.RawFrameSource, null, consumer.PacketBlockTarget, consumer.FlowRecordTarget, ingestOptions);

// process input pcap file
var fileInfo = new FileInfo(inputPath);
reader.ReadFrom(fileInfo);
reader.Complete();

// wait till the process ends:
Task.WaitAll(ingest.Completion);
consumer.Close();
```


## Export Streams
The following snippet extracts application streams for all conversations 
satisfying ```flowFilter```. It stores the output to single zip file.
```CSharp
// Parameters:
// string infile - path to the source MCAP file.
// string outfile - path to the output ZIP file.
// Func<FlowKey,bool> flowFilter - function used to filter conversations.
var mcap = McapFile.Open(infile);

using (var outArchive = ZipFile.Open(outfile, ZipArchiveMode.Update))
{
    foreach (var capId in mcap.Captures)
    {
        var biflows = mcap.GetConversations(capId).Where(entries => entries.Any(entry =>flowFilter(entry.Key)));
        foreach (var biflow in biflows)
        {
            var conversationStream = mcap.GetConversationStream(capId, biflow, out FlowKey flowKey);

            var path = $"{flowKey.Protocol}@{flowKey.SourceAddress}.{flowKey.SourcePort}-{flowKey.DestinationAddress}.{flowKey.DestinationPort}";
            var entry = outArchive.CreateEntry(path);
            using (var stream = entry.Open())
            {
                foreach (var segment in conversationStream)
                {
                    var bas = segment.Packet.PayloadPacket.BytesHighPerformance;
                    stream.Write(bas.Bytes, bas.Offset, bas.Length);
                }
            }
        }
    }
}
```


## Metacap file format
Metacap is ZIP archive that contains meta information and index for PCAP files.
The structure of Metacap is as follows:
```
foo.mcap
  |--- index
  |--- files
  |         |--- 00000001 : PcapFile
  |         |--- 00000002	: PcapFile
  |
  |--- conversations
  |		  |--- 00000001	: Conversation
  |		  |--- 00000002	: Conversation
  |
  |
  |--- packets
  |		  |--- 00000001	: PacketBlock
  |		  |--- 00000002	: PacketBlock
  |
  |--- 
```