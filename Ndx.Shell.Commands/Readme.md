# Ndx.Shell.Commands
The library provide a simple implementation of command pattern. It ressembles commands in PowerShell, but 
without all the dependencies and complexity of PowerShell environment.
This library can be comnined with Microsoft CommandLineUtils extension.

To use with CommandLineUtils, the main class instantiates `CommandLineApplication`
and defines all available commands by invoking `Command` method.

```csharp
using Microsoft.Extensions.CommandLineUtils;

class MainClass
{
    public static void Main(string[] args)
    {
        var commandLineApplication = new CommandLineApplication(throwOnUnexpectedArg: false);
        
        commandLineApplication.Command("Print-CurrentTime", (target) =>
        {
            target.Description = "Prints the current date and time.";
            target.HelpOption("-?|-h|--help");
            target.OnExecute(() =>
            {
                var cmd = new PrintDateCommand();
                var results = cmd.Invoke().Cast<string>();
                results.ForEach(Console.WriteLine);
                return 0;         
            });
        });

        commandLineApplication.HelpOption("-? | -h | --help");
        commandLineApplication.FullName = "DateTime";
        commandLineApplication.Description = "Tool that provides various Date/Time information.";

        commandLineApplication.OnExecute(() =>
        {
            commandLineApplication.ShowHelp();
            return 0;
        });
        commandLineApplication.Execute(args);
    }
}
```

Each command is implemented in a separate class. Class `PrintDateCommand`
overrides method that are executed before the data are processed, after the data are processed and for each input record.

```csharp
internal class PrintDateCommand : Command
{
    [Parameter(Mandatory = false)]
    public string Format { get; set; }

    protected override void BeginProcessing()
    {        
    }

    protected override void EndProcessing()
    {           
    }

    protected override void ProcessRecord()
    {
        var dateString = thisFormat != null ? DateTime.Now.ToString(Format) : DateTime.Now.ToString();
        WriteObject(dateString);
    }
}
```