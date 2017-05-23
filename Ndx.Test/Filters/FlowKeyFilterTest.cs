using Ndx.Ingest.Trace;
using NUnit.Framework;
using PacketDotNet;
using System.Net;
using System.Net.Sockets;

namespace Ndx.Test.Filters
{



    [TestFixture]
    public class FlowKeyFilterTest
    {

        [Test]
        public void FlowKeyFilter_SimpleExpressions()
        {
            var flowKey = new FlowKey(AddressFamily.InterNetwork, IPProtocolType.TCP, IPAddress.Parse("192.168.1.1"), 12345, IPAddress.Parse("192.168.200.100"), 80);

            var proto = FlowKeyFilterExpression.Parse("Protocol == TCP");
            var sourcePort = FlowKeyFilterExpression.Parse("SourcePort == 12345");
            var sourceAddress = FlowKeyFilterExpression.Parse("SourceAddress == 192.168.1.1");
            var destinationPort = FlowKeyFilterExpression.Parse("DestinationPort == 80");
            var destinationAddress = FlowKeyFilterExpression.Parse("DestinationAddress == 192.168.200.100");

            Assert.IsTrue(sourcePort.FlowFilter(flowKey));
            Assert.IsTrue(proto.FlowFilter(flowKey));

            Assert.IsTrue(sourceAddress.FlowFilter(flowKey));
            Assert.IsTrue(destinationPort.FlowFilter(flowKey));
            Assert.IsTrue(destinationAddress.FlowFilter(flowKey));
        }

        [Test]
        public void FlowKeyFilter_CoumpoundExpressions()
        {
            var flowKey = new FlowKey(AddressFamily.InterNetwork, IPProtocolType.TCP, IPAddress.Parse("192.168.1.1"), 12345, IPAddress.Parse("192.168.200.100"), 80);

            var expr1 = FlowKeyFilterExpression.Parse("SourcePOrt == 12345 && DEstinationPOrt == 80");
            Assert.IsTrue(expr1.FlowFilter(flowKey));

            var expr2 = FlowKeyFilterExpression.Parse("(DestinationPort == 80 || DestinationPort == 8080)");
            Assert.IsTrue(expr2.FlowFilter(flowKey));

            var expr3 = FlowKeyFilterExpression.Parse("Protocol == TCP &&(SourcePort == 12345 && DEstinationPOrt == 8080 || DestinationPort == 80)");
            Assert.IsTrue(expr3.FlowFilter(flowKey));

            var expr4 = FlowKeyFilterExpression.Parse("Protocol == TCP && (DestinationPort == 80 || DestinationPort == 8080)");
            Assert.IsTrue(expr4.FlowFilter(flowKey));

            var expr5 = FlowKeyFilterExpression.Parse("Protocol == TCP && SourceAddress == 192.168.1.1 && SourcePort == 12345 && DestinationAddress == 192.168.200.100 && DEstinationPOrt == 80");
            Assert.IsTrue(expr5.FlowFilter(flowKey));

            var expr6 = FlowKeyFilterExpression.Parse("Protocol == TCP && SourceAddress == 192.168.1.1 && SourcePort > 1024 && DestinationAddress == 192.168.200.100 && DEstinationPOrt == 80");
            Assert.IsTrue(expr6.FlowFilter(flowKey));

        }

        [Test]
        public void FlowKeyFilter_SyntaxErrorTest()
        {
            var expr1 = FlowKeyFilterExpression.TryParse("Source POrt == 12345 && DEstinationPOrt == 80", out string error1);
            Assert.IsNull(expr1);

            var expr2 = FlowKeyFilterExpression.TryParse("(SourcePOrt == 12345 && DEstinationPOrt == 80", out string error2);
            Assert.IsNull(expr2);


            var expr3 = FlowKeyFilterExpression.TryParse("(SourcePOrt > 1024 && DEstinationPOrt == 80", out string error3);
            Assert.IsNull(expr2);

        }
    }
}
