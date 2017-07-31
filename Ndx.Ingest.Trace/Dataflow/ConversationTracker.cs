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
    /// This class tracks the conversation at the transport layer. Communication that has not transport layer (UDP or TCP) is simply ignored. 
    /// </summary>
    /// <remarks>          
    /// Conversation trancker uses the following rules for expiring records from the cache entries:
    /// <list type="bullet">
    /// <item>
    /// <term>
    /// Inactive time
    /// </term>
    /// <description>
    /// Conversations that have been idle for a specified time are expired and removed from the cache for export.The inactive timer has a default setting of 15 seconds of traffic inactivity. The user can configure the inactive timer between 10 and 600 seconds.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// Active timer
    /// </term>
    /// <description>
    /// Long-lived flows are expired and removed from the cache.By default, flows are not allowed to live longer than 30 minutes, even if the underlying packet conversation remains active. The user can configure the active timer between 1 and 60 minutes.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// Cache Full
    /// </term>
    /// <description>
    /// If the cache reaches its maximum size, a number of heuristic expiry functions are applied to export flows faster to free up space for new entries.Note that in this case, the "free-up" function has a higher priority than the active and passive timers do!
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// Tcp Control
    /// </term>
    /// <description>
    /// TCP connections that have reached the end of byte stream (FIN) or that have been reset (RST).
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
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
            m_activeConversations = new Dictionary<FlowKey, Conversation>(m_initialConversationDictionaryCapacity, new FlowKey.ValueComparer());
            m_frameAnalyzer.Completion.ContinueWith(t =>
            {
                Console.WriteLine($"[INFO] ConversationTracker: FrameAnalyzer Completed, conversation cache contains {m_metaframeOutput.Count} items.");
                m_metaframeOutput.Complete();
                });
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

                    // Workaround the BUG in PacketDotNET:
                    if (packet.PayloadPacket != null) packet.PayloadPacket.ParentPacket = packet;

                    packet.Accept(analyzer);

                    if (analyzer.Conversation != null)
                    {
                        await m_metaframeOutput.SendAsync(new KeyValuePair<Conversation, MetaFrame>(analyzer.Conversation, analyzer.MetaFrame));
                    }
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
        Dictionary<FlowKey, Conversation> m_activeConversations;
        /// <summary>
        /// Gets or create a conversation for the specified <see cref="FlowKey"/>.
        /// </summary>
        /// <param name="flowKey">A flow key that is used to find the conversation.</param>
        /// <param name="parentConversationId">Parent conversation Id if used.</param>
        /// <param name="flowAttributes">Attributes for the flow that corresponds to the direction of the flow key.</param>
        /// <param name="flowPackets">Collection of flow packets that corresponds to the direction of the flow key.</param>
        /// <param name="flowOrientation">The flow orientation with respect to flow key.</param>
        /// <returns>Converdation instance that corresponds to the specified <see cref="FlowKey"/>.</returns>
        internal Conversation GetNetworkConversation(FlowKey flowKey, int parentConversationId, out FlowAttributes flowAttributes, out IList<long> flowPackets, out FlowOrientation flowOrientation)
        {
            return GetConversation(m_activeConversations, flowKey, parentConversationId, out flowAttributes, out flowPackets, out flowOrientation);
        }

        /// <summary>
        /// Gets a new conversation. If conversation for the given <paramref name="flowKey"/> exists a new conversation will be created.
        /// </summary>
        /// <param name="flowKey"></param>
        /// <param name="parentConversationId"></param>
        /// <param name="flowAttributes"></param>
        /// <param name="flowPackets"></param>
        /// <param name="flowOrientation"></param>
        /// <returns></returns>
        internal Conversation CreateNetworkConversation(FlowKey flowKey, int parentConversationId, out FlowAttributes flowAttributes, out IList<long> flowPackets, out FlowOrientation flowOrientation)
        {
            lock(m_lockObject)
            {
                m_activeConversations.Remove(flowKey);
            }
            return GetNetworkConversation(flowKey, parentConversationId, out flowAttributes, out flowPackets, out flowOrientation);
        }

        private object m_lockObject = new object();
        private Conversation GetConversation(Dictionary<FlowKey, Conversation> dictionary, FlowKey flowKey, int parentConversationId, out FlowAttributes flowAttributes, out IList<long> flowPackets, out FlowOrientation flowOrientation)
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
        /// Gets the data flow source block that provides <see cref="MetaFrame"/> objects. For each object also its <see cref="Conversation"/>
        /// is provided. The provided conversation object is still maintained by <see cref="ConversationTracker"/> and thus its properties 
        /// can be modified. Check the <see cref="Conversation.State"/> for determining the current state of the conversation.
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
        public Task Completion => m_metaframeOutput.Completion;

        public int TotalConversations => m_totalConversationCounter;
        public int ActiveConversations => m_activeConversations.Count;

    }
}