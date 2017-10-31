using Ndx.Model;
using NLog;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Ndx.Diagnostics
{

    /// <summary>
    /// Implements a parser for wirshakr display filter language.
    /// </summary>
    /// <remarks>
    /// See https://www.wireshark.org/docs/man-pages/wireshark-filter.html for details.
    /// </remarks>
    internal partial class ExpressionParser
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

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

        static Parser<MethodInfo> MakeOperator(string op, MethodInfo opType) =>
            from _ in Sprache.Parse.String(op).Token()
            select (opType);

        static readonly Parser<MethodInfo> AndAlso = MakeOperator("&&", OperatorMethod.AndAlso);

        static readonly Parser<MethodInfo> OrElse = MakeOperator("||", OperatorMethod.OrElse);

        static readonly Parser<MethodInfo> Not = MakeOperator("!", OperatorMethod.Not);

        static readonly Parser<MethodInfo> Equal = MakeOperator("==", OperatorMethod.Equal);

        static readonly Parser<MethodInfo> NotEqual = MakeOperator("!=", OperatorMethod.NotEqual);

        static readonly Parser<MethodInfo> GreaterThan = MakeOperator(">", OperatorMethod.GreaterThan);

        static readonly Parser<MethodInfo> LessThan = MakeOperator("<", OperatorMethod.LessThan);

        static readonly Parser<MethodInfo> GreaterThanOrEqual = MakeOperator(">=", OperatorMethod.GreaterThanOrEqual);

        static readonly Parser<MethodInfo> LessThanOrEqual = MakeOperator("<=", OperatorMethod.LessThanOrEqual);

        static readonly Parser<MethodInfo> Add = MakeOperator("+", OperatorMethod.Add);

        static readonly Parser<MethodInfo> Divide = MakeOperator("/", OperatorMethod.Divide);

        static readonly Parser<MethodInfo> Modulo = MakeOperator("%", OperatorMethod.Modulo);

        static readonly Parser<MethodInfo> Multiply = MakeOperator("*", OperatorMethod.Multiply);

        static readonly Parser<MethodInfo> Subtract = MakeOperator("-", OperatorMethod.Subtract);

        static readonly Parser<MethodInfo> StringContains = MakeOperator("contains", OperatorMethod.StringContains);

        static readonly Parser<MethodInfo> StringMatches = MakeOperator("matches", OperatorMethod.StringMatches);


        static Regex rxIpAddress = new Regex(@"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)", RegexOptions.Compiled);
        static Regex rxField = new Regex(@"([A-Za-z_][A-Za-z_0-9]*)(\.([A-Za-z_][A-Za-z_0-9]*))*", RegexOptions.Compiled);

        static readonly Parser<Expression> IpAddress =
            from ipv4 in Sprache.Parse.Regex(rxIpAddress)
            select Expression.Constant(ipv4);

        static readonly Parser<Expression> NumberConstant =
            from num in Sprache.Parse.Number
            select Expression.Convert(Expression.Constant(long.Parse(num)), typeof(object));

        static readonly Parser<Expression> StringConstant =
            from str in Sprache.Parse.Regex("['\"][^\"]*[\"']")
            select Expression.Constant(str.Trim('"','\''));

        static readonly Parser<Expression> Constant =
            IpAddress.XOr(NumberConstant).XOr(StringConstant);

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
                    var indexExpr = Expression.Property(eventSourceExpression, typeof(DecodedFrame).GetProperty("Item"), Expression.Constant(String.Join("_", path)));
                    return indexExpr;
                }
            }
            else
            {   // access the field using e[0][name]
                var eventSourceExpression = Expression.ArrayIndex(packetFieldsArray, Expression.Constant(0));
                var indexExpr = Expression.Property(eventSourceExpression, typeof(DecodedFrame).GetProperty("Item"), Expression.Constant(String.Join("_", parts)));
                return indexExpr;
            }
        }

        Parser<Expression> OrExpr(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(OrElse, AndExpr(packetFields), MakeBinary);
        }

        Parser<Expression> AndExpr(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(AndAlso, PredicateDecimal(packetFields), MakeBinary);
        }

        Parser<Expression> PredicateDecimal(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(Equal.Or(NotEqual).Or(GreaterThanOrEqual).Or(LessThanOrEqual).Or(GreaterThan).Or(LessThan),
                PredicateString(packetFields), MakeBinary);
        }

        Parser<Expression> PredicateString(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(StringMatches.Or(StringContains),
                WeakTerm(packetFields), MakeBinary);
        }

        Parser<Expression> WeakTerm(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(Add.XOr(Subtract),
                StrongTerm(packetFields), MakeBinary);
        }

        Parser<Expression> StrongTerm(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(Multiply.XOr(Divide).XOr(Modulo),
                Operand(packetFields), MakeBinary);
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

        public Parser<Expression<Func<DecodedFrame[], bool?>>> Lambda()
        {
            var param = Expression.Parameter(typeof(DecodedFrame[]), "@t");
            return OrExpr(param).End().Select(body => Expression.Lambda<Func<DecodedFrame[], bool?>>(body, param));
        }

        /// <summary>
        /// Creates a binary expression for the given operator and converts arguments to <see cref="decimal"/> when necessary.
        /// </summary>
        /// <param name="operatorMethod"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        Expression MakeBinary(MethodInfo operatorMethod, Expression arg1, Expression arg2)
        {
            var arg1Expr = Expression.Call(Diagnostics.OperatorExpression._ToDecimalMethodInfo.MakeGenericMethod(arg1.Type), arg1);
            var arg2Expr = Expression.Call(Diagnostics.OperatorExpression._ToDecimalMethodInfo.MakeGenericMethod(arg2.Type), arg2);
            return Expression.Call(operatorMethod, arg1Expr, arg2Expr);
        }

        /// <summary>
        /// Provides <see cref="Expresssion"/> object for operators. TODO: convert to singleton where suitable.
        /// </summary>
        internal static class OperatorMethod
        {
            internal static MethodInfo AndAlso => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.AndAlso));
            internal static MethodInfo Equal => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.Equal));
            internal static MethodInfo GreaterThan => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.GreaterThan));
            internal static MethodInfo GreaterThanOrEqual => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.GreaterThanOrEqual));
            internal static MethodInfo LessThan => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.LessThan));
            internal static MethodInfo LessThanOrEqual => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.LessThanOrEqual));
            internal static MethodInfo Not => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.BitwiseNot));
            internal static MethodInfo NotEqual => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.NotEqual));
            internal static MethodInfo OrElse => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.OrElse));

            internal static MethodInfo Add => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.Add));
            internal static MethodInfo Divide => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.Divide));
            internal static MethodInfo Modulo => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.Modulo));
            internal static MethodInfo Multiply => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.Multiply));
            internal static MethodInfo Subtract => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.Subtract));
            internal static MethodInfo StringContains => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.StringContains));
            internal static MethodInfo StringMatches => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.StringMatches));
            internal static MethodInfo StringConcat => typeof(VariantArithmetic).GetMethod(nameof(VariantArithmetic.StringConcat));
        }
    }
}
