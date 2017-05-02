//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using Ndx.Utils;
using PacketDotNet;

namespace Ndx.Ingest.Trace
{

    /// <summary>
    /// This enumeration encodes address family and protocol type into a singe value.
    /// </summary>
    public enum FlowProtocol
    {
        IPV4 = IPProtocolType.IP,
        IPV6 = IPProtocolType.IPV6,
        TCP4 = AddressFamily.InterNetwork << 8 + IPProtocolType.TCP,
        UDP4 = AddressFamily.InterNetwork << 8 + IPProtocolType.UDP,
        OSPF4 = AddressFamily.InterNetwork << 8 + IPProtocolType.OSPF,
        ICMP4 = AddressFamily.InterNetwork << 8 + IPProtocolType.ICMP,
        IGMP4 = AddressFamily.InterNetwork << 8 + IPProtocolType.IGMP,

        TCP6 = AddressFamily.InterNetworkV6 << 8 + IPProtocolType.TCP,
        UDP6 = AddressFamily.InterNetworkV6 << 8 + IPProtocolType.UDP,
        ICMV6 = AddressFamily.InterNetworkV6 << 8 + IPProtocolType.ICMPV6,
        OSPF6 = AddressFamily.InterNetworkV6 << 8 + IPProtocolType.OSPF,

    }


    /// <summary>
    /// This structure is a compact representation of a Flow Key.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = __size)]
    unsafe public struct _FlowKey
    {
        /// <summary>
        /// The size of this structure in 32-bit words.
        /// </summary>
        public const int __size = 42;

        [FieldOffset(0)] public ushort protocol;
        [FieldOffset(2)] public fixed byte sourceAddress[16];
        [FieldOffset(18)] public fixed byte destinationAddress[16];
        [FieldOffset(34)] public ushort sourcePort;
        [FieldOffset(36)] public ushort destinationPort;
        [FieldOffset(38)] public uint flowId;

        /// <summary>
        /// Creates the flow key from the provided bytes.
        /// </summary>
        /// <param name="bytes"></param>
        public _FlowKey(byte[] bytes, int offset=0)
        {
            if (bytes.Length + offset < __size)
            {
                throw new ArgumentException($"Not enough bytes for intialization of {nameof(_FlowKey)} instance.");
            }

            fixed (byte* pdata = bytes)
            {
                this = *(_FlowKey*)(pdata+offset);
            }
        }

        /// <summary>
        /// Gets bytes that represents the current instance.
        /// </summary>
        /// <returns>byte array representing data of the current object.</returns>
        public byte[] GetBytes()
        {
            return ExplicitStruct.GetBytes<_FlowKey>(this);
        }

        public override bool Equals(object obj)
        {
            if (obj is _FlowKey other)
            {
                fixed (_FlowKey* x = &this)
                {
                    return Equals(x, &other);
                }
            }
            return false;
        }

        public override int GetHashCode()
        {
            fixed (_FlowKey* ptr = &this)
            {
                return ExplicitStruct.GetHash((int*)ptr, __size / sizeof(int));
            }
        }

        public static bool Equals(_FlowKey *x, _FlowKey *y)
        {
            return ExplicitStruct.CmpInt((int*)x, (int*)y, __size / sizeof(int));
        }

        public void SetSourceAddress(byte[] bytes, int offset, int count)
        {
            fixed (byte* ptr = sourceAddress)
            {
                Marshal.Copy(bytes, offset, new IntPtr(ptr), count);
            }
        }

        public void SetDestinationAddress(byte[] bytes, int offset, int count)
        {
            fixed (byte* ptr = destinationAddress)
            {
                Marshal.Copy(bytes, offset, new IntPtr(ptr), count);
            }
        }

        public byte[] GetDestinationAddressBytes()
        {
            var bytes = AllocateAddressByteBuffer();
            fixed (byte* ptr = this.destinationAddress)
            {
                Marshal.Copy(new IntPtr(ptr), bytes, 0, bytes.Length);
            }
            return bytes;
        }

        public byte[] GetSourceAddressBytes()
        {

            var bytes = AllocateAddressByteBuffer();
            fixed (byte* ptr = this.sourceAddress)
            {
                Marshal.Copy(new IntPtr(ptr), bytes, 0, bytes.Length);
            }
            return bytes;
        }

        public AddressFamily GetAddressFamily()
        {
            return (AddressFamily)((protocol >> 8) & 0xff);
        }

        public IPProtocolType GetProtocolType()
        {
            return (IPProtocolType)(protocol & 0xff);
        }

        private byte[] AllocateAddressByteBuffer()
        {
            switch (GetAddressFamily())
            {
                case AddressFamily.InterNetwork: return new byte[4];
                case AddressFamily.InterNetworkV6: return new byte[16];
                default: throw new ArgumentException($"Unknown or unsupported AddressFamily={GetAddressFamily()}");
            }
        }

        internal void SetFlowProtocol(AddressFamily family, IPProtocolType proto)
        {
            var x = (int)family << 8;
            var y = (int)proto;
            this.protocol = (ushort)(x|y);
        }

