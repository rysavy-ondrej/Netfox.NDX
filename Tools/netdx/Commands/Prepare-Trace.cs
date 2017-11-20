using Microsoft.Extensions.CommandLineUtils;
using Ndx.Captures;
using Ndx.Decoders;
using Ndx.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Netdx
{
    internal class PrepareTrace
    {
        public void Execute(string inpath, string outpath)
        {
            var factory = new DecoderFactory();
            using (var json = new PcapJsonStream(new StreamReader(File.OpenRead(inpath))))
            {
                JsonPacket packet;
                while((packet = json.ReadPacket()) != null)
                {
                    foreach(var proto in packet.Protocols)
                    {
                        var protoObj = packet.GetProtocol(proto);
                        var message = factory.DecodeProtocol(proto, protoObj);
                    }
                }
            }
        }

        internal static string Name = "Prepare-Trace";

        internal static Action<CommandLineApplication> Register()
        {
            return 
            (CommandLineApplication target) =>
            {
                var infiles = target.Argument("input", "Input trace in JSON format produces with 'TShark -T ek' command.", true);
                var outdir = target.Option("-o|--outdir", "The output directory were to put files with decoded packets.", CommandOptionType.SingleValue);
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
                    var cmd = new PrepareTrace();

                    foreach (var file in infiles.Values)
                    {
                        var infile = file;
                        var outfile = Path.ChangeExtension(outdir.HasValue() ? Path.Combine(outdir.Value(), Path.GetFileName(infile)) : infile, "zip");
                        Console.WriteLine($"{file}->{outfile}");
                        try
                        {
                            cmd.Execute(infile, outfile);
                        }
                        catch (Exception e)
                        {
                            target.Error.WriteLine();
                            target.Error.WriteLine($"ERROR: {e.Message}");
                            target.Error.WriteLine("Use switch -d to see details about this error.");
                        }

                    }
                    return 0;
                });
            };
        }
    }
}
