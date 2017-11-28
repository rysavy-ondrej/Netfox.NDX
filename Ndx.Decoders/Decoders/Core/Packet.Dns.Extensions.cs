using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Decoders.Core
{
    public partial class Dns
    {
        /// <summary>
        /// The DNS TYPE (RFC1035 3.2.2/3) - 4 types are currently supported. Also, I know that this
        /// enumeration goes against naming guidelines, but I have done this as an ANAME is most
        /// definetely an 'ANAME' and not an 'Aname'
        /// </summary>
        public enum DnsType
        {
            None = 0, A = 1, NS = 2, CNAME = 5, SOA = 6, PTR = 12, MX = 15, TXT = 16, AAAA = 28, SRV = 33, NAPTR = 35
        }

        /// <summary>
        /// The DNS CLASS (RFC1035 3.2.4/5)
        /// Internet will be the one we'll be using (IN), the others are for completeness
        /// </summary>
        public enum DnsClass
        {
            None = 0, IN = 1, CS = 2, CH = 3, HS = 4
        }

        /// <summary>
        /// (RFC1035 4.1.1) These are the return codes the server can send back
        /// </summary>
        public enum ReturnCode
        {
            Success = 0,
            FormatError = 1,
            ServerFailure = 2,
            NameError = 3,
            NotImplemented = 4,
            Refused = 5,
            Other = 6
        }

        /// <summary>
        /// (RFC1035 4.1.1) These are the Query Types which apply to all questions in a request
        /// </summary>
        public enum Opcode
        {
            StandardQuery = 0,
            InverseQuerty = 1,
            StatusRequest = 2,
            Reserverd3 = 3,
            Reserverd4 = 4,
            Reserverd5 = 5,
            Reserverd6 = 6,
            Reserverd7 = 7,
            Reserverd8 = 8,
            Reserverd9 = 9,
            Reserverd10 = 10,
            Reserverd11 = 11,
            Reserverd12 = 12,
            Reserverd13 = 13,
            Reserverd14 = 14,
            Reserverd15 = 15,
        }

        public partial class Types {
            public partial class _DnsQry
            {
                public string DnsQryTypeString => ((DnsType)(this.DnsQryType)).ToString();
                public string DnsQryClassString => ((DnsClass)(this.DnsQryClass)).ToString();
            }
            public partial class _DnsResp
            {
                public string DnsRespTypeString => ((DnsType)(this.DnsRespType)).ToString();
                public string DnsRespClassString => ((DnsClass)(this.DnsRespClass)).ToString();
                public string DnsRespValueString
                {
                    get
                    {
                        switch((DnsType)DnsRespType)
                        {
                            case DnsType.A: return new IPAddress(this.DnsA.ToByteArray()).ToString();
                            case DnsType.AAAA: return new IPAddress(this.DnsAaaa.ToByteArray()).ToString();
                            case DnsType.CNAME: return  this.DnsCname;
                            case DnsType.MX: return  this.DnsMxMailExchange;
                            case DnsType.NAPTR: return this.DnsNaptrService;
                            case DnsType.NS: return this.DnsNs;
                            case DnsType.PTR: return this.DnsPtrDomainName;
                            case DnsType.SOA: return $" {this.DnsSoaMname} {this.DnsSoaRname} {this.DnsSoaSerialNumber} {this.DnsSoaRefreshInterval} {this.DnsSoaRetryInterval} {this.DnsSoaExpireLimit} {this.DnsSoaMininumTtl}";
                            case DnsType.SRV: return this.DnsSrvService;
                            case DnsType.TXT: return this.DnsTxt;
                        }
                        return String.Empty;
                    }                
                }               
            }
        }
    }
}
