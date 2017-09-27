using Ndx.Metacap;
using NUnit.Framework;
using PacketDotNet;
using System.Net;
using System.Net.Sockets;
using Ndx.Model;
using System.Net.NetworkInformation;
using System.IO;

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
                IpProtocol = Model.IpProtocolType.Udp,
                SourceIpAddress = srcIp,
                SourcePort = 53,
                DestinationIpAddress = dstIp,
                DestinationPort = 53
            };
            var bytes = fk1.GetBytes();
            var fk2 = new FlowKey(bytes);
            Assert.AreEqual(fk1, fk2);
        }
        [Test]
        public void FlowKey_Various()
        { 
            var icmp = new FlowKey()
            {
                IpProtocol = IpProtocolType.Icmp,
                SourceIpAddress = IPAddress.Parse("147.229.13.130"),
                DestinationIpAddress = IPAddress.Parse("8.8.8.8")
            };
            var icmpBytes = icmp.GetBytes();

            var arp = new FlowKey()
            {
                EthernetType = Model.EthernetPacketType.Arp,
                SourceMacAddress = PhysicalAddress.Parse("60-57-18-3E-2E-75"),
                DestinationMacAddress = PhysicalAddress.Parse("FF-FF-FF-FF-FF-FF")
            };
            var arpBytes = arp.GetBytes();
        }
    }
}
