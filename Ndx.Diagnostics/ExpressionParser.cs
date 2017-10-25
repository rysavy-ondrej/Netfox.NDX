using Ndx.Model;
using NLog;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        static readonly Parser<Expression> StringContains = MakeOperator("contains", OperatorExpression.StringContains);

        static readonly Parser<Expression> StringEqual = MakeOperator("eq", OperatorExpression.StringEqual);

        static readonly Parser<Expression> StringNotEqual = MakeOperator("ne", OperatorExpression.StringNotEqual);

        static readonly Parser<Expression> StringMatches = MakeOperator("matches", OperatorExpression.StringMatches);


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
            return Sprache.Parse.ChainOperator(OrElse, AndExpr(packetFields), MakeBinaryBool);
        }

        Parser<Expression> AndExpr(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(AndAlso, PredicateDecimal(packetFields), MakeBinaryBool);
        }

        Parser<Expression> PredicateDecimal(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(Equal.Or(NotEqual).Or(GreaterThanOrEqual).Or(LessThanOrEqual).Or(GreaterThan).Or(LessThan),
                PredicateString(packetFields), MakeBinaryDecimal);
        }

        Parser<Expression> PredicateString(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(StringEqual.Or(StringMatches).Or(StringContains).Or(StringNotEqual),
                WeakTerm(packetFields), MakeBinaryString);
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

        public Parser<Expression<Func<DecodedFrame[], bool?>>> Lambda()
        {
            var param = Expression.Parameter(typeof(DecodedFrame[]), "@t");
            return OrExpr(param).End().Select(body => Expression.Lambda<Func<DecodedFrame[], bool?>>(body, param));
        }

        /// <summary>
        /// Creates a binary expression for the given operator and converts arguments to <see cref="decimal"/> when necessary.
        /// </summary>
        /// <param name="operatorExpression"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        Expression MakeBinaryDecimal(Expression operatorExpression, Expression arg1, Expression arg2)
        {
            var arg1Expr = Expression.Call(Diagnostics.OperatorExpression._ToDecimalMethodInfo.MakeGenericMethod(arg1.Type), arg1);
            var arg2Expr = Expression.Call(Diagnostics.OperatorExpression._ToDecimalMethodInfo.MakeGenericMethod(arg2.Type), arg2);
            var methodInfo = operatorExpression.Type.GetMethod("Apply");
            return Expression.Call(operatorExpression, methodInfo, arg1Expr, arg2Expr);
        }

        Expression MakeBinaryString(Expression operatorExpression, Expression arg1, Expression arg2)
        {
            var methodInfo = operatorExpression.Type.GetMethod("Apply");
            return Expression.Call(operatorExpression, methodInfo, arg1, arg2);
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
            internal static Expression AndAlso => Expression.New(typeof(Diagnostics.OperatorExpression.AndAlso));
            internal static Expression Equal => Expression.New(typeof(Diagnostics.OperatorExpression.Equal));
            internal static Expression GreaterThan => Expression.New(typeof(Diagnostics.OperatorExpression.GreaterThan));
            internal static Expression GreaterThanOrEqual => Expression.New(typeof(Diagnostics.OperatorExpression.GreaterThanOrEqual));
            internal static Expression LessThan => Expression.New(typeof(Diagnostics.OperatorExpression.LessThan));
            internal static Expression LessThanOrEqual => Expression.New(typeof(Diagnostics.OperatorExpression.LessThanOrEqual));
            internal static Expression Not => Expression.New(typeof(Diagnostics.OperatorExpression.Not));
            internal static Expression NotEqual => Expression.New(typeof(Diagnostics.OperatorExpression.NotEqual));
            internal static Expression OrElse => Expression.New(typeof(Diagnostics.OperatorExpression.OrElse));

            internal static Expression Add => Expression.New(typeof(Diagnostics.OperatorExpression.Add));
            internal static Expression Divide => Expression.New(typeof(Diagnostics.OperatorExpression.Divide));
            internal static Expression Modulo => Expression.New(typeof(Diagnostics.OperatorExpression.Modulo));
            internal static Expression Multiply => Expression.New(typeof(Diagnostics.OperatorExpression.Multiply));
            internal static Expression Subtract => Expression.New(typeof(Diagnostics.OperatorExpression.Subtract));
            internal static Expression StringContains => Expression.New(typeof(Diagnostics.OperatorExpression.StringContains));
            internal static Expression StringMatches => Expression.New(typeof(Diagnostics.OperatorExpression.StringMatches));
            internal static Expression StringEqual => Expression.New(typeof(Diagnostics.OperatorExpression.StringEqual));
            internal static Expression StringNotEqual => Expression.New(typeof(Diagnostics.OperatorExpression.StringNotEqual));
        }
    }
}
