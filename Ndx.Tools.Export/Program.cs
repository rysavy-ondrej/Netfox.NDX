using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Ndx.Tools.Export
{
    class Program
    {
        static void Main(string[] args)
        {

            var commandLineApplication =
                new CommandLineApplication(throwOnUnexpectedArg: false);

            var infile = commandLineApplication.Option("-r|--read <infile>",
                "Read metacap file from the specifid file name.",
                CommandOptionType.SingleValue);

            var outfile = commandLineApplication.Option("-w|--write <outfile>",
                "Write the exported items to the specified file/folder.",
                CommandOptionType.SingleValue);

            commandLineApplication.Command("ConvertTo-Rocks", (target) =>
            {
                target.Description = "Exports metacap file to RocksDB.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    var inputFile = infile.Value();
                    if (!File.Exists(inputFile))
                    {
                        Console.Error.WriteLine("Input file not found.");
                        return -1;
                    }
                    var outputFile = outfile.Value();
                    if (Directory.Exists(outputFile))
                    {
                        Console.Error.WriteLine("Output database already exists, it will be replaced.");
                        Directory.Delete(outputFile, true);

                    }


                    var cmd = new ConvertToRocks()
                    {
                        Metacap = inputFile,
                        RocksDbFolder = outputFile
                    };
                    // execute command
                    var results = cmd.Invoke().Cast<string>();
                    return results.Count();
                });
            });


            commandLineApplication.Command("Show-Rocks", (target) =>
            {
                target.Description = "Exports metacap file to RocksDB.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    var cmd = new ShowRocks()
                    {
                        RocksDbFolder = infile.Value()
                    };
                    // execute command
                    var results = cmd.Invoke().Cast<string>();
                    foreach (var item in results)
                    {
                        Console.WriteLine(item);
                    }
                    return 0;
                });
            });


            commandLineApplication.Command("Show-Rocks", (target) =>
            {
                target.Description = "Prints the content of the RocksDB.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    var cmd = new ShowRocks()
                    {
                        RocksDbFolder = infile.Value()
                    };
                    // execute command
                    var results = cmd.Invoke().Cast<string>();
                    foreach (var item in results)
                    {
                        Console.WriteLine(item);
                    }
                    return 0;
                });
            });

            commandLineApplication.Command("ConvertTo-Json", (target) =>
            {
                target.Description = "Prints the content of the metacap in JSON representation.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    var cmd = new ConvertToJson()
                    {
                        Metacap = infile.Value()
                    };
                    // execute command
                    var results = cmd.Invoke().Cast<string>();
                    foreach (var item in results)
                    {
                        Console.WriteLine(item);
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
    }
}
