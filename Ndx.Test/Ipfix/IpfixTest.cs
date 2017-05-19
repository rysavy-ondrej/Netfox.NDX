
using Ndx.Ingest.Trace;
using NUnit.Framework;
using System;
using Ndx.Ipfix;
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
            template[0].FieldId = (ushort)IpfixInfoElements.SourceIPv4Address;
            template[0].FieldLength = (ushort)IpfixInfoElements.SourceIPv4Address_Length;

            template[1].FieldId = (ushort)IpfixInfoElements.DestinationIPv4Address;
            template[1].FieldLength = (ushort)IpfixInfoElements.DestinationIPv4Address_Length;

            template[2].FieldId = (ushort)IpfixInfoElements.IPNextHopIPv4Address;
            template[2].FieldLength = (ushort)IpfixInfoElements.IPNextHopIPv4Address_Length;

            template[3].FieldId = (ushort)IpfixInfoElements.PacketDeltaCount;
            template[3].FieldLength = (ushort)IpfixInfoElements.PacketDeltaCount_Length;

            template[4].FieldId = (ushort)IpfixInfoElements.OctetDeltaCount;
            template[4].FieldLength = (ushort)IpfixInfoElements.OctetDeltaCount_Length;

            var t256 = template.Compile();
            var bytes = new byte[] {
                192, 0, 2, 12,          // 192.0.2.12 
                192, 0, 2, 254,         // 192.0.2.254 
                192, 0, 2, 1,           // 192.0.2.1 
                0x0, 0x0, 0x13, 0x91,   // 5009
                0x0, 0x51, 0x8C, 0x81,  // 5344385
            
            };

            var record = new ByteArraySegment(bytes);


            var srcIp = t256.GetBytes(record, IpfixInfoElements.SourceIPv4Address);
            var dstIp = t256.GetBytes(record, IpfixInfoElements.DestinationIPv4Address);
            var nxtIp = t256.GetBytes(record, IpfixInfoElements.IPNextHopIPv4Address);
            var pckts = t256.GetBytes(record, IpfixInfoElements.PacketDeltaCount);
            var octts = t256.GetBytes(record, IpfixInfoElements.OctetDeltaCount);
        }
    }
}
