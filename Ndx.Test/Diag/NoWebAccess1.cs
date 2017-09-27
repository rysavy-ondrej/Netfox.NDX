using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Ndx.Captures;
using NUnit.Framework;

namespace Ndx.Test.Diag
{
    [TestFixture]
    class NoWebAccess1
    {
        static TestContext m_testContext = TestContext.CurrentContext;
        string source = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\PPA\nowebaccess1.json");
        [Test]
        public void ArpGatewayOk()
        {
            // event source
            var events = PcapFile.ReadJson(source).ToEnumerable();

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

            var resultSet =
            from e1 in events.Where(e => e.FrameProtocols.Contains("arp") && e["eth_eth_src"].Equals(hostMac) && e["arp_arp_opcode"].Equals(ARP_REQUEST))
            join e2 in events.Where(e => e.FrameProtocols.Contains("arp") && e["eth_eth_dst"].Equals(hostMac) && e["arp_arp_src_proto_ipv4"].Equals(targetIp) && e["arp_arp_opcode"].Equals(ARP_REPLY))
            on e1["arp_arp_src_proto_ipv4"] equals e2["arp_arp_dst_proto_ipv4"]
            where e1.Timestamp <= e2.Timestamp && e2.Timestamp <= e1.Timestamp + 5000
            select new { req = e1, res = e2, descr = $"ARP OK: Host {e1["arp_arp_src_proto_ipv4"]} asked: who has {e1["arp_arp_dst_proto_ipv4"]}? Response was {e2["arp_arp_src_hw_mac"]}" };

            foreach (var result in resultSet)
            {
                Console.WriteLine(result.descr);
            }
        }
        [Test]
        public void DnsTest()
        {
            var events = PcapFile.ReadJson(source).ToEnumerable();
            // rule:
            //      id: dns_test_ok
            //      description: Rule that select sucessful DNS communication of the specified host.
            // params:
            //      - hostMac
            // events:
            //      e1: dns.flags.response == 0
            //      e2: dns.flags.response == 1 && dns.flags.rcode == 0
            // assert:
            //      - e1.dns.id == e2.dns.id
            //      - e1[0 - 5s]~> e2
            // select:
            //      query: e1
            //      answer: e2
            //      desc: "DNS error replied."
            var results =
            from e1 in events.Where(e => e.FrameProtocols.Contains("dns") && e["dns_dns_flags_response"].Equals("0"))
            join e2 in events.Where(e => e.FrameProtocols.Contains("dns") && e["dns_dns_flags_response"].Equals("1") && e["dns_dns_flags_rcode"].Equals("0"))
            on e1["dns.id"] equals e2["dns.id"]
            where e1.Timestamp <= e2.Timestamp && e2.Timestamp <= e1.Timestamp + 5000
            select new { query = e1, answer = e2, desc = $"DNS OK: Query '', reply received within {DateTime.FromBinary(e2.Timestamp - e1.Timestamp)}s." };
        }
    }
}
