//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using Ndx.Utils;
using PacketDotNet;
using PacketDotNet.LLDP;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace Ndx.Ingest.Trace
{

    [StructLayout(LayoutKind.Explicit, Size = __size)]
    unsafe public struct _FlowKey
    {
        /// <summary>
        /// The size of this structure in 32-bit words.
        /// </summary>
        public const int __size = 40;

        [FieldOffset(0)] public ushort protocol;
        [FieldOffset(2)] public fixed byte sourceAddress[16];
        [FieldOffset(18)] public fixed byte destinationAddress[16];
        [FieldOffset(34)] public ushort sourcePort;
        [FieldOffset(36)] public ushort destinationPort;
        [FieldOffset(38)] public ushort family;

        public _FlowKey(byte[] bytes)
        {
            fixed (byte* pdata = bytes)
            {
                this = *(_FlowKey*)pdata;
            }
        }

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
                    return equals(x, &other);
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

        private static bool equals(_FlowKey *x, _FlowKey *y)
        {
            return ExplicitStruct.CmpInt((int*)x, (int*)y, __size / sizeof(int));
        }

        internal void SetSourceAddress(byte[] bytes, int offset, int count)
        {
            fixed (byte* ptr = sourceAddress)
            {
                Marshal.Copy(bytes, offset, new IntPtr(ptr), count);
            }
        }

        internal void SetDestinationAddress(byte[] bytes, int offset, int count)
        {
            fixed (byte* ptr = destinationAddress)
            {
                Marshal.Copy(bytes, offset, new IntPtr(ptr), count);
            }
        }
    }

    [DebuggerDisplay("[FlowKey: {Protocol}@{SourceAddress}:{SourcePort}->{DestinationAddress}:{DestinationPort}]")]
    public class FlowKey : IEquatable<FlowKey>
    {
        private _FlowKey m_data;

        internal FlowKey(_FlowKey data)
        {
            m_data = data;
        }

        AddressFamily GetAddressFamily(System.Net.Sockets.AddressFamily af)
        {
            switch(af)
            {
                case System.Net.Sockets.AddressFamily.InterNetwork: return AddressFamily.IPv4;
                case System.Net.Sockets.AddressFamily.InterNetworkV6: return AddressFamily.IPv6;
                default: throw new ArgumentException("Unknown or unsupported AddressFamily.", nameof(af));
            }
        }
        public FlowKey(IPProtocolType proto, IPAddress srcIp, ushort srcPort, IPAddress dstIp, ushort dstPort): this()
        {
            if (srcIp.AddressFamily != dstIp.AddressFamily) throw new ArgumentException("AddressFamily mismatch.",nameof(srcIp));
            m_data.family = (ushort)(GetAddressFamily(srcIp.AddressFamily));
            m_data.protocol = (byte)proto;
            SetSourceAddress(srcIp);
            m_data.sourcePort = srcPort;
            SetDestinationAddress(dstIp);
            m_data.destinationPort = dstPort;
        }

        public FlowKey()
        {
            this.m_data = new _FlowKey();
        }


        public override string ToString()
        {
            return $"{Protocol}@{SourceAddress}{SourcePort}->{DestinationAddress}:{DestinationPort}";
        }

        public AddressFamily AddressFamily
        {
            get
            {
                return (AddressFamily)m_data.family;
            }
        }

        /// <summary>
        /// This represents IP protocol type. 
        /// </summary>
        public IPProtocolType Protocol
        {
            get
            {
                return (IPProtocolType)m_data.protocol;
            }
            set
            {
                m_data.protocol = (byte)value;
            }
        }

        public ushort SourcePort
        {
            get { return m_data.sourcePort;  }
            set { m_data.sourcePort = value; }
        }

        public ushort DestinationPort
        {
            get { return m_data.destinationPort; }
            set { m_data.destinationPort = value; }
        }

        public unsafe void SetSourceAddress(IPAddress sourceAddress)
        {
            var bytes = sourceAddress.GetAddressBytes();
            m_data.SetSourceAddress(bytes, 0, bytes.Length);
        }

        public unsafe void SetDestinationAddress(IPAddress destinationAddress)
        {
            var bytes = destinationAddress.GetAddressBytes();
            m_data.SetDestinationAddress(bytes, 0, bytes.Length);
        }


        unsafe byte[] GetSourceAddressBytes()
        {

            var bytes = GetAddressByteBuffer((AddressFamily)(m_data.family));
            fixed (byte* ptr = m_data.sourceAddress)
            {
                Marshal.Copy(new IntPtr(ptr), bytes, 0, bytes.Length);
            }
            return bytes;
        }

        private byte[] GetAddressByteBuffer(AddressFamily af)
        {
            switch (af)
            {
                case AddressFamily.Eth802: // return new byte[6];
                case AddressFamily.IPv4: return new byte[4];
                case AddressFamily.IPv6: return new byte[16];
                default: throw new ArgumentException("Unknown or unsupported AddressFamily.", nameof(af));
            }
        }

        unsafe byte[] GetDestinationAddressBytes()
        {
            var bytes = GetAddressByteBuffer((AddressFamily)(m_data.family));
            fixed (byte* ptr = m_data.destinationAddress)
            {
                Marshal.Copy(new IntPtr(ptr), bytes, 0, bytes.Length);
            }
            return bytes;
        }

        public IPAddress SourceAddress
        {
            get
            {
                var bytes = GetSourceAddressBytes();
                return new IPAddress(bytes);                
            }
        }
        public IPAddress DestinationAddress
        {
            get
            {
                var bytes = GetDestinationAddressBytes();
                return new IPAddress(bytes);
            }
        }


        public byte[] GetBytes()
        {   
            return ExplicitStruct.GetBytes<_FlowKey>(this.m_data);
        }

        public unsafe static FlowKey FromBytes(byte []bytes)
        {
            var data = new _FlowKey(bytes);
            return new FlowKey(data);            
        }

        public bool Equals(FlowKey other)
        {
            if (other == null) return false;
            return _FlowKey.Equals(m_data, other.m_data);            
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
            public bool CanRead => true;

            public bool CanWrite => true;

            public FlowKey ReadObject(BinaryReader reader)
            {
                var buf = reader.ReadBytes(_FlowKey.__size);
                if (buf.Length < _FlowKey.__size)
                    return null;
                else
                    return FlowKey.FromBytes(buf);
            }

            public void WriteObject(BinaryWriter writer, FlowKey value)
            {
                writer.Write(value.GetBytes());
            }
        }
        public static BinaryConverter Converter = new BinaryConverter();
    }
}
