using Ndx.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Ndx.Ipflow
{
    [Flags]
    public enum ConversationCloseFlags { None = 0, TcpFin = 1, Timeout = 2, Forced = 4 }
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
    public class ConversationTracker<TSource> : ISubject<TSource, Tuple<Conversation,TSource>>
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
        /// Stores conversation at network level. The key is represented as
        /// (IpProtocolType, IpAddress, Selector, IpAddress, Selector)
        /// </summary>
        /// <remarks>
        /// All IP based communication can have a conversation at this level. The selector 
        /// depends on the protocol encapsulated in IP packet, for instance, port numbers, 
        /// ICMP type and code, etc.
        /// </remarks>
        private Dictionary<FlowKey, Conversation> m_openConversations;

        /// <summary>
        /// Subject that provides observable of closed conversations.
        /// </summary>
        private ISubject<Tuple<Conversation, ConversationCloseFlags>> m_closedConversations;
        /// <summary>
        /// Gets the observable collection of closed conversations.
        /// </summary>
        public IObservable<Tuple<Conversation, ConversationCloseFlags>> ClosedConversations => m_closedConversations;
       
        /// <summary>
        /// Specifies the active flow timeout. Default is 1800s.
        /// </summary>
        public TimeSpan TimeOutActive { get; set; }

        /// <summary>
        /// Specifies the active flow timeout. Default is 1800s.
        /// </summary>
        public TimeSpan TimeOutInactive { get; set; }


        /// <summary>
        /// Gets the total number of processed conversations.
        /// </summary>
        public int TotalConversations => m_totalConversationCounter;

        /// <summary>
        /// Gets the number of active conversations.
        /// </summary>
        public int ActiveConversations => m_openConversations.Count;



        /// <summary>
        /// Initializes private memebrs of the newly created instance.
        /// </summary>
        private ConversationTracker()
        {
            m_openConversations = new Dictionary<FlowKey, Conversation>(m_initialConversationDictionaryCapacity);
            m_closedConversations = new Subject<Tuple<Conversation, ConversationCloseFlags>>();
        }

        /// <summary>
        /// Creates a new instance of conversation tracker. 
        /// </summary>
        public ConversationTracker(Func<TSource, (FlowKey,FlowFlags)> getKey, Func<TSource, FlowAttributes, long> updateFlow) : this()
        {
            m_getKeyFunc = getKey;
            m_updateFlowFunc = updateFlow;
        }

        /// <summary>
        /// Creates a new instance of conversation tracker using provided flow helper. 
        /// </summary>
        /// <param name="helper">The flow helper instance.</param>
        public ConversationTracker(IFlowHelper<TSource> helper) : this()
        {
            m_getKeyFunc = helper.GetFlowKey;
            m_updateFlowFunc = helper.UpdateConversation;
        }

        /// <summary>
        /// This method is called for each frame and it updates the frame's conversation and labels the frame with <see cref="Frame.ConversationId"/>.
        /// </summary>
        /// <param name="frame">Frame to be processed.</param>
        /// <returns>Conversation object that owns the input frame.</returns>
        Tuple<Conversation,TSource> ProcessRecord(TSource record)
        {
            if (record == null) return null;
            try
            {                
                (FlowKey flowkey, FlowFlags flowFlags) = m_getKeyFunc(record);
               
                if (flowFlags.HasFlag(FlowFlags.StartNewConversation))
                {
                    
                    var conversation = CreateNetworkConversation(flowkey, 0, out var flowAttributes, out var flowPackets, out var flowDirection);
                    flowPackets.Add(m_updateFlowFunc(record, flowAttributes));                    
                    return Tuple.Create(conversation,record);
                }
                else
                {
                    var conversation = GetNetworkConversation(flowkey, 0, out var flowAttributes, out var flowPackets, out var flowDirection);
                    flowPackets.Add(m_updateFlowFunc(record, flowAttributes));
                    return Tuple.Create(conversation,record);
                }
            }
            catch (Exception e)
            {
                m_logger.Error($"AcceptMessage: Error when processing record {record}: {e}. Frame is ignored.");
                return null;
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
            return GetConversation(m_openConversations, flowKey, parentConversationId, out flowAttributes, out flowPackets, out flowOrientation);
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
            var conversation = RemoveConversation(m_openConversations, flowKey, parentConversationId);
            if (conversation != null) ConversationClose(conversation, ConversationCloseFlags.Forced);
            return GetNetworkConversation(flowKey, parentConversationId, out flowAttributes, out flowPackets, out flowOrientation);
        }

        private Conversation RemoveConversation(Dictionary<FlowKey, Conversation> dictionary, FlowKey flowKey, int parentConversationId)
        {
            lock (m_lockObject)
            {
                if (dictionary.TryGetValue(flowKey, out var conversation))
                {
                    dictionary.Remove(flowKey);
                    return conversation;
                }
                else
                if (dictionary.TryGetValue(flowKey.Swap(), out conversation))
                {
                    dictionary.Remove(flowKey.Swap());
                    return conversation;
                }
                return null;
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

        #region Private Helper Methods
        /// <summary>
        /// This method causes that all active conversations will be completed.
        /// </summary>
        private void CloseAllConversations()
        {
            var conversations = m_openConversations;
            m_openConversations = null;
            foreach (var conversation in conversations)
            {
                m_closedConversations.OnNext(Tuple.Create(conversation.Value, ConversationCloseFlags.Forced));
            }
        }
        /// <summary>
        /// Informs observers that the conversation was closed.
        /// </summary>
        /// <param name="conversation">The closed conversation.</param>
        /// <param name="flag">The reason for closing.</param>
        private void ConversationClose(Conversation conversation, ConversationCloseFlags flag)
        {
            m_closedConversations.OnNext(Tuple.Create(conversation, ConversationCloseFlags.Forced));
        }
        #endregion

        #region Implementation of ISubject<> interface
        private List<IObserver<Tuple<Conversation,TSource>>> m_observerList = new List<IObserver<Tuple<Conversation, TSource>>>();
        private bool m_isDisposed;
        private bool m_isStopped;
        object m_gate = new object();
        Exception m_exception;


        public void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentException("Exception error should not be null.");
            lock (m_gate)
            {
                CheckDisposed();

                if (!m_isStopped)
                {
                    m_exception = error;

                    foreach (var observer in m_observerList)
                    {
                        observer.OnError(error);
                    }

                    m_observerList.Clear();
                    m_isStopped = true;
                }
            }

            m_logger.Error(error, "ConversationTracker.OnError");
        }

        public void OnCompleted()
        {
            lock (m_gate)
            {
                CheckDisposed();

                if (!m_isStopped)
                {
                    CloseAllConversations();
                    m_closedConversations.OnCompleted();
                    var observerList = m_observerList.ToArray();
                    foreach (var observer in observerList)
                    {
                        observer.OnCompleted();
                    }

                    m_observerList.Clear();
                    m_isStopped = true;
                }
            }
           
        }


        public void OnNext(TSource value)
        {
            //****************************************************************************************//
            //*** Make sure the OnNext operation is not preempted by another operation which       ***//
            //*** would break the expected behavior.  For example, don't allow unsubscribe, errors ***//
            //*** or an OnCompleted operation to preempt OnNext from another thread. This would    ***//
            //*** have the result of items in a sequence following completion, errors, or          ***//
            //*** unsubscribe.  That would be an incorrect behavior.                               ***//
            //****************************************************************************************//

            lock (m_gate)
            {
                CheckDisposed();

                if (!m_isStopped)
                {

                    var result = ProcessRecord(value);
                    if (result != null)
                    {
                        foreach (var observer in m_observerList)
                        {
                            observer.OnNext(result);
                        }
                    }
                }
            }
        }

        public IDisposable Subscribe(IObserver<Tuple<Conversation, TSource>> observer)
        {
            if (observer == null)
                throw new ArgumentException("observer should not BehaviorSubject null.");

            //****************************************************************************************//
            //*** Make sure Subscribe occurs in sync with the other operations so we keep the      ***//
            //*** correct behavior depending on whether an error has occurred or the observable    ***//
            //*** sequence has completed.                                                          ***//
            //****************************************************************************************//

            lock (m_gate)
            {
                CheckDisposed();

                if (!m_isStopped)
                {
                    m_observerList.Add(observer);
                    return new Subscription(observer, this);
                }
                else if (m_exception != null)
                {
                    observer.OnError(m_exception);
                    return Disposable.Empty;
                }
                else
                {
                    observer.OnCompleted();
                    return Disposable.Empty;
                }
            }
        }

        private void Unsubscribe(IObserver<Tuple<Conversation, TSource>> observer)
        {
            //****************************************************************************************//
            //*** Make sure Unsubscribe occurs in sync with the other operations so we keep the    ***//
            //*** correct behavior.                                                                ***//
            //****************************************************************************************//

            lock (m_gate)
            {
                m_observerList.Remove(observer);
            }
        }

        public void Dispose()
        {
            //****************************************************************************************//
            //*** Make sure Dispose occurs in sync with the other operations so we keep the        ***//
            //*** correct behavior. For example, Dispose shouldn't preempt the other operations    ***//
            //*** changing state variables after they have been checked.                           ***//
            //****************************************************************************************//

            lock (m_gate)
            {
                m_observerList.Clear();
                m_isStopped = true;
                m_isDisposed = true;
            }
        }

        private void CheckDisposed()
        {
            if (m_isDisposed)
                throw new ObjectDisposedException("Subject has been disposed.");
        }

        //************************************************************************************//
        //***                                                                              ***//
        //*** The Subscription class wraps each observer that creates a subscription. This ***//
        //*** is needed to expose an IDisposable interface through which a observer can    ***//
        //*** cancel the subscription.                                                     ***//
        //***                                                                              ***//
        //************************************************************************************//
        class Subscription : IDisposable
        {
            private ConversationTracker<TSource> m_subject;
            private IObserver<Tuple<Conversation, TSource>> m_observer;

            public Subscription(IObserver<Tuple<Conversation, TSource>> observer, ConversationTracker<TSource> subject)
            {
                m_subject = subject;
                m_observer = observer;
            }

            public void Dispose()
            {
                m_subject.Unsubscribe(m_observer);
            }
        }
        #endregion
    }
}