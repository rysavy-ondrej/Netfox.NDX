//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Ndx.Model
{

    [DebuggerDisplay("[FlowKey: {Protocol}@{SourceIpAddress}:{SourcePort}->{DestinationIpAddress}:{DestinationPort}]")]
    public partial class FlowKey : IEquatable<FlowKey>
    {
        /// <summary>
        /// Creates a new flow key for the specified values.
        /// </summary>
        /// <param name="proto">Protocol type.</param>
        /// <param name="srcIp">Source IP address.</param>
        /// <param name="srcPort">Source port number.</param>
        /// <param name="dstIp">Destination IP address.</param>
        /// <param name="dstPort">Destination port number.</param>
        public FlowKey(AddressFamily family, IpProtocolType proto, IPAddress srcIp, ushort srcPort, IPAddress dstIp, ushort dstPort) : this()
        {
            this.addressFamily_ = family;
            this.protocol_ = proto;
            this.sourceAddress_ = Google.Protobuf.ByteString.CopyFrom(srcIp.GetAddressBytes());
            this.destinationAddress_ = Google.Protobuf.ByteString.CopyFrom(dstIp.GetAddressBytes());
            this.sourcePort_ = srcPort;
            this.destinationPort_ = dstPort;
        }

        public IPAddress SourceIpAddress
        {
            get => this.sourceAddress_.IsEmpty ? IPAddress.None : new IPAddress(this.sourceAddress_.ToByteArray());
            set => this.sourceAddress_ = Google.Protobuf.ByteString.CopyFrom(value.GetAddressBytes());
        }

        public IPAddress DestinationIpAddress
        {
            get => this.sourceAddress_.IsEmpty ? IPAddress.None : new IPAddress(this.destinationAddress_.ToByteArray());
            set => this.destinationAddress_ = Google.Protobuf.ByteString.CopyFrom(value.GetAddressBytes());
        }
        /// <summary>
        /// Creates an empty flow key. All key fields will have thair default values. 
        /// Default address family is IPv4.
        /// </summary>
        public FlowKey(AddressFamily af = AddressFamily.InterNetwork)
        {
            this.addressFamily_ = af;
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
    }
}
