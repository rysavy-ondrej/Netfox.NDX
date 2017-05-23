using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using UInt8 = System.Byte;
namespace Ndx.Ipfix
{
    /// <summary>
    /// This static class enumerates IPFIX Information Elements. 
    /// https://www.iana.org/assignments/ipfix/ipfix.xhtml
    /// </summary>
    public static class IpfixInfoElements
    {
        public static ushort GetFieldId(string fieldName)
        {
            var typ = typeof(IpfixInfoElements);
            var f = typ.GetField(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (f == null)
            {
                throw new ArgumentException($"'{fieldName}' is not known field name.");
            }
            else
            {
                return (ushort)(f.GetRawConstantValue());
            }
        }

        public static ushort GetFieldLength(string fieldLen)
        {
            var typ = typeof(Sizeof);
            var f = typ.GetField(fieldLen, BindingFlags.IgnoreCase | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (f == null)
            {
                throw new ArgumentException($"'{fieldLen}' is not valid field type.");
            }
            else
            {
                return (ushort)(f.GetRawConstantValue());
            }
        }

        public static class Sizeof
        {
            public const ushort Unsigned8 = 1;
            public const ushort Unsigned16 = 2;
            public const ushort Unsigned32 = 4;
            public const ushort Unsigned64 = 8;
            public const ushort Macaddress = 6;
            public const ushort Ipv4address = 4;
            public const ushort Ipv6address = 16;
            public const ushort Datetimeseconds = 8;
            public const ushort Datetimemilliseconds = 8;
            public const ushort Datetimemicroseconds = 8;
            public const ushort Octetarray = 0;
            public const ushort String = 0;
            public const ushort Guid = 16;
        }

        public const ushort Protocolidentifier = 4;
        public const ushort Protocolidentifier_Length = Sizeof.Unsigned8;
        public const ushort Ipclassofservice = 5;
        public const ushort Ipclassofservice_Length = Sizeof.Unsigned8;
        public const ushort Tcpcontrolbits = 6;
        public const ushort Tcpcontrolbits_Length = Sizeof.Unsigned16;
        public const ushort Sourcetransportport = 7;
        public const ushort Sourcetransportport_Length = Sizeof.Unsigned16;
        public const ushort Sourceipv4address = 8;
        public const ushort Sourceipv4address_Length = Sizeof.Ipv4address;
        public const ushort Sourceipv4prefixlength = 9;
        public const ushort Sourceipv4prefixlength_Length = Sizeof.Unsigned8;
        public const ushort Ingressinterface = 10;
        public const ushort Ingressinterface_Length = Sizeof.Unsigned32;
        public const ushort Destinationtransportport = 11;
        public const ushort Destinationtransportport_Length = Sizeof.Unsigned16;
        public const ushort Destinationipv4address = 12;
        public const ushort Destinationipv4address_Length = Sizeof.Ipv4address;
        public const ushort Destinationipv4prefixlength = 13;
        public const ushort Destinationipv4prefixlength_Length = Sizeof.Unsigned8;
        public const ushort Egressinterface = 14;
        public const ushort Egressinterface_Length = Sizeof.Unsigned32;
        public const ushort Bgpsourceasnumber = 16;
        public const ushort Bgpsourceasnumber_Length = Sizeof.Unsigned32;
        public const ushort Bgpdestinationasnumber = 17;
        public const ushort Bgpdestinationasnumber_Length = Sizeof.Unsigned32;
        public const ushort Minimumiptotallength = 25;
        public const ushort Minimumiptotallength_Length = Sizeof.Unsigned64;
        public const ushort Maximumiptotallength = 26;
        public const ushort Maximumiptotallength_Length = Sizeof.Unsigned64;
        public const ushort Sourceipv6address = 27;
        public const ushort Sourceipv6address_Length = Sizeof.Ipv6address;
        public const ushort Destinationipv6address = 28;
        public const ushort Destinationipv6address_Length = Sizeof.Ipv6address;
        public const ushort Sourceipv6prefixlength = 29;
        public const ushort Sourceipv6prefixlength_Length = Sizeof.Unsigned8;
        public const ushort Destinationipv6prefixlength = 30;
        public const ushort Destinationipv6prefixlength_Length = Sizeof.Unsigned8;
        public const ushort Flowlabelipv6 = 31;
        public const ushort Flowlabelipv6_Length = Sizeof.Unsigned32;
        public const ushort Icmptypecodeipv4 = 32;
        public const ushort Icmptypecodeipv4_Length = Sizeof.Unsigned16;
        public const ushort Igmptype = 33;
        public const ushort Igmptype_Length = Sizeof.Unsigned8;
        public const ushort Flowactivetimeout = 36;
        public const ushort Flowactivetimeout_Length = Sizeof.Unsigned16;
        public const ushort Flowidletimeout = 37;
        public const ushort Flowidletimeout_Length = Sizeof.Unsigned16;
        public const ushort Enginetype = 38;
        public const ushort Enginetype_Length = Sizeof.Unsigned8;
        public const ushort Engineid = 39;
        public const ushort Engineid_Length = Sizeof.Unsigned8;
        public const ushort Minimumttl = 52;
        public const ushort Minimumttl_Length = Sizeof.Unsigned8;
        public const ushort Maximumttl = 53;
        public const ushort Maximumttl_Length = Sizeof.Unsigned8;
        public const ushort Fragmentidentification = 54;
        public const ushort Fragmentidentification_Length = Sizeof.Unsigned32;
        public const ushort Sourcemacaddress = 56;
        public const ushort Sourcemacaddress_Length = Sizeof.Macaddress;
        public const ushort Postdestinationmacaddress = 57;
        public const ushort Postdestinationmacaddress_Length = Sizeof.Macaddress;
        public const ushort Vlanid = 58;
        public const ushort Vlanid_Length = Sizeof.Unsigned16;
        public const ushort Postvlanid = 59;
        public const ushort Postvlanid_Length = Sizeof.Unsigned16;
        public const ushort Ipversion = 60;
        public const ushort Ipversion_Length = Sizeof.Unsigned8;
        public const ushort Flowdirection = 61;
        public const ushort Flowdirection_Length = Sizeof.Unsigned8;
        public const ushort Ipnexthopipv6address = 62;
        public const ushort Ipnexthopipv6address_Length = Sizeof.Ipv6address;
        public const ushort Octettotalcount = 85;
        public const ushort Octettotalcount_Length = Sizeof.Unsigned64;
        public const ushort Packettotalcount = 86;
        public const ushort Packettotalcount_Length = Sizeof.Unsigned64;
        public const ushort Applicationid = 95;
        public const ushort Applicationid_Length = Sizeof.Octetarray;
        public const ushort Applicationname = 96;
        public const ushort Applicationname_Length = Sizeof.String;
        public const ushort Flowendreason = 136;
        public const ushort Flowendreason_Length = Sizeof.Unsigned8;
        public const ushort Icmptypecodeipv6 = 139;
        public const ushort Icmptypecodeipv6_Length = Sizeof.Unsigned16;
        public const ushort Flowid = 148;
        public const ushort Flowid_Length = Sizeof.Unsigned64;
        public const ushort Flowstartseconds = 150;
        public const ushort Flowstartseconds_Length = Sizeof.Datetimeseconds;
        public const ushort Flowendseconds = 151;
        public const ushort Flowendseconds_Length = Sizeof.Datetimeseconds;
        public const ushort Flowstartmilliseconds = 152;
        public const ushort Flowstartmilliseconds_Length = Sizeof.Datetimemilliseconds;
        public const ushort Flowendmilliseconds = 153;
        public const ushort Flowendmilliseconds_Length = Sizeof.Datetimemilliseconds;
        public const ushort Flowstartmicroseconds = 154;
        public const ushort Flowstartmicroseconds_Length = Sizeof.Datetimemicroseconds;
        public const ushort Flowendmicroseconds = 155;
        public const ushort Flowendmicroseconds_Length = Sizeof.Datetimemicroseconds;
        public const ushort Flowdurationmilliseconds = 161;
        public const ushort Flowdurationmilliseconds_Length = Sizeof.Unsigned32;
        public const ushort Flowdurationmicroseconds = 162;
        public const ushort Flowdurationmicroseconds_Length = Sizeof.Unsigned32;
        public const ushort Icmptypeipv4 = 176;
        public const ushort Icmptypeipv4_Length = Sizeof.Unsigned8;
        public const ushort Icmpcodeipv4 = 177;
        public const ushort Icmpcodeipv4_Length = Sizeof.Unsigned8;
        public const ushort Icmptypeipv6 = 178;
        public const ushort Icmptypeipv6_Length = Sizeof.Unsigned8;
        public const ushort Icmpcodeipv6 = 179;
        public const ushort Icmpcodeipv6_Length = Sizeof.Unsigned8;
        public const ushort Udpsourceport = 180;
        public const ushort Udpsourceport_Length = Sizeof.Unsigned16;
        public const ushort Udpdestinationport = 181;
        public const ushort Udpdestinationport_Length = Sizeof.Unsigned16;
        public const ushort Tcpsourceport = 182;
        public const ushort Tcpsourceport_Length = Sizeof.Unsigned16;
        public const ushort Tcpdestinationport = 183;
        public const ushort Tcpdestinationport_Length = Sizeof.Unsigned16;
        public const ushort Tcpsequencenumber = 184;
        public const ushort Tcpsequencenumber_Length = Sizeof.Unsigned32;
        public const ushort Tcpacknowledgementnumber = 185;
        public const ushort Tcpacknowledgementnumber_Length = Sizeof.Unsigned32;
        public const ushort Tcpwindowsize = 186;
        public const ushort Tcpwindowsize_Length = Sizeof.Unsigned16;
        public const ushort Tcpurgentpointer = 187;
        public const ushort Tcpurgentpointer_Length = Sizeof.Unsigned16;
        public const ushort Tcpheaderlength = 188;
        public const ushort Tcpheaderlength_Length = Sizeof.Unsigned8;
        public const ushort Ipheaderlength = 189;
        public const ushort Ipheaderlength_Length = Sizeof.Unsigned8;
        public const ushort Totallengthipv4 = 190;
        public const ushort Totallengthipv4_Length = Sizeof.Unsigned16;
        public const ushort Payloadlengthipv6 = 191;
        public const ushort Payloadlengthipv6_Length = Sizeof.Unsigned16;
        public const ushort Ipttl = 192;
        public const ushort Ipttl_Length = Sizeof.Unsigned8;
        public const ushort Datalinkframesize = 312;
        public const ushort Datalinkframesize_Length = Sizeof.Unsigned16;
        public const ushort Datalinkframetype = 408;
        public const ushort Datalinkframetype_Length = Sizeof.Unsigned16;
        public const ushort Sectionoffset = 409;
        public const ushort Sectionoffset_Length = Sizeof.Unsigned16;
        public const ushort Datalinkframetimestamp = 512;
        public const ushort Datalinkframetimestamp_Length = Sizeof.Datetimemilliseconds;
        public const ushort Datalinkframeoffset = 513;
        public const ushort Datalinkframeoffset_Length = Sizeof.Unsigned64;
        public const ushort Datalinkframenumber = 514;
        public const ushort Datalinkframenumber_Length = Sizeof.Unsigned32;

        public const ushort Datalinksectionoffset = 515;
        public const ushort Datalinksectionoffset_Length = Sizeof.Unsigned16;

        public const ushort Networksectionoffset = 516;
        public const ushort Networksectionoffset_Length = Sizeof.Unsigned16;

        public const ushort Transportsectionoffset = 517;
        public const ushort Transportsectionoffset_Length = Sizeof.Unsigned16;

        public const ushort Payloadsectionoffset = 518;
        public const ushort Payloadsectionoffset_Length = Sizeof.Unsigned16;

        public const ushort Datalinksectionsize = 519;
        public const ushort Datalinksectionsize_Length = Sizeof.Unsigned16;

        public const ushort Networksectionsize = 520;
        public const ushort Networksectionsize_Length = Sizeof.Unsigned16;

        public const ushort Transportsectionsize = 521;
        public const ushort Transportsectionsize_Length = Sizeof.Unsigned16;

        public const ushort Payloadsectionsize = 522;
        public const ushort Payloadsectionsize_Length = Sizeof.Unsigned16;

        public const ushort ConversationLabel = 1024;
        public const ushort ConversationLabel_Length = Sizeof.Guid;

        /// <summary>
        /// Use this function to generate constant for information elements in C# Interactive:
        /// var y = IpfixInfoElements.GetElementDefinitionsFromTable(File.ReadAllText("Ipfix\\InformationElements.txt")); 
        /// Console.Write(y);
        /// </summary>
        /// <param name="elemTable"></param>
        /// <returns></returns>
        public static string GetElementDefinitionsFromTable(string elemTable)
        {
            var result = new List<Tuple<int, string, string>>();
            var lines = elemTable.Split('\n');
            var ti = new CultureInfo("en-US", false).TextInfo;
            foreach(var lin in lines)
            {
                var cols = lin.Trim().Split(new char[]{ ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (cols.Length < 3) continue;
                var id = int.Parse(cols[0]);
                var name = cols[1];
                var typ = cols[2];
                result.Add(Tuple.Create(id, $"public const ushort {ti.ToTitleCase(name)} = {id};", $"public const ushort {ti.ToTitleCase(name)}_Length = Sizeof.{ti.ToTitleCase(typ)};"));
            }
            var items = result.OrderBy(x => x.Item1).SelectMany(x => new[] { x.Item2, x.Item3 });
            return String.Join("\n", items);
        }
    }
}
