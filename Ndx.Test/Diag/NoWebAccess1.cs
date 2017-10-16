using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Ndx.Captures;
using NUnit.Framework;
using Ndx.Ingest;
using Ndx.Diagnostics;
using System.Dynamic;
using System.Collections.Generic;
using Ndx.Model;

namespace Ndx.Test.Diag
{
    [TestFixture]
    class NoWebAccess1
    {
        static TestContext m_testContext = TestContext.CurrentContext;
        string m_source = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\PPA\nowebaccess1.json");
        [Test]
        public void ArpGatewayOk()
        {
            // event source
            var events = PcapFile.ReadJson(m_source).ToEnumerable();

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
        public void DnsOk()
        {
            var hostIp = "172.16.0.8";
            var events = PcapFile.ReadJson(m_source).ToEnumerable();
            // rule:
            //      id: dns_test_ok
            //      description: Rule that select sucessful DNS communication of the specified host.
            // params:
            //      - hostIp
            // events:
            //      e1: dns.flags.response == 0 && ip.src == hostIp
            //      e2: dns.flags.response == 1 && dns.flags.rcode == 0
            // assert:
            //      - e1.dns.id == e2.dns.id
            //      - e1[0 - 5s]~> e2
            // select:
            //      query: e1
            //      answer: e2
            //      description: "DNS OK."
            var resultSet =
            from e1 in events.Where(e => e.FrameProtocols.Contains("dns") && e["dns_flags_dns_flags_response"].Equals("0") && e["ip_ip_src"].Equals(hostIp))
            join e2 in events.Where(e => e.FrameProtocols.Contains("dns") && e["dns_flags_dns_flags_response"].Equals("1") && e["dns_flags_dns_flags_rcode"].Equals("0"))
            on e1["dns.id"] equals e2["dns.id"]
            where e1.Timestamp <= e2.Timestamp && e2.Timestamp <= e1.Timestamp + 5000
            select new { query = e1, answer = e2, description = $"DNS OK: Query '', reply received within {DateTime.FromBinary(e2.Timestamp - e1.Timestamp)}s." };

            Console.WriteLine("Diagnostic Trace:");
            foreach (var result in resultSet)
            {
                Console.WriteLine(result.description);
            }
        }
        [Test]
        public void DnsError()
        {
            var hostIp = "172.16.0.8";
            var events = PcapFile.ReadJson(m_source).ToEnumerable();
            // rule:
            //      id: dns_test_ok
            //      description: Rule that select sucessful DNS communication of the specified host.
            // params:
            //      - host
            // events:
            //      e1: dns.flags.response == 0
            //      e2: dns.flags.response == 1 && dns.flags.rcode == 1
            // assert:
            //      - e1.ip.src == host.ip.src
            //      - e1.dns.id == e2.dns.id
            //      - e1 [0 - 5s]~> e2
            // select:
            //      query: e1
            //      answer: e2
            //      description: "DNS Error."
            var resultSet =
            from e1 in events.Where(e => e.FrameProtocols.Contains("dns") && e["dns_flags_dns_flags_response"].Equals("0") && e["ip_ip_src"].Equals(hostIp))
            from e2 in events.Where(e => e.FrameProtocols.Contains("dns") && e["dns_flags_dns_flags_response"].Equals("1") && e["dns_flags_dns_flags_rcode"].Equals("1"))
            where  
                   e1["dns.id"].Equals(e2["dns.id"]) 
                && e1.Timestamp <= e2.Timestamp && e2.Timestamp <= e1.Timestamp + 5000
            select new { query = e1, answer = e2, description = $"DNS Error: Error reply received within {DateTime.FromBinary(e2.Timestamp - e1.Timestamp)}s." };

            Console.WriteLine("Diagnostic Trace:");
            foreach (var result in resultSet)
            {
                Console.WriteLine(result.description);
            }
        }

        [Test]
        public void DnsNoResponseRule()
        {
            var rule = new Rule()
            {
                Id = "Dns.NoResponse",
                Description = "DNS server not responding error."
            };
            rule.Events.Add("e1", ctx => ctx.Input.Where(e => e.FrameProtocols.Contains("dns") && e["dns_flags_dns_flags_response"].Equals("0") && e["ip_ip_src"].Equals(ctx["host"]["ip_ip_src"])));
            rule.Events.Add("e2", ctx => ctx.Input.Where(e => e.FrameProtocols.Contains("dns") && e["dns_flags_dns_flags_response"].Equals("1")));
            rule.Assert.Add(ctx => ctx["e1"]["dns.id"].Equals(ctx["e2"]["dns.id"]));


            rule.Assert.Add(ctx => ctx.NotLeadsTo(TimeSpan.Zero, TimeSpan.FromSeconds(5), ctx["e1"], ctx["e2"]));


            var events = PcapFile.ReadJson(m_source).ToEnumerable().ToList();
            var args = new Dictionary<string, PacketFields>
            {
                {"host", PacketFields.FromFields( new Dictionary<string,string>() { { "ip_ip_src" , "172.16.0.8" } } ) }
            };
            
            var resultSet = rule.Evaluate(events, args, ctx => new { query = ctx["e1"],
                description = $"{ctx["e1"].DateTime}: DNS Response Lost: {ctx["e1"].GetFlowKey().IpFlowKeyString}: {ctx["e1"]["dns_text"]} : {ctx["e1"]["text_text"]}"  });
            Console.WriteLine("Diagnostic Trace:");
            foreach (var result in resultSet)
            {
                Console.WriteLine(result.description);
            }
        }

        [Test]
        public void DnsNoResponse()
        {
            var hostIp = "172.16.0.8";
            var events = PcapFile.ReadJson(m_source).ToEnumerable().ToList();
            // rule:
            //      id: dns_no_response
            //      description: DNS server not responding error. 
            // params:
            //      - host
            // events:
            //      e1: dns.flags.response == 0 && ip.src == host
            //      e2: dns.flags.response == 1 
            // assert:
            //      - e1.dns.id == e2.dns.id
            //      - e1 [0 - 5s]~> !e2
            // select:
            //      query: e1
            //      description: "{e1.timestamp}: DNS Response Lost: {e1.flow}: {e1.dns.text}: {e1.text.text}"
            var resultSet =
                from e1 in events.Where(e => e.FrameProtocols.Contains("dns") && e["dns_flags_dns_flags_response"].Equals("0") && e["ip_ip_src"].Equals(hostIp))
                let _x0001 =
                    from _e1 in events.Where(e => e.FrameProtocols.Contains("dns") && e["dns_flags_dns_flags_response"].Equals("0") && e["ip_ip_src"].Equals(hostIp))
                    from _e2 in events.Where(e => e.FrameProtocols.Contains("dns") && e["dns_flags_dns_flags_response"].Equals("1"))
                    where
                           _e1["dns.id"].Equals(_e2["dns.id"])
                        && _e1.Timestamp <= _e2.Timestamp && _e2.Timestamp <= _e1.Timestamp + 5000
                    select _e1
                where !_x0001.Contains(e1)
                select new { query = e1,
                             description = $"{e1.DateTime}: DNS Response Lost: {e1.GetFlowKey().IpFlowKeyString}: {e1["dns_text"]} : {e1["text_text"]}" };
            Console.WriteLine("Diagnostic Trace:");
            foreach (var result in resultSet)
            {
                Console.WriteLine(result.description);
            }
        }
        // Perform custom join operations:
        // https://docs.microsoft.com/en-us/dotnet/csharp/linq/perform-custom-join-operations


        public void LinqTest001()
        {
            var xs = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var ys = new int[] { 2, 4, 6, 8, 10 };
            var zs = new int[] { 1, 3, 5, 7, 9 };

            // var rs =
            //    from x in xs
            //    from y in ys
            //    from z in zs
            //    where x == y && y == z
            //    select new { left = x, middle = y, right = z };
            var rs = xs.SelectMany(x => ys, (x, y) => new {x, y})
                .SelectMany(@t => zs, (@t, z) => new {@t, z})
                .Where(@t => @t.@t.x == @t.@t.y && @t.@t.y == @t.z)
                .Select(@t => new {left = @t.@t.x, middle = @t.@t.y, right = @t.z});
        }
    }
}
