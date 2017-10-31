using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ndx.Model
{
    /// <summary>
    /// This class provides arithmetic operations for <see cref="Variant"/> class.
    /// See also:
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms221430(v=vs.85).aspx
    /// </summary>
    /// <remarks>
    /// Implementation of Variant arithmetic requires coercion among number types
    /// and conversion among non-compatible types.
    /// 
    /// Coercion is defined considering the priorities:
    /// decimal, double, float, int64, int32
    /// 
    /// 
    /// </remarks>
    public class VariantArithmetic
    {
        /// <summary>
        /// Returns the absolute value of a variant.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Variant Abs(Variant value)
        {
            if (value == null) return null;

            switch (value.ValueCase)
            {
                case Variant.ValueOneofCase.Int32Value: return new Variant(Math.Abs(value.Int32Value));
                case Variant.ValueOneofCase.Int64Value: return new Variant(Math.Abs(value.Int64Value));
                case Variant.ValueOneofCase.FloatValue: return new Variant(Math.Abs(value.FloatValue));
                case Variant.ValueOneofCase.DoubleValue: return new Variant(Math.Abs(value.DoubleValue));
                case Variant.ValueOneofCase.DecimalValue: return new Variant(Math.Abs(value.ToDecimal()));
            }
            return Variant.None;
        }

        /// <summary>
        /// Returns the sum of two variants.
        /// </summary>
        /// <param name="left">The first variant.</param>
        /// <param name="right">The second variant.</param>
        /// <returns>The result variant.</returns>
        public static Variant Add(Variant left, Variant right)
        {
            if (left == null || right == null) return null;

            if (left.ValueCase == Variant.ValueOneofCase.StringValue && right.ValueCase == Variant.ValueOneofCase.StringValue)
            {
                return new Variant(left.StringValue + right.StringValue);
            }

            if (left.ValueCase == Variant.ValueOneofCase.BoolValue && right.ValueCase == Variant.ValueOneofCase.BoolValue)
            {
                return new Variant(left.BoolValue | right.BoolValue);
            }

            if (!left.IsNumeric || !right.IsNumeric)
            {
                return Variant.None;
            }

            if (left.ValueCase == Variant.ValueOneofCase.DecimalValue || right.ValueCase == Variant.ValueOneofCase.DecimalValue)
            {
                return new Variant(left.ToDecimal() + right.ToDecimal());
            }
            if (left.ValueCase == Variant.ValueOneofCase.DoubleValue || right.ValueCase == Variant.ValueOneofCase.DoubleValue)
            {
                return new Variant(left.ToDouble() + right.ToDouble());
            }
            if (left.ValueCase == Variant.ValueOneofCase.FloatValue || right.ValueCase == Variant.ValueOneofCase.FloatValue)
            {
                return new Variant(left.ToFloat() + right.ToFloat());
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int64Value || right.ValueCase == Variant.ValueOneofCase.Int64Value)
            {
                return new Variant(left.ToInt64() + right.ToInt64());
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int32Value || right.ValueCase == Variant.ValueOneofCase.Int32Value)
            {
                return new Variant(left.ToInt32() + right.ToInt32());
            }
            return Variant.None;
        }

        /// <summary>
        /// Subtracts two variants.
        /// </summary>
        /// <param name="left">The first variant.</param>
        /// <param name="right">The second variant.</param>
        /// <returns>The result variant.</returns>
        public static Variant Subtract(Variant left, Variant right)
        {
            if (left == null || right == null) return null;

            if (!left.IsNumeric || !right.IsNumeric)
            {
                return Variant.None;
            }

            if (left.ValueCase == Variant.ValueOneofCase.DecimalValue || right.ValueCase == Variant.ValueOneofCase.DecimalValue)
            {
                return new Variant(left.ToDecimal() - right.ToDecimal());
            }
            if (left.ValueCase == Variant.ValueOneofCase.DoubleValue || right.ValueCase == Variant.ValueOneofCase.DoubleValue)
            {
                return new Variant(left.ToDouble() - right.ToDouble());
            }
            if (left.ValueCase == Variant.ValueOneofCase.FloatValue || right.ValueCase == Variant.ValueOneofCase.FloatValue)
            {
                return new Variant(left.ToFloat() - right.ToFloat());
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int64Value || right.ValueCase == Variant.ValueOneofCase.Int64Value)
            {
                return new Variant(left.ToInt64() - right.ToInt64());
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int32Value || right.ValueCase == Variant.ValueOneofCase.Int32Value)
            {
                return new Variant(left.ToInt32() - right.ToInt32());
            }
            return Variant.None;
        }


        /// <summary>
        /// Performs a bitwise AND operation between two variants of any integral type.
        /// </summary>
        /// <param name="left">The first variant.</param>
        /// <param name="right">The second variant.</param>
        /// <returns>
        /// Operator is predefined for the integral types and bool. For integral types, It computes the bitwise AND of its operands. For bool operands, it computes the logical AND of its operands.
        /// For other types it returns <see cref="Variant.None"/>.
        /// </returns>
        public static Variant BitwiseAnd(Variant left, Variant right)
        {
            if (left == null || right == null) return null;


            if (left.ValueCase == Variant.ValueOneofCase.BoolValue || right.ValueCase == Variant.ValueOneofCase.BoolValue)
            {
                return new Variant(left.ToBoolean() & right.ToBoolean());
            }
            if (!left.IsInteger || !right.IsInteger)
            {
                return Variant.None;
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int64Value || right.ValueCase == Variant.ValueOneofCase.Int64Value)
            {
                return new Variant(left.ToInt64() & right.ToInt64());
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int32Value || right.ValueCase == Variant.ValueOneofCase.Int32Value)
            {
                return new Variant(left.ToInt32() & right.ToInt32());
            }
            return Variant.None;
        }

        /// <summary>
        /// Performs a bitwise OR operation between two variants of any integral type.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>
        /// Operator is predefined for the integral types and bool. For integral types, It computes the bitwise OR of its operands. For bool operands, it computes the logical OR of its operands.
        /// For other types it returns <see cref="Variant.None"/>.
        /// </returns>
        public static Variant BitwiseOr(Variant left, Variant right)
        {
            if (left == null || right == null) return null;

            if (left.ValueCase == Variant.ValueOneofCase.BoolValue || right.ValueCase == Variant.ValueOneofCase.BoolValue)
            {
                return new Variant(left.ToBoolean() | right.ToBoolean());
            }
            if (!left.IsInteger || !right.IsInteger)
            {
                return Variant.None;
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int64Value || right.ValueCase == Variant.ValueOneofCase.Int64Value)
            {
                return new Variant(left.ToInt64() | right.ToInt64());
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int32Value || right.ValueCase == Variant.ValueOneofCase.Int32Value)
            {
                return new Variant(left.ToInt32() | right.ToInt32());
            }
            return Variant.None;
        }

        /// <summary>
        /// Performs the bitwise complement operation on a variant.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// The Not operator performs a bitwise complement operation on its operand, which has the effect of reversing each bit. 
        /// Bitwise complement operators are predefined for int, uint, long, and ulong. Not operator performs logical negation for boolean type.
        /// For other types it returns <see cref="Variant.None"/>.
        /// </returns>
        public static Variant BitwiseNot(Variant value)
        {
            if (value == null) return null;

            if (value.ValueCase == Variant.ValueOneofCase.BoolValue)
            {
                return new Variant(!value.ToBoolean());
            }
            if (value.ValueCase == Variant.ValueOneofCase.Int64Value )
            {
                return new Variant(~ value.Int64Value );
            }
            if (value.ValueCase == Variant.ValueOneofCase.Int32Value )
            {
                return new Variant(~ value.Int32Value);
            }
            return Variant.None;
        }

        /// <summary>
        /// Performs logical AND on a variant.
        /// </summary>
        /// <param name="left">The first variant.</param>
        /// <param name="right">The second variant.</param>
        /// <returns></returns>
        public static Variant AndAlso(Variant left, Variant right)
        {
            if (left == null || right == null) return null;
            return new Variant(left.ToBoolean() & right.ToBoolean());
        }

        /// <summary>
        /// Performs logical OR on a variant.
        /// </summary>
        /// <param name="left">The first variant.</param>
        /// <param name="right">The second variant.</param>
        /// <returns></returns>
        public static Variant OrElse(Variant left, Variant right)
        {
            if (left == null || right == null) return null;
            return new Variant(left.ToBoolean() | right.ToBoolean());
        }

        /// <summary>
        /// Performs logical negation on a variant.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Variant of type <see cref="Boolean"/>.</returns>
        public static Variant Neg(Variant value)
        {
            if (value == null) return null;
            return new Variant(!value.ToBoolean());

        }


        /// <summary>
        /// Returns the result from multiplying two variants
        /// </summary>
        /// <param name="left">The first variant.</param>
        /// <param name="right">The second variant.</param>
        /// <returns>The result variant.</returns>
        public static Variant Multiply(Variant left, Variant right)
        {
            if (left == null || right == null) return null;

            if (!left.IsNumeric || !right.IsNumeric)
            {
                return Variant.None;
            }

            if (left.ValueCase == Variant.ValueOneofCase.DecimalValue || right.ValueCase == Variant.ValueOneofCase.DecimalValue)
            {
                return new Variant(left.ToDecimal() * right.ToDecimal());
            }
            if (left.ValueCase == Variant.ValueOneofCase.DoubleValue || right.ValueCase == Variant.ValueOneofCase.DoubleValue)
            {
                return new Variant(left.ToDouble() * right.ToDouble());
            }
            if (left.ValueCase == Variant.ValueOneofCase.FloatValue || right.ValueCase == Variant.ValueOneofCase.FloatValue)
            {
                return new Variant(left.ToFloat() * right.ToFloat());
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int64Value || right.ValueCase == Variant.ValueOneofCase.Int64Value)
            {
                return new Variant(left.ToInt64() * right.ToInt64());
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int32Value || right.ValueCase == Variant.ValueOneofCase.Int32Value)
            {
                return new Variant(left.ToInt32() * right.ToInt32());
            }
            return Variant.None;
        }

        /// <summary>
        /// Returns the result from dividing two variants.
        /// </summary>
        /// <param name="left">The first variant.</param>
        /// <param name="right">The second variant.</param>
        /// <returns>The result variant.</returns>
        /// <remarks>
        /// The result value is double if both arguments are numericm otherwise the returnvalue is <see cref="Variant.None"/>.
        /// </remarks>
        public static Variant Divide(Variant left, Variant right)
        {
            if (left == null || right == null) return null;

            if (left.IsNumeric && right.IsNumeric)
            {
                return new Variant(left.ToDouble() / right.ToDouble());
            }
            else
            {
                return Variant.None;
            }
        }

        /// <summary>
        /// Divides two variants and returns only the remainder.
        /// </summary>
        /// <param name="left">The first variant.</param>
        /// <param name="right">The second variant.</param>
        /// <returns>
        /// The Modulo operator computes the remainder after dividing its first operand by its second. All numeric types have predefined remainder operators.
        /// If either operand is non numeric type <see cref="Variant.None"/> is the result.
        /// </returns>
        public static Variant Modulo(Variant left, Variant right)
        {
            if (left == null || right == null) return null;

            if (!left.IsNumeric || !right.IsNumeric)
            {
                return Variant.None;
            }

            if (left.ValueCase == Variant.ValueOneofCase.DecimalValue || right.ValueCase == Variant.ValueOneofCase.DecimalValue)
            {
                return new Variant(left.ToDecimal() % right.ToDecimal());
            }
            if (left.ValueCase == Variant.ValueOneofCase.DoubleValue || right.ValueCase == Variant.ValueOneofCase.DoubleValue)
            {
                return new Variant(left.ToDouble() % right.ToDouble());
            }
            if (left.ValueCase == Variant.ValueOneofCase.FloatValue || right.ValueCase == Variant.ValueOneofCase.FloatValue)
            {
                return new Variant(left.ToFloat() % right.ToFloat());
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int64Value || right.ValueCase == Variant.ValueOneofCase.Int64Value)
            {
                return new Variant(left.ToInt64() % right.ToInt64());
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int32Value || right.ValueCase == Variant.ValueOneofCase.Int32Value)
            {
                return new Variant(left.ToInt32() % right.ToInt32());
            }
            return Variant.None;
        }

        /// <summary>
        /// Tests if two variants represents an equal value.
        /// </summary>
        /// <param name="left">The first variant.</param>
        /// <param name="right">The second variant.</param>
        /// <returns>
        /// 
        /// </returns>
        public static Variant Equal(Variant left, Variant right)
        {
            if (left == null || right == null) return null;
            var c = Compare(left, right);
            return c == 0 ? Variant.True : Variant.False;
        }
        public static Variant NotEqual(Variant left, Variant right)
        {
            if (left == null || right == null) return null;
            var c = Compare(left, right);
            return c != 0 ? Variant.True : Variant.False;
        }
        public static Variant GreaterThan(Variant left, Variant right)
        {
            if (left == null || right == null) return null;
            var c = Compare(left, right);
            return c > 0 ? Variant.True : Variant.False;
        }
        public static Variant GreaterThanOrEqual(Variant left, Variant right)
        {
            if (left == null || right == null) return null;
            var c = Compare(left, right);
            return c >= 0 ? Variant.True : Variant.False;
        }
        public static Variant LessThan(Variant left, Variant right)
        {
            if (left == null || right == null) return null;
            var c = Compare(left, right);
            return c < 0 ? Variant.True : Variant.False;
        }
        public static Variant LessThanOrEqual(Variant left, Variant right)
        {
            if (left == null || right == null) return null;
            var c = Compare(left, right);
            return c <= 0 ? Variant.True : Variant.False;
        }
        public static int? Compare(Variant left, Variant right)
        {
            if (left == null || right == null) return null;
            if (left.ValueCase == Variant.ValueOneofCase.StringValue && right.ValueCase == Variant.ValueOneofCase.StringValue)
            {
                return string.Compare(left.StringValue, right.StringValue);
            }
            if (!left.IsNumeric || !right.IsNumeric)
            {
                return null;
            }

            if (left.ValueCase == Variant.ValueOneofCase.DecimalValue || right.ValueCase == Variant.ValueOneofCase.DecimalValue)
            {
                return Decimal.Compare(left.ToDecimal(),right.ToDecimal());
            }
            if (left.ValueCase == Variant.ValueOneofCase.DoubleValue || right.ValueCase == Variant.ValueOneofCase.DoubleValue)
            {
                return left.ToDouble().CompareTo(right.ToDouble());
            }
            if (left.ValueCase == Variant.ValueOneofCase.FloatValue || right.ValueCase == Variant.ValueOneofCase.FloatValue)
            {
                return left.ToFloat().CompareTo(right.ToFloat());
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int64Value || right.ValueCase == Variant.ValueOneofCase.Int64Value)
            {
                return left.ToInt64().CompareTo(right.ToInt64());
            }
            if (left.ValueCase == Variant.ValueOneofCase.Int32Value || right.ValueCase == Variant.ValueOneofCase.Int32Value)
            {
                return left.ToInt32().CompareTo(right.ToInt32());
            }
            return null;
        }

        /// <summary>
        /// Tests if the string representation of <paramref name="left"/> variant contains the string representaiton of <paramref name="right"/> variant.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Variant StringContains(Variant left, Variant right)
        {
            if (left == null || right == null) return null;
            var value = left.ToSTRING();
            var substr = right.ToSTRING();
            return new Variant(value.Contains(substr));
        }

        /// <summary>
        /// <see cref="StringMaches"/> operator applies a specified Perl-compatible regular expression (PCRE). 
        /// The operator is only defined for string operands.  
        /// </summary>
        /// <param name="left">The string to be matched against regular expression pattern.</param>
        /// <param name="right">The string representing regular expression patttern.</param>
        /// <returns></returns>
        public static Variant StringMatches(Variant left, Variant right)
        {
            if (left == null || right == null) return null;
            if (left.ValueCase == Variant.ValueOneofCase.StringValue && right.ValueCase == Variant.ValueOneofCase.StringValue)
            {
                return new Variant(Regex.IsMatch(left.StringValue, right.StringValue));
            }
            else
                return Variant.None;
        }


        /// <summary>
        /// Concatenates string representations of operands.
        /// </summary>
        /// <param name="left">The first variant.</param>
        /// <param name="right">The second variant.</param>
        /// <returns>Concatenated string representations of operands. This operator is defined for all types.</returns>
        public static Variant StringConcat(Variant left, Variant right)
        {
            if (left == null || right == null) return null;
            return new Variant(left.ToSTRING() + right.ToSTRING());
        }
    }   
}
