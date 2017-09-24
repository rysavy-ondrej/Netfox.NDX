//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Ndx.Model
{
    [DebuggerDisplay("[FlowKey: {IpProtocol} {SourceIpAddress}:{SourcePort} -> {DestinationIpAddress}:{DestinationPort}]")]
    public partial class FlowKey : IEquatable<FlowKey>
    {

        private static readonly FlowKey m_none = new FlowKey() { IpProtocol = IpProtocolType.None, SourceIpAddress = IPAddress.None, SourcePort = 0, DestinationIpAddress = IPAddress.None, DestinationPort = 0 };
        public static FlowKey None => m_none;

        public IPAddress SourceIpAddress
        {
            get => this.sourceAddress_.IsEmpty ? IPAddress.None : new IPAddress(this.sourceAddress_.ToByteArray());
            set => this.sourceAddress_ = Google.Protobuf.ByteString.CopyFrom(value.GetAddressBytes());
        }

        public IPAddress DestinationIpAddress
        {
            get => this.destinationAddress_.IsEmpty ? IPAddress.None : new IPAddress(this.destinationAddress_.ToByteArray());
            set => this.destinationAddress_ = Google.Protobuf.ByteString.CopyFrom(value.GetAddressBytes());
        }

        public ushort SourcePort
        {
            get => this.sourceSelector_.IsEmpty ? (ushort)0 : BitConverter.ToUInt16(this.sourceSelector_.ToByteArray(), 0);
            set => this.sourceSelector_ = Google.Protobuf.ByteString.CopyFrom(BitConverter.GetBytes(value));
        }
        public ushort DestinationPort
        {
            get => this.destinationSelector_.IsEmpty ? (ushort)0 : BitConverter.ToUInt16(this.destinationSelector_.ToByteArray(), 0);
            set => this.destinationSelector_ = Google.Protobuf.ByteString.CopyFrom(BitConverter.GetBytes(value));
        }
        public PhysicalAddress SourceMacAddress
        {
            get => this.sourceAddress_.IsEmpty ? PhysicalAddress.None : new PhysicalAddress(sourceAddress_.ToByteArray());
            set => this.sourceAddress_ = Google.Protobuf.ByteString.CopyFrom(value.GetAddressBytes());
        }
        public PhysicalAddress DestinationMacAddress
        {
            get => this.destinationAddress_.IsEmpty ? PhysicalAddress.None : new PhysicalAddress(destinationAddress_.ToByteArray());
            set => this.destinationAddress_ = Google.Protobuf.ByteString.CopyFrom(value.GetAddressBytes());
        }
        public IPEndPoint SourceEndpoint
        {
            get
            {
                if (sourceAddress_.IsEmpty)
                {
                    return new IPEndPoint(IPAddress.None, 0);
                }
                else
                {
                    return new IPEndPoint(SourceIpAddress, SourcePort);
                }
            }
            set
            {
                var bytes = value.Address.GetAddressBytes().Concat(BitConverter.GetBytes(value.Port)).ToArray();
                this.sourceAddress_ = Google.Protobuf.ByteString.CopyFrom(bytes);
            }
        }
        public IPEndPoint DestinationEndpoint
        {
            get
            {
                if (destinationAddress_.IsEmpty)
                {
                    return new IPEndPoint(IPAddress.None, 0);
                }
                else
                {
                    return new IPEndPoint(DestinationIpAddress, DestinationPort);
                }
            }
            set
            {
                var bytes = value.Address.GetAddressBytes().Concat(BitConverter.GetBytes(value.Port)).ToArray();
                this.destinationAddress_ = Google.Protobuf.ByteString.CopyFrom(bytes);
            }
        }

        public IpProtocolType IpProtocol
        {
            get => this.protocol_.IsEmpty ? IpProtocolType.None : (IpProtocolType)(BitConverter.ToInt32(protocol_.ToByteArray(), 0));
            set => this.protocol_ = Google.Protobuf.ByteString.CopyFrom(BitConverter.GetBytes((Int32)value));
        }

        public EthernetPacketType EthernetType
        {
            get => this.protocol_.IsEmpty ? EthernetPacketType.None : (EthernetPacketType)(BitConverter.ToInt32(protocol_.ToByteArray(), 0));
            set => this.protocol_ = Google.Protobuf.ByteString.CopyFrom(BitConverter.GetBytes((Int32)value));
        }

        /// <summary>
        /// Creates a new instance from the byte array provided.
        /// </summary>
        /// <param name="bytes"></param>
        public FlowKey(byte[] bytes, int offset = 0)
        {
            using (var ms = new MemoryStream(bytes, offset, bytes.Length - offset))
            {
                using (var cis = new Google.Protobuf.CodedInputStream(ms))
                {
                    this.MergeFrom(cis);
                }
            }
        }

        /// <summary>
        /// Gets bytes that represents the current object.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var cos = new Google.Protobuf.CodedOutputStream(ms))
                {
                    WriteTo(cos);
                }
                return ms.ToArray();
            }
        }

        public FlowKey Swap()
        {
            return new FlowKey()
            {
                Type = this.Type,
                Protocol = this.Protocol,
                SourceAddress = this.DestinationAddress,
                SourceSelector = this.DestinationSelector,
                DestinationAddress = this.SourceAddress,
                DestinationSelector = this.SourceSelector
            };
        }

        public class ReferenceComparer : IEqualityComparer<FlowKey>
        {
            public bool Equals(FlowKey x, FlowKey y)
            {
                return Object.ReferenceEquals(x, y);
            }

            public int GetHashCode(FlowKey obj)
            {
                return RuntimeHelpers.GetHashCode(obj);
            }
        }

        public class ValueComparer : IEqualityComparer<FlowKey>
        {
            public bool Equals(FlowKey x, FlowKey y)
            {
                return x?.Equals(y) ?? false;
            }

            public int GetHashCode(FlowKey obj)
            {
                return obj?.GetHashCode() ?? 0;
            }
        }
        public string IpFlowKeyString => $"{IpProtocol}!{SourceIpAddress}:{SourcePort}->{DestinationIpAddress}{DestinationPort}";
    }
}
