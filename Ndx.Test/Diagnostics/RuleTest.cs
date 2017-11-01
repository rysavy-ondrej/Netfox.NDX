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
            var events = PcapFile.ReadJson(m_source, decoder).ToEnumerable().ToList();
            Console.WriteLine($"{events.Count()} events readed in {sw.ElapsedMilliseconds} ms.");
            var host = new DecodedFrame();
            sw.Restart();
            host.Fields["ip_src"] = new Variant("192.168.111.100");

            var dginfo = rule.Evaluate(events, new Dictionary<string, DecodedFrame>() { { "dnsClient", host } }, x => x).ToList();
            Console.WriteLine($"Matching DNS messages count={dginfo.Count()}, computed in {sw.ElapsedMilliseconds} ms.");
            foreach (var item in dginfo)
            {
                Console.WriteLine($"{item[1].FrameNumber} <- {item[1]["dns_id"]}  -> {item[2].FrameNumber}, RTT = {(Convert.ToInt64(item[2]["timestamp"]) - Convert.ToInt64(item[1]["timestamp"]))} ms");
            }
        }


        private Tuple<string,Variant> decoder(string protocol, string field, string value)
        {
            switch (field)
            {
                case "dns_flags_dns_flags_response" :   return new Tuple<string, Variant>("dns.flags.response", new Variant(value).AsInt32());
                case "dns_flags_dns_flags_rcode"    :   return new Tuple<string, Variant>("dns.flags.rcode", new Variant(value).AsInt32());
                case "dns_dns_id":                      return new Tuple<string, Variant>("dns.id", new Variant(value).AsInt32());
                case "ip_ip_src":                       return new Tuple<string, Variant>("ip.src", new Variant(value));
                default: return null;
            }
        }


        private const string m_theRule = @"---
rule:
    id: dns_query_response_ok
    description: Correlates sucessful DNS query and response into a single event. 
    result: ( query:e1, reply:e2 )
params:
    - dnsClient
events:
    e1: dns.flags.response == 0
    e2: dns.flags.response == 1 && dns.flags.rcode == 0
assert:    
    - dnsClient.ip.src == e1.ip.src
    - e1.ip.src == '192.168.111.100'
    - e1.dns.id == e2.dns.id
    - e1.timestamp < e2.timestamp && e2.timestamp <= e1.timestamp + 2000 ";
    }
}


