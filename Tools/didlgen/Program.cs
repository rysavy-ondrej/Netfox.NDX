using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace didlgen
{
    class Program
    {
        static Regex rx = new Regex("{\\s*\"(?<Description>[^\"]*)\"\\s*,\\s*\"(?<Name>[^\"]*)\"\\s*,\\s*(?<Type>\\w+)\\s*,\\s*(?<Radix>\\w+)");

        static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                Console.WriteLine("Usage: didlgen FOLDER");
                return;
            }
            var packetFiles = Directory.EnumerateFiles(args[0], "packet-*.c");


            foreach(var infile in packetFiles)
            {
                using (var outfile = new StreamWriter(File.OpenWrite(Path.ChangeExtension(infile, "yaml"))))
                {
                    outfile.WriteLine($"---");
                    var content = File.ReadAllText(infile);
                    var ms = rx.Matches(content);

                    foreach (Match m in ms)
                    {
                        var info = m.Groups["Description"].Value;
                        var name = m.Groups["Name"].Value;
                        var type = m.Groups["Type"].Value;
                        var radix = m.Groups["Radix"].Value;

                        outfile.WriteLine($"{name}:");
                        outfile.WriteLine($"  type: {type}");
                        outfile.WriteLine($"  base: {radix}");
                        outfile.WriteLine($"  info: {info}");
                    }
                }
            }
        }
    }
}
