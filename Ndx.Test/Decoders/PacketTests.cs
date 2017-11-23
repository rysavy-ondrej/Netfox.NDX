using NUnit.Framework;
using Ndx.Decoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Decoders.Basic;
using System.IO;
using Ndx.Captures;
using Ndx.Decoders.Core;
using Ndx.Decoders.Base;

namespace Ndx.Decoders.Tests
{
    [TestFixture()]
    public class PacketTests
    {
        [Test()]
        public void InitializeSettersTest()
        {
            PacketDecoder.InitializeSetters();
        }

        [Test()]
        public void PacketSettersTest()
        {
            var packetDecoder = new PacketDecoder();
            var eth1 = packetDecoder.CreateProtocol(new Eth() { EthLen = 10 });
            var eth2 = packetDecoder.CreateProtocol((object)new Eth() { EthLen = 10 });
        }

        
        [Test()]
        public void DecodeTest()
        {

            var input = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\TestData\http.json");
            using (var reader = new StreamReader(File.OpenRead(input)))
            {
                var factory = new DecoderFactory();
                var decoder = new PacketDecoder();
                var stream = new PcapJsonStream(reader);
                JsonPacket packet;
                while((packet = stream.ReadPacket()) != null)
                {
                    var decodedPacket = decoder.Decode(factory, packet);
                    Console.WriteLine(decodedPacket);
                }
            }            
        }

        [Test()]
        public void DecodeTestSelect()
        {

            var input = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\TestData\http.json");
            List<Packet> packets = new List<Packet>();
            using (var reader = new StreamReader(File.OpenRead(input)))
            {
                var factory = new DecoderFactory();
                var decoder = new PacketDecoder();
                var stream = new PcapJsonStream(reader);
                JsonPacket packet;
                while ((packet = stream.ReadPacket()) != null)
                {
                    var decodedPacket = decoder.Decode(factory, packet);
                    packets.Add(decodedPacket);
                }
            }
            var https = packets.Select(x => x.Protocol<Http>()).Where(x => x!=null);
            foreach (var http in https)
            {
                Console.WriteLine(http);
            }
        }
    }
}