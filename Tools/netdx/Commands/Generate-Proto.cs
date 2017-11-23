using System;
using Microsoft.Extensions.CommandLineUtils;
using System.IO;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;
using System.Linq;
using YamlDotNet.Serialization;
using Ndx.Model;
using System.Collections.Generic;
using Google.Protobuf;

namespace Netdx
{
    internal class GenerateProto
    {
        public static string Name => "Generate-Proto";

        internal static Action<CommandLineApplication> Register()
        {
            return
              (CommandLineApplication target) =>
              {
                  var infiles = target.Argument("input", "Input 'packet-*.yaml' files to be used for generating proto files.", true);
                  var outdir = target.Option("-o|--outdir", "The output directory where to store the generated files.", CommandOptionType.SingleValue);
                  var jsonReader = target.Option("-j|--json", "If specified, it generates also C# class for reading packet from JSON string.", CommandOptionType.NoValue);
                  var pckgNs = target.Option("-n|--ns", "Specifies package namespace.", CommandOptionType.SingleValue);

                  target.Description = "Reads packet dissector type information and generates proto speficification and optinally also C# classes with helpers.";
                  target.HelpOption("-?|-h|--help");
                  target.OnExecute(() =>
                  {
                      if (infiles.Values.Count == 0)
                      {
                          target.Error.WriteLine("No input specified!");                          
                          target.ShowHelp(Name);
                          return 0;
                      }
                      var cmd = new GenerateProto();
                      if (pckgNs.HasValue()) cmd.m_packageNamespace = pckgNs.Value();
                      int count=0;
                      foreach (var infile in infiles.Values)
                      {
                          var outfile = Path.ChangeExtension(outdir.HasValue() ? Path.Combine(outdir.Value(), Path.GetFileName(infile)) : infile, "proto");
                          Console.Write($"{infile} -> {outfile}: ");

                          try
                          {
                              var fields = cmd.Execute(infile, outfile, jsonReader.HasValue());
                              Console.WriteLine($" {fields} fields.");
                          }
                          catch (Exception e)
                          {
                              target.Error.WriteLine();
                              target.Error.WriteLine($"ERROR: {e.Message}");
                              target.Error.WriteLine("Use switch -d to see details about this error.");
                          }
                      }
                      return count;
                  });
              };
        }

