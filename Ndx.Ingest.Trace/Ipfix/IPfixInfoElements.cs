using System;
using UInt8 = System.Byte;
namespace Ndx.Ipfix
{
    public static class IpfixInfoElements
    {
        public const ushort OctetDeltaCount = 1;
        public const ushort OctetDeltaCount_Length = sizeof(UInt32);

        public const ushort PacketDeltaCount = 2;
        public const ushort PacketDeltaCount_Length = sizeof(UInt32);

        public const ushort DeltaFlowCount = 3;
        public const ushort DeltaFlowCount_Length = sizeof(UInt64);

        public const ushort ProtocolIdentifier = 4;
        public const ushort ProtocolIdentifier_Length = sizeof(UInt8);

        public const ushort SourceIPv4Address = 8;
        public const ushort SourceIPv4Address_Length = sizeof(UInt32);

        public const ushort DestinationIPv4Address = 12;
        public const ushort DestinationIPv4Address_Length = sizeof(UInt32);

        public const ushort IPNextHopIPv4Address = 15;
        public const ushort IPNextHopIPv4Address_Length = sizeof(UInt32);
        /*
                5   ipClassOfService    unsigned8
                6   tcpControlBits  unsigned16
                7   sourceTransportPort unsigned16
                9   sourceIPv4PrefixLength  unsigned8
                10  ingressInterface    unsigned32  
                11  destinationTransportPort    unsigned16
                13  destinationIPv4PrefixLength unsigned8
                14  egressInterface unsigned32
                16  bgpSourceAsNumber   unsigned32  
                17  bgpDestinationAsNumber  unsigned32

                25  minimumIpTotalLength    unsigned64
                26  maximumIpTotalLength    unsigned64
                27  sourceIPv6Address   ipv6Address
                28  destinationIPv6Address  ipv6Address
                29  sourceIPv6PrefixLength  unsigned8
                30  destinationIPv6PrefixLength unsigned8
                31  flowLabelIPv6   unsigned32
                32  icmpTypeCodeIPv4    unsigned16
                33  igmpType    unsigned8
                36  flowActiveTimeout   unsigned16
                37  flowIdleTimeout unsigned16
                38  engineType  unsigned8 
                39  engineId    unsigned8
                52  minimumTTL  unsigned8
                53  maximumTTL  unsigned8
                54  fragmentIdentification  unsigned32
                56  sourceMacAddress    macAddress
                57  postDestinationMacAddress   macAddress
                58  vlanId  unsigned16
                59  postVlanId  unsigned16
                60  ipVersion   unsigned8
                61  flowDirection   unsigned8 //0 - ingress, 1 -egress
                62  ipNextHopIPv6Address    ipv6Address
                95  applicationId   octetArray   
                96  applicationName string
                136 flowEndReason   unsigned8
                139 icmpTypeCodeIPv6    unsigned16
                148 flowId  unsigned64
                150 flowStartSeconds    dateTimeSeconds
                151 flowEndSeconds  dateTimeSeconds
                152 flowStartMilliseconds   dateTimeMilliseconds
                153 flowEndMilliseconds dateTimeMilliseconds
                154 flowStartMicroseconds   dateTimeMicroseconds
                155 flowEndMicroseconds dateTimeMicroseconds
                161 flowDurationMilliseconds    unsigned32
                162 flowDurationMicroseconds    unsigned32
                176 icmpTypeIPv4    unsigned8
                177 icmpCodeIPv4    unsigned8
                178 icmpTypeIPv6    unsigned8
                179 icmpCodeIPv6    unsigned8
                180 udpSourcePort   unsigned16
                181 udpDestinationPort  unsigned16
                182 tcpSourcePort   unsigned16
                183 tcpDestinationPort  unsigned16
                184 tcpSequenceNumber   unsigned32
                185 tcpAcknowledgementNumber    unsigned32
                186 tcpWindowSize   unsigned16
                187 tcpUrgentPointer    unsigned16
                188 tcpHeaderLength unsigned8
                189 ipHeaderLength  unsigned8
                190 totalLengthIPv4 unsigned16
                191 payloadLengthIPv6   unsigned16
                192 ipTTL   unsigned8
           */

    }
}
