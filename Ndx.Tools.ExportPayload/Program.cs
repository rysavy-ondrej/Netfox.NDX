using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Ndx.Ingest;
using Ndx.Ingest.Trace;
using System.IO;
using System.IO.Compression;
using PacketDotNet.Utils;
using PacketDotNet;
using System.Diagnostics;

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

            var volume = commandLineApplication.Option("-v|--volume <volume>",
                "Specifies a type of the output volume.",
                CommandOptionType.SingleValue);
                

            commandLineApplication.Command("packet", (target) =>
            {
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    var voltype = VolumeType.Directory;
                    Enum.TryParse<VolumeType>(volume.Value(), true, out voltype);
                    
                    var filterFun = getFilterFunction(filter.Value());
                    ExportPackets(infile.Value(), outfile.Value(), voltype, filterFun);
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
                    StreamFollow(infile.Value(), outfile.Value(), filterFun);
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

        /// <summary>
        /// Gets a filter function for the given filter expression.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private static Func<FlowKey, bool> getFilterFunction(string v)
        {
            if (String.IsNullOrEmpty(v)) return (x) => true;
            var expr = FilterExpression.TryParse(v);

            return expr.GetFlowFilter();
        }


        /// <summary>
        /// Exports packets from the specified source file to the given output folder or Zip file.
        /// </summary>
        /// <param name="infile"></param>
        /// <param name="outfile"></param>
        /// <param name="filter">A function that represents a filter on flow key.</param>
        private static void ExportPackets(string infile, string outfile, VolumeType outtype, Func<FlowKey,bool> flowFilter)
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
                        var packets = mcap.GetPacketsContent(capId, flow, McapFile.TransportContent);                        
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


        public enum FlowDirection { Upflow, Downflow }
        public struct Segment
        {
            public FlowDirection Direction;
            public TcpPacket Packet;

            public Segment(FlowDirection direction, TcpPacket packet) : this()
            {
                this.Direction = direction;
                this.Packet = packet;
            }

            public uint S => Direction == FlowDirection.Upflow ? Packet.SequenceNumber : Packet.AcknowledgmentNumber;
            public uint R => Direction == FlowDirection.Downflow ? Packet.SequenceNumber : Packet.AcknowledgmentNumber;
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
                        if (biflow.Length == 1)
                        { // unidirectional conversation...ignore now but implement later.
                            continue;
                        }

                        var flow0 = biflow[0];
                        var flow1 = biflow[1];
                        
                        var tcp0 = mcap.GetPacketsContent(capId, flow0, McapFile.TransportContent).Select(x => new PacketDotNet.TcpPacket(new ByteArraySegment(x.Item1))).ToList();
                        var tcp1 = mcap.GetPacketsContent(capId, flow1, McapFile.TransportContent).Select(x => new PacketDotNet.TcpPacket(new ByteArraySegment(x.Item1))).ToList();

                        IList<TcpPacket> clientFlow = null;
                        IList<TcpPacket> serverFlow = null;
                        uint clientIsn = 0;
                        uint serverIsn = 0;
                        // find who initiated conversation:
                        var tcp0syn = tcp0.FirstOrDefault(x => x.Syn);
                        var tcp1syn = tcp1.FirstOrDefault(x => x.Syn);

                        // currently, we do not support incomplete conversations...but this will be implemented in future.
                        if (tcp0syn == null || tcp1syn == null)
                        {
                            continue;
                        }

                        // Note: SYN and FIN flags are treated as representing 1-byte payload
                        if (tcp1syn.Ack && tcp1syn.AcknowledgmentNumber == tcp0syn.SequenceNumber+1)
                        {
                            clientFlow = tcp0;
                            serverFlow = tcp1;
                            clientIsn = tcp0syn.SequenceNumber;
                            serverIsn = tcp1syn.SequenceNumber;
                        }
                        else
                        {
                            clientFlow = tcp1;
                            serverFlow = tcp0;
                            clientIsn = tcp1syn.SequenceNumber;
                            serverIsn = tcp0syn.SequenceNumber;                                                                                             
                        }

                        // compute a total order on the packets:
                        //
                        // s ... sequence # of client
                        // r ... sequence # of server
                        //
                        // considering that each message can contain seq and ack numbers
                        // then each TCP segment is associated with (s,r) pair:
                        //
                        // client->server message:  s = seq, r = ack
                        // server->client message:  s = ack, r = seq
                        //
                        // It holds that:
                        // for all (s,r),(s',r'): s < s' ==> r <= r'
                        // and
                        // for all (s,r),(s',r'): r < r' ==> s <= s' .
                        // 
                        // in other words a sequence {(si,ri)} is monotonic
                        // for total ordering of TCP segments that we are looking for.
                        var conversation = Enumerable.Union(
                            clientFlow.Select(x => new Segment(FlowDirection.Upflow, x)),
                            serverFlow.Select(x => new Segment(FlowDirection.Downflow, x))).OrderBy(x => x, new RSComparer()).ToList();

                        // print to file
                        var path = $"{flow0.Key.Protocol}@{flow0.Key.SourceAddress}.{flow0.Key.SourcePort}-{flow0.Key.DestinationAddress}.{flow0.Key.DestinationPort}";
                        var entry = outArchive.CreateEntry(path);
                        using (var stream = entry.Open())
                        {
                            foreach (var segment in conversation)
                            {
                                var bas = segment.Packet.PayloadPacket.BytesHighPerformance;
                                stream.Write(bas.Bytes, bas.Offset, bas.Length);
                            }
                        }
                    }       
                    
                }
            }
        }


        public class RSComparer : IComparer<Segment>
        {
            public int Compare(Segment x, Segment y)
            {
                if (x.S == y.S && x.R == y.R) return 0;
                if (x.S <= y.S && x.R <= y.R) return -1;
                if (x.S >= y.S && x.R >= y.R) return 1;
                throw new ArgumentException($"Cannot compare ({x.S},{x.R}) and ({y.S},{y.R}).");
            }
        }
    }
}
