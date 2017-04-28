using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;

namespace Ndx.Hadoop.LoadTrace
{
    class Program
    {
        /// <summary>
        /// Loads the metacap file into Cassandra DB.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var commandLineApplication =
                new CommandLineApplication(throwOnUnexpectedArg: false);

            var dbhost = commandLineApplication.Option("-d|--db <hostURL>",
                "Represents URL(s) of the Cassandra DB node(s).",
                CommandOptionType.MultipleValue);
            dbhost.Template = "localhost";

            commandLineApplication.HelpOption("-? | -h | --help");
            commandLineApplication.FullName = Resources.ApplicationName;
            commandLineApplication.Description = Resources.Description;


            commandLineApplication.Command("Load-Trace", (target) =>
            {
                target.Description = "Exports packets payload to output zip file.";
                target.HelpOption("-?|-h|--help");

                var infile = new CommandArgument()
                {
                    Name = "-i|--input <path>",
                    Description = "Read metacap file from the specified file name.",
                    ShowInHelpText = true
                };

                var targetFile = new CommandArgument()
                {
                    Name = "-t|--target <path>",
                    ShowInHelpText = true,
                    Description = "Specifies the HDFS path where to store the PCAP file."
                };

                target.Arguments.Add(infile);
                target.Arguments.Add(targetFile);
                target.OnExecute(() =>
                {

                    if (String.IsNullOrEmpty(infile.Value))
                    {
                        Console.WriteLine("Error: No input file specified!");
                        commandLineApplication.ShowHelp();
                        return -1;
                    }
                    if (!dbhost.HasValue())
                    {
                        Console.WriteLine("Error: No Cassandra host specified!");
                        commandLineApplication.ShowHelp();
                        return -1;
                    }
                    if (String.IsNullOrEmpty(targetFile.Value))
                    {
                        Console.WriteLine("Error: No target path specified!");
                        commandLineApplication.ShowHelp();
                        return -1;
                    }

                    var inputPath = Path.GetFullPath(infile.Value);
                    var inputName = Path.GetFileName(infile.Value);                    
                    var keyspace = targetFile.Value.Replace('/', '_');
                    var targetPath = $"hdfs://{targetFile.Value}";
                    Console.WriteLine($"Load metacap '{inputPath}' to Cassandra DB '{dbhost.Value()}' and uploading pcap to '{targetFile.Value}'.");

                    throw new NotImplementedException();
                    var command = new UploadTraceCommand()
                    {

                    };
                    // execute command
                    var results = command.Invoke().Cast<string>();
                    Console.WriteLine(String.Join(Environment.NewLine, results));
                    return 0;

                   
                });
            });



            commandLineApplication.OnExecute(() =>
            {
                commandLineApplication.ShowRootCommandFullNameAndVersion();
                commandLineApplication.ShowHelp();
                return 0;
            });
            commandLineApplication.Execute(args);
        }
    }
}
