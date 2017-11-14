using System;
using Microsoft.Extensions.CommandLineUtils;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Ndx.Model;
using YamlDotNet.Serialization;

namespace Netdx
{
    internal class GenerateTypeInfo
    {
        public static string Name => "Generate-TypeInfo";

        internal static Action<CommandLineApplication> Register()
        {
            return
              (CommandLineApplication target) =>
              {
                  var infiles = target.Argument("input", "Input 'packet-*.c' files to be analyzed command.", true);
                  var outdir = target.Option("-o|--outdir", "The output directory where to store the type information files.", CommandOptionType.SingleValue);
                  target.Description = "Reads packet dissector source file(s) and produces type information in form of YAML document.";
                  target.HelpOption("-?|-h|--help");
                  target.OnExecute(() =>
                  {
                      if (infiles.Values.Count == 0)
                      {
                          target.Error.WriteLine("No input specified!");                          
                          target.ShowHelp(Name);
                          return 0;
                      }
                      var cmd = new GenerateTypeInfo();
                      int count=0;
                      foreach (var infile in infiles.Values)
                      {
                          var check = Regex.Match(infile, "packet-(?<PROTOCOL>[^\\.]+).c");
                          if (check.Success)
                          {
                              var prefix = check.Groups["PROTOCOL"].Value;
                              var outfile = Path.ChangeExtension(outdir.HasValue() ? Path.Combine(outdir.Value(), Path.GetFileName(infile)) : infile, "yaml");

                              Console.Write($"{infile} -> {outfile}: protocol = {prefix}, ");
                              var fields = cmd.Execute(prefix, infile, outfile);
                              Console.WriteLine($" {fields} fields.");
                          }
                          else
                          {
                              Console.WriteLine($"Bad input file '{infile}': this does not contain any protocol dissector.");
                          }
                      }
                      return count;
                  });
              };
        }
        
        static Regex m_rx = new Regex("{\\s*\"(?<Description>[^\"]*)\"\\s*,\\s*\"(?<Name>[^\"]*)\"\\s*,\\s*(?<Type>\\w+)\\s*,\\s*(?<Radix>\\w+)");

        public int Execute(string prefix, string inpath, string outpath)
        {
            var content = File.ReadAllText(inpath);
            var ms = m_rx.Matches(content);
            var protocol = new Protocol()
            {
                Name = prefix
            };

            foreach (Match m in ms)
            {
                var info = m.Groups["Description"].Value;
                var name = m.Groups["Name"].Value;
                var type = m.Groups["Type"].Value;
                var radix = m.Groups["Radix"].Value;

                if (name.StartsWith(prefix))
                {
                    var field = new ProtocolField()
                    {
                        Name = name,
                        Display = GetFieldDisplay(radix),
                        Info = info,
                        Type = GetFieldType(type),
                    };
                    if (!protocol.Fields.ContainsKey(name))
                        protocol.Fields.Add(name, field);
                }
            }

            var serializer = new Serializer();
            using (var outstream = new StreamWriter(File.OpenWrite(outpath)))
            {
                serializer.Serialize(outstream, protocol);
            }
            return ms.Count;    
        }

        private FieldType GetFieldType(string type)
        {
            return (FieldType)Enum.Parse(typeof(FieldType), type.Replace("_", ""), true);           
        }

        private FieldDisplay GetFieldDisplay(string display)
        {
            return (FieldDisplay)Enum.Parse(typeof(FieldDisplay), display.Replace("_", ""), true);
        }
    }
}