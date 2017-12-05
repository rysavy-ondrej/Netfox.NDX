using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Decoders.Core
{
    public enum ArpOpcode : uint { ARP_REQUEST = 1, ARP_REPLY = 2 }

    public partial class Arp
    {
        public IPAddress ArpSrcProtoIpv4Address => new IPAddress(this.ArpSrcProtoIpv4.ToByteArray());
        public IPAddress ArpDstProtoIpv4Address => new IPAddress(this.ArpDstProtoIpv4.ToByteArray());
        public PhysicalAddress ArpSrcHwMacAddress => new PhysicalAddress(this.ArpSrcHwMac.ToByteArray());
        public PhysicalAddress ArpDstHwMacAddress => new PhysicalAddress(this.ArpDstHwMac.ToByteArray());
    }
}
