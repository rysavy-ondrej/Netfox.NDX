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
using NLog;

namespace Ndx.Ingest
{
    /// <summary>
    /// This class tracks the conversations at all supported levels.
    /// </summary>
    public class ConversationTracker
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        private int m_lastConversationId;
        private int m_initialConversationDictionaryCapacity = 1024;
        ActionBlock<RawFrame> m_frameAnalyzer;
        BufferBlock<KeyValuePair<Conversation, MetaFrame>> m_metaframeOutput;

        int m_totalConversationCounter;

        /// <summary>
        /// Creates a new instance of conversation tracker. 
        /// </summary>
        public ConversationTracker(int maxDegreeOfParallelism = 1)
        {
            m_frameAnalyzer = new ActionBlock<RawFrame>(AcceptFrame, new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = maxDegreeOfParallelism });
            m_metaframeOutput = new BufferBlock<KeyValuePair<Conversation, MetaFrame>>();
            m_conversations = new Dictionary<FlowKey, Conversation>(m_initialConversationDictionaryCapacity);
            m_frameAnalyzer.Completion.ContinueWith(t => m_metaframeOutput.Complete());
        }

        async Task AcceptFrame(RawFrame rawframe)
        {
            if (rawframe == null)
            {
                return;
            }

            if (rawframe.LinkType == DataLinkType.Ethernet)
            {   
                var analyzer = new PacketAnalyzer(this, rawframe);
                try
                {
                    var packet = Packet.ParsePacket(LinkLayers.Ethernet, rawframe.Data.ToByteArray());
                    packet.Accept(analyzer);
                    ;
                    await m_metaframeOutput.SendAsync(new KeyValuePair<Conversation, MetaFrame>(analyzer.Conversation, analyzer.MetaFrame));
                }
                catch(Exception e)
                {
                    m_logger.Error($"AcceptMessage: Error when processing frame {rawframe.FrameNumber}: {e}. Frame is ignored.");
                }
            }
            else
            {
                m_logger.Warn($"AcceptMessage: {rawframe.LinkType} is not supported. Frame is ignored.");
            }
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

        private object m_lockObject = new object();
        private Conversation GetConversation(Dictionary<FlowKey, Conversation> dictionary, FlowKey flowKey, int parentConversationId, out FlowAttributes flowAttributes, out IList<int> flowPackets, out FlowOrientation flowOrientation)
        {
            lock (m_lockObject)
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
                            Upflow = new FlowAttributes() { FirstSeen = Int64.MaxValue, LastSeen = Int64.MinValue },
                            Downflow = new FlowAttributes() { FirstSeen = Int64.MaxValue, LastSeen = Int64.MinValue },
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
        }

        /// <summary>
        /// Gets the data flow target block that accepts <see cref="RawFrame"/>.
        /// </summary>
        public ActionBlock<RawFrame> Input { get => m_frameAnalyzer; }
        /// <summary>
        /// Gets the data flow source block that provides <see cref="MetaFrame"/> objects.
        /// </summary>
        public ISourceBlock<KeyValuePair<Conversation,MetaFrame>> Output { get => m_metaframeOutput; }
        /// <summary>
        /// Gets the next available conversation ID value.
        /// </summary>
        /// <returns></returns>
        internal int GetNewConversationId()
        {
            return Interlocked.Increment(ref m_lastConversationId);
        }

        /// <summary>
        /// Gets the <see cref="Taks"/> that complete when all data were processed. 
        /// </summary>
        public Task Completion => Task.WhenAll(m_metaframeOutput.Completion);

        public int TotalConversations => m_totalConversationCounter;
        public int ActiveConversations => m_conversations.Count;

    }
}