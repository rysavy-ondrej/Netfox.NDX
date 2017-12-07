using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Decoders.Base
{
    public sealed partial class Ip
    {
        public IPAddress IpSrcAddress => new IPAddress(IpSrc.ToByteArray());
        public IPAddress IpDstAddress => new IPAddress(IpDst.ToByteArray());
    }
}
