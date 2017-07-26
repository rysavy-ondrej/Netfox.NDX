﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ndx.Ingest;
using Ndx.Model;

namespace Ndx.Shell.Console
{
    /// <summary>
    /// This class helps in creating filter predicate.
    /// </summary>
    public static class Filter
    {
        public static Func<FlowKey,bool> Address(string address)
        {
            var ip = IPAddress.Parse(address);
            return (FlowKey f) => ip.Equals(f.SourceIpAddress) || ip.Equals(f.DestinationIpAddress);
        }
        public static Func<FlowKey,bool> SourceAddress(string address)
        {
            var ip = IPAddress.Parse(address);
            return (FlowKey f) => ip.Equals(f.SourceIpAddress);
        }
        public static Func<FlowKey,bool> DestinationAddress(string address)
        {
            var ip = IPAddress.Parse(address);
            return (FlowKey f) => ip.Equals(f.DestinationIpAddress);
        }
        public static Func<FlowKey,bool> Port(int port)
        {
            return (FlowKey f) => port== f.SourcePort || port == f.DestinationPort;
        }
        public static Func<FlowKey,bool> SourcePort(int port)
        {
            return (FlowKey f) => port == f.SourcePort;
        }
        public static Func<FlowKey,bool> DestinationPort(int port)
        {
            return (FlowKey f) => port == f.DestinationPort;
        }
        public static Func<FlowKey,bool> Protocol(IpProtocolType protocol)
        {
            return (FlowKey f) => protocol == f.IpProtocol;
        }
        public static Func<FlowKey,bool> And(params Func<FlowKey,bool>[] preds)
        {
            return (FlowKey f) => preds.All(x => x(f));
        }
        public static Func<FlowKey,bool> Or(params Func<FlowKey,bool>[] preds)
        {
            return (FlowKey f) => preds.Any(x => x(f));
        }
        public static Func<FlowKey,bool> Not(Func<FlowKey,bool> pred)
        {
            return (FlowKey f) => pred(f) == false;
        }

        public static Func<RawFrame,bool> GetFrameFilter(this Func<FlowKey,bool> filter)
        {
            return new Func<RawFrame,bool>((RawFrame frame) =>
            {
                var flowKey = PacketAnalyzer.GetFlowKey(frame);
                return (flowKey != null && filter(flowKey));
            });
        }

        public static Func<Conversation, bool> GetConversationFilter(this Func<FlowKey, bool> filter)
        {
            return new Func<Conversation, bool>((Conversation conv) =>
            {
                return filter(conv.ConversationKey);
            });
        }
    }
}
