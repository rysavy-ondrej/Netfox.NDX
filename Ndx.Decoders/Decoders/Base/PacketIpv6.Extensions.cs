using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Decoders.Base
{
    public sealed partial class Ipv6
    {
        public IPAddress IpSrcAddress => new IPAddress(Ipv6Src.ToByteArray());
        public IPAddress IpDstAddress => new IPAddress(Ipv6Dst.ToByteArray());
    }
}
