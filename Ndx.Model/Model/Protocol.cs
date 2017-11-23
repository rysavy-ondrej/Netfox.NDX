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
    public sealed partial class ProtocolField
    {
        public IDictionary<string, ProtocolField> Fields
        {
            set
            {
                this.FieldMap.Add(value);
            }
            get => FieldMap;
        }
    }

    public sealed partial class Protocol
    {
        public IDictionary<string, ProtocolField> Fields
        {
            set
            {
                this.FieldMap.Add(value);
            }
            get => FieldMap;
        }

        public static Protocol DeserializeFromYaml(Stream stream)
        {
            using (var input = new StreamReader(stream))
            { 
                var deserializer = new Deserializer();
                var proto = deserializer.Deserialize<Protocol>(input);
                return proto;
            }            
        }
    }
}
