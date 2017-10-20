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

            var dginfo = rule.Evaluate(events, new Dictionary<string, PacketFields>() { { "dnsClient", host } }, x => x).ToArray();
            Console.WriteLine($"Matching DNS messages count={dginfo.Count()}, computed in {sw.ElapsedMilliseconds} ms.");
            foreach (var item in dginfo)
            {
                Console.WriteLine($"{item[1].FrameNumber} <- {item[1]["dns_id"]}  -> {item[2].FrameNumber}, RTT = {(Convert.ToInt64(item[2]["timestamp"]) - Convert.ToInt64(item[1]["timestamp"]))} ms");
            }
        }


        private const string m_theRule = @"---
rule:
    id: dns_test_ok
    description: DNS success queries. 
params:
    - dnsClient
events:
    e1: dns.flags.response == 0
    e2: dns.flags.response == 1 && dns.flags.rcode == 0
assert:    
    - dnsClient.ip.src == e1.ip.src
    - e1.ip.src eq '192.168.111.100'
    - e1.dns.id == e2.dns.id
    - e1.timestamp < e2.timestamp && e2.timestamp <= e1.timestamp + 2000 
select:
    host: dnsClient
    query: e1
    answer: e2
    info: '{e1.timestamp}: {e1.ip.src} -> {e1.ip.dst}: {e1.frame.number} <- {e1.dns.id} -> {e2.frame.number}, RTT = {e2.timestamp - e1.timestamp} ms'
";
    }
}


