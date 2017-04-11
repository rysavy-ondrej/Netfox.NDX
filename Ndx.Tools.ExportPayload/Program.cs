using Microsoft.Extensions.CommandLineUtils;
using Ndx.Ingest.Trace;
using PacketDotNet;
using PacketDotNet.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ndx.Tools.ExportPayload
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
        /// </remarks>
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
                    if (filterFun == null)
                    {
                        return -1;
                    }

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
                    Console.WriteLine($"export flow content, infile='{infile.Value()}', outfile='{outfile.Value()}', filter='{filter.Value()}'.");
                    throw new NotImplementedException();
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
                    IndexPcap(infile.Value(), outfile.Value(), filterFun);
                    return 0;
                });
            });

            commandLineApplication.HelpOption("-? | -h | --help");

            commandLineApplication.OnExecute(() =>
            {
                commandLineApplication.ShowHelp();
                return 0;
            });
            commandLineApplication.Execute(args);

        }


        private static void IndexPcap(string inputPath, string outputPath, Func<FlowKey, bool> filterFun)
        {
            var consumer = new ZipFileConsumer(inputPath, outputPath);
            var cts = new CancellationTokenSource();
            var reader = new PcapReaderProvider(32768, 1000, cts.Token);

            var ingestOptions = new IngestOptions() { FlowFilter = filterFun };
            var ingest = new PcapFileIngestor(reader.RawFrameSource, null, consumer.PacketBlockTarget, consumer.FlowRecordTarget, ingestOptions);

            var fileInfo = new FileInfo(inputPath);
            reader.ReadFrom(fileInfo);
            reader.Complete();

            Task.WaitAll(ingest.Completion);
            consumer.Close();
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

            var exportfun = McapFilePacketProviderExtension.FrameContent;
            switch (pdutype)
            {
                case PduType.Network: exportfun = McapFilePacketProviderExtension.NetworkContent; break;
                case PduType.Frame: exportfun = McapFilePacketProviderExtension.FrameContent; break;
                case PduType.Transport: exportfun = McapFilePacketProviderExtension.TransportContent; break;
                case PduType.Application: exportfun = McapFilePacketProviderExtension.PayloadContent; break;
            }

            using (var outArchive = ZipFile.Open(outfile, ZipArchiveMode.Update))
            {
                foreach (var flow in mcap.GetKeyTable().Where(x => flowFilter(x.Key)))
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

        /// <summary>
        /// This function exports complete TCP streams into a single file with an index file.
        /// </summary>
        /// <param name="infile"></param>
        /// <param name="outfile"></param>
        /// <param name="flowFilter"></param>
        private static void StreamFollow(string pcapfile, string outfile, Func<FlowKey, bool> flowFilter)
        {
            var mcapfile = Path.ChangeExtension(pcapfile, "mcap");
            var mcap = McapFile.Open(mcapfile, pcapfile);
            if (File.Exists(outfile))
            {
                File.Delete(outfile);
            }

            using (var outArchive = ZipFile.Open(outfile, ZipArchiveMode.Create))
            {
                throw new NotImplementedException();
                /*
                Console.WriteLine("Writing streams:");
                var biflows = mcap.GetConversations().Where(entries => entries.Any(entry => entry.Key.Protocol == PacketDotNet.IPProtocolType.TCP && flowFilter(entry.Key)));
                foreach (var biflow in biflows)
                {
                    var conversationStream = mcap.GetConversationStream(biflow, out FlowKey flowKey);
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