        internal void SetProtocolType(IPProtocolType value)
        {
            this.protocol = (ushort)(this.protocol & 0xff00 | ((int)value));
        }
        internal void SetAddressFamily(AddressFamily family)
        {
            this.protocol = (ushort)(this.protocol & 0x00ff | ((int)family)<<8);
        }
    }

    /// <summary>
    /// Represents a wrapper around <see cref="_FlowKey"/> structure and provides convenient methods
    /// for accessing the underlying data.
    /// </summary>
    [DebuggerDisplay("[FlowKey: {Protocol}@{SourceAddress}:{SourcePort}->{DestinationAddress}:{DestinationPort}]")]
    public class FlowKey : IEquatable<FlowKey>
    {
        private _FlowKey m_data;

        /// <summary>
        /// Intializes a new instance with the data provided.
        /// </summary>
        /// <param name="data"></param>
        internal FlowKey(_FlowKey data)
        {
            m_data = data;
        }

        /// <summary>
        /// Creates a new flow key for the specified values.
        /// </summary>
        /// <param name="proto">Protocol type.</param>
        /// <param name="srcIp">Source IP address.</param>
        /// <param name="srcPort">Source port number.</param>
        /// <param name="dstIp">Destination IP address.</param>
        /// <param name="dstPort">Destination port number.</param>
        public FlowKey(AddressFamily family, IPProtocolType proto, IPAddress srcIp, ushort srcPort, IPAddress dstIp, ushort dstPort, uint flowId) : this()
        {
            if (srcIp.AddressFamily != dstIp.AddressFamily)
            {
                throw new ArgumentException("AddressFamily mismatch.", nameof(srcIp));
            }

            m_data.flowId = flowId;
            m_data.SetFlowProtocol(family, proto);
            SourceAddress = srcIp;
            SourcePort = srcPort;
            DestinationAddress = dstIp;
            DestinationPort = dstPort;
        }

        /// <summary>
        /// Creates an empty flow key. All key fields will have thair default values. 
        /// Default address family is IPv4.
        /// </summary>
        public FlowKey(AddressFamily af = AddressFamily.InterNetwork)
        {
            this.m_data = new _FlowKey();
            m_data.SetFlowProtocol(af, IPProtocolType.NONE);
        }

        /// <summary>
        /// Creates a new instance from the byte array provided.
        /// </summary>
        /// <param name="bytes"></param>
        public FlowKey(byte[] bytes, int offset = 0)
        {
            this.m_data = new _FlowKey(bytes, offset);
        }


        public override string ToString()
        {
            return $"{Protocol}@{SourceAddress}:{SourcePort}->{DestinationAddress}:{DestinationPort}";
        }

        /// <summary>
        /// Gets or sets the address family of the current flow key.
        /// </summary>
        public System.Net.Sockets.AddressFamily AddressFamily
        {
            get => m_data.GetAddressFamily();
            set => m_data.SetAddressFamily(value);
        }

        /// <summary>
        /// This represents IP protocol type. 
        /// </summary>
        public IPProtocolType Protocol
        {
            get => m_data.GetProtocolType();
            set => m_data.SetProtocolType(value);
        }

        public uint FlowId
        {
            get => m_data.flowId;
            set => m_data.flowId = FlowId;
        }

        /// <summary>
        /// Gets or sets the source port.
        /// </summary>
        public ushort SourcePort
        {
            get => m_data.sourcePort; set => m_data.sourcePort = value;
        }

        /// <summary>
        /// Gets or sets the destination port.
        /// </summary>
        public ushort DestinationPort
        {
            get => m_data.destinationPort; set => m_data.destinationPort = value;
        }

        /// <summary>
        /// Gets or sets the source address.
        /// </summary>
        public IPAddress SourceAddress
        {
            get
            {
                var bytes = m_data.GetSourceAddressBytes();
                return new IPAddress(bytes);
            }
            set
            {
                var bytes = value.GetAddressBytes();
                m_data.SetSourceAddress(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// Gets or sets the destination address.
        /// </summary>
        public IPAddress DestinationAddress
        {
            get
            {
                var bytes = m_data.GetDestinationAddressBytes();
                return new IPAddress(bytes);
            }
            set
            {
                var bytes = value.GetAddressBytes();
                m_data.SetDestinationAddress(bytes, 0, bytes.Length);
            }
        }

        /// <summary>
        /// Gets bytes that represents the current object.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            return ExplicitStruct.GetBytes<_FlowKey>(this.m_data);
        }

        public bool Equals(FlowKey other)
        {
            if (other == null)
            {
                return false;
            }

            return Equals(m_data, other.m_data);
        }

        public override int GetHashCode()
        {
            return m_data.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var flowKey = obj as FlowKey;
            return Equals(flowKey);
        }

        public class BinaryConverter : IBinaryConverter<FlowKey>
        {
            public FlowKey ReadObject(BinaryReader reader)
            {
                var buf = reader.ReadBytes(_FlowKey.__size);
                if (buf.Length < _FlowKey.__size)
                {
                    return null;
                }
                else
                {
                    return new FlowKey(buf);
                }
            }

            public void WriteObject(BinaryWriter writer, FlowKey value)
            {
                writer.Write(value.GetBytes());
            }
        }
        public static IBinaryConverter<FlowKey> Converter = new BinaryConverter();
    }
}
