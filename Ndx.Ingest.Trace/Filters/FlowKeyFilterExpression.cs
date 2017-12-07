using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Sprache;
using Ndx.Model;
namespace Ndx.Ipflow
{
    /// <summary>
    /// This class represents filter expression. 
    /// </summary>
    /// <remarks>
    /// For supported syntax see: https://www.wireshark.org/docs/wsug_html_chunked/ChWorkBuildDisplayFilterSection.html
    /// </remarks>
    public class FlowKeyFilterExpression
    {
        public static class Test
        {
            public static bool IsString(object value)
            {
                return value is string;
            }
            public static bool IsNumber(object value)
            {
                return value is sbyte
                        || value is byte
                        || value is short
                        || value is ushort
                        || value is int
                        || value is uint
                        || value is long
                        || value is ulong
                        || value is float
                        || value is double
                        || value is decimal;
            }

            public static bool IsEnum(object value)
            {
                return value is Enum;
            }

            public static bool Equal(object x, object y)
            {
                if (IsNumber(x) && IsNumber(y)) return System.Convert.ToDecimal(x) == System.Convert.ToDecimal(y);
                if (IsString(x) || IsString(y)) return String.Equals(System.Convert.ToString(x),System.Convert.ToString(y), StringComparison.InvariantCultureIgnoreCase);
                return false;
            }

            public static bool NotEqual(object x, object y)
            {
                return !Equal(x,y);
            }

            public static bool GreaterThan(object x, object y)
            {
                if (IsNumber(x) && IsNumber(y)) return System.Convert.ToDecimal(x) > System.Convert.ToDecimal(y);
                return false;
            }

            public static bool LessThan(object x, object y)
            {
                if (IsNumber(x) && IsNumber(y)) return System.Convert.ToDecimal(x) < System.Convert.ToDecimal(y);
                return false;

            }

            public static bool GreaterThanOrEqual(object x, object y)
            {
                if (IsNumber(x) && IsNumber(y)) return System.Convert.ToDecimal(x) >= System.Convert.ToDecimal(y);
                return false;                
            }

            public static bool LessThanOrEqual(object x, object y)
            {
                if (IsNumber(x) && IsNumber(y)) return System.Convert.ToDecimal(x) <= System.Convert.ToDecimal(y);
                return false;
            }
        }


        /// <summary>
        /// Parses the input string to <see cref="FlowKeyFilterExpression"/>.
        /// </summary>
        /// <param name="filter">An input string using flow filter syntax to express the filter.</param>
        /// <returns><see cref="FlowKeyFilterExpression"/> instance containig the filter. Use <see cref="FlowKeyFilterExpression.FlowFilter"/> to get filter function.</returns>
        /// <exception cref="ParseException">thrown on syntax error.</exception>
        public static FlowKeyFilterExpression Parse(string filter)
        {
            var t = Parser.Lambda().Parse(filter);
            return new FlowKeyFilterExpression(t);
        }


        /// <summary>
        /// Tries to parse the input filter string and returns <see cref="FlowKeyFilterExpression"/> on success. When 
        /// input string ocntains syntax error the return value is null and <paramref name="errorMessage"/> contains 
        /// description of the error.
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="errorMessage">Description of the parse error. Null on success.</param>
        /// <returns></returns>
        public static FlowKeyFilterExpression TryParse(string filter, out string errorMessage)
        {
            try
            {
                errorMessage = null;
                return Parse(filter);
            }
            catch(Sprache.ParseException e)
            {
                errorMessage = e.Message;
                return null;
            }
        }

        public static class Parser
        {
            static Parser<ExpressionType> Operator(string op, ExpressionType opType) =>
                from _ in Sprache.Parse.String(op).Token()
                select (opType);

            static readonly Parser<ExpressionType> AndAlso = Operator("&&", ExpressionType.AndAlso);

            static readonly Parser<ExpressionType> OrElse = Operator("||", ExpressionType.OrElse);

            static readonly Parser<ExpressionType> Not = Operator("!", ExpressionType.Not);

            static readonly Parser<ExpressionType> Equal = Operator("==", ExpressionType.Equal);

            static readonly Parser<ExpressionType> NotEqual = Operator("!=", ExpressionType.NotEqual);

            static readonly Parser<ExpressionType> GreaterThan = Operator(">", ExpressionType.GreaterThan);

