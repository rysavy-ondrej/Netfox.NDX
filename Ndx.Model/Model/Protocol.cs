using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;



namespace Ndx.Model
{
    public sealed partial class Protocol
    {
        /// <summary>
        /// This class is a serialization hepler, because it is not possible to 
        /// deserializes YAML directly to <see cref="Protocol"/> object.
        /// </summary>
        class _Protocol
        {
            public string Name { get; set; }
            public Dictionary<string, ProtocolField> Fields { get; set; }
        }

        public static Protocol DeserializeFromYaml(Stream stream)
        {
            using (var input = new StreamReader(stream))
            { 
                var deserializer = new Deserializer();
                var doc = deserializer.Deserialize<_Protocol>(input);
                var proto = new Protocol()
                {
                    Name = doc.Name
                };
                proto.Fields.Add(doc.Fields);
                return proto;
            }            
        }
    }
}
