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
        /// </remarks>
        enum VolumeType { Directory, Zip }
        static void Main(string[] args)
        {
            CommandLineApplication commandLineApplication =
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
                

            commandLineApplication.Command("packet", (target) =>
            {
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    
                    var filterFun = getFilterFunction(filter.Value());
                    if (filterFun == null) return -1;
                    ExportPackets(infile.Value(), outfile.Value(), filterFun);
                    return 0;
                });
            });
            commandLineApplication.Command("flow", (target) =>
            {
                target.Description = "Exports flow payload to specified output object.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    Console.WriteLine($"export flow content, infile='{infile.Value()}', outfile='{outfile.Value()}', filter='{filter.Value()}'.");
                    return 0;
                });
            });

            commandLineApplication.Command("stream", (target) =>
            {
                target.Description = "Exports stream payload to specified output object.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    var filterFun = getFilterFunction(filter.Value());
                    if (filterFun == null) return -1;
                    StreamFollow(infile.Value(), outfile.Value(), filterFun);
                    return 0;
                });
            });

            commandLineApplication.Command("index", (target) =>
            {
                target.Description = "Index specified input file(s) and generates MCAP output.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    var filterFun = getFilterFunction(filter.Value());
                    if (filterFun == null) return -1;
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

        private static void IndexPcap(string v1, string v2, Func<FlowKey, bool> filterFun)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a filter function for the given filter expression.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static Func<FlowKey, bool> getFilterFunction(string filterString)
        {
            if (String.IsNullOrEmpty(filterString)) return (x) => true;
            var expr = FlowKeyFilterExpression.TryParse(filterString, out string errorMessage);
            if (expr == null)
            {
                Console.Error.WriteLine($"Filter error: {errorMessage}");
            }
            return expr.FlowFilter;
        }


        /// <summary>
        /// Exports packets from the specified source file to the given output folder or Zip file.
        /// </summary>
        /// <param name="infile"></param>
        /// <param name="outfile"></param>
        /// <param name="filter">A function that represents a filter on flow key.</param>
        private static void ExportPackets(string infile, string outfile, Func<FlowKey,bool> flowFilter)
        {
            var mcap = McapFile.Open(infile);
            if (File.Exists(outfile)) File.Delete(outfile);

            using (var outArchive = ZipFile.Open(outfile, ZipArchiveMode.Update))
            {
                foreach (var capId in mcap.Captures)
                {
                    foreach (var flow in mcap.GetKeyTable(capId).Where(x => flowFilter(x.Key)))
                    {
                        var path = $"{flow.Key.Protocol}@{flow.Key.SourceAddress}.{flow.Key.SourcePort}-{flow.Key.DestinationAddress}.{flow.Key.DestinationPort}";
                        var packets = mcap.GetPacketsBytes(capId, flow, McapFile.TransportContent);                        
                        foreach (var packet in packets)
                        {
                            var entry = outArchive.CreateEntry(Path.Combine(path, packet.Item2.Frame.FrameNumber.ToString().PadLeft(6, '0')));
                            using (var stream = entry.Open())
                            {
                                stream.Write(packet.Item1, 0, packet.Item1.Length);
                            }                            
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
        private static void StreamFollow(string infile, string outfile, Func<FlowKey, bool> flowFilter)
        {
            var mcap = McapFile.Open(infile);
            if (File.Exists(outfile)) File.Delete(outfile);

            using (var outArchive = ZipFile.Open(outfile, ZipArchiveMode.Update))
            {
                foreach (var capId in mcap.Captures)
                {
                    var biflows = mcap.GetConversations(capId).Where(entries => entries.Any(entry => entry.Key.Protocol == PacketDotNet.IPProtocolType.TCP && flowFilter(entry.Key)));
                    foreach (var biflow in biflows)
                    {
                        var conversationStream = mcap.GetConversationStream(capId, biflow, out FlowKey flowKey);

                        // print to file
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
        }        
    }
}
