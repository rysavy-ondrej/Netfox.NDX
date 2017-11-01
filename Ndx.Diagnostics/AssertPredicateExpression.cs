using Ndx.Model;
using Sprache;
using System;
using System.Linq.Expressions;
namespace Ndx.Diagnostics
{
    public class AssertPredicateExpression
    {
        private Expression<Func<DecodedFrame[], Variant>> m_filterExpression;
        public AssertPredicateExpression(Expression<Func<DecodedFrame[], Variant>> expression)
        {
            this.m_filterExpression = expression;
        }

        private Func<DecodedFrame[], Variant> m_filterFunc;
        public Func<DecodedFrame[], Variant> FlowFilter
        {
            get
            {
                if (m_filterFunc == null)
                    m_filterFunc = m_filterExpression.Compile();
                return m_filterFunc;
            }
        }

        /// <summary>
        /// Parses the input string to <see cref="FlowKeyFilterExpression"/>.
        /// </summary>
        /// <param name="filter">An input string using flow filter syntax to express the filter.</param>
        /// <returns><see cref="FlowKeyFilterExpression"/> instance containig the filter. Use <see cref="FlowKeyFilterExpression.FlowFilter"/> to get filter function.</returns>
        /// <exception cref="ParseException">thrown on syntax error.</exception>
        public static AssertPredicateExpression Parse(string filter, string[] events)
        {
            var expression = new ExpressionParser(events).Lambda().Parse(filter);
            return new AssertPredicateExpression(expression);
        }


        /// <summary>
        /// Tries to parse the input filter string and returns <see cref="FlowKeyFilterExpression"/> on success. When 
        /// input string ocntains syntax error the return value is null and <paramref name="errorMessage"/> contains 
        /// description of the error.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="errorMessage">Description of the parse error. Null on success.</param>
        /// <returns></returns>
        public static AssertPredicateExpression TryParse(string filter, string[] events, out string errorMessage)
        {
            try
            {
                errorMessage = null;
                return Parse(filter, events);
            }
            catch (Sprache.ParseException e)
            {
                errorMessage = e.Message;
                return null;
            }
        }
    }
}
