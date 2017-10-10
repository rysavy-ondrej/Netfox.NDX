using Ndx.Model;
using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ndx.Diagnostics
{
    internal class ExpressionParser
    {
        private Dictionary<string,int> m_args = null;

        internal ExpressionParser()
        {
        }

        internal ExpressionParser(string[] args)
        {
            m_args = new Dictionary<string,int>();
            for(int i =0; i< args.Length; i++)
            {
                m_args[args[i]] = i;
            }
        }

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

        Parser<Expression> Field(ParameterExpression flowKey) =>
            from name in Sprache.Parse.Regex(@"([A-Za-z_][A-Za-z_0-9]*)(\.([A-Za-z_][A-Za-z_0-9]*))*")
            select AccessField(flowKey, name);

        Expression AccessField(ParameterExpression packetFieldsArray, string name)
        {
            if (m_args != null)
            {   // access the field using e[index][name]
                var source = name.Split('.')[0];
                m_args.TryGetValue(source, out int index);
                var eventSourceExpression = Expression.ArrayIndex(packetFieldsArray, Expression.Constant(index));
                var indexExpr = Expression.Property(eventSourceExpression, typeof(PacketFields).GetProperty("Item"), Expression.Constant(name));
                return indexExpr;
            }
            else
            {   // access the field using e[0][name]
                var eventSourceExpression = Expression.ArrayIndex(packetFieldsArray, Expression.Constant(0));
                var indexExpr = Expression.Property(eventSourceExpression, typeof(PacketFields).GetProperty("Item"), Expression.Constant(name));
                return indexExpr;
            }
        }

        Parser<Expression> OrExpr(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(OrElse, AndExpr(packetFields), Expression.MakeBinary);
        }

        Parser<Expression> AndExpr(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(AndAlso, Term(packetFields), Expression.MakeBinary);
        }

        Parser<Expression> Term(ParameterExpression packetFields)
        {
            return Sprache.Parse.ChainOperator(Equal.Or(NotEqual).Or(GreaterThan).Or(GreaterThanOrEqual).Or(LessThan).Or(LessThanOrEqual),
                Operand(packetFields), MakeBinary);
        }

        Expression MakeBinary(ExpressionType exprType, Expression arg1, Expression arg2)
        {
            var methodInfo = typeof(Test).GetMethod(exprType.ToString(), BindingFlags.Static | BindingFlags.Public | BindingFlags.IgnoreCase);
            return Expression.Call(methodInfo, arg1, arg2);
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

        public Parser<Expression<Func<PacketFields[], bool>>> Lambda()
        {
            var param = Expression.Parameter(typeof(PacketFields), "es");
            return OrExpr(param).End().Select(body => Expression.Lambda<Func<PacketFields[], bool>>(body, param));
        }

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
                if (IsString(x) || IsString(y)) return String.Equals(System.Convert.ToString(x), System.Convert.ToString(y), StringComparison.InvariantCultureIgnoreCase);
                return false;
            }

            public static bool NotEqual(object x, object y)
            {
                return !Equal(x, y);
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
    }
}
