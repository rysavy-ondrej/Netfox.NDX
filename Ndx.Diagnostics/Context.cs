using System;
using System.Collections.Generic;
using Ndx.Model;

namespace Ndx.Diagnostics
{
    public class Context 
    {
        private Rule m_rule;
        private IDictionary<string,PacketFields> m_objects;
        private IEnumerable<PacketFields> m_input;
        public Context(Rule rule, IEnumerable<PacketFields> input, IDictionary<string,PacketFields> objects)
        {
            m_rule = rule;
            m_input = input;
            m_objects = objects;
        }

        public PacketFields this[string name]
        {
            get
            {
                if (m_objects.TryGetValue(name, out PacketFields value))
                {
                    return value;
                }
                else
                {
                    return PacketFields.Empty;
                }
            }
        }

        public Rule Rule => m_rule;
        public IEnumerable<PacketFields> Input { get => m_input; set => m_input = value; }

        /// <summary>
        /// Represents left [from-to]~> right temporal operator.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public bool LeadsTo(TimeSpan from, TimeSpan to, PacketFields left, PacketFields right)
        {           
            if (left.IsEmpty) return true;
            return
                !right.IsEmpty
                && left.DateTime + from <= right.DateTime
                && right.DateTime <= left.DateTime + to;
        }
        /// <summary>
        /// Represents left [from-to]~!> right temporal operator.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public bool NotLeadsTo(TimeSpan from, TimeSpan to, PacketFields left, PacketFields right)
        {
            if (left.IsEmpty) return true;
            if (right.IsEmpty) return true;
            return (left.DateTime + from <= right.DateTime
                   && right.DateTime <= left.DateTime + to) == false;
        }

    }
}
