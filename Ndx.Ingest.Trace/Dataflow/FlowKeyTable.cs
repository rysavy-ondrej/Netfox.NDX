using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Ingest.Trace
{
    /// <summary>
    /// Represents flow key table. This table is read from the file and 
    /// represented as <see cref="Dictionary{TKey, TValue}"/> in the memory for efficient access.
    /// </summary>
    public class FlowKeyTable : IEnumerable<KeyValuePair<FlowKey, FlowKeyTableEntry>>
    {
        private Dictionary<FlowKey, FlowKeyTableEntry> m_entries;

        public FlowKeyTable()
        {
            m_entries = new Dictionary<FlowKey, Trace.FlowKeyTableEntry>();
        }

        public FlowKeyTable(IEnumerable<FlowKeyTableEntry> entries)
        {
            m_entries = entries.ToDictionary(x => x.Key);
        }

        public IEnumerable<FlowKeyTableEntry> Entries => m_entries.Values;

        public IEnumerable<FlowKey> Keys => m_entries.Keys;

        public int Count => m_entries.Count;

        public IEnumerator<KeyValuePair<FlowKey, FlowKeyTableEntry>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<FlowKey, FlowKeyTableEntry>>)m_entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<FlowKey, FlowKeyTableEntry>>)m_entries).GetEnumerator();
        }
    }
}
