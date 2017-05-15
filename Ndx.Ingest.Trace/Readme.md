# Ndx.Ingest.Trace

Library for manipulation of packet trace files. The main idea behind is to 
compute flow key index on source PCAP and use this index to access packet in PCAP files.

In particular, this library provides the following features:

* Computation of PCAP index to improve efficiency of PCAP manipulation
* Sequential access to PCAP files
* Access to packets which are part of a specific flow
* Stream export (similar to Follow Stream function of Wireshark)


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

* flows.key - a folder that contains flow key files.
* flows.record - a folder that contains flow record files.
* flows.features - a folder that contains flow feature files.
* blocks - a folder that contains subfolders. There is a subfolder for each flow.
* conversations - a file that contains a list of conversations. 

TODO: describe the content of all files....

TODO: reimplement the mcap file writer and reader for reflect this documentation.