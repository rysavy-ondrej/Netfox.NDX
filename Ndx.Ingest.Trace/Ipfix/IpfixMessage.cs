using System;
namespace Ndx.Ipfix
{
    /// <summary>
    /// Ipfix message header.
    /// </summary>
    public class IpfixMessageHeader
    {
        /// <summary>
        /// Version of Flow Record format that is exported in this message. The value of this field is 0x000a for the current version, incrementing by one the version that is used in the NetFlow services export version 9
        /// </summary>
        ushort m_version;
        /// <summary>
        /// Total length of the IPFIX Message, which is measured in octets, including Message Header and Sets.
        /// </summary>
        uint m_length;
        /// <summary>
        /// Time, in seconds, since 0000 Coordinated Universal Time Jan 1, 1970, at which the IPFIX Message Header leaves the Exporter.
        /// </summary>
        uint m_exportTime;
        /// <summary>
        /// Incremental sequence counter-modulo 2^32 of all IPFIX Data Records sent on this PR-SCTP stream from the current Observation Domain by the Exporting Process.
        /// </summary>
        uint m_sequenceNumber;
        /// <summary>
        /// A 32-bit identifier of the Observation Domain that is locally unique to the Exporting Process. 
        /// </summary>
        uint m_observationDomain;
    }

    /// <summary>
    /// An IPFIX message consists of a message header followed by multiple Sets of different types. A Set is a generic term for collection of records that have a similar structure. 
    /// There are three types of Sets - Data Set, Template Set, and Options Template Set. 
    /// Each of these have a Set header and one or more records.
    /// </summary>
    public class IpfixSet
    {
        /// <summary>
        /// Set ID value identifies the Set. A value of 2 is reserved for the Template Set. A value of 3 is reserved for the Option Template Set. All other values 4-255 are reserved for future use. Values more than 255 are used for Data Sets. The Set ID values of 0 and 1 are not used for historical reasons
        /// </summary>
        byte m_setId;
        /// <summary>
        /// Total length of the Set, in octets, including the Set Header, all records, and the optional padding. Because an individual Set MAY contain multiple records, the Length value must be used to determine the position of the next Set.
        /// </summary>
        uint m_length;
    }

    /// <summary>
    /// An IPFIX Message consisting of interleaved Template, Data, and Options Template Sets.
    /// All binary integers of the Message Header and the different Sets in network-byte order 
    /// (also known as the big-endian byte ordering).
    /// </summary>
    public class IpfixMessage
    {
        IpfixMessageHeader m_header;
        IpfixSet[] m_sets;
        public IpfixMessage()
        {
        }
    }
}
