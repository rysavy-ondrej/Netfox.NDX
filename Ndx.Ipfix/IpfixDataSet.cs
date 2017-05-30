using System;
using PacketDotNet.Utils;

namespace Ndx.Ipfix
{
    /// <summary>
    /// IPFIX Data Set is a collection of data records. 
    /// </summary>
    public class IpfixDataSet
    {
        /// <summary>
        /// A byte array segment that contains data of the current data set.
        /// </summary>
        ByteArraySegment m_rawData;

        /// <summary>
        /// Set id of the data set. This should be equal to 2.
        /// </summary>
        /// <value>The set identifier.</value>
        ushort SetId => MiscUtil.Conversion.EndianBitConverter.Big.ToUInt16(m_rawData.Bytes, m_rawData.Offset);

        /// <summary>
        /// Length of the data set in octets. It includes bot header and payload.
        /// </summary>
        /// <value>The length.</value>
        ushort Length => MiscUtil.Conversion.EndianBitConverter.Big.ToUInt16(m_rawData.Bytes, m_rawData.Offset + 2);

        /// <summary>
        /// The content of the current data set.
        /// </summary>
        public ByteArraySegment Content => new ByteArraySegment(m_rawData.Bytes, m_rawData.Offset + 4, Length);

        public IpfixDataSet(byte[] bytes)
        {
            var len = MiscUtil.Conversion.EndianBitConverter.Big.ToUInt16(bytes, 2);
            m_rawData = new ByteArraySegment(bytes, 0, len);
        }

        public IpfixDataSet(ByteArraySegment bas)
        {
            var len = MiscUtil.Conversion.EndianBitConverter.Big.ToUInt16(bas.Bytes, bas.Offset + 2);
            m_rawData = new ByteArraySegment(bas.Bytes, bas.Offset, len);
        }
    }
}