        string m_packageNamespace;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inpath">Path to YAML file with protocol definition.</param>
        /// <param name="outpath">Path to output PROTO file.</param>
        /// <returns></returns>
        public int Execute(string inpath, string outpath, bool generateCSharp = false)
        {
            var protocol = Protocol.DeserializeFromYaml(File.OpenRead(inpath));
            var fcount = 0;
            var protocolName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(protocol.Name);

            using (var outfile = new StreamWriter(File.Open(outpath, FileMode.Create, FileAccess.Write)))
            {
                outfile.WriteLine("syntax = \"proto3\";");
                if (!String.IsNullOrEmpty(m_packageNamespace)) outfile.WriteLine($"package {m_packageNamespace};");

                fcount += WriteMessage(protocol.FieldMap, protocolName, outfile, String.Empty);
                outfile.Flush();
            }
            if (generateCSharp)
                using (var outfile = new StreamWriter(File.Open(Path.ChangeExtension(outpath, ".Decode.cs"), FileMode.Create, FileAccess.Write)))
                {
                    outfile.WriteLine("using Newtonsoft.Json.Linq;");
                    outfile.WriteLine("using Google.Protobuf;");
                    outfile.WriteLine("using System;");
                    outfile.WriteLine($"namespace {m_packageNamespace}");
                    outfile.WriteLine("{");
                    outfile.WriteLine($"  public sealed partial class {protocolName}");
                    outfile.WriteLine( "  {");
                    outfile.WriteLine($"    public static {protocolName} DecodeJson(string jsonLine)");
                    outfile.WriteLine( "    {");
                    outfile.WriteLine( "      var jsonObject = JToken.Parse(jsonLine);");
                    outfile.WriteLine( "      return DecodeJson(jsonObject);");
                    outfile.WriteLine( "    }");
                    outfile.WriteLine($"    public static {protocolName} DecodeJson(JToken token)");
                    outfile.WriteLine( "    {");
                    outfile.WriteLine($"      var obj = new {protocolName}();");
                    foreach (var field in protocol.FieldMap)
                    {
                        var fieldName = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(field.Value.Name).Replace(".", "").Replace("_","").Replace("-", "");
                        var tsharkFieldName = field.Value.JsonName;
                        var fieldPbType = GetPbType(field.Value.Type);
                        var fieldNativeType = GetNativeType(fieldPbType);
                        var convert = GetConvertFunc(field.Value.Type, field.Value.Display, $"default({fieldNativeType.Name})");
                        outfile.WriteLine("      {");
                        outfile.WriteLine($"        var val = token[\"{tsharkFieldName}\"];");
                        outfile.WriteLine($"        if (val != null) obj.{fieldName} = {convert};");                        
                        outfile.WriteLine("      }");
                    }
                    outfile.WriteLine("      return obj;");
                    outfile.WriteLine("    }");

                    outfile.WriteLine(@"
                    public static Google.Protobuf.ByteString StringToBytes(string str)
                    {
                        var bstrArr = str.Split(':');
                        var byteArray = new byte[bstrArr.Length];
                        for (int i = 0; i < bstrArr.Length; i++)
                        {
                            byteArray[i] = Convert.ToByte(bstrArr[i], 16);
                        }
                        return Google.Protobuf.ByteString.CopyFrom( byteArray );
                    }
                    ");


                    outfile.WriteLine("  }");
                    outfile.WriteLine("}");
                }
            return fcount;
        }

        private int WriteMessage(IDictionary<string, ProtocolField> fieldMap, string protocolName, StreamWriter outfile, string indent)
        {
            var fcount = 1;
            outfile.WriteLine($"{indent}message {protocolName} {{");

            foreach (var field in fieldMap)
            {
                var fieldTypeString = GetPbType(field.Value.Type).ToString().ToLowerInvariant();
                string fieldName = GetFieldName(field.Value);
                if (field.Value.Type == FieldType.FtGroup)
                {
                    WriteMessage(field.Value.Fields, $"_{fieldName}", outfile, indent + "    ");
                    fieldTypeString = $"_{fieldName}";
                }
                var tsharkFieldName = field.Value.JsonName;
                var multStr = (field.Value.Mult == FieldMultiplicity.FmMany) ? "repeated" : "";
                outfile.WriteLine($"{indent}    // {field.Value.Info} ('{tsharkFieldName}')");
                outfile.WriteLine($"{indent}    {multStr} {fieldTypeString} {fieldName} = {fcount};");
                outfile.WriteLine();
                fcount++;
            }
            outfile.WriteLine($"{indent}}}");
            return fcount;
        }

        private static string GetFieldName(ProtocolField field)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(field.Name).Replace(".", "").Replace("_", "").Replace("-", "");
        }

        public static ulong _t(FieldType type, FieldDisplay display)
        {
            ulong hi = ((ulong)type << 32);
            ulong lo = (ulong)display;
            return hi | lo;
        }

