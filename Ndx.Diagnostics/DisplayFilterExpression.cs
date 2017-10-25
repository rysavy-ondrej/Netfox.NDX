using Ndx.Model;
using Sprache;
using System;
using System.Linq.Expressions;

namespace Ndx.Diagnostics
{

    public class DisplayFilterExpression
    {
        private Expression<Func<DecodedFrame, bool?>> m_filterExpression;
        public DisplayFilterExpression(Expression<Func<DecodedFrame[], bool?>> expression)
        {
            var paramExpression = Expression.Parameter(typeof(DecodedFrame), "e");
            var arrayExpression = Expression.NewArrayInit(typeof(DecodedFrame), paramExpression);
            
            this.m_filterExpression = Expression.Lambda<Func<DecodedFrame, bool?>>(Expression.Invoke(expression, arrayExpression), paramExpression);   
        }

        private Func<DecodedFrame, bool?> m_filterFunc;
        public Func<DecodedFrame, bool?> FilterFunction
        {
            get
            {
                if (m_filterFunc == null)
                {
                    m_filterFunc = m_filterExpression.Compile();
                }

                return m_filterFunc;
            }
        }

        /// <summary>
        /// Parses the input string to <see cref="FlowKeyFilterExpression"/>.
        /// </summary>
        /// <param name="filter">An input string using flow filter syntax to express the filter.</param>
        /// <returns><see cref="FlowKeyFilterExpression"/> instance containig the filter. Use <see cref="FlowKeyFilterExpression.FlowFilter"/> to get filter function.</returns>
        /// <exception cref="ParseException">thrown on syntax error.</exception>
        public static DisplayFilterExpression Parse(string filter)
        {
            var expression = new ExpressionParser().Lambda().Parse(filter);
            return new DisplayFilterExpression(expression);
        }


        /// <summary>
        /// Tries to parse the input filter string and returns <see cref="FlowKeyFilterExpression"/> on success. When 
        /// input string ocntains syntax error the return value is null and <paramref name="errorMessage"/> contains 
        /// description of the error.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="errorMessage">Description of the parse error. Null on success.</param>
        /// <returns></returns>
        public static DisplayFilterExpression TryParse(string filter, out string errorMessage)
        {
            try
            {
                errorMessage = null;
                return Parse(filter);
            }
            catch (ParseException e)
            {
                errorMessage = e.Message;
                return null;
            }
        }
    }
}
