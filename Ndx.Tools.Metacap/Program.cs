using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Ndx.Ingest.Trace;
using PacketDotNet;

namespace Ndx.Tools.Metacap
{
    class Program
    {
        /// <summary>
        /// Specifies the volume type. It is used to specify which 
        /// target volume to use for writing output data.
        /// </summary>
        /// <remarks>
        /// Notes on the usage of Microsoft.Extensions.CommandLineUtils:
        /// http://jameschambers.com/2015/09/supporting-options-and-arguments-in-your-dnx-commands/
        /// 
        /// Example of the usage:
        /// ndx.tools.exportpayload.exe -r "C:\Users\Ondrej\Documents\Network Monitor 3\Captures\bb7de71e185a2a7818fff92d3ec0dc05.cap" -w bb7de71e185a2a7818fff92d3ec0dc05.mcap Create-Index
        /// ndx.tools.exportpayload.exe -r "C:\Users\Ondrej\Documents\Network Monitor 3\Captures\bb7de71e185a2a7818fff92d3ec0dc05.cap" -w out.zip -f "SourcePort == 80" Export-Stream
        /// ndx.tools.exportpayload.exe -r "C:\Users\Ondrej\Documents\Network Monitor 3\Captures\bb7de71e185a2a7818fff92d3ec0dc05.cap" -w out.zip -f "SourcePort == 80" Export-Packets
        /// 
        /// The implementation relies on LINQ:
        /// https://code.msdn.microsoft.com/101-LINQ-Samples-3fb9811b
        /// 
        /// RX and TPL Dataflow
        /// http://blogs.interknowlogy.com/2013/01/31/mixing-tpl-dataflow-with-reactive-extensions/
        /// </remarks>
        /// 
        enum VolumeType { Directory, Zip }
        enum PduType { Frame, Network, Transport, Application }
        static void Main(string[] args)
        {
            var commandLineApplication =
                new CommandLineApplication(throwOnUnexpectedArg: false);

            var infile = commandLineApplication.Option("-r|--read <infile>",
                "Read packet data from infile, can be any supported capture file format.",
                CommandOptionType.SingleValue);

            var outfile = commandLineApplication.Option("-w|--write <outfile>",
                "Write the output items to file rather than printing them out.",
                CommandOptionType.SingleValue);

            var filter = commandLineApplication.Option("-f|--filter <expression>",
                "Selects which packets will be processed. If no expression is given, all packets will be processed.",
                CommandOptionType.SingleValue);


            commandLineApplication.Command("Export-Packets", (target) =>
            {
                target.Description = "Exports packets payload to output zip file.";
                target.HelpOption("-?|-h|--help");
                var pdu = new CommandArgument() { Name = "Pdu", ShowInHelpText = true, Description ="A type of pdu to export. It can be one of the following values: Network, Transport, Application or Frame." };
                target.Arguments.Add(pdu);
                target.OnExecute(() =>
                {
                    Enum.TryParse(pdu.Value, out PduType pdutype);
                    var filterFun = GetFilterFunction(filter.Value());
                    ExportPackets(infile.Value(), outfile.Value(), pdutype, filterFun);
                    return 0;
                });
            });
            commandLineApplication.Command("Export-Flow", (target) =>
            {
                target.Description = "Exports flow payload to specified output object.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    var filterFun = GetFilterFunction(filter.Value());
                    ExportFlow(infile.Value(), outfile.Value(), filterFun);
                    return 0;
                });
            });

