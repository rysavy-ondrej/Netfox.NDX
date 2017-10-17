using NLog;
using System;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Ndx.Diagnostics
{

    /// <summary>
    /// Derived classes implement operator expressions.
    /// </summary>
    public abstract class OperatorExpression
    {
        private static Logger m_logger = LogManager.GetCurrentClassLogger();

        internal static MethodInfo _ToDecimalMethodInfo => typeof(OperatorExpression).GetMethod("ToDecimal");
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
                    if (s.StartsWith("0x")) { return Convert.ToDecimal(Convert.ToInt64(s.Substring(2), 16)); }

                    // ipaddress
                    if (Regex.IsMatch(s, "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$"))
                        return Convert.ToDecimal(IPAddress.Parse(s).Address);

                    if (Regex.IsMatch(s, @"^\d+(.\d+)?$")) return Convert.ToDecimal(x);

                    return null;
                }

                return Convert.ToDecimal(x);
            }
            // This is just for sure, using exception is very very slow, so this should not never happen.
            catch (Exception e)
            {
                m_logger.Error(e, $"Cannot convert '{x}' to decimal.", x);
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

        internal class AndAlso : OperatorExpression
        {
            public bool? Apply(bool? x, bool? y)
            {
                if (!x.HasValue || !y.HasValue) return null;
                return x.Value && y.Value;
            }
        }
        internal class OrElse : OperatorExpression
        {
            public bool? Apply(bool? x, bool? y)
            {
                if (!x.HasValue || !y.HasValue) return null;
                return x.Value || y.Value;
            }
        }
        internal class Not : OperatorExpression
        {
            public bool? Apply(bool? x)
            {
                return !x;
            }
        }

        internal class Equal : OperatorExpression
        {
            public bool? Apply(decimal? x, decimal? y) => x == y;
        }
        internal class NotEqual : OperatorExpression
        {
            public bool? Apply(decimal? x, decimal? y) => x != y;
        }
        internal class GreaterThan : OperatorExpression
        {
            public bool? Apply(decimal? x, decimal? y) => x > y;
        }
        internal class GreaterThanOrEqual : OperatorExpression
        {

            public bool? Apply(decimal? x, decimal? y) => x >= y;
        }
        internal class LessThan : OperatorExpression
        {
            public bool? Apply(decimal? x, decimal? y) => x < y;
        }
        internal class LessThanOrEqual : OperatorExpression
        {
            public bool? Apply(decimal? x, decimal? y) => x <= y;
        }

        internal class Add : OperatorExpression
        {
            public decimal? Apply(decimal? x, decimal? y) => x + y;
        }

        internal class Divide : OperatorExpression
        {
            public decimal? Apply(decimal? x, decimal? y) => x / y;
        }

        internal class Modulo : OperatorExpression
        {
            public decimal? Apply(decimal? x, decimal? y) => x % y;
        }

        internal class Multiply : OperatorExpression
        {
            public decimal? Apply(decimal? x, decimal? y) => x * y;
        }

        internal class Subtract : OperatorExpression
        {
            public decimal? Apply(decimal? x, decimal? y) => x - y;
        }
        internal class StringEqual : OperatorExpression
        {
            public bool? Apply(string x, string y) => x?.Equals(y);
        }
        internal class StringContains : OperatorExpression
        {
            public bool? Apply(string x, string y) => x?.Contains(y);
        }
        internal class StringNotEqual : OperatorExpression
        {
            public bool? Apply(string x, string y) => !(x?.Equals(y));
        }
        internal class StringMatches : OperatorExpression
        {
            public bool? Apply(string x, string y) => x!= null ? Regex.IsMatch(x, y) : (bool?)null;
        }
    }
}
