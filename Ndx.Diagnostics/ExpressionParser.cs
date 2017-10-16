using Ndx.Model;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        // [0-5s]~>
        static readonly Parser<Expression> LeadsTo =
            from lexpr in Sprache.Parse.Regex(@"\[\s*\d+\s*\-\s*\d+\s*\]~>")
            select OperatorExpression.LeadsTo(lexpr);

        static readonly Parser<Expression> NotLeadsTo =
            from lexpr in Sprache.Parse.Regex(@"\[\s*\d+\s*\-\s*\d+\s*\]~!>")
            select OperatorExpression.NotLeadsTo(lexpr);

        static readonly Parser<Expression> IpAddress =
            from ipv4 in Sprache.Parse.Regex(@"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)")
            select Expression.Constant(ipv4);

        static readonly Parser<Expression> Number =
            from num in Sprache.Parse.Number
            select Expression.Convert(Expression.Constant(long.Parse(num)), typeof(object));

        static readonly Parser<Expression> Constant =
            IpAddress.XOr(Number);

        Parser<Expression> Field(ParameterExpression flowKey) =>
            from name in Sprache.Parse.Regex(@"([A-Za-z_][A-Za-z_0-9]*)(\.([A-Za-z_][A-Za-z_0-9]*))*")
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
            return Sprache.Parse.ChainOperator(OrElse, AndExpr(packetFields), MakeBinary);
        }

        Parser<Expression> AndExpr(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(AndAlso, TempExpr(packetFields), MakeBinary);
        }

        Parser<Expression> TempExpr(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(LeadsTo.Or(NotLeadsTo), Term(packetFields), MakeBinary);
        }

        Parser<Expression> Term(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(Equal.Or(NotEqual).Or(GreaterThan).Or(GreaterThanOrEqual).Or(LessThan).Or(LessThanOrEqual),
                Operand(packetFields), MakeBinary);
        }

        Expression MakeBinary(Expression operatorExpression, Expression arg1, Expression arg2)
        {
            var methodInfo = operatorExpression.Type.GetMethod("Apply");
            return Expression.Call(operatorExpression,methodInfo, arg1, arg2);
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
           select expr).Named("expression")
           .XOr(Constant)
           .XOr(Field(flowKey));

        public Parser<Expression<Func<PacketFields[], bool?>>> Lambda()
        {
            var param = Expression.Parameter(typeof(PacketFields[]), "args");
            return OrExpr(param).End().Select(body => Expression.Lambda<Func<PacketFields[], bool?>>(body, param));
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
            internal static Expression LeadsTo(string str) => Expression.New(Operator.LeadsTo._ConstructorInfo, Expression.Constant(str));
            internal static Expression LessThan => Expression.New(typeof(Operator.LessThan));
            internal static Expression LessThanOrEqual => Expression.New(typeof(Operator.LessThanOrEqual));
            internal static Expression Not => Expression.New(typeof(Operator.Not));
            internal static Expression NotEqual => Expression.New(typeof(Operator.NotEqual));
            internal static Expression NotLeadsTo(string str) => Expression.New(Operator.NotLeadsTo._ConstructorInfo, Expression.Constant(str));
            internal static Expression OrElse => Expression.New(typeof(Operator.OrElse));

        }
        /// <summary>
        /// Derived classes implement operator expressions.
        /// </summary>
        internal abstract class Operator
        {
            /// <summary>
            /// Applies operator between numbers.
            /// </summary>
            /// <param name="f"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public static bool? ApplyDecimalOperator(Func<decimal, decimal, bool> f, object x, object y)
            {
                try
                {
                    if (x is string s1 && s1.StartsWith("0x")) { x = Convert.ToInt64(s1.Substring(2), 16); }
                    if (y is string s2 && s2.StartsWith("0x")) { y = Convert.ToInt64(s2.Substring(2), 16); }

                    var arg1 = System.Convert.ToDecimal(x);
                    var arg2 = System.Convert.ToDecimal(y);
                    return f(arg1,arg2);
                }
                catch (Exception)
                {
                    return null;
                }
            }

            public static bool? ApplyStringOperator(Func<string,string,bool> f, object x, object y)
            {
                try
                {
                    var arg1 = x.ToString();
                    var arg2 = y.ToString();
                    return f(arg1, arg2);
                }
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
                public bool? Apply(object x, object y) { return ApplyDecimalOperator(Decimal.Equals, x, y); }
            }
            internal class NotEqual : Operator
            {
                public bool? Apply(object x, object y) { return !ApplyDecimalOperator(Decimal.Equals, x, y); }
            }
            internal class GreaterThan : Operator
            {
                public bool? Apply(object x, object y)
                {
                    return ApplyDecimalOperator((l,r)=> l>r, x, y);
                }
            }
            internal class GreaterThanOrEqual : Operator
            {

                public bool? Apply(object x, object y)
                {
                    return ApplyDecimalOperator((l, r) => l >= r, x, y);
                }
            }
            internal class LessThan : Operator
            {
                public bool? Apply(object x, object y)
                {
                    return ApplyDecimalOperator((l, r) => l < r, x, y);

                }
            }
            internal class LessThanOrEqual : Operator
            {
                public bool? Apply(object x, object y)
                {
                    return ApplyDecimalOperator((l, r) => l <= r, x, y);
                }
            }
            internal class LeadsTo : Operator
            {
                TimeSpan m_from;
                TimeSpan m_to;
                internal static ConstructorInfo _ConstructorInfo => typeof(LeadsTo).GetConstructor(new Type[] { typeof(string) });

                public LeadsTo(string lexpr)
                {
                    var regex = new Regex(@"\[\s*(?<from>\d+)\s*\-\s*(?<to>\d+)\s*\]~>");
                    var match = regex.Match(lexpr);
                    var fromString = match.Groups["from"].Value;
                    var toString = match.Groups["to"].Value;
                    m_from = TimeSpan.FromSeconds(Int32.Parse(fromString));
                    m_to = TimeSpan.FromSeconds(Int32.Parse(toString));
                }

                internal LeadsTo(int from, int to)
                { m_from = TimeSpan.FromSeconds(from); m_to = TimeSpan.FromSeconds(to); }


                public bool? Apply(PacketFields left, PacketFields right)
                {
                    return _Apply(this.m_from, this.m_to, left, right);
                }
                /// <summary>
                /// Represents left [from-to]~> right temporal operator.
                /// </summary>
                /// <param name="from"></param>
                /// <param name="to"></param>
                /// <param name="left"></param>
                /// <param name="right"></param>
                /// <returns></returns>
                static bool _Apply(TimeSpan from, TimeSpan to, PacketFields left, PacketFields right)
                {
                    if (PacketFields.IsNullOrEmpty(left))
                    {
                        return true;
                    }
                    else
                    {
                        return
                            !PacketFields.IsNullOrEmpty(right)
                            && left.DateTime + from <= right.DateTime
                            && right.DateTime <= left.DateTime + to;
                    }
                }
            }
            internal class NotLeadsTo : Operator
            {
                TimeSpan m_from;
                TimeSpan m_to;
                internal static ConstructorInfo _ConstructorInfo => typeof(NotLeadsTo).GetConstructor(new Type[] { typeof(string) });

                public NotLeadsTo(string lexpr)
                {
                    var regex = new Regex(@"\[\s*(?<from>\d+)\s*\-\s*(?<to>\d+)\s*\]~!>");
                    var match = regex.Match(lexpr);
                    var fromString = match.Groups["from"].Value;
                    var toString = match.Groups["to"].Value;
                    m_from = TimeSpan.FromSeconds(Int32.Parse(fromString));
                    m_to = TimeSpan.FromSeconds(Int32.Parse(toString));
                }

                internal NotLeadsTo(int from, int to)
                { m_from = TimeSpan.FromSeconds(from); m_to = TimeSpan.FromSeconds(to); }


                public bool? Apply(PacketFields left, PacketFields right)
                {
                    return _Apply(this.m_from, this.m_to, left, right);
                }
                /// <summary>
                /// Represents left [from-to]~!> right temporal operator.
                /// </summary>
                /// <param name="from"></param>
                /// <param name="to"></param>
                /// <param name="left"></param>
                /// <param name="right"></param>
                /// <returns></returns>
                static bool _Apply(TimeSpan from, TimeSpan to,PacketFields left, PacketFields right)
                {
                    if (PacketFields.IsNullOrEmpty(left)) return true;
                    if (PacketFields.IsNullOrEmpty(right)) return true;
                    return (left.DateTime + from <= right.DateTime
                           && right.DateTime <= left.DateTime + to) == false;

                }
            }
        }
    }
}
