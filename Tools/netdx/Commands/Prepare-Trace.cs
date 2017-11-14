using Microsoft.Extensions.CommandLineUtils;
using Ndx.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Netdx
{
    internal class PrepareTrace
    {
        public string Input { get; set; }
        public string Output { get; set; }

        public void Execute()
        {
            using (var stream = File.OpenRead(Input))
            {
                
            }            
        }

        internal static string Name = "Prepare-Trace";

        internal static Action<CommandLineApplication> Register()
        {
            return 
            (CommandLineApplication target) =>
            {
                var infiles = target.Argument("input", "Input trace in JSON format produces with 'TShark -T ek' command.", true);
                var outdir = target.Option("-o|--outdir", "The output directory where to store the decoded packets.", CommandOptionType.SingleValue);
                target.Description = "Prepares data for further processing by NDX tools.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    if (infiles.Value == null)
                    {
                        target.Error.WriteLine("No input file specified!");
                        target.ShowHelp(Name);
                        return -1;
                    }
                    var cmd = new PrepareTrace()
                    {
                    };

                    foreach (var file in infiles.Values)
                    {
                        var infile = file;
                        var outfile = Path.ChangeExtension(outdir.HasValue() ? Path.Combine(outdir.Value(), Path.GetFileName(infile)) : infile, "dcap");
                        Console.WriteLine($"{file}->{outfile}");
                        if (File.Exists(infile))
                        {
                            cmd.Input = infile;
                            cmd.Output = outfile;
                            cmd.Execute();
                        }
                    }
                    return 0;
                });
            };
        }
    }
}