        public static Dictionary<ulong, string> decoders =
            new Dictionary<ulong, string>()
            {                                                              
                { _t(FieldType.FtBoolean,FieldDisplay.BaseNone),   "Convert.ToInt32(val.Value<string>(), 10) != 0"},
                { _t(FieldType.FtChar,FieldDisplay.BaseDec),       "Convert.ToUInt32(val.Value<string>(), 10)"},
                { _t(FieldType.FtUint8,FieldDisplay.BaseDec),      "Convert.ToUInt32(val.Value<string>(), 10)"},
                { _t(FieldType.FtUint16,FieldDisplay.BaseDec),     "Convert.ToUInt32(val.Value<string>(), 10)"},
                { _t(FieldType.FtUint24,FieldDisplay.BaseDec),     "Convert.ToUInt32(val.Value<string>(), 10)"},
                { _t(FieldType.FtUint32,FieldDisplay.BaseDec),     "Convert.ToUInt32(val.Value<string>(), 10)"},
                { _t(FieldType.FtUint40,FieldDisplay.BaseDec),     "Convert.ToUInt64(val.Value<string>(), 10)"},
                { _t(FieldType.FtUint48,FieldDisplay.BaseDec),     "Convert.ToUInt64(val.Value<string>(), 10)"},
                { _t(FieldType.FtUint56,FieldDisplay.BaseDec),     "Convert.ToUInt64(val.Value<string>(), 10)"},
                { _t(FieldType.FtUint64,FieldDisplay.BaseDec),     "Convert.ToUInt64(val.Value<string>(), 10)"},
                { _t(FieldType.FtInt8,FieldDisplay.BaseDec),       "Convert.ToInt32(val.Value<string>(), 10)"},
                { _t(FieldType.FtInt16,FieldDisplay.BaseDec),      "Convert.ToInt32(val.Value<string>(), 10)"},
                { _t(FieldType.FtInt24,FieldDisplay.BaseDec),      "Convert.ToInt32(val.Value<string>(), 10)"},
                { _t(FieldType.FtInt32,FieldDisplay.BaseDec),      "Convert.ToInt32(val.Value<string>(), 10)"},
                { _t(FieldType.FtInt40,FieldDisplay.BaseDec),      "Convert.ToInt64(val.Value<string>(), 10)"},
                { _t(FieldType.FtInt48,FieldDisplay.BaseDec),      "Convert.ToInt64(val.Value<string>(), 10)"},
                { _t(FieldType.FtInt56,FieldDisplay.BaseDec),      "Convert.ToInt64(val.Value<string>(), 10)"},
                { _t(FieldType.FtInt64,FieldDisplay.BaseDec),      "Convert.ToInt64(val.Value<string>(), 10)"},
                { _t(FieldType.FtUint16,FieldDisplay.BasePtUdp),     "Convert.ToUInt32(val.Value<string>(), 10)"},
                { _t(FieldType.FtChar,FieldDisplay.BaseHex),       "Convert.ToUInt32(val.Value<string>(), 16)"},
                { _t(FieldType.FtUint8,FieldDisplay.BaseHex),      "Convert.ToUInt32(val.Value<string>(), 16)"},
                { _t(FieldType.FtUint16,FieldDisplay.BaseHex),     "Convert.ToUInt32(val.Value<string>(), 16)"},
                { _t(FieldType.FtUint24,FieldDisplay.BaseHex),     "Convert.ToUInt32(val.Value<string>(), 16)"},
                { _t(FieldType.FtUint32,FieldDisplay.BaseHex),     "Convert.ToUInt32(val.Value<string>(), 16)"},
                { _t(FieldType.FtUint40,FieldDisplay.BaseHex),     "Convert.ToUInt64(val.Value<string>(), 16)"},
                { _t(FieldType.FtUint48,FieldDisplay.BaseHex),     "Convert.ToUInt64(val.Value<string>(), 16)"},
                { _t(FieldType.FtUint56,FieldDisplay.BaseHex),     "Convert.ToUInt64(val.Value<string>(), 16)"},
                { _t(FieldType.FtUint64,FieldDisplay.BaseHex),     "Convert.ToUInt64(val.Value<string>(), 16)"},
                { _t(FieldType.FtInt8,FieldDisplay.BaseHex),       "Convert.ToInt32(val.Value<string>(), 16)"},
                { _t(FieldType.FtInt16,FieldDisplay.BaseHex),      "Convert.ToInt32(val.Value<string>(), 16)"},
                { _t(FieldType.FtInt24,FieldDisplay.BaseHex),      "Convert.ToInt32(val.Value<string>(), 16)"},
                { _t(FieldType.FtInt32,FieldDisplay.BaseHex),      "Convert.ToInt32(val.Value<string>(), 16)"},
                { _t(FieldType.FtInt40,FieldDisplay.BaseHex),      "Convert.ToInt64(val.Value<string>(), 16)"},
                { _t(FieldType.FtInt48,FieldDisplay.BaseHex),      "Convert.ToInt64(val.Value<string>(), 16)"},
                { _t(FieldType.FtInt56,FieldDisplay.BaseHex),      "Convert.ToInt64(val.Value<string>(), 16)"},
                { _t(FieldType.FtInt64,FieldDisplay.BaseHex),      "Convert.ToInt64(val.Value<string>(), 16)"},

                { _t(FieldType.FtChar,FieldDisplay.BaseOct),       "Convert.ToUInt32(val.Value<string>(), 8)"},
                { _t(FieldType.FtUint8,FieldDisplay.BaseOct),      "Convert.ToUInt32(val.Value<string>(), 8)"},
                { _t(FieldType.FtUint16,FieldDisplay.BaseOct),     "Convert.ToUInt32(val.Value<string>(), 8)"},
                { _t(FieldType.FtUint24,FieldDisplay.BaseOct),     "Convert.ToUInt32(val.Value<string>(), 8)"},
                { _t(FieldType.FtUint32,FieldDisplay.BaseOct),     "Convert.ToUInt32(val.Value<string>(), 8)"},
                { _t(FieldType.FtUint40,FieldDisplay.BaseOct),     "Convert.ToUInt64(val.Value<string>(), 8)"},
                { _t(FieldType.FtUint48,FieldDisplay.BaseOct),     "Convert.ToUInt64(val.Value<string>(), 8)"},
                { _t(FieldType.FtUint56,FieldDisplay.BaseOct),     "Convert.ToUInt64(val.Value<string>(), 8)"},
                { _t(FieldType.FtUint64,FieldDisplay.BaseOct),     "Convert.ToUInt64(val.Value<string>(), 8)"},
                { _t(FieldType.FtInt8,FieldDisplay.BaseOct),       "Convert.ToInt32(val.Value<string>(), 8)"},
                { _t(FieldType.FtInt16,FieldDisplay.BaseOct),      "Convert.ToInt32(val.Value<string>(), 8)"},
                { _t(FieldType.FtInt24,FieldDisplay.BaseOct),      "Convert.ToInt32(val.Value<string>(), 8)"},
                { _t(FieldType.FtInt32,FieldDisplay.BaseOct),      "Convert.ToInt32(val.Value<string>(), 8)"},
                { _t(FieldType.FtInt40,FieldDisplay.BaseOct),      "Convert.ToInt64(val.Value<string>(), 8)"},
                { _t(FieldType.FtInt48,FieldDisplay.BaseOct),      "Convert.ToInt64(val.Value<string>(), 8)"},
                { _t(FieldType.FtInt56,FieldDisplay.BaseOct),      "Convert.ToInt64(val.Value<string>(), 8)"},
                { _t(FieldType.FtInt64,FieldDisplay.BaseOct),      "Convert.ToInt64(val.Value<string>(), 8)"},

                { _t(FieldType.FtFloat,FieldDisplay.BaseNone),     "Convert.ToSingle(val.Value<string>())"},
                { _t(FieldType.FtDouble,FieldDisplay.BaseNone),    "Convert.ToDouble(val.Value<string>())"},

                { _t(FieldType.FtString,FieldDisplay.BaseNone),    "val.Value<string>()"},
                { _t(FieldType.FtBytes,FieldDisplay.BaseNone),     "StringToBytes(val.Value<string>())"},

                { _t(FieldType.FtEther,FieldDisplay.BaseNone),     "Google.Protobuf.ByteString.CopyFrom(System.Net.NetworkInformation.PhysicalAddress.Parse(val.Value<string>().ToUpperInvariant().Replace(':','-')).GetAddressBytes())"},
                { _t(FieldType.FtIpv4,FieldDisplay.BaseNone),      "Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes())"},
                { _t(FieldType.FtIpv6,FieldDisplay.BaseNone),      "Google.Protobuf.ByteString.CopyFrom(System.Net.IPAddress.Parse(val.Value<string>()).GetAddressBytes())"},
            };
        private string GetConvertFunc(FieldType type, FieldDisplay display, string defvalue)
        {
            if (decoders.TryGetValue(_t(type, display), out var value)) 
            {
                return value;
            }
            else if (decoders.TryGetValue(_t(type, FieldDisplay.BaseNone), out value) )
            {
                return value;
            }
            else return defvalue;
        }

