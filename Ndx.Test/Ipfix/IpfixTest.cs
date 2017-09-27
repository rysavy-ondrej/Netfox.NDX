using Ndx.Ipfix;
using NUnit.Framework;
using PacketDotNet.Utils;

namespace Ndx.Test.Ipfix
{
    [TestFixture]
    public class IpfixTest
    {
        public IpfixTest()
        {
        }

        [Test]
        public void CreateTemplate()
        {
            var template = new IpfixTemplateRecord(256, 5);
            template[0].Id = (ushort)IpfixInfoElements.Sourceipv4address;
            template[0].Length = (ushort)IpfixInfoElements.Sourceipv4address_Length;

            template[1].Id = (ushort)IpfixInfoElements.Destinationipv4address;
            template[1].Length = (ushort)IpfixInfoElements.Destinationipv4address_Length;

            template[2].Id = (ushort)IpfixInfoElements.Destinationipv4address;
            template[2].Length = (ushort)IpfixInfoElements.Destinationipv4address_Length;

            template[3].Id = (ushort)IpfixInfoElements.Packettotalcount;
            template[3].Length = (ushort)IpfixInfoElements.Packettotalcount_Length;

            template[4].Id = (ushort)IpfixInfoElements.Octettotalcount;
            template[4].Length = (ushort)IpfixInfoElements.Octettotalcount_Length;

            var t256 = template.Compile();
            var bytes = new byte[] {
                192, 0, 2, 12,          // 192.0.2.12 
                192, 0, 2, 254,         // 192.0.2.254 
                192, 0, 2, 1,           // 192.0.2.1 
                0x0, 0x0, 0x0,0x0, 0x0, 0x0, 0x13, 0x91,   // 5009
                0x0, 0x0, 0x0,0x0, 0x0, 0x51, 0x8C, 0x81,  // 5344385
            
            };

            var record = new ByteArraySegment(bytes);


            var srcIp = t256.GetBytes(record, IpfixInfoElements.Sourceipv4address);
            var dstIp = t256.GetBytes(record, IpfixInfoElements.Destinationipv4address);
            var nxtIp = t256.GetBytes(record, IpfixInfoElements.Destinationipv4address);
            var pckts = t256.GetBytes(record, IpfixInfoElements.Packettotalcount);
            var octts = t256.GetBytes(record, IpfixInfoElements.Octettotalcount);
        }


        [Test]
        public void LoadTemplate()
        {
            var ipv4 = IpfixTemplateRecord.ConversationIpv4;
            var ipv4c = ipv4.Compile();

            var ipv6 = IpfixTemplateRecord.ConversationIpv6;
            var ipv6c = ipv6.Compile();

            var ipv4f = IpfixTemplateRecord.FlowIpv4;
            var ipv4fc = ipv4f.Compile();

            var ipv6f = IpfixTemplateRecord.FlowIpv6;
            var ipv6cf = ipv6f.Compile();

        }

    }
}
