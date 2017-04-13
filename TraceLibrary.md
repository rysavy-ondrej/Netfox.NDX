# Ndx.Ingest.Trace

This library provides methods for ingestgin packet trace and creating metacap
and xcap file formats.


## Library Usage
Bellow are several code snippets illustrating the usage of ```NdxIndest.Trace``` library.


### Ingest Packet Capture in to Metacap file
To process packet trace in to metacap file, ```PcapFileIngestor``` class can be used.

The sample code is following:
```CSharp
void IndexPcap(string inputPath, string outputPath, Func<FlowKey, bool> filterFun)
{
    using(var consumer = new ZipFileConsumer(outputPath))
    {
        var cts = new CancellationTokenSource();
        var reader = new PcapReaderProvider(32768, 1000, cts.Token);
    
        var ingestOptions = new IngestOptions() { FlowFilter = filterFun };
        var ingest = new PcapFileIngestor(reader.RawFrameSource, consumer.RawFrameTarget, consumer.PacketBlockTarget, consumer.FlowRecordTarget, ingestOptions);

        var fileInfo = new FileInfo(inputPath);
        reader.ReadFrom(fileInfo);
        reader.Complete();

        Task.WaitAll(ingest.Completion, consumer.Completion);
    }
}
```
To initialize ```PcapFileIngestor``` we need to provide packet trace reader and several consumers, namely, 
a consumer of Frames, a consumer of PacketBlocks, and a consumer of FlowRecords.

When we want to produce zipped mcap file, we can utilize ```ZipFileConsumer``` class that 
provides all required consumers.