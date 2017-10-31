using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Model
{
    /// <summary>
    /// Implements Variant data type. This type can represent 
    /// a basic collection of scalar values.
    /// </summary>
    public sealed partial class Variant
    {
        public static Variant None => new Variant();

        public static Variant False => new Variant(false);

        public static Variant True => new Variant(true);

        public bool IsNumeric => this.ValueCase == ValueOneofCase.DecimalValue
            || this.ValueCase == ValueOneofCase.DoubleValue
            || this.ValueCase == ValueOneofCase.FloatValue
            || this.ValueCase == ValueOneofCase.Int64Value
            || this.ValueCase == ValueOneofCase.Int32Value;

        public bool IsInteger => this.ValueCase == ValueOneofCase.Int64Value
            || this.ValueCase == ValueOneofCase.Int32Value;

        public bool IsEmpty => this.ValueCase == ValueOneofCase.None;

        public Variant(bool value)
        {
            this.BoolValue = value;
        }

        public Variant(int value)
        {
            this.Int32Value = value;   
        }

        public Variant(long value)
        {
            this.Int64Value = value;
        }

        public Variant(byte[] value)
        {
            this.BytesValue = ByteString.CopyFrom(value);
        }

        public Variant(ByteString value)
        {
            this.BytesValue =value;
        }

        public Variant(DateTime value)
        {
            this.DateTimeValue = value.ToBinary();
        }

        public Variant(decimal value)
        {
            this.DecimalValue = value.ToString();
        }

        public Variant(double value)
        {
            this.DoubleValue = value;
        }

        public Variant(float value)
        {
            this.FloatValue = value;
        }

        public Variant(string value)
        {
            this.StringValue = value;
        }

        /// <summary>
        /// Creates a new Variant instance from the object value.
        /// The Variant type is inferred from the object type.
        /// </summary>
        /// <param name="value"></param>
        public Variant(object value)
        {
            switch(value)
            {
                case bool boolValue:        BoolValue = boolValue; break;
                case string stringValue:    StringValue = stringValue; break;
                case SByte intValue:        Int32Value = intValue; break;
                case Int16 intValue:        Int32Value = intValue; break;
                case Int32 intValue:        Int32Value = intValue; break;
                case Int64 intValue:        Int64Value = intValue; break;
                case UInt16 intValue:       Int32Value = intValue; break;
                case UInt32 intValue:       Int64Value = intValue; break;
                case UInt64 intValue:       DecimalValue = intValue.ToString(); break;
                case Decimal decimalValue:  DecimalValue = decimalValue.ToString(); break; 
                case byte[] bytes:          BytesValue = ByteString.CopyFrom(bytes); break;
                default: break;                
            }
        }

        public object ToObject()
        {
            return this.value_;
        }

        public bool ToBoolean()
        {
            return (bool)Convert.ChangeType(value_, TypeCode.Boolean);
        }

        public Variant AsBoolean()
        {
            return new Variant(ToBoolean());
        }

        public int ToInt32()
        {
            return (int)Convert.ChangeType(value_, TypeCode.Int32);
        }

        public Variant AsInt32()
        {
            return new Variant(ToInt32());
        }

        public long ToInt64()
        {
            return (long)Convert.ChangeType(value_, TypeCode.Int64);
        }

        public Variant AsInt64()
        {
            return new Variant(ToInt64());
        }

        public decimal ToDecimal()
        {
            return (decimal)Convert.ChangeType(value_, TypeCode.Decimal);
        }

        public Variant AsDecimal()
        {
            return new Variant(ToDecimal());
        }

        public float ToFloat()
        {
            return (float)Convert.ChangeType(value_, TypeCode.Single);
        }

        public Variant AsFloat()
        {
            return new Variant(ToFloat());
        }

        public double ToDouble()
        {
            return (double)Convert.ChangeType(value_, TypeCode.Double);
        }

        public Variant AsDouble()
        {
            return new Variant(ToDouble());
        }

        /// <summary>
        /// Gets the string representation of the Variant or null if the variant is empty.
        /// </summary>
        /// <returns>
        /// The string representation of the Variant. If the current Variant is empty then the result if null.
        /// </returns>
        public string ToSTRING()
        {
            if (ValueCase == ValueOneofCase.StringValue) return StringValue;
            return (string)Convert.ChangeType(value_, TypeCode.String);
        }

        public Variant AsString()
        {
            return new Variant(ToSTRING());
        }

        ValueOneofCase GetCase(TypeCode tc)
        {
            switch(tc)
            {
                case TypeCode.Empty: return ValueOneofCase.None;
                case TypeCode.Boolean: return ValueOneofCase.BoolValue;
                case TypeCode.DateTime: return ValueOneofCase.DateTimeValue;
                case TypeCode.Decimal: return ValueOneofCase.DecimalValue;
                case TypeCode.Double: return ValueOneofCase.DoubleValue;
                case TypeCode.String: return ValueOneofCase.StringValue;
                case TypeCode.Int32: return ValueOneofCase.Int32Value;
                case TypeCode.Int64: return ValueOneofCase.Int64Value;
                default: return ValueOneofCase.BytesValue;
            }
        }

        public Variant As(TypeCode type)
        {
            return new Variant() { value_ = Convert.ChangeType(value_, type), valueCase_ = GetCase(type) };
        }

        /// <summary>
        /// Gets the IP address representation of the current Variant.
        /// </summary>
        /// <returns><see cref="IPAddress"/> that represents the current variant. If the variant
        /// has not suitable type or format to be arepresented as IP address, <see cref="null"/> returned.</returns>
        public IPAddress ToIPAddress()
        {
            switch(ValueCase)
            {
                case ValueOneofCase.StringValue: return IPAddress.TryParse(StringValue, out var ipaddress) ? ipaddress : null;
                case ValueOneofCase.Int32Value: return new IPAddress(Int32Value);
                case ValueOneofCase.Int64Value: return new IPAddress(Int64Value);
                case ValueOneofCase.BoolValue: return new IPAddress(BytesValue.ToByteArray());
                default: return null;
            }
        }
    }
}
