﻿using System;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;

namespace ExportIec104
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var commandLineApplication = new CommandLineApplication(throwOnUnexpectedArg: false);

            var infile = commandLineApplication.Option("-i|--input <infile>",
                "Read packet data from infile, can be any supported capture file format.",
                CommandOptionType.SingleValue);

            var outfile = commandLineApplication.Option("-o|--output <outfolder>",
                "Write the output items to the specified folder.",
                CommandOptionType.SingleValue);

            commandLineApplication.Command("Export-Packets", (target) =>
            {
                target.Description = "Exports IEC104 packets to output folder.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {

                    var cmd = new ExportIecCommand()
                    {
                        InputPath = infile.Value(),
                        OutputPath = outfile.Value(),
                    };
                    // execute command
                    var results = cmd.Invoke().Cast<string>();
                   
                    var count = results.Count();
                    Console.WriteLine("$Exported {count} IEC104 PDUs.");
                    return count;
                });
            });                         
           

            commandLineApplication.HelpOption("-? | -h | --help");
            commandLineApplication.FullName = "Ndx.Tools.ExportIec104";
            commandLineApplication.Description = "Exports all IEC PDUs found in the input PCAP file.";
            commandLineApplication.OnExecute(() =>
            {
                commandLineApplication.ShowHelp();
                return 0;
            });
            commandLineApplication.Execute(args);
        }

    }
}
