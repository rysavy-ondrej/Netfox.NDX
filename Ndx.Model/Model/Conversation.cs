using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Model
{
    public enum ConversationState { Active, Completed }

    public partial class Conversation
    {
        public class ReferenceComparer : IEqualityComparer<Conversation>
        {
            public bool Equals(Conversation x, Conversation y)
            {
                return Object.ReferenceEquals(x, y);
            }

            public int GetHashCode(Conversation obj)
            {
                return RuntimeHelpers.GetHashCode(obj);
            }
        }

        public class ValueComparer : IEqualityComparer<Conversation>
        {
            public bool Equals(Conversation x, Conversation y)
            {
                return x?.Equals(y)??false;
            }

            public int GetHashCode(Conversation obj)
            {
                return obj?.GetHashCode()??0;
            }
        }

        internal void Complete()
        {
            m_conversationState = ConversationState.Completed;
        }

        ConversationState m_conversationState = ConversationState.Active;
        public ConversationState State => m_conversationState;


        public static long PacketPointer(int source, int number)
        {
            return ((long)source << 32) + (long)number;
        }

        public static int PacketSource(long pointer)
        {
            return (int)((long)pointer >> 32);
        }

        public static int PacketNumber(long pointer)
        {
            return (int)(pointer & 0xffffffff);
        }
    
        /// <summary>
        /// Gets all packet pointers for the current conversation.
        /// </summary>
        public IEnumerable<long> Packets =>
            this.UpflowPackets.Union(this.DownflowPackets);

        public long FirstSeen =>
            Math.Min(this.Upflow.FirstSeen, this.Downflow.FirstSeen);

        public long LastSeen =>
            Math.Min(this.Upflow.LastSeen, this.Downflow.LastSeen);

        public int PacketCount =>
            this.Upflow.Packets + this.Downflow.Packets;

        public long Octets =>
            this.Upflow.Octets + this.Downflow.Octets;
    }
}
