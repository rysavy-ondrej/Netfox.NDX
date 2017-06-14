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
using System.Threading;
using PacketDotNet;

namespace Ndx.Ingest
{
    /// <summary>
    /// This class tracks the conversations at all supported levels.
    /// </summary>
    public class ConversationTracker
    {
        private int m_lastConversationId;
        private int m_initialConversationDictionaryCapacity = 1024;
        ActionBlock<RawFrame> m_frameAnalyzer;
        BufferBlock<Conversation> m_conversationBuffer;
        BufferBlock<MetaFrame> m_metaframeBuffer;

        int m_totalConversationCounter;

        /// <summary>
        /// Creates a new instance of conversation tracker. 
        /// </summary>
        /// <param name="metacap"></param>
        public ConversationTracker()
        {
            m_frameAnalyzer = new ActionBlock<RawFrame>((Action<RawFrame>)AcceptFrame);
            m_conversationBuffer = new BufferBlock<Conversation>();
            m_metaframeBuffer = new BufferBlock<MetaFrame>();
            m_conversations = new Dictionary<FlowKey, Conversation>(m_initialConversationDictionaryCapacity);
            m_frameAnalyzer.Completion.ContinueWith(async t => await CompleteAsync());
        }

        void AcceptFrame(RawFrame rawframe)
        {
            var analyzer = new PacketAnalyzer(this, rawframe);
            var packet = Packet.ParsePacket((LinkLayers)rawframe.LinkType, rawframe.Data.ToByteArray());
            packet.Accept(analyzer);
            m_metaframeBuffer.Post(analyzer.MetaFrame);    
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
        Dictionary<FlowKey, Conversation> m_conversations;
        public Conversation GetNetworkConversation(FlowKey flowKey, int parentConversationId, out FlowAttributes flowAttributes, out IList<int> flowPackets, out FlowOrientation flowOrientation)
        {
            return GetConversation(m_conversations, flowKey, parentConversationId, out flowAttributes, out flowPackets, out flowOrientation);
        }

        private Conversation GetConversation(Dictionary<FlowKey, Conversation> dictionary, FlowKey flowKey, int parentConversationId, out FlowAttributes flowAttributes, out IList<int> flowPackets, out FlowOrientation flowOrientation)
        {
            if (!dictionary.TryGetValue(flowKey, out var conversation))
            {
                if (!dictionary.TryGetValue(flowKey.Swap(), out conversation))
                {
                    // create new conversation object
                    conversation = new Conversation()
                    {
                        ParentId = parentConversationId,
                        ConversationId = GetNewConversationId(),
                        ConversationKey = flowKey,
                        Upflow = new FlowAttributes(),
                        Downflow = new FlowAttributes()
                    };
                    dictionary.Add(flowKey, conversation);
                    m_totalConversationCounter++;
                }
                else
                {
                    flowOrientation = FlowOrientation.Downflow;
                    flowAttributes = conversation.Downflow;
                    flowPackets = conversation.DownflowPackets;
                    return conversation;
                }
            }
            flowOrientation = FlowOrientation.Upflow;
            flowAttributes = conversation.Upflow;
            flowPackets = conversation.UpflowPackets;
            
            return conversation;
        }

        /// <summary>
        /// Gets the data flow target block that accepts <see cref="RawFrame"/>.
        /// </summary>
        public ActionBlock<RawFrame> FrameAnalyzer { get => m_frameAnalyzer; }
        /// <summary>
        /// Gets the data flow source block that provides <see cref="Conversation"/> objects.
        /// </summary>
        public BufferBlock<Conversation> ConversationBuffer { get => m_conversationBuffer; }
        /// <summary>
        /// Gets the data flow source block that provides <see cref="MetaFrame"/> objects.
        /// </summary>
        public BufferBlock<MetaFrame> MetaframeBuffer { get => m_metaframeBuffer; }
        /// <summary>
        /// Gets the next available conversation ID value.
        /// </summary>
        /// <returns></returns>
        internal int GetNewConversationId()
        {
            return Interlocked.Increment(ref m_lastConversationId);
        }

        /// <summary>
        /// Causes that the conversation tracker completes as soon as possible. 
        /// It flushes the content of the conversation dictionary to output and 
        /// mark both outputs as completed.
        /// </summary>
        /// <returns></returns>
        public async Task CompleteAsync()
        {
            foreach(var c in m_conversations)
            {
                await m_conversationBuffer.SendAsync(c.Value);    
            }
            m_metaframeBuffer.Complete();
            m_conversationBuffer.Complete();
        }

        /// <summary>
        /// Causes that the conversation tracker complete. It returns when all 
        /// activities complete.
        /// </summary>
        public void Complete()
        {
            var t = CompleteAsync();
            Task.WaitAll(t, Completion);
        }

        /// <summary>
        /// Gets the <see cref="Taks"/> that complete when all data were processed. 
        /// </summary>
        public Task Completion => Task.WhenAll(m_metaframeBuffer.Completion, m_conversationBuffer.Completion);

        public int TotalConversations => m_totalConversationCounter;
        public int ActiveConversations => m_conversations.Count;

    }
}