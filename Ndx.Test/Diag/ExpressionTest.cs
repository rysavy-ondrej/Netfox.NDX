using NUnit.Framework;
using PacketDotNet;
using System.Net;
using System.Net.Sockets;
using Ndx.Model;
using Ndx.Diagnostics;

namespace Ndx.Test.Filters
{
    [TestFixture]
    public class ExpressionTests
    {
        [Test]
        public void DisplayFilterExpressionTest_Parsing()
        {
            var e1 = DisplayFilterExpression.Parse("dns.flags.response == 0");
            var e2 = DisplayFilterExpression.Parse("dns.flags.response == 1 && dns.flags.rcode == 1");
        }


        [Test]
        public void AssertPredicateExpressionTest_Parsing()     
        {
            var a1 = AssertPredicateExpression.Parse("e1.dns.flags.response == 0", new string[] { "e1", "e2" });
            var a2 = AssertPredicateExpression.Parse("e1.dns.flags.response == 1 && e2.dns.flags.rcode == 1", new string[] { "e1", "e2" });
            var a3 = AssertPredicateExpression.Parse("e1 [0-5]~> e2", new string[] { "e1", "e2" });
            var a4 = AssertPredicateExpression.Parse("e1 [0-5]~!> e2", new string[] { "e1", "e2" });
            var a5 = AssertPredicateExpression.Parse("e1.ip.src == host.ip.src", new string[] { "host", "e1", "e2" });

        }
    }
}
