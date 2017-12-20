using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Packets.IoT
{
    
    public partial class Coap
    {

        public bool IsRequest => (int)Code < 0x40 && Code != CoapCode.Empty;
        public string Info
        {
            get
            {
                var token = BitConverter.ToString(Token).Replace("-", "");
                var str = $"{this.Type.ToString().ToUpperInvariant()}, MID:{this.MessageId}, TOKEN: {token}, {this.Code.ToString().ToUpperInvariant()}";
                return str;
            }
        }

        /// <summary>
        /// Gets the URI from options. 
        /// </summary>
        /// <returns></returns>
        IEnumerable<byte[]> GetOptionValues(CoapOptions option)
        {
            if (Options == null) yield break;
            var currentOption = 0;
            foreach(var opt in Options)
            {
                if (opt.IsPayloadMarker) break;
                currentOption += opt.Delta;
                if (currentOption == (int)option)
                {
                    yield return opt.Value;
                }
            }        
        }
        static byte[] emptyByteArray = new byte[] { };
        /// <summary>
        /// Gets CoAP URI. This string has the following format: coap-URI = "coap:" "//" host [ ":" port ] path-abempty [ "?" query ]
        /// </summary>
        public Uri GetUri(string defaultHost, int defaultPort = 5683)
        {
            var host = GetOptionValues(CoapOptions.UriHost).FirstOrDefault();
            var port = GetOptionValues(CoapOptions.UriPort).FirstOrDefault();
            var path = GetOptionValues(CoapOptions.UriPath);
            var query = GetOptionValues(CoapOptions.UriQuery);

            var urb = new UriBuilder
            {
                Scheme = "coap",
                Host = host != null ? Encoding.ASCII.GetString(host) : defaultHost,
                Port = port != null ? BitConverter.ToUInt16(port, 0) : defaultPort,
                Path = String.Join("/", (path.ToArray().Select(Encoding.ASCII.GetString))),
                Query = String.Join("/", (query.ToArray().Select(Encoding.ASCII.GetString)))
            };
            return urb.Uri;
        }
    }
}
