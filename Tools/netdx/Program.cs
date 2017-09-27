using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
namespace netdx
{
    class Program
    {
        static void Main(string[] args)
        {
            var commandLineApplication = new CommandLineApplication(throwOnUnexpectedArg: false);

            commandLineApplication.Command("Check-Trace", (target) =>
            {
                target.Arguments.Add(target.Argument("-i | --input", "Input trace in JSON format produces with 'TShark -T ek' command."));
                target.Arguments.Add(target.Argument("-r | --rules", "Rule file that represents the rules to be evaulated."));
                target.Description = "Evaluates rule based for the passed input trace.";
                target.HelpOption("-?|-h|--help");
                target.OnExecute(() =>
                {
                    var cmd = new CheckTrace();
                    var results = cmd.Invoke().Cast<string>();
                    foreach(var result in results)
                    {
                        Console.WriteLine(result);
                    }
                    return 0;
                });
            });

            commandLineApplication.HelpOption("-? | -h | --help");
            commandLineApplication.FullName = "netdx";
            commandLineApplication.Description = "Rule-based diagnostics of network communication.";

            commandLineApplication.OnExecute(() =>
            {
                commandLineApplication.ShowHelp();
                return 0;
            });
            commandLineApplication.Execute(args);
        }
    }
}
