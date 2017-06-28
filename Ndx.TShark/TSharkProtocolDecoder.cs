using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using Newtonsoft.Json.Linq;

namespace Ndx.TShark
{
    public class TSharkProtocolDecoder : TSharkProcess
    {
        private List<string> m_protocols;
        /// <summary>
        /// Gets a collection of fields that is to be exported by TSHARK.
        /// </summary>
        public IList<string> Protocols { get => m_protocols; }

        public TSharkProtocolDecoder() : base()
        {
            m_protocols = new List<string>();
        }

        public TSharkProtocolDecoder(string pipeName) : base(pipeName)
        {
            m_protocols = new List<string>();
        }

        public TSharkProtocolDecoder(IEnumerable<string> protocols) : base()
        {
            m_protocols = new List<string>(protocols);
        }

        public TSharkProtocolDecoder(string pipeName, IEnumerable<string> protocols) : base(pipeName)
        {
            m_protocols = new List<string>(protocols);
        }

        protected override string GetOutputFilter()
        {
            var protocols = String.Join(" ", m_protocols);
            return $"-J \"frame {protocols}\"";
        }

        protected override PacketFields GetResult(string line)
        {
            var jsonObject = JToken.Parse(line);
            var layers = jsonObject["layers"]; 
            var frame = layers["frame"];
            var result = new PacketFields()
            {
                Timestamp = (long)jsonObject["timestamp"],
                FrameNumber = (int)frame["frame_frame_number"],
                FrameProtocols = (string)frame["frame_frame_protocols"]
            };

            foreach (var proto in m_protocols)
            {
                var fields = layers[proto];
                if (fields!=null)
                {
                    foreach (var _field in fields)
                    {
                        var field = (JProperty)_field;
                        if (field?.Value.Type == JTokenType.String)
                        {
                            result.Fields.Add(field.Name, (string)field.Value);
                        }
                    }
                }                       
            }
            return result;
        }
    }
}