            commandLineApplication.Command("Export-Stream", (target) =>
            {
                target.Description = "Exports stream payload to specified output object.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    var filterFun = GetFilterFunction(filter.Value());
                    StreamFollow(infile.Value(), outfile.Value(), filterFun);
                    return 0;
                });
            });

            commandLineApplication.Command("Create-Index", (target) =>
            {
                target.Description = "Index specified input file(s) and generates MCAP output.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    var filterFun = GetFilterFunction(filter.Value());
                    if (String.IsNullOrEmpty(infile.Value()) || !File.Exists(infile.Value()))
                    {
                        Console.Error.WriteLine($"Unable to find input file '{infile.Value()}'");
                        return -1;
                    }
                    var mcapfile = String.IsNullOrEmpty(outfile.Value()) ? Path.ChangeExtension(infile.Value(), "mcap") : outfile.Value();
                    IndexPcap(infile.Value(), mcapfile, filterFun);
                    return 0;
                });
            });

            commandLineApplication.Command("Verify-Index", (target) =>
            {
                target.Description = "Verifies the integrity of the specified mcap.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    // prepare command

                    var cmd = new VerifyIndex()
                    {
                        Capfile = infile.Value()
                    };
                    // execute command
                    var results = cmd.Invoke().Cast<string>();
                    // print results
                    if (results.Count()>0)
                    {
                        Console.WriteLine("Metacap integrity errors:");
                        foreach (var item in results)
                        {
                            Console.WriteLine(item);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No integrity errors found.");
                    }

                    return 0;
                });
            });
                    

            commandLineApplication.HelpOption("-? | -h | --help");
            commandLineApplication.FullName = Resources.ApplicationName;
            commandLineApplication.Description = Resources.Description;
            commandLineApplication.OnExecute(() =>
            {
                commandLineApplication.ShowHelp();
                return 0;
            });
            commandLineApplication.Execute(args);

        }


        private static void IndexPcap(string inputPath, string outputPath, Func<FlowKey, bool> filterFun)
        {
            using (var consumer = new McapFileConsumer(outputPath))
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

        /// <summary>
        /// Gets a filter function for the given filter expression.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static Func<FlowKey, bool> GetFilterFunction(string filterString)
        {
            if (String.IsNullOrEmpty(filterString))
            {
                return (x) => true;
            }

            var expr = FlowKeyFilterExpression.TryParse(filterString, out string errorMessage);
            if (expr == null)
            {
                Console.Error.WriteLine($"Filter error: {errorMessage}. No filter will be applied.");
                return (x) => true;
            }
            
            return expr.FlowFilter;
        }


        /// <summary>
        /// Exports packets from the specified source file to the given output folder or Zip file.
        /// </summary>
        /// <param name="infile">Input pcap file.</param>
        /// <param name="outfile">Output zip archive with exported packets.</param>
        /// <param name="pdutype">A type of pdu to export.</param>
        /// <param name="filter">A function that represents a filter on flow key.</param>
        private static void ExportPackets(string pcapfile, string outfile, PduType pdutype, Func<FlowKey, bool> flowFilter)
        {
            var mcapfile = Path.ChangeExtension(pcapfile, "mcap");
            var mcap = McapFile.Open(mcapfile, pcapfile);
            if (File.Exists(outfile)) File.Delete(outfile);

            var exportfun = McapFileFlowExtension.FrameContent;
            switch (pdutype)
            {
                case PduType.Network: exportfun = McapFileFlowExtension.NetworkContent; break;
                case PduType.Frame: exportfun = McapFileFlowExtension.FrameContent; break;
                case PduType.Transport: exportfun = McapFileFlowExtension.TransportContent; break;
                case PduType.Application: exportfun = McapFileFlowExtension.PayloadContent; break;
            }

            using (var outArchive = ZipFile.Open(outfile, ZipArchiveMode.Update))
            {
                foreach (var flow in mcap.FlowKeyTable.Where(x => flowFilter(x.Key)))
                {
                    var path = $"{flow.Key.Protocol}@{flow.Key.SourceAddress}.{flow.Key.SourcePort}-{flow.Key.DestinationAddress}.{flow.Key.DestinationPort}";
                    var packets = mcap.GetPacketsBytes(flow.Value, exportfun);
                    foreach (var packet in packets)
                    {
                        var entry = outArchive.CreateEntry(Path.Combine(path, packet.Item1.Frame.FrameNumber.ToString().PadLeft(6, '0')));
                        using (var stream = entry.Open())
                        {
                            stream.Write(packet.Item2, 0, packet.Item2.Length);
                        }
                    }
                }

            }
        }

        public static void ExportFlow(string pcapfile, string outfile, Func<FlowKey, bool> flowFilter)
        {
            var mcapfile = Path.ChangeExtension(pcapfile, "mcap");
            var mcap = McapFile.Open(mcapfile, pcapfile);
            if (File.Exists(outfile))
            {
                File.Delete(outfile);
            }

            using (var outArchive = ZipFile.Open(outfile, ZipArchiveMode.Create))
            {
                bool filter(FlowKey key)
                {
                    return key.Protocol == IPProtocolType.TCP && flowFilter(key);
                }

                var flowTable = mcap.FlowKeyTable.Where(x => filter(x.Key));

                foreach(var flow in flowTable)
                {
                    var path = $"{flow.Key.Protocol}@{flow.Key.SourceAddress}.{flow.Key.SourcePort}-{flow.Key.DestinationAddress}.{flow.Key.DestinationPort}";
                    var entry = outArchive.CreateEntry(path);
                    using (var stream = entry.Open())
                    {
                        mcap.WriteTcpStream(flow.Value, stream);
                    }
                }
            }
        }

        public static void StreamFollow(string pcapfile, string outfile, Func<FlowKey, bool> flowFilter)
        { 
            var mcapfile = Path.ChangeExtension(pcapfile, "mcap");
            var mcap = McapFile.Open(mcapfile, pcapfile);
            if (File.Exists(outfile))
            {
                File.Delete(outfile);
            }

            using (var outArchive = ZipFile.Open(outfile, ZipArchiveMode.Create))
            {
                bool filter(FlowKey key)
                {
                    return key.Protocol == PacketDotNet.IPProtocolType.TCP && flowFilter(key);
                }

                var convTable = mcap.ConversationTable;
                /*
                foreach (var conv in convTable.Entries)
                {
                    var conversationStream = mcap.GetConversationStream(conv.OriginatorKey, conv.ResponderKey);
                    if (conversationStream == null) continue;
                    // print to file

                    var path = $"{flowKey.Protocol}@{flowKey.SourceAddress}.{flowKey.SourcePort}-{flowKey.DestinationAddress}.{flowKey.DestinationPort}";
                    Console.WriteLine($"{flowKey.Protocol}@{flowKey.SourceAddress}.{flowKey.SourcePort}<->{flowKey.DestinationAddress}.{flowKey.DestinationPort}");
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
                */
            }
        }

    }
}
