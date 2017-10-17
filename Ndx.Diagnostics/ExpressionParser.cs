using Ndx.Model;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Ndx.Diagnostics
{

    internal class ExpressionParser
    {
        private Dictionary<string, int> m_args = null;

        internal ExpressionParser()
        {
        }

        internal ExpressionParser(string[] args)
        {
            m_args = new Dictionary<string, int>();
            for (int i = 0; i < args.Length; i++)
            {
                m_args[args[i]] = i;
            }
        }

        static Parser<Expression> MakeOperator(string op, Expression opType) =>
            from _ in Sprache.Parse.String(op).Token()
            select (opType);

        static readonly Parser<Expression> AndAlso = MakeOperator("&&", OperatorExpression.AndAlso);

        static readonly Parser<Expression> OrElse = MakeOperator("||", OperatorExpression.OrElse);

        static readonly Parser<Expression> Not = MakeOperator("!", OperatorExpression.Not);

        static readonly Parser<Expression> Equal = MakeOperator("==", OperatorExpression.Equal);

        static readonly Parser<Expression> NotEqual = MakeOperator("!=", OperatorExpression.NotEqual);

        static readonly Parser<Expression> GreaterThan = MakeOperator(">", OperatorExpression.GreaterThan);

        static readonly Parser<Expression> LessThan = MakeOperator("<", OperatorExpression.LessThan);

        static readonly Parser<Expression> GreaterThanOrEqual = MakeOperator(">=", OperatorExpression.GreaterThanOrEqual);

        static readonly Parser<Expression> LessThanOrEqual = MakeOperator("<=", OperatorExpression.LessThanOrEqual);

        static readonly Parser<Expression> Add = MakeOperator("+", OperatorExpression.Add);

        static readonly Parser<Expression> Divide = MakeOperator("/", OperatorExpression.Divide);

        static readonly Parser<Expression> Modulo = MakeOperator("%", OperatorExpression.Modulo);

        static readonly Parser<Expression> Multiply = MakeOperator("*", OperatorExpression.Multiply);

        static readonly Parser<Expression> Subtract = MakeOperator("-", OperatorExpression.Subtract);

        static Regex rxIpAddress = new Regex(@"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)");
        static Regex rxField = new Regex(@"([A-Za-z_][A-Za-z_0-9]*)(\.([A-Za-z_][A-Za-z_0-9]*))*");

        static readonly Parser<Expression> IpAddress =
            from ipv4 in Sprache.Parse.Regex(rxIpAddress)
            select Expression.Constant(ipv4);

        static readonly Parser<Expression> Number =
            from num in Sprache.Parse.Number
            select Expression.Convert(Expression.Constant(long.Parse(num)), typeof(object));

        static readonly Parser<Expression> Constant =
            IpAddress.XOr(Number);

        Parser<Expression> Field(ParameterExpression flowKey) =>
            from name in Sprache.Parse.Regex(rxField)
            select AccessField(flowKey, name);


        Expression AccessField(ParameterExpression packetFieldsArray, string name)
        {
            var parts = name.Split('.');
            if (m_args != null)
            {   // access the field using e[index][name]
                
                var source = parts.First();
                var path = parts.Skip(1).ToArray();

                m_args.TryGetValue(source, out int eventIndex);
                var eventSourceExpression = Expression.ArrayIndex(packetFieldsArray, Expression.Constant(eventIndex));

                if (path.Length == 0)
                {
                    return eventSourceExpression;
                }
                else
                {
                    var indexExpr = Expression.Property(eventSourceExpression, typeof(PacketFields).GetProperty("Item"), Expression.Constant(String.Join("_", path)));
                    return indexExpr;
                }
            }
            else
            {   // access the field using e[0][name]
                var eventSourceExpression = Expression.ArrayIndex(packetFieldsArray, Expression.Constant(0));
                var indexExpr = Expression.Property(eventSourceExpression, typeof(PacketFields).GetProperty("Item"), Expression.Constant(String.Join("_", parts)));
                return indexExpr;
            }
        }

        Parser<Expression> OrExpr(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(OrElse, AndExpr(packetFields), MakeBinaryBool);
        }

        Parser<Expression> AndExpr(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(AndAlso, Predicate(packetFields), MakeBinaryBool);
        }

        Parser<Expression> Predicate(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(Equal.Or(NotEqual).Or(GreaterThanOrEqual).Or(LessThanOrEqual).Or(GreaterThan).Or(LessThan),
                WeakTerm(packetFields), MakeBinaryDecimal);
        }

        Parser<Expression> WeakTerm(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(Add.XOr(Subtract),
                StrongTerm(packetFields), MakeBinaryDecimal);
        }

        Parser<Expression> StrongTerm(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(Multiply.XOr(Divide).XOr(Modulo),
                Operand(packetFields), MakeBinaryDecimal);
        }

        Parser<Expression> Operand(ParameterExpression flowKey) =>
             ((from sign in Sprache.Parse.Char('!')
               from factor in Factor(flowKey)
               select Expression.Not(factor)
             ).XOr(Factor(flowKey))).Token();

        Parser<Expression> Factor(ParameterExpression flowKey) =>
          (from lparen in Sprache.Parse.Char('(')
           from expr in Sprache.Parse.Ref(() => OrExpr(flowKey))
           from rparen in Sprache.Parse.Char(')')
           select expr).XOr(Constant).XOr(Field(flowKey));

        public Parser<Expression<Func<PacketFields[], bool?>>> Lambda()
        {
            var param = Expression.Parameter(typeof(PacketFields[]), "@t");
            return OrExpr(param).End().Select(body => Expression.Lambda<Func<PacketFields[], bool?>>(body, param));
        }

        Expression MakeBinaryDecimal(Expression operatorExpression, Expression arg1, Expression arg2)
        {
            var arg1Expr = Expression.Call(Operator._ToDecimalMethodInfo.MakeGenericMethod(arg1.Type), arg1);
            var arg2Expr = Expression.Call(Operator._ToDecimalMethodInfo.MakeGenericMethod(arg2.Type), arg2);
            var methodInfo = operatorExpression.Type.GetMethod("Apply");
            return Expression.Call(operatorExpression, methodInfo, arg1Expr, arg2Expr);
        }

        Expression MakeBinaryBool(Expression operatorExpression, Expression arg1, Expression arg2)
        {
            var methodInfo = operatorExpression.Type.GetMethod("Apply");
            return Expression.Call(operatorExpression, methodInfo, arg1, arg2);
        }
        /// <summary>
        /// Provides <see cref="Expresssion"/> object for operators. TODO: convert to singleton where suitable.
        /// </summary>
        internal static class OperatorExpression
        {
            internal static Expression AndAlso => Expression.New(typeof(Operator.AndAlso));
            internal static Expression Equal => Expression.New(typeof(Operator.Equal));
            internal static Expression GreaterThan => Expression.New(typeof(Operator.GreaterThan));
            internal static Expression GreaterThanOrEqual => Expression.New(typeof(Operator.GreaterThanOrEqual));
            internal static Expression LessThan => Expression.New(typeof(Operator.LessThan));
            internal static Expression LessThanOrEqual => Expression.New(typeof(Operator.LessThanOrEqual));
            internal static Expression Not => Expression.New(typeof(Operator.Not));
            internal static Expression NotEqual => Expression.New(typeof(Operator.NotEqual));
            internal static Expression OrElse => Expression.New(typeof(Operator.OrElse));

            public static Expression Add => Expression.New(typeof(Operator.Add));
            public static Expression Divide => Expression.New(typeof(Operator.Divide));
            public static Expression Modulo => Expression.New(typeof(Operator.Modulo));
            public static Expression Multiply => Expression.New(typeof(Operator.Multiply));
            public static Expression Subtract => Expression.New(typeof(Operator.Subtract));
        }
        /// <summary>
        /// Derived classes implement operator expressions.
        /// </summary>
        public abstract class Operator
        {

            internal static MethodInfo _ToDecimalMethodInfo => typeof(Operator).GetMethod("ToDecimal");
            /// <summary>
            /// Converts input object of type <typeparamref name="T"/> to <see cref="decimal?"/> value.
            /// </summary>
            /// <remarks>
            /// This method can convert string reperesentation of ordinal number, flow, 
            /// hexadecimal number starting with 0x prefix, and IPv4 address. 
            /// </remarks>
            /// <param name="x">The input string.</param>
            /// <returns><see cref="decimal?"/> value for the provided input string.</returns>
            public static decimal? ToDecimal<T>(T x)
            {
                try
                {
                    if (x is decimal d) return d;

                    if (x is string s)
                    {
                        // empty string is converted to null
                        if (String.IsNullOrEmpty(s)) return null;

                        // hexadecimal number:
                        if (s.StartsWith("0x")) { return System.Convert.ToDecimal(Convert.ToInt64(s.Substring(2), 16)); }

                        // ipaddress
                        if (Regex.IsMatch(s, "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$"))
                            return Convert.ToDecimal(IPAddress.Parse(s).Address);

                        if (Regex.IsMatch(s, @"^\d+(.\d+)?$")) return System.Convert.ToDecimal(x);

                        return null;
                    }
                    
                    return System.Convert.ToDecimal(x);
                }
                // This is just for sure, using exception is very very slow, so this should not happen in most of the cases!
                catch(Exception)
                {
                    return null;
                }
            }

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
            public static bool IsBoolean(object value)
            {
                return value is bool;
            }

            public static bool IsEnum(object value)
            {
                return value is Enum;
            }

            internal class AndAlso : Operator
            {
                public bool? Apply(bool? x, bool? y)
                {
                    if (!x.HasValue || !y.HasValue) return null;
                    return x.Value && y.Value;
                }
            }
            internal class OrElse : Operator
            {
                public bool? Apply(bool? x, bool? y)
                {
                    if (!x.HasValue || !y.HasValue) return null;
                    return x.Value || y.Value;
                }
            }
            internal class Not : Operator
            {
                public bool? Apply(bool? x)
                {
                    return !x;
                }
            }

            internal class Equal : Operator
            {
                public bool? Apply(decimal? x, decimal? y) => x == y;
            }
            internal class NotEqual : Operator
            {
                public bool? Apply(decimal? x, decimal? y) => x != y;
            }
            internal class GreaterThan : Operator
            {
                public bool? Apply(decimal? x, decimal? y) => x > y;
            }
            internal class GreaterThanOrEqual : Operator
            {

                public bool? Apply(decimal? x, decimal? y) => x >= y;
            }
            internal class LessThan : Operator
            {
                public bool? Apply(decimal? x, decimal? y) => x < y;
            }
            internal class LessThanOrEqual : Operator
            {
                public bool? Apply(decimal? x, decimal? y) => x <= y;
            }

            internal class Add : Operator
            {
                public decimal? Apply(decimal? x, decimal? y) => x + y;
            }

            internal class Divide : Operator
            {
                public decimal? Apply(decimal? x, decimal? y) => x / y;
            }

            internal class Modulo : Operator
            {
                public decimal? Apply(decimal? x, decimal? y) => x % y;
            }

            internal class Multiply : Operator
            {
                public decimal? Apply(decimal? x, decimal? y) => x * y;
            }

            internal class Subtract : Operator
            {
                public decimal? Apply(decimal? x, decimal? y) => x - y;
            }
        }
    }
}
