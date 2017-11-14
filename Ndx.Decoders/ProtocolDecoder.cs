using Ndx.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace Ndx.Decoders
{
    /// <summary>
    /// Represents procedures to be applied for decoding field values of a single protocol.
    /// </summary>
    public class DecodingReceipt
    {


        /// <summary>
        /// Converts string value that represents a sequence of bytes to byte array.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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
        public static Dictionary<string, Func<string, Variant>> decoders =
            new Dictionary<string, Func<string, Variant>>()
            {    
                { "FT_BOOLEAN:8", (string x) => new Variant(Convert.ToByte(x) != 0) },
                { "FT_BOOLEAN:16", (string x) => new Variant(Convert.ToInt16(x,10) != 0) },
                { "FT_BOOLEAN:BASE_NONE", (string x) => new Variant(Convert.ToInt16(x,10) != 0) },
                { "FT_CHAR:BASE_DEC", (string x) => new Variant(Convert.ToUInt32(x,10)) },
                { "FT_UINT8:BASE_DEC", (string x) => new Variant(Convert.ToUInt32(x,10)) },
                { "FT_UINT16:BASE_DEC", (string x) => new Variant(Convert.ToUInt32(x,10)) },
                { "FT_UINT24:BASE_DEC", (string x) => new Variant(Convert.ToUInt32(x,10)) },
                { "FT_UINT32:BASE_DEC", (string x) => new Variant(Convert.ToUInt32(x,10)) },
                { "FT_UINT40:BASE_DEC", (string x) => new Variant(Convert.ToUInt64(x,10)) },
                { "FT_UINT48:BASE_DEC", (string x) => new Variant(Convert.ToUInt64(x,10)) },
                { "FT_UINT56:BASE_DEC", (string x) => new Variant(Convert.ToUInt64(x,10)) },
                { "FT_UINT64:BASE_DEC", (string x) => new Variant(Convert.ToUInt64(x,10)) },
                { "FT_INT8:BASE_DEC", (string x) => new Variant(Convert.ToInt32(x,10)) },
                { "FT_INT16:BASE_DEC", (string x) => new Variant(Convert.ToInt32(x,10)) },
                { "FT_INT24:BASE_DEC", (string x) => new Variant(Convert.ToInt32(x,10)) },
                { "FT_INT32:BASE_DEC", (string x) => new Variant(Convert.ToInt32(x,10)) },
                { "FT_INT40:BASE_DEC", (string x) => new Variant(Convert.ToInt64(x,10)) },
                { "FT_INT48:BASE_DEC", (string x) => new Variant(Convert.ToInt64(x,10)) },
                { "FT_INT56:BASE_DEC", (string x) => new Variant(Convert.ToInt64(x,10)) },
                { "FT_INT64:BASE_DEC", (string x) => new Variant(Convert.ToInt64(x,10)) },

                { "FT_CHAR:BASE_HEX", (string x) => new Variant(Convert.ToUInt32(x,16)) },
                { "FT_UINT8:BASE_HEX", (string x) => new Variant(Convert.ToUInt32(x,16)) },
                { "FT_UINT16:BASE_HEX", (string x) => new Variant(Convert.ToUInt32(x,16)) },
                { "FT_UINT24:BASE_HEX", (string x) => new Variant(Convert.ToUInt32(x,16)) },
                { "FT_UINT32:BASE_HEX", (string x) => new Variant(Convert.ToUInt32(x,16)) },
                { "FT_UINT40:BASE_HEX", (string x) => new Variant(Convert.ToUInt64(x,16)) },
                { "FT_UINT48:BASE_HEX", (string x) => new Variant(Convert.ToUInt64(x,16)) },
                { "FT_UINT56:BASE_HEX", (string x) => new Variant(Convert.ToUInt64(x,16)) },
                { "FT_UINT64:BASE_HEX", (string x) => new Variant(Convert.ToUInt64(x,16)) },
                { "FT_INT8:BASE_HEX", (string x) => new Variant(Convert.ToInt32(x,16)) },
                { "FT_INT16:BASE_HEX", (string x) => new Variant(Convert.ToInt32(x,16)) },
                { "FT_INT24:BASE_HEX", (string x) => new Variant(Convert.ToInt32(x,16)) },
                { "FT_INT32:BASE_HEX", (string x) => new Variant(Convert.ToInt32(x,16)) },
                { "FT_INT40:BASE_HEX", (string x) => new Variant(Convert.ToInt64(x,16)) },
                { "FT_INT48:BASE_HEX", (string x) => new Variant(Convert.ToInt64(x,16)) },
                { "FT_INT56:BASE_HEX", (string x) => new Variant(Convert.ToInt64(x,16)) },
                { "FT_INT64:BASE_HEX", (string x) => new Variant(Convert.ToInt64(x,16)) },

                { "FT_CHAR:BASE_OCT", (string x) => new Variant(Convert.ToUInt32(x,8)) },
                { "FT_UINT8:BASE_OCT", (string x) => new Variant(Convert.ToUInt32(x,8)) },
                { "FT_UINT16:BASE_OCT", (string x) => new Variant(Convert.ToUInt32(x,8)) },
                { "FT_UINT24:BASE_OCT", (string x) => new Variant(Convert.ToUInt32(x,8)) },
                { "FT_UINT32:BASE_OCT", (string x) => new Variant(Convert.ToUInt32(x,8)) },
                { "FT_UINT40:BASE_OCT", (string x) => new Variant(Convert.ToUInt64(x,8)) },
                { "FT_UINT48:BASE_OCT", (string x) => new Variant(Convert.ToUInt64(x,8)) },
                { "FT_UINT56:BASE_OCT", (string x) => new Variant(Convert.ToUInt64(x,8)) },
                { "FT_UINT64:BASE_OCT", (string x) => new Variant(Convert.ToUInt64(x,8)) },
                { "FT_INT8:BASE_OCT", (string x) => new Variant(Convert.ToInt32(x,8)) },
                { "FT_INT16:BASE_OCT", (string x) => new Variant(Convert.ToInt32(x,8)) },
                { "FT_INT24:BASE_OCT", (string x) => new Variant(Convert.ToInt32(x,8)) },
                { "FT_INT32:BASE_OCT", (string x) => new Variant(Convert.ToInt32(x,8)) },
                { "FT_INT40:BASE_OCT", (string x) => new Variant(Convert.ToInt64(x,8)) },
                { "FT_INT48:BASE_OCT", (string x) => new Variant(Convert.ToInt64(x,8)) },
                { "FT_INT56:BASE_OCT", (string x) => new Variant(Convert.ToInt64(x,8)) },
                { "FT_INT64:BASE_OCT", (string x) => new Variant(Convert.ToInt64(x,8)) },

                { "FT_FLOAT:BASE_NONE", (string x) => new Variant(Convert.ToSingle(x)) },
                { "FT_DOUBLE:BASE_NONE", (string x) => new Variant(Convert.ToDouble(x)) },

                { "FT_STRING:BASE_NONE", (string x) => new Variant(x) },
                { "FT_BYTES:BASE_NONE", (string x) => new Variant(StringToBytes(x)) },

                { "FT_ETHER:BASE_NONE", (string x) => new Variant(x) },     
                { "FT_IPv4:BASE_NONE", (string x) => new Variant(IPAddress.Parse(x)) },
                { "FT_IPv6:BASE_NONE", (string x) => new Variant(IPAddress.Parse(x)) },
            };


        string m_protocol;
        Dictionary<string, Func<string, Variant>> m_decodingFunctions;
        /// <summary>
        /// Gets the protocol name of this receipt.
        /// </summary>
        public string Protocol { get => m_protocol; set => m_protocol = value; }


        /// <summary>
        /// Decodes the field into the typed value.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns>Variant representing the decoded value.</returns>
        public Variant DecodeField(string fieldName, string fieldValue)
        {
            var key = fieldName;
            if (m_decodingFunctions.TryGetValue(key, out var func))
            {

                return func(fieldValue);
            }
            else
                return null;

        }


        /// <summary>
        /// Loads the decoding receipt from yaml field specifications.
        /// </summary>
        /// <param name="path">Path to file with field specifications.</param>
        /// <returns>New <see cref="DecodingReceipt"/> object built from the specified file.</returns>
        public static DecodingReceipt LoadFrom(string protocol, string path)
        {
            return LoadFrom(protocol, File.OpenRead(path));
        }

        /// <summary>
        /// Loads the decoding receipt from yaml field specifications.
        /// </summary>
        /// <param name="path">Path to file with field specifications.</param>
        /// <returns>New <see cref="DecodingReceipt"/> object built from the specified file.</returns>
        public static DecodingReceipt LoadFrom(string protocol, Stream content)
        {
            var decodingFunctions = new Dictionary<string, Func<string, Variant>>();
            using (var reader = new StreamReader(content))
            {
                var yaml = new YamlStream();
                yaml.Load(reader);
                var fields = (YamlMappingNode)yaml.Documents[0].RootNode;
                foreach(var field in fields)
                {
                    var fieldName = ((YamlScalarNode)field.Key).Value;
                    var fieldType = ((YamlScalarNode)field.Value[new YamlScalarNode("type")])?.Value ?? String.Empty;
                    var fieldBase = ((YamlScalarNode)field.Value[new YamlScalarNode("base")])?.Value ?? String.Empty;

                    if (decoders.TryGetValue($"{fieldType}:{fieldBase}", out var func))
                    {
                        decodingFunctions.Add(fieldName.Replace('.','_'), func);
                    }
                }
            }
            var dr = new DecodingReceipt() { m_protocol = protocol, m_decodingFunctions = decodingFunctions };
            return dr;
        }

        public static DecodingReceipt Tcp => LoadFrom("tcp", new MemoryStream(Resources.packet_tcp));

        public IDictionary<string, Func<string, Variant>> DecodingFunctions => m_decodingFunctions; 
    }


    /// <summary>
    /// This class implements configurable protocol decoder.
    /// </summary>
    public class ProtocolDecoder
    {

    }
}
