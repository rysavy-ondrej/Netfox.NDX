using Microsoft.Extensions.CommandLineUtils;

namespace Netdx
{
    class Program
    {
        static void Main(string[] args)
        {            
            var commandLineApplication = new CommandLineApplication(true);

            using (var progress = new ProgressBar())
            {

                commandLineApplication.Command(GenerateProto.Name, GenerateProto.Register(progress));
                commandLineApplication.Command(GenerateTypeInfo.Name, GenerateTypeInfo.Register(progress));
                commandLineApplication.Command(DecodeTrace.Name, DecodeTrace.Register(progress));
                commandLineApplication.Command(ExportTrace.Name, ExportTrace.Register(progress));
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
                catch (CommandParsingException e)
                {
                    commandLineApplication.Error.WriteLine(e.Message);
                }
            }
        }
    }
}
