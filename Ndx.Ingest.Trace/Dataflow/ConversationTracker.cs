using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using Ndx.Metacap;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks.Dataflow;

namespace Ndx.Metacap.Dataflow
{
    class ConversationTracker
    {
        Metacap m_metacap;
        ActionBlock<RawFrame> m_frameAnalyzer;
        BufferBlock<PacketBlock> m_packetBlockBuffer;

        public ConversationTracker(Metacap metacap)
        {
            m_metacap = metacap;
        }

        /// <summary>
        /// Stores conversation at datalink level. The key is represented as
        /// (EthernetProtocolType, PhysicalAddress, Selector, PhysicalAddress, Selector)
        /// </summary>
        /// <remarks>
        /// Conversation at the datalink level is created for non-ip communication. Examples
        /// of such communication are ARP, BPDU, CDP, LLDP, etc.
        /// </remarks>
        Dictionary<FlowKey, Conversation> m_datalinkConversation;
        public Conversation GetDatalinkConversation(FlowKey flowKey, out FlowAttributes flowAttributes, out IList<int> flowPackets, out FlowOrientation flowOrientation)
        {
            return GetConversation(m_datalinkConversation, flowKey, 0, out flowAttributes, out flowPackets, out flowOrientation);
        }

        /// <summary>
        /// Stores conversation at network level. The key is represented as
        /// (IpProtocolType, IpAddress, Selector, IpAddress, Selector)
        /// </summary>
        /// <remarks>
        /// All IP based communication can have a conversation at this level. The selector 
        /// depends on the protocol encapsulated in IP packet, for instance, port numbers, 
        /// ICMP type and code, etc.
        /// </remarks>
        Dictionary<FlowKey, Conversation> m_networkConversation;
        public Conversation GetNetworkConversation(FlowKey flowKey, int parentConversationId, out FlowAttributes flowAttributes, out IList<int> flowPackets, out FlowOrientation flowOrientation)
        {
            return GetConversation(m_networkConversation, flowKey, parentConversationId, out flowAttributes, out flowPackets, out flowOrientation);
        }

        private Conversation GetConversation(Dictionary<FlowKey, Conversation> dictionary, FlowKey flowKey, int parentConversationId, out FlowAttributes flowAttributes, out IList<int> flowPackets, out FlowOrientation flowOrientation)
        {
            if (!dictionary.TryGetValue(flowKey, out var conversation))
            {
                if (!dictionary.TryGetValue(flowKey.Swap(), out conversation))
                {
                    conversation = new Conversation()
                    {
                        ParentId = parentConversationId,
                        ConversationId = GetNewConversationId(),
                        ConversationKey = flowKey
                    };
                }
                else
                {
                    flowOrientation = FlowOrientation.Downflow;
                    flowAttributes = conversation.Downflow;
                    flowPackets = conversation.DownflowPackets;
                }
            }
            flowOrientation = FlowOrientation.Upflow;
            flowAttributes = conversation.Upflow;
            flowPackets = conversation.UpflowPackets;
            
            return conversation;
        }

        private int GetNewConversationId()
        {
            throw new NotImplementedException();
        }
    }
}