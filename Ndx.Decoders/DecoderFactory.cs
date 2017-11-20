using Ndx.Decoders.Basic;
using Ndx.Decoders.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Decoders
{
    public class DecoderFactory
    {
        // TODO: replace this hardwired factory method with reflection...

        //use getShape method to get object of type shape 
        public object DecodeProtocol(String protocol, JToken token)
        {
            if (String.IsNullOrEmpty(protocol)) return null;
            switch(protocol.ToLowerInvariant())
            {
                case "arp": return Arp.DecodeJson(token);
                case "dns": return Dns.DecodeJson(token);
                case "eth": return Eth.DecodeJson(token);
                case "http": return Http.DecodeJson(token);
                case "icmp": return Icmp.DecodeJson(token);
               // case "imap": return Imap.DecodeJson(token);
                case "ip": return Ip.DecodeJson(token);
                case "ipsec": return Ipsec.DecodeJson(token);
               // case "pop": return Pop.DecodeJson(token);
              //  case "sip": return Sip.DecodeJson(token);
               // case "smtp": return Smtp.DecodeJson(token);
               // case "ssh": return Ssh.DecodeJson(token);
               // case "ssl": return Ssl.DecodeJson(token);
                case "tcp": return Tcp.DecodeJson(token);
               // case "telnet": return Telnet.DecodeJson(token);
                case "udp": return Udp.DecodeJson(token);
            }
            return null;
        }
    }
}
