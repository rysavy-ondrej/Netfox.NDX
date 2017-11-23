using System;
using Microsoft.Extensions.CommandLineUtils;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Ndx.Model;
using YamlDotNet.Serialization;
using System.Linq;

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
                              try
                              {
                                  var fields = cmd.Execute(prefix, infile, outfile);
                                  Console.WriteLine($" {fields} fields.");
                              }
                              catch(Exception e)
                              {
                                  target.Error.WriteLine();
                                  target.Error.WriteLine($"ERROR: {e.Message}");
                                  target.Error.WriteLine("Use switch -d to see details about this error.");
                              }
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

        public int Execute(string protocolName, string inpath, string outpath)
        {
            var content = File.ReadAllText(inpath);
            var ms = m_rx.Matches(content);
            var protocol = new Protocol()
            {
                Name = protocolName
            };

            // first iteration: collect all available fields:
            foreach (Match m in ms)
            {
                var info = m.Groups["Description"].Value;
                var name = m.Groups["Name"].Value;
                var type = m.Groups["Type"].Value;
                var radix = m.Groups["Radix"].Value;

                if (name.StartsWith(protocolName))
                {
                    var field = new ProtocolField()
                    {
                        Name = name,
                        Display = GetFieldDisplay(radix),
                        Info = info,
                        Type = GetFieldType(type),
                    };
                    if (!protocol.FieldMap.ContainsKey(name))
                        protocol.FieldMap.Add(name, field);
                }
            }

            // second iteration: Supply JSON names:
            foreach(var f in protocol.FieldMap)
            {
                var prefix = GetPrefix(f.Key);
                if (protocol.FieldMap.TryGetValue(prefix, out var value))
                {
                    f.Value.JsonName = $"{value.Name}.{f.Value.Name}".Replace('.', '_');
                }
                else
                {   // no other field is a parent of this field, just use protocol prefix for name:
                    f.Value.JsonName = $"{protocolName}.{f.Value.Name}".Replace('.', '_');
                }
            }


            var serializer = new Serializer();
            using (var outstream = new StreamWriter(File.OpenWrite(outpath)))
            {
                serializer.Serialize(outstream, protocol);
            }
            return ms.Count;    
        }

        private string GetPrefix(string key)
        {
            var parts = key.Split('.');
            return String.Join(".", parts.Take(parts.Length - 1));
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