            static readonly Parser<ExpressionType> LessThan = Operator("<", ExpressionType.LessThan);

            static readonly Parser<ExpressionType> GreaterThanOrEqual = Operator(">=", ExpressionType.GreaterThanOrEqual);

            static readonly Parser<ExpressionType> LessThanOrEqual = Operator("<=", ExpressionType.LessThanOrEqual);



            static readonly Parser<Expression> IpAddress =
                from ipv4 in Sprache.Parse.Regex(@"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)")
                select Expression.Constant(ipv4);

            static readonly Parser<Expression> Number =
                from num in Sprache.Parse.Number
                select Expression.Convert(Expression.Constant(long.Parse(num)), typeof(object));

            static readonly Parser<Expression> Constant =
                IpAddress.XOr(Number);

            static Parser<Expression> Field(ParameterExpression flowKey) =>
                from name in Sprache.Parse.Letter.AtLeastOnce().Text()
                select AccessField(flowKey, name);

            static Expression AccessField(ParameterExpression flowKey, string name)
            {
                switch (name.ToLowerInvariant())
                {
                    case "sourceport":
                        return Expression.Convert(Expression.PropertyOrField(flowKey, nameof(FlowKey.SourcePort)), typeof(object));
                    case "destinationport":
                        return Expression.Convert(Expression.PropertyOrField(flowKey, nameof(FlowKey.DestinationPort)), typeof(object));
                    case "sourceaddress":
                        return Expression.Convert(Expression.PropertyOrField(flowKey, nameof(FlowKey.SourceIpAddress)), typeof(object));
                    case "destinationaddress":
                        return Expression.Convert(Expression.PropertyOrField(flowKey, nameof(FlowKey.DestinationIpAddress)), typeof(object));
                    case "protocol":
                        return Expression.Convert(Expression.PropertyOrField(flowKey, nameof(FlowKey.IpProtocol)), typeof(object));
                    default:
                        return Expression.Constant(name);
                }
            }

            static Parser<Expression> OrExpr(ParameterExpression flowKey)
            {
                return Sprache.Parse.ChainOperator(OrElse, AndExpr(flowKey), Expression.MakeBinary);
            }

            private static Parser<Expression> AndExpr(ParameterExpression flowKey)
            {
                return Sprache.Parse.ChainOperator(AndAlso, Term(flowKey), Expression.MakeBinary);
            }

            private static Parser<Expression> Term(ParameterExpression flowKey)
            {
                return Sprache.Parse.ChainOperator(Equal.Or(NotEqual).Or(GreaterThan).Or(GreaterThanOrEqual).Or(LessThan).Or(LessThanOrEqual),
                    Operand(flowKey), MakeBinary);
            }

            private static Expression MakeBinary(ExpressionType exprType, Expression arg1, Expression arg2)
            {
                var methodInfo = typeof(Test).GetMethod(exprType.ToString(), BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);
                return Expression.Call(methodInfo, arg1, arg2);
            }

            private static Parser<Expression> Operand(ParameterExpression flowKey) =>
                 ((from sign in Sprache.Parse.Char('!')
                   from factor in Factor(flowKey)
                   select Expression.Not(factor)
                 ).XOr(Factor(flowKey))).Token();

            static Parser<Expression> Factor(ParameterExpression flowKey) =>
              (from lparen in Sprache.Parse.Char('(')
               from expr in Sprache.Parse.Ref(() => OrExpr(flowKey))
               from rparen in Sprache.Parse.Char(')')
               select expr).Named("expression")
               .XOr(Constant)
               .XOr(Field(flowKey));

            public static Parser<Expression<Func<FlowKey, bool>>> Lambda()
            {
                var flowKeyParam = Expression.Parameter(typeof(FlowKey), "flowKey");
                return OrExpr(flowKeyParam).End().Select(body => Expression.Lambda<Func<FlowKey, bool>>(body, flowKeyParam));
            }
        }


        private Expression<Func<FlowKey, bool>> m_filterExpression;
        public FlowKeyFilterExpression(Expression<Func<FlowKey, bool>> expression)
        {
            this.m_filterExpression = expression;
        }

        private Func<FlowKey, bool> m_filterFunc;
        public Func<FlowKey, bool> FlowFilter
        {
            get
            {
                if (m_filterFunc == null)
                    m_filterFunc = m_filterExpression.Compile();
                return m_filterFunc;
            }
        }
    }
}
