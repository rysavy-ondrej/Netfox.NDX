using Ndx.Ingest.Trace;
using NUnit.Framework;
using PacketDotNet;
using System.Net;

namespace Ndx.Test
{

    [TestFixture]
    public class FlowKeyTest
    {
        [Test]
        public void FlowKey_StoreLoad()
        {
            var srcIp = IPAddress.Parse("8.8.8.8");
            var dstIp = IPAddress.Parse("124.42.52.238");

            var fk1 = new FlowKey(IPProtocolType.UDP, srcIp,53, dstIp, 53);

            var bytes = fk1.GetBytes();        
            var fk2 = new FlowKey(bytes);
            Assert.AreEqual(fk1, fk2);
            bytes[4] = 9;
            var fk3 = new FlowKey(bytes);            
            Assert.AreNotEqual(fk2, fk3);
        }
    }
}
