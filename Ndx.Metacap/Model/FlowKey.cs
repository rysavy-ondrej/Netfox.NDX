//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Ndx.Model
{
    [DebuggerDisplay("[FlowKey: ???]")]
    public partial class FlowKey : IEquatable<FlowKey>
    {
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
            get => this.sourceSelector_.IsEmpty ? (ushort)0 : BitConverter.ToUInt16(this.sourceSelector_.ToByteArray(),0);
            set => this.sourceSelector_= Google.Protobuf.ByteString.CopyFrom(BitConverter.GetBytes(value));
        }
        public ushort DestinationPort
        {
            get => this.destinationSelector_.IsEmpty ? (ushort)0 : BitConverter.ToUInt16(this.destinationSelector_.ToByteArray(), 0);
            set => this.destinationSelector_ = Google.Protobuf.ByteString.CopyFrom(BitConverter.GetBytes(value));
        }
        public PhysicalAddress SourceMacAddress
        {
            get => this.sourceAddress_.IsEmpty ? PhysicalAddress.None: new PhysicalAddress(sourceAddress_.ToByteArray());
            set => this.sourceAddress_ = Google.Protobuf.ByteString.CopyFrom(value.GetAddressBytes());
        }
        public PhysicalAddress DestinationMacAddress
        {
            get => this.destinationAddress_.IsEmpty ? PhysicalAddress.None : new PhysicalAddress(destinationAddress_.ToByteArray());
            set => this.destinationAddress_ = Google.Protobuf.ByteString.CopyFrom(value.GetAddressBytes());
        }
        public IPEndPoint SourceEndpoint
        {
            get => this.sourceAddress_.IsEmpty ? new IPEndPoint(IPAddress.None, 0) : new PhysicalAddress(destinationAddress_.ToByteArray());
            set => this.destinationAddress_ = Google.Protobuf.ByteString.CopyFrom(value.GetAddressBytes());
        }


        public IpProtocolType IpProtocol
        {
            get => this.protocol_.IsEmpty ? IpProtocolType.None : (IpProtocolType)(BitConverter.ToInt32(protocol_.ToByteArray(),0));
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
    }
}
