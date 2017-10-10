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
        public void ExpressionTest_Parsing()
        {
            var e1 = DisplayFilterExpression.Parse("dns.flags.response == 0");
            var e2 = DisplayFilterExpression.Parse("dns.flags.response == 1 && dns.flags.rcode == 1");
        }



       
    }
}
