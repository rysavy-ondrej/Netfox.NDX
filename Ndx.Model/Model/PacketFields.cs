using System;
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

    }
}
