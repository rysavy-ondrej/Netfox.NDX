using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Metacap;

namespace Ndx.Tools.Metacap
{
    public static class FilterHelper
    {
        /// <summary>
        /// Gets a filter function for the given filter expression.
        /// </summary>
        /// <param name="filterString">STring that defines a flow filter.</param>
        /// <returns>A function that realizes flow filter.</returns>
        public static Func<FlowKey, bool> GetFilterFunction(string filterString)
        {
            if (String.IsNullOrEmpty(filterString))
            {
                return (x) => true;
            }

            var expr = FlowKeyFilterExpression.TryParse(filterString, out string errorMessage);
            if (expr == null)
            {
                Console.Error.WriteLine($"Filter error: {errorMessage}. No filter will be applied.");
                return (x) => true;
            }

            return expr.FlowFilter;
        }
    }
}
