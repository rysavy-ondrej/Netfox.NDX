using Ndx.Metacap;
using NUnit.Framework;
using PacketDotNet;
using System.Net;
using System.Net.Sockets;
using Ndx.Model;

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

            var fk1 = new FlowKey()
            {
                AddressFamily = Model.AddressFamily.InterNetwork,
                Protocol = Model.IpProtocolType.Udp,
                SourceIpAddress = srcIp,
                SourcePort = 53,
                DestinationIpAddress = dstIp,
                DestinationPort = 53
            };
            var bytes = fk1.GetBytes();        
            var fk2 = new FlowKey(bytes);
            Assert.AreEqual(fk1, fk2);
        }
    }
}
