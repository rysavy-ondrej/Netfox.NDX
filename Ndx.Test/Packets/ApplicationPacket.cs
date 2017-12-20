using Kaitai;
using Ndx.Packets.Common;
using NUnit.Framework;
using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Test
{
    [TestFixture]
    public class ApplicationPacket
    {
        static TestContext m_testContext = TestContext.CurrentContext;

        [Test]        
        public void ApplicationPacket_ParseSnmp()
        {
            string source = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\snmp-get-response.raw");
            var bytes = File.ReadAllBytes(source);
            var p = Packet.ParsePacket(LinkLayers.Ethernet, bytes);
            var ap = p.Extract(typeof(PacketDotNet.ApplicationPacket)) as PacketDotNet.ApplicationPacket;
            var snmp = new Snmp(new KaitaiStream(ap.Bytes));
        }

        [Test]
        public void ApplicationPacket_ParseSnmp2()
        {
            string source = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\snmp-request2-v2.raw");
            var bytes = File.ReadAllBytes(source);
            var snmp = new Snmp(new KaitaiStream(bytes));
        }


        [Test]
        public void ApplicationPacket_ParseSnmpPerf()
        {
            var sw = new Stopwatch();
            string source = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\snmp-get-response.raw");
            var bytes = File.ReadAllBytes(source);
            sw.Start();
            for (int i = 0; i < 100000; i++)
            {
                var p = Packet.ParsePacket(LinkLayers.Ethernet, bytes);
                var ap = p.Extract(typeof(PacketDotNet.ApplicationPacket)) as PacketDotNet.ApplicationPacket;
                var snmp = new Snmp(new KaitaiStream(ap.Bytes));
            }
            sw.Stop();
            Console.WriteLine($"Parsing packet (100 bytes) with SNMP get-response message from memory buffer: 100,000 iterations took {sw.ElapsedMilliseconds} ms.");
        }

        [Test]
        public void ApplicationPacket_ParseCoap()
        {
            string source = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\coap_only.pcap");
            var frames = Captures.PcapFile.ReadFile(source);
            foreach(var frame in frames.ToEnumerable())
            {
                var p = Packet.ParsePacket(LinkLayers.Ethernet, frame.Bytes);
                var ip = p.Extract(typeof(IpPacket)) as IpPacket;
                var ap = p.Extract(typeof(PacketDotNet.ApplicationPacket)) as PacketDotNet.ApplicationPacket;
                var coap = new Packets.IoT.Coap(new KaitaiStream(ap.Bytes));                
                Console.WriteLine($"{frame.FrameNumber, 3}{frame.FrameOffset, 10} {ip.SourceAddress} -> {ip.DestinationAddress} : {coap.Info} {(coap.IsRequest ? coap.GetUri(ip.DestinationAddress.ToString()).ToString() : String.Empty)} Payload {coap.Body?.Length ?? 0} bytes.");
            }
        }

    }
}

