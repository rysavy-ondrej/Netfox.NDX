using Ndx.Shell.Console;
using Ndx.Captures;
using System.Reactive.Linq;

// constants
var host_mac = "00:25:b3:bf:91:ee";
var host_ip = "172.16.0.8";
var ip_gw = "172.16.0.10";

// event source
var events = PcapFile.ReadJson(@"C:\Users\Ondrej\Documents\GitHub\Netfox.NDX\TestData\PPA\nowebaccess1.json").ToEnumerable();

// rule:
//      id: arp_gw_ok
//      description: Get success ARP requests reply pairs for given host and target IP.
// params:
//      - hostMac
//      - targetIp
// events:
//      e1: arp && eth.src == hostMac && arp.opcode == ARP_REQUEST && arp.dst.proto_ipv4 == gwIp
//      e2: arp && eth.dst == hostMac && arp.opcode == ARP_RESPONSE && arp.src_proto_ipv4 == gwIp
// assert:
//     - e1.arp.src.proto == arp.dst.proto
//     - e1 [5s]~> e2
// select:
//     req: e1
//     res: e2 
//     desc: $"ARP OK: Host {e1.arp.src_proto_ipv4"} asked: who has {e1.arp.dst_proto_ipv4}? Response was {e2.arp.src.hw_mac}."
// 
const string ARP_REQUEST = "1";
const string ARP_REPLY = "2";
var hostMac = "00:25:b3:bf:91:ee";
var targetIp = "172.16.0.10";

var res =
from e1 in events.Where(e => e.FrameProtocols.Contains("arp") && e["eth_eth_src"].Equals(host_mac) && e["arp_arp_opcode"].Equals(ARP_REQUEST))
join e2 in events.Where(e => e.FrameProtocols.Contains("arp") && e["eth_eth_dst"].Equals(host_mac) && e["arp_arp_src_proto_ipv4"].Equals(targetIp) && e["arp_arp_opcode"].Equals(ARP_REPLY))
on e1["arp_arp_src_proto_ipv4"] equals e2["arp_arp_dst_proto_ipv4"]
where e1.Timestamp <= e2.Timestamp && e2.Timestamp <= e1.Timestamp + 5000
select new { req = e1, res = e2, descr = $"ARP OK: Host {e1["arp_arp_src_proto_ipv4"]} asked: who has {e1["arp_arp_dst_proto_ipv4"]}? Response was {e2["arp_arp_src_hw_mac"]}" };
