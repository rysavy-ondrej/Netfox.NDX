using Ndx.Decoders;
using Ndx.Decoders.Basic;
using Ndx.Decoders.Core;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Test.Decoders
{
    [TestFixture()]
    class DnsDecoderTest
    {
        static string dnsStr = "{ 'dns_dns_id': '0x00000b89','dns_dns_flags': '0x00000100','dns_flags_dns_flags_response': '0','dns_flags_dns_flags_opcode': '0','dns_flags_dns_flags_truncated': '0','dns_flags_dns_flags_recdesired': '1','dns_flags_dns_flags_z': '0','dns_flags_dns_flags_checkdisable': '0','dns_dns_count_queries': '1','dns_dns_count_answers': '0','dns_dns_count_auth_rr': '0','dns_dns_count_add_rr': '0','dns_text': 'Queries','text_text': 'web.vortex.data.microsoft.com: type A, class IN','text_dns_qry_name': 'web.vortex.data.microsoft.com','text_dns_qry_name_len': '29','text_dns_count_labels': '5','text_dns_qry_type': '1','text_dns_qry_class': '0x00000001'}";

        [Test()]                    
        public void DnsDecoder_Test()
        {
            var dns = Dns.DecodeJson(dnsStr);
            Assert.AreEqual(dns.DnsId, 0x00000b89);
            Assert.AreEqual(dns.DnsCountQueries, 1);
            Assert.AreEqual(dns.DnsQry[0].DnsQryName, "web.vortex.data.microsoft.com");
            Assert.AreEqual(dns.DnsQry[0].DnsQryType,  1);     // A
            Assert.AreEqual(dns.DnsQry[0].DnsQryClass, 1);     // Internet (IN) [RFC1035]
        }

        [Test()]
        public void DnsFactoryDecoderTest()
        {            
            var factory = new DecoderFactory();
            var dns = factory.DecodeProtocol("dns", JToken.Parse(dnsStr)) as Dns;
            Assert.AreEqual(dns.DnsCountQueries, 1);
            Assert.AreEqual(dns.DnsQry[0].DnsQryName, "web.vortex.data.microsoft.com");
            Assert.AreEqual(dns.DnsQry[0].DnsQryType, 1);     // A
            Assert.AreEqual(dns.DnsQry[0].DnsQryClass, 1);     // Internet (IN) [RFC1035]
        }

        string dnsStr2 = @"{'dns_dns_response_to': '1','dns_dns_time': '0.000671000','dns_dns_id': '0x00000b89','dns_dns_flags': '0x00008180','dns_flags_dns_flags_response': '1','dns_flags_dns_flags_opcode': '0','dns_flags_dns_flags_authoritative': '0','dns_flags_dns_flags_truncated': '0','dns_flags_dns_flags_recdesired': '1','dns_flags_dns_flags_recavail': '1','dns_flags_dns_flags_z': '0','dns_flags_dns_flags_authenticated': '0','dns_flags_dns_flags_checkdisable': '0','dns_flags_dns_flags_rcode': '0','dns_dns_count_queries': '1','dns_dns_count_answers': '4','dns_dns_count_auth_rr': '10','dns_dns_count_add_rr': '0','dns_text': 'Queries','text_text': 'web.vortex.data.microsoft.com: type A, class IN','text_dns_qry_name': 'web.vortex.data.microsoft.com','text_dns_qry_name_len': '29','text_dns_count_labels': '5','text_dns_qry_type': '1','text_dns_qry_class': '0x00000001','dns_text': 'Answers','text_text': 'web.vortex.data.microsoft.com: type CNAME, class IN, cname web.vortex.data.microsoft.com.akadns.net','text_dns_resp_name': 'web.vortex.data.microsoft.com','text_dns_resp_type': '5','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '1808','text_dns_resp_len': '42','text_dns_cname': 'web.vortex.data.microsoft.com.akadns.net','text_text': 'web.vortex.data.microsoft.com.akadns.net: type CNAME, class IN, cname geo.vortex.data.microsoft.com.akadns.net','text_dns_resp_name': 'web.vortex.data.microsoft.com.akadns.net','text_dns_resp_type': '5','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '120','text_dns_resp_len': '6','text_dns_cname': 'geo.vortex.data.microsoft.com.akadns.net','text_text': 'geo.vortex.data.microsoft.com.akadns.net: type CNAME, class IN, cname db5.vortex.data.microsoft.com.akadns.net','text_dns_resp_name': 'geo.vortex.data.microsoft.com.akadns.net','text_dns_resp_type': '5','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '110','text_dns_resp_len': '6','text_dns_cname': 'db5.vortex.data.microsoft.com.akadns.net','text_text': 'db5.vortex.data.microsoft.com.akadns.net: type A, class IN, addr 40.77.226.250','text_dns_resp_name': 'db5.vortex.data.microsoft.com.akadns.net','text_dns_resp_type': '1','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '125','text_dns_resp_len': '4','text_dns_a': '40.77.226.250','dns_text': 'Authoritative nameservers','text_text': 'akadns.net: type NS, class IN, ns a11-129.akadns.net','text_dns_resp_name': 'akadns.net','text_dns_resp_type': '2','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '147180','text_dns_resp_len': '10','text_dns_ns': 'a11-129.akadns.net','text_text': 'akadns.net: type NS, class IN, ns a5-130.akadns.org','text_dns_resp_name': 'akadns.net','text_dns_resp_type': '2','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '147180','text_dns_resp_len': '19','text_dns_ns': 'a5-130.akadns.org','text_text': 'akadns.net: type NS, class IN, ns a9-128.akadns.net','text_dns_resp_name': 'akadns.net','text_dns_resp_type': '2','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '147180','text_dns_resp_len': '9','text_dns_ns': 'a9-128.akadns.net','text_text': 'akadns.net: type NS, class IN, ns a12-131.akadns.org','text_dns_resp_name': 'akadns.net','text_dns_resp_type': '2','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '147180','text_dns_resp_len': '10','text_dns_ns': 'a12-131.akadns.org','text_text': 'akadns.net: type NS, class IN, ns a1-128.akadns.net','text_dns_resp_name': 'akadns.net','text_dns_resp_type': '2','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '147180','text_dns_resp_len': '9','text_dns_ns': 'a1-128.akadns.net','text_text': 'akadns.net: type NS, class IN, ns a13-130.akadns.org','text_dns_resp_name': 'akadns.net','text_dns_resp_type': '2','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '147180','text_dns_resp_len': '10','text_dns_ns': 'a13-130.akadns.org','text_text': 'akadns.net: type NS, class IN, ns a18-128.akadns.org','text_dns_resp_name': 'akadns.net','text_dns_resp_type': '2','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '147180','text_dns_resp_len': '10','text_dns_ns': 'a18-128.akadns.org','text_text': 'akadns.net: type NS, class IN, ns a3-129.akadns.net','text_dns_resp_name': 'akadns.net','text_dns_resp_type': '2','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '147180','text_dns_resp_len': '9','text_dns_ns': 'a3-129.akadns.net','text_text': 'akadns.net: type NS, class IN, ns a28-129.akadns.org','text_dns_resp_name': 'akadns.net','text_dns_resp_type': '2','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '147180','text_dns_resp_len': '10','text_dns_ns': 'a28-129.akadns.org','text_text': 'akadns.net: type NS, class IN, ns a7-131.akadns.net','text_dns_resp_name': 'akadns.net','text_dns_resp_type': '2','text_dns_resp_class': '0x00000001','text_dns_resp_ttl': '147180','text_dns_resp_len': '9','text_dns_ns': 'a7-131.akadns.net'}";

        [Test()]
        public void DnsFactoryDnsMultipleQueriesTest()
        {
            var dns = Dns.DecodeJson(dnsStr2);
        }

        [Test()]
        public void DnsDecoderPerformanceTest()
        {
            int repeat = 100000;
            Console.Write($"Running 'Dns.DecodeJson(dnsStr2)' {repeat} times...");
            var sw = new Stopwatch();
            sw.Start();
            for(int i=0; i< repeat; i++)
            {
                var dns = Dns.DecodeJson(dnsStr2);
            }
            sw.Stop();
            Console.WriteLine($"took {sw.Elapsed}. Rate {(float)repeat / (sw.ElapsedMilliseconds / 1000)}.");
        }
    }
}