        private Type GetNativeType(PbTypes pbtype)
        {
            switch(pbtype)
            {
                case PbTypes.Bool: return typeof(Boolean);
                case PbTypes.Bytes: return typeof(ByteString);
                case PbTypes.Double: return typeof(Double);
                case PbTypes.Float: return typeof(Single);
                case PbTypes.Int32: return typeof(Int32);
                case PbTypes.Int64: return typeof(Int64);
                case PbTypes.String: return typeof(String);
                case PbTypes.UInt32: return typeof(UInt32);
                case PbTypes.UInt64: return typeof(UInt64);
                default: return typeof(object);
            }
        }

        public enum PbTypes { Bool, Bytes, Int32, Int64, Float, Double, String, UInt32, UInt64  };

        private PbTypes GetPbType(FieldType type)
        {
            switch(type)
            {
                case FieldType.FtAbsoluteTime: return PbTypes.Int64;
                case FieldType.FtAx25: return PbTypes.Bytes;
                case FieldType.FtBoolean: return PbTypes.Bool;
                case FieldType.FtBytes: return PbTypes.Bytes;
                case FieldType.FtChar: return PbTypes.UInt32;
                case FieldType.FtDouble: return PbTypes.Double;
                case FieldType.FtEther: return PbTypes.Bytes;
                case FieldType.FtEui64: return PbTypes.Bytes;
                case FieldType.FtFcwwn: return PbTypes.Bytes;
                case FieldType.FtFloat: return PbTypes.Float;
                case FieldType.FtFramenum: return PbTypes.Int64;
                case FieldType.FtGuid: return PbTypes.Bytes;
                case FieldType.FtIeee11073Float: return PbTypes.Float;
                case FieldType.FtIeee11073Sfloat: return PbTypes.Float;
                case FieldType.FtInt8: 
                case FieldType.FtInt16: 
                case FieldType.FtInt24:
                case FieldType.FtInt32: return PbTypes.Int32;
                case FieldType.FtInt40:
                case FieldType.FtInt48:
                case FieldType.FtInt56:
                case FieldType.FtInt64: return PbTypes.Int64;
                case FieldType.FtIpv4: return PbTypes.Bytes;
                case FieldType.FtIpv6: return PbTypes.Bytes;
                case FieldType.FtIpxnet: return PbTypes.Bytes;
                case FieldType.FtNone: return PbTypes.Int32;
                case FieldType.FtNumTypes: return PbTypes.Int64;
                case FieldType.FtOid: return PbTypes.Bytes;
                case FieldType.FtPcre: return PbTypes.Bytes;
                case FieldType.FtProtocol: return PbTypes.String;
                case FieldType.FtRelativeTime: return PbTypes.Int64;
                case FieldType.FtRelOid: return PbTypes.Bytes;
                case FieldType.FtString: return PbTypes.String;
                case FieldType.FtStringz: return PbTypes.String;
                case FieldType.FtStringzpad: return PbTypes.String;
                case FieldType.FtSystemId: return PbTypes.Int64;
                case FieldType.FtUint8:
                case FieldType.FtUint16: 
                case FieldType.FtUint24:
                case FieldType.FtUint32: return PbTypes.UInt32;
                case FieldType.FtUint40:
                case FieldType.FtUint48:
                case FieldType.FtUint56:
                case FieldType.FtUint64: return PbTypes.UInt64;
                case FieldType.FtUintBytes: return PbTypes.Bytes;
                case FieldType.FtUintString: return PbTypes.String;
                case FieldType.FtVines: return PbTypes.Bytes;
                default:
                    return PbTypes.Bytes;
            }
        }
    }
}