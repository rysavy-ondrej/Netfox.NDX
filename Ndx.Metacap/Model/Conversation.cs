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
    }
}
