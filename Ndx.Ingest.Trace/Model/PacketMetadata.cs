//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
using System;
using System.Runtime.InteropServices;
using Ndx.Utils;

namespace Ndx.Metacap
{
    /// <summary>
    /// Represents a collection of extracted fields of the parsed packet.
    /// </summary>
    public class PacketMetadata
    {
        public const int MetadataSize = _PacketMetadata.__size;

        private FlowKey m_flowkey;
        private _PacketMetadata m_metadata;
        private byte[] value;
        private int v;

        /// <summary>
        /// Gets a flow key for the current <see cref="PacketMetadata"/>.
        /// </summary>
        public FlowKey Flow => m_flowkey;

        public FrameMetadata Frame => new FrameMetadata(m_metadata.frame);

        public _ByteRange Link => m_metadata.link;

        public _ByteRange Network => m_metadata.network;

        public _ByteRange Transport => m_metadata.transport;

        public _ByteRange Payload => m_metadata.payload;

        public void SetLink(int start, int length) { m_metadata.link = new _ByteRange(start, length); }

        public void SetNetwork(int start, int length) { m_metadata.network = new _ByteRange(start, length); }

        public void SetTransport(int start, int length) { m_metadata.transport = new _ByteRange(start, length); }

        public void SetPayload(int start, int length) { m_metadata.payload = new _ByteRange(start, length); }

        public void SetFrameMetadata(FrameMetadata meta)
        {
            m_metadata.frame = meta.Data;
        }

        public PacketMetadata()
        {
            m_flowkey = new FlowKey();
            m_metadata = new _PacketMetadata();
        }

        internal PacketMetadata(FlowKey flowKey, _PacketMetadata metadata)
        {
            this.m_flowkey = flowKey;
            this.m_metadata = metadata;
        }

        public PacketMetadata(FrameMetadata frameMeta)
        {
            m_flowkey = new FlowKey();
            m_metadata = new _PacketMetadata() { frame = frameMeta.Data };
        }

        public PacketMetadata(byte[] bytes, int start)
        {
            this.m_metadata = new _PacketMetadata(bytes, start);
                
        }

        internal _PacketMetadata Data => m_metadata;

        public byte[] GetBytes() => m_metadata.GetBytes();
    }
    [StructLayout(LayoutKind.Explicit, Size = __size)]
    internal unsafe struct _PacketMetadata
    {
        internal const int __size = _FrameMetadata.__size + _ByteRange.__size * 4;
        [FieldOffset(0)] public _FrameMetadata frame;
        [FieldOffset(_FrameMetadata.__size)] public _ByteRange link;
        [FieldOffset(_FrameMetadata.__size + _ByteRange.__size)] public _ByteRange network;
        [FieldOffset(_FrameMetadata.__size + _ByteRange.__size * 2)] public _ByteRange transport;
        [FieldOffset(_FrameMetadata.__size + _ByteRange.__size * 3)] public _ByteRange payload;

        internal byte[] GetBytes()
        {
            return ExplicitStruct.GetBytes<_PacketMetadata>(this);
        }
        /// <summary>
        /// Creates the flow key from the provided bytes.
        /// </summary>
        /// <param name="bytes"></param>
        public _PacketMetadata(byte[] bytes, int offset = 0)
        {
            if (bytes.Length + offset < __size)
            {
                throw new ArgumentException($"Not enough bytes for intialization of {nameof(_PacketMetadata)} instance.");
            }

            fixed (byte* pdata = bytes)
            {
                this = *(_PacketMetadata*)(pdata + offset);
            }
        }
    }
}
