using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Ndx.Ingest.Trace;
using System.Net;
using PacketDotNet;

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
            var fk2 = FlowKey.FromBytes(bytes);
            Assert.AreEqual(fk1, fk2);
            bytes[4] = 9;
            var fk3 = FlowKey.FromBytes(bytes);            
            Assert.AreNotEqual(fk2, fk3);
        }
    }
}
