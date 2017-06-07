using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ndx.Model;

namespace Ndx.Metacap
{
    public class ConversationTable : IEnumerable<KeyValuePair<int,ConversationTableEntry>>
    {
        private Dictionary<int, ConversationTableEntry> m_entries;

        public ConversationTable()
        {
            m_entries = new Dictionary<int, ConversationTableEntry>();
        }

        public ConversationTable(IEnumerable<ConversationTableEntry> entries)
        {
            m_entries = entries.ToDictionary(x => x.ConversationId);
        }

        public IEnumerable<ConversationTableEntry> Entries => m_entries.Values;

        public int Count => m_entries.Count;

        public IEnumerator<KeyValuePair<int, ConversationTableEntry>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<int, ConversationTableEntry>>)m_entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<int, ConversationTableEntry>>)m_entries).GetEnumerator();
        }
    }
}
