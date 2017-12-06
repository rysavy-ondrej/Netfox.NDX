    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using Newtonsoft.Json.Linq;

namespace Ndx.TShark
{
    public class TSharkProtocolDecoderProcess : TSharkProcess<DecodedFrame>
    {
        private List<string> m_protocols;
        /// <summary>
        /// Gets a collection of fields that is to be exported by TSHARK.
        /// </summary>
        public IList<string> Protocols { get => m_protocols; }

        public TSharkProtocolDecoderProcess() : base()
        {
            m_protocols = new List<string>();
        }

        public TSharkProtocolDecoderProcess(string pipeName) : base(pipeName)
        {
            m_protocols = new List<string>();
        }

        public TSharkProtocolDecoderProcess(IEnumerable<string> protocols) : base()
        {
            m_protocols = new List<string>(protocols);
        }

        public TSharkProtocolDecoderProcess(string pipeName, IEnumerable<string> protocols) : base(pipeName)
        {
            m_protocols = new List<string>(protocols);
        }

        protected override string GetOutputFilter()
        {
            var protocols = String.Join(" ", m_protocols);
            return $"-J \"frame {protocols}\"";
        }

        protected override DecodedFrame GetResult(string line)
        {
            return DecodeJsonLine(m_protocols, line);
        }
        public static DecodedFrame DecodeJsonLine(IEnumerable<string> protocols, string line)
        { 
            var jsonObject = JToken.Parse(line);
            var layers = jsonObject["layers"]; 
            var frame = layers["frame"];
            var result = new DecodedFrame()
            {
                Timestamp = (long)jsonObject["timestamp"],
                FrameNumber = (int)frame["frame_frame_number"],
                FrameProtocols = (string)frame["frame_frame_protocols"]
            };

            foreach (var proto in protocols)
            {
                var fields = layers[proto];
                if (fields!=null)
                {
                    foreach (var _field in fields)
                    {
                        var field = (JProperty)_field;
                        if (field?.Value.Type == JTokenType.String)
                        {
                            result.Fields.Add(field.Name, new Variant((string)field.Value));
                        }
                    }
                }                       
            }
            return result;
        }
    }
}
