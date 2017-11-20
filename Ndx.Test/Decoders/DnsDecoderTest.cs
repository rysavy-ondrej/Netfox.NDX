using Ndx.Decoders.Basic;
using Ndx.Decoders.Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Test.Decoders
{
    [TestFixture]
    class DnsDecoderTest
    {
        [Test]                    
        public void DnsDecoder_Test()
        {
            var dnsStr = "{ 'dns_dns_id': '0x00000b89','dns_dns_flags': '0x00000100','dns_flags_dns_flags_response': '0','dns_flags_dns_flags_opcode': '0','dns_flags_dns_flags_truncated': '0','dns_flags_dns_flags_recdesired': '1','dns_flags_dns_flags_z': '0','dns_flags_dns_flags_checkdisable': '0','dns_dns_count_queries': '1','dns_dns_count_answers': '0','dns_dns_count_auth_rr': '0','dns_dns_count_add_rr': '0','dns_text': 'Queries','text_text': 'web.vortex.data.microsoft.com: type A, class IN','text_dns_qry_name': 'web.vortex.data.microsoft.com','text_dns_qry_name_len': '29','text_dns_count_labels': '5','text_dns_qry_type': '1','text_dns_qry_class': '0x00000001'}".Replace("text_", "dns_");

            var dns = Dns.DecodeJson(dnsStr);
            Assert.AreEqual(dns.DnsId, 0x00000b89);
            Assert.AreEqual(dns.DnsCountQueries, 1);
            Assert.AreEqual(dns.DnsQryName, "web.vortex.data.microsoft.com");
            Assert.AreEqual(dns.DnsQryType,  1);     // A
            Assert.AreEqual(dns.DnsQryClass, 1);     // Internet (IN) [RFC1035]
        }
    }
}
