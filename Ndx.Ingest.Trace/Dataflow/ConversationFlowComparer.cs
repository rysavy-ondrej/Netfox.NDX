﻿using System;
using System.Collections;
using System.Collections.Generic;
using Ndx.Utils;

namespace Ndx.Ingest.Trace
{
    internal class ConversationFlowComparer : IEqualityComparer<FlowKey>
    {
        public ConversationFlowComparer() 
        {
        }

        public bool Equals(FlowKey x, FlowKey y)
        {
            return FlowKey.Equals(x, y) || 
                       x.Protocol == y.Protocol
                    && IPAddressComparer.Equals(x.SourceAddress, y.DestinationAddress)
                    && IPAddressComparer.Equals(x.DestinationAddress, y.SourceAddress)
                    && x.SourcePort == y.DestinationPort
                    && x.DestinationPort == y.SourcePort;
        }

        public int GetHashCode(FlowKey obj)
        {
            return obj.Protocol.GetHashCode()
                ^ obj.SourceAddress.GetHashCode()
                ^ obj.SourcePort.GetHashCode()
                ^ obj.DestinationAddress.GetHashCode()
                ^ obj.DestinationPort.GetHashCode();
        }
    }
}