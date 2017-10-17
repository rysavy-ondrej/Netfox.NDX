using NUnit.Framework;
using PacketDotNet;
using System.Net;
using System.Net.Sockets;
using Ndx.Model;
using Ndx.Diagnostics;
using System.IO;
using Ndx.Captures;
using System.Reactive.Linq;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace Ndx.Test.Filters
{
    [TestFixture]
    public class RuleTest
    {
        static TestContext m_testContext = TestContext.CurrentContext;
        string m_source = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\dns.json");

        [Test]
        public void Rule_LoadFromYaml()
        {
            var rule = Rule.Load(m_theRule);
        }


        [Test]
        public void Rule_LoadAndEvaluate()
        {
            var sw = new Stopwatch();
            sw.Start();
            var rule = Rule.Load(m_theRule);
            var events = PcapFile.ReadJson(m_source).ToEnumerable().ToList();
            Console.WriteLine($"{events.Count()} events readed in {sw.ElapsedMilliseconds} ms.");
            var host = new PacketFields();
            sw.Restart();
            host.Fields["ip_src"] = "192.168.111.100";

            var dginfo = rule.Evaluate(events, new Dictionary<string, PacketFields>() { { "dnsClient", host } }, x => x ).ToArray();
            Console.WriteLine($"Matching DNS messages count={dginfo.Count()}, computed in {sw.ElapsedMilliseconds} ms.");
            foreach (var item in dginfo)
            {
                Console.WriteLine($"{item[1].FrameNumber} <- {item[1]["dns_id"]}  -> {item[2].FrameNumber}, RTT = {(item[2].DateTime - item[1].DateTime).TotalMilliseconds} ms");
            }
        }


        private const string m_theRule = @"---
rule:
    id: dns_test_ok
    description: Rule that select successful DNS communication of the specified host.
params:
    - dnsClient
events:
    e1: dns.flags.response == 0
    e2: dns.flags.response == 1 && dns.flags.rcode == 0
assert:    
    - dnsClient.ip.src == e1.ip.src
    - e1.dns.id == e2.dns.id
    - e1.ts < e2.ts && e2.ts <= e1.ts + 10 
select:
    host: dnsClient
    query: e1
    answer: e2
    description: DNS Ok.
";
    }
}


