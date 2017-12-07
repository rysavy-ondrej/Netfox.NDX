using Ndx.Model;
using NLog;
using PacketDotNet;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Threading;

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
    /// See also: 
    /// * https://www.cisco.com/c/en/us/td/docs/ios/fnetflow/command/reference/fnf_book/fnf_01.html 
    /// * https://research.utwente.nl/files/6519120/tutorial.pdf
    /// </remarks>
    public class ConversationTracker<TSource> 
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Keeps the number of last conversation.
        /// </summary>
        private int m_lastConversationId;

        /// <summary>
        /// COnfigurable capacity of dictionary that stores conversations.
        /// </summary>
        private int m_initialConversationDictionaryCapacity = 1024;

        /// <summary>
        /// Number of conversations tracked by this object so far.
        /// </summary>
        int m_totalConversationCounter;

        /// <summary>
        /// A function used to get <see cref="FlowKey"/> for the input packet type.
        /// </summary>
        private Func<TSource, (FlowKey, FlowFlags)> m_getKeyFunc;

        /// <summary>
        /// A function used to update flow attributes from packet.
        /// </summary>
        private Func<TSource, FlowAttributes, long> m_updateFlowFunc;
        /// <summary>
        /// Used for synchronization.
        /// </summary>
        private object m_lockObject = new object();


        /// <summary>
        /// Object that is observer as well as observation object.
        /// </summary>
        Subject<Conversation> m_conversationSubject;

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
        /// Specifies the active flow timeout. Default is 1800s.
        /// </summary>
        public TimeSpan TimeOutActive { get; set; }

        /// <summary>
        /// Specifies the active flow timeout. Default is 1800s.
        /// </summary>
        public TimeSpan TimeOutInactive { get; set; }

        /// <summary>
        /// Creates a new instance of conversation tracker. 
        /// </summary>
        public ConversationTracker(Func<TSource, (FlowKey,FlowFlags)> getKey, Func<TSource, FlowAttributes, long> updateFlow)
        {
            m_getKeyFunc = getKey;
            m_updateFlowFunc = updateFlow;
            m_activeConversations = new Dictionary<FlowKey, Conversation>(m_initialConversationDictionaryCapacity);
            m_conversationSubject = new Subject<Conversation>();
        }

        public ConversationTracker(IFlowHelper<TSource> helper)
        {
            m_getKeyFunc = helper.GetFlowKey;
            m_updateFlowFunc = helper.UpdateConversation;
        }

        /// <summary>
        /// This method is called for each frame and it updates the frame's conversation and labels the frame with <see cref="Frame.ConversationId"/>.
        /// </summary>
        /// <param name="frame">Frame to be processed.</param>
        /// <returns>Conversation object that owns the input frame.</returns>
        public Conversation ProcessRecord(TSource record)
        {
            if (record == null) return null;
            try
            {                
                (FlowKey flowkey, FlowFlags flowFlags) = m_getKeyFunc(record);
               
                if (flowFlags.HasFlag(FlowFlags.StartNewConversation))
                {
                    
                    var conversation = CreateNetworkConversation(flowkey, 0, out var flowAttributes, out var flowPackets, out var flowDirection);
                    flowPackets.Add(m_updateFlowFunc(record, flowAttributes));                    
                    return conversation;
                }
                else
                {
                    var conversation = GetNetworkConversation(flowkey, 0, out var flowAttributes, out var flowPackets, out var flowDirection);
                    flowPackets.Add(m_updateFlowFunc(record, flowAttributes));
                    return conversation;
                }
            }
            catch (Exception e)
            {
                m_logger.Error($"AcceptMessage: Error when processing record {record}: {e}. Frame is ignored.");
                return null;
            }
        }


        /// <summary>
        /// This method causes that all active conversations will be completed.
        /// </summary>
        public void Complete()
        {
            var conversations = m_activeConversations;
            m_activeConversations = null;
            foreach(var conversation in conversations)
            {
                SendConversation(conversation.Value);
            }
            SendCompleted();
        }

        /// <summary>
        /// Forces the <see cref="ConversationTracker"/> to flush flow cache. 
        /// </summary>
        public void Flush()
        {
            var conversations = m_activeConversations;
            m_activeConversations = new Dictionary<FlowKey, Conversation>(m_initialConversationDictionaryCapacity);
            foreach (var conversation in conversations)
            {
                SendConversation(conversation.Value);
            }
        }

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
            RemoveConversations(m_activeConversations, flowKey, parentConversationId);
            return GetNetworkConversation(flowKey, parentConversationId, out flowAttributes, out flowPackets, out flowOrientation);
        }

        private void RemoveConversations(Dictionary<FlowKey, Conversation> dictionary, FlowKey flowKey, int parentConversationId)
        {
            lock (m_lockObject)
            {
                if (dictionary.TryGetValue(flowKey, out var conversation))
                {
                    dictionary.Remove(flowKey);
                    SendConversation(conversation);
                }
                else
                if (dictionary.TryGetValue(flowKey.Swap(), out conversation))
                {
                    dictionary.Remove(flowKey.Swap());
                    SendConversation(conversation); 
                }
            }
        }


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
        /// Gets the next available conversation ID value.
        /// </summary>
        /// <returns></returns>
        internal int GetNewConversationId()
        {
            return Interlocked.Increment(ref m_lastConversationId);
        }


        private void SendConversation(Conversation conversation)
        {
                m_conversationSubject.OnNext(conversation);

        }

        private void SendCompleted()
        {
                m_conversationSubject.OnCompleted();
        }


        public IObservable<Conversation> Conversations => m_conversationSubject;

        public int TotalConversations => m_totalConversationCounter;
        public int ActiveConversations => m_activeConversations.Count;
    }
}