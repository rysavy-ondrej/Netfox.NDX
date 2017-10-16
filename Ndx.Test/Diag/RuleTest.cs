using NUnit.Framework;
using PacketDotNet;
using System.Net;
using System.Net.Sockets;
using Ndx.Model;
using Ndx.Diagnostics;

namespace Ndx.Test.Filters
{
    [TestFixture]
    public class RuleTest
    {



        [Test]
        public void LoadRuleFromYaml()
        {
            var rule = Rule.Load(theRule);
        }



        private const string theRule = @"---
rule:
    id: dns_test_ok
    description: Rule that select successful DNS communication of the specified host.
params:
    - dnsClient
events:
    e1: dns.flags.response == 0 && ip.src == dnsClient.ip.src
    e2: dns.flags.response == 1 && dns.flags.rcode == 1
assert:
    - e1.dns.id == e2.dns.id
    - e1 [0 - 5s]~> e2
select:
    host: dnsClient
    query: e1
    answer: e2
    description: DNS Error.
";
    }
}


