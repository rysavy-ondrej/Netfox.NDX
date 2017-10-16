﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Utils;

namespace Ndx.Model
{
    public partial class PacketFields
    {
        /// <summary>
        /// Gets the value of the field or provided defaultValue.
        /// </summary>
        /// <param name="field">The name of the field for which to get its value.</param>
        /// <param name="defaultValue">The default value if the field has not any value.</param>
        /// <returns>Value of the field or the provided default value.</returns>
        public string GetFieldValue(string field, string defaultValue)
        {
            return Fields.TryGetValue(field, out string value) ? value : defaultValue;
        }

        public string this[string name]
        {
            get
            {
                if (Fields.TryGetValue(name, out string value))
                {
                    return value;
                }
                else
                {
                    return String.Empty;
                }
            }
        }
        /// <summary>
        /// Get the timestamp as <see cref="DateTime"/> value.
        /// </summary>
        public DateTime DateTime => DateTime.FromBinary(this.Timestamp);

        static readonly PacketFields m_empty = new PacketFields();
        public static PacketFields Empty => m_empty;
        public bool IsEmpty => ReferenceEquals(this, m_empty);

        public static PacketFields FromFields(IDictionary<string, string> entries)
        {
            var pf = new PacketFields();
            pf.Fields.Add(entries);
            return pf;
        }

        /// <summary>
        /// Tests if the <see cref="PacketFields"/> object is null or equal to the Empty instance.
        /// </summary>
        /// <param name="pf">The <see cref="PacketFields"/> object to test.</param>
        /// <returns>true if the <see cref="PacketFields"/> object is null or equal to the Empty instance.</returns>
        public static bool IsNullOrEmpty(PacketFields pf)
        {
            return pf == null || pf == PacketFields.Empty;
        }
    }
}