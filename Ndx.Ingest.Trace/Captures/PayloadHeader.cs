//  
// Copyright (c) BRNO UNIVERSITY OF TECHNOLOGY. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//

namespace Ndx.Ingest.Trace
{
    /// <summary>
    ///  This class represents a PayloadHeader frame. Network Monitor prepends the metadata to reassembled frames.
    /// </summary>
    class PayloadHeader
    {
        /// <summary>
        /// Must be 0x0200.
        /// </summary>
        ushort version;
        /// <summary>
        /// The length of the reassembly header.
        /// </summary>
        ushort headerLength;
        /// <summary>
        /// Must be 0x1.
        /// </summary>
        byte payload;

        /// <summary>
        /// The name of the protocol being reassembled.
        /// </summary>
        string reassembledProtocolName;

        /// <summary>
        /// The result of, or reason for, the reassembly operation. This is defined in payload.npl::ReassemblyStatusStringTable.
        /// </summary>
        uint reassemblyStatus;

        /// <summary>
        /// The number of ReassembledProtocolInfo structures in the next block.
        /// </summary>
        byte lowerProtocolCount;

        /// <summary>
        /// An array of ReassembledProtocolInfo structures.
        /// </summary>
        ReassembledProtocolInfo[] reassembledProtocolInfoBlock;

        /// <summary>
        /// The number of fragments that have been reassembled.
        /// </summary>
        ushort frameCount;

        /// <summary>
        /// The size, in bytes, of the protocol data following this header.
        /// </summary>
        uint payloadLength;
    }

    /// <summary>
    /// Conversation key information for a reassembled protocol.
    /// </summary>
    public class ReassembledProtocolInfo
    {
        /// <summary>
        /// The length of the conversation key for the protocol.
        /// </summary>
        uint conversationKeyLength;
        /// <summary>
        /// The conversation key for this protocol. This is a byte array.
        /// </summary>
        byte[] conversationKey;
        /// <summary>
        /// The length of the property block array specified in NPL and constructed by the reassembly plug-in.
        /// </summary>
        ushort propertyBlockLength;
    }
}
