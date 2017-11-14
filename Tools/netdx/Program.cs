using Microsoft.Extensions.CommandLineUtils;

namespace Netdx
{
    class Program
    {
        static void Main(string[] args)
        {            
            var commandLineApplication = new CommandLineApplication(true);

            commandLineApplication.Command(GenerateProto.Name, GenerateProto.Register());
            commandLineApplication.Command(GenerateTypeInfo.Name, GenerateTypeInfo.Register());
            commandLineApplication.Command(PrepareTrace.Name, PrepareTrace.Register());
            commandLineApplication.HelpOption("-? | -h | --help");
            commandLineApplication.Name = typeof(Program).Assembly.GetName().Name;
            commandLineApplication.FullName = $"NDX Command Line Tools ({typeof(Program).Assembly.GetName().Version})";

            commandLineApplication.OnExecute(() =>
            {
                commandLineApplication.Error.WriteLine();
                commandLineApplication.ShowHelp();
                return -1;
            });
            try
            {
                commandLineApplication.Execute(args);
            }
            catch(CommandParsingException e)
            {
                commandLineApplication.Error.WriteLine(e.Message);
            }
        }
    }
}
