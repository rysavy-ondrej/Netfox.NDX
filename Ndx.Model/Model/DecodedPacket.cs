using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Utils;

namespace Ndx.Model
{
    /// <summary>
    /// This class represents decoded frames. Tshark can be used to parse frames to JSON output, which is used to create objects of this class. 
    /// </summary>
    public partial class DecodedFrame
    {
        public Variant GetFieldValue(string field, bool defaultValue)
        {
            return GetFieldValue(field, new Variant(defaultValue));
        }

        public Variant GetFieldValue(string field, int defaultValue)
        {
            return GetFieldValue(field, new Variant(defaultValue));
        }

        public Variant GetFieldValue(string field, long defaultValue)
        {
            return GetFieldValue(field, new Variant(defaultValue));
        }

        public Variant GetFieldValue(string field, float defaultValue)
        {
            return GetFieldValue(field, new Variant(defaultValue));
        }

        public Variant GetFieldValue(string field, double defaultValue)
        {
            return GetFieldValue(field, new Variant(defaultValue));
        }

        public Variant GetFieldValue(string field, decimal defaultValue)
        {
            return GetFieldValue(field, new Variant(defaultValue));
        }

        public Variant GetFieldValue(string field, string defaultValue)
        {
            return GetFieldValue(field, new Variant(defaultValue));
        }

        public Variant GetFieldValue(string field, byte[] defaultValue)
        {
            return GetFieldValue(field, new Variant(defaultValue));
        }

        public Variant GetFieldValue(string field, DateTime defaultValue)
        {
            return GetFieldValue(field, new Variant(defaultValue));
        }

        /// <summary>
        /// Gets the value of the field or provided defaultValue. Field name is 
        /// qualified path as defined by Wireshark display filter, e.g., 'ip.src'.
        /// </summary>
        /// <param name="field">The name of the field for which to get its value.</param>
        /// <param name="defaultValue">The default value if the field has not any value.</param>
        /// <returns>Value of the field or the provided default value.</returns>
        public Variant GetFieldValue(string field, Variant defaultValue)
        {
            return Fields.TryGetValue(field, out Variant value) ? value : defaultValue;
        }

        /// <summary>
        /// Gets or sets the value of the field. Field name is 
        /// qualified path as defined by Wireshark display filter, e.g., 'ip.src'.
        /// 
        /// </summary>
        public Variant this[string name]
        {
            get
            {
                if (Fields.TryGetValue(name, out Variant value))
                {
                    return value;
                }
                else
                {
                    return Variant.None;
                }
            }
        }
        /// <summary>
        /// Get the timestamp as <see cref="DateTime"/> value.
        /// </summary>
        public DateTime DateTime => DateTime.FromBinary(this.Timestamp);

        static readonly DecodedFrame m_empty = new DecodedFrame();

        public static DecodedFrame Empty => m_empty;

        public bool IsEmpty => ReferenceEquals(this, m_empty);

        public static DecodedFrame FromFields(IDictionary<string, Variant> entries)
        {
            var pf = new DecodedFrame();
            pf.Fields.Add(entries);
            return pf;
        }

        /// <summary>
        /// Tests if the <see cref="DecodedFrame"/> object is null or equal to the Empty instance.
        /// </summary>
        /// <param name="pf">The <see cref="DecodedFrame"/> object to test.</param>
        /// <returns>true if the <see cref="DecodedFrame"/> object is null or equal to the Empty instance.</returns>
        public static bool IsNullOrEmpty(DecodedFrame pf)
        {
            return pf == null || pf == DecodedFrame.Empty;
        }
    }
}
