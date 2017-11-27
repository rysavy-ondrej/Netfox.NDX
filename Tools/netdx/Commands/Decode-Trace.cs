using Google.Protobuf;
using Microsoft.Extensions.CommandLineUtils;
using Ndx.Captures;
using Ndx.Decoders;
using Ndx.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Netdx
{
    internal class DecodeTrace
    {
        public void Execute(Stream instream, Stream outstream)
        {
            var factory = new DecoderFactory();
            var decoder = new PacketDecoder();
            using (var pcapstream = new PcapJsonStream(new StreamReader(instream)))
            {
                string jsonLine;
                while((jsonLine = pcapstream.ReadPacketLine()) != null)
                {
                    var packet = decoder.Decode(factory, jsonLine);
                    packet.WriteDelimitedTo(outstream);

                }
                outstream.Flush();
            }
        }

        internal static string Name = "Decode-Trace";

        internal static Action<CommandLineApplication> Register()
        {
            return 
            (CommandLineApplication target) =>
            {
                var infiles = target.Argument("input", "Input trace in JSON format produces with 'TShark -T ek' command. Use - for reading from stdin.", true);
                var outdir = target.Option("-o|--outdir", "The output directory were to put files with decoded packets.", CommandOptionType.SingleValue);
                var outfile = target.Option("-" +"w|--writeTo", "The output filename were to put decoded packets. If multiple files are used, the output is concatenated in this single file.", CommandOptionType.SingleValue);
                target.Description = "Prepares data for further processing by NDX tools.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    if (infiles.Values.Count == 0)
                    {
                        target.Error.WriteLine("No input specified!");
                        target.ShowHelp(Name);
                        return 0;
                    }
                    var cmd = new DecodeTrace();


                    
                    Stream GetOutstream(string infile, out string outpath)
                    {
                        var pathPrefix = outdir.HasValue() ? outdir.Value() : String.Empty;
                        if (outfile.HasValue())
                        {                            
                            outpath = Path.Combine(pathPrefix, outfile.Value());
                            return File.Open(outpath, FileMode.Append, FileAccess.Write);
                        }
                        else
                        {
                            outpath = Path.ChangeExtension(Path.Combine(pathPrefix, infile), "dcap");
                            return File.Open(outpath, FileMode.OpenOrCreate, FileAccess.Write);
                        }
                    }

                    foreach (var infile in infiles.Values)
                    {
                        using (var instream = infile.Equals("STDIN") ? Console.OpenStandardInput() : File.OpenRead(infile))
                        using (var outstream = GetOutstream(infile, out var filename))
                        {
                            Console.WriteLine($"{infile}->{filename}");

                            try
                            {
                                cmd.Execute(instream, outstream);
                            }
                            catch (Exception e)
                            {
                                target.Error.WriteLine();
                                target.Error.WriteLine($"ERROR: {e.Message}");
                                target.Error.WriteLine("Use switch -d to see details about this error.");
                            }
                        }
                    }
                    return 0;
                });
            };
        }
    }
}
