using System;
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
                        OutputPath = outfile.Value(),
                    };
                    // execute command
                    cmd.Execute(infile.Value());

                    Console.WriteLine("$Exported {cmd.Count()} IEC104 PDUs.");
                    return cmd.Count();
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
