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

        public bool IsNumeric => 
               this.ValueCase == ValueOneofCase.DoubleValue
            || this.ValueCase == ValueOneofCase.FloatValue
            || this.ValueCase == ValueOneofCase.Int64Value
            || this.ValueCase == ValueOneofCase.Int32Value
            || this.ValueCase == ValueOneofCase.UInt64Value
            || this.ValueCase == ValueOneofCase.UInt32Value;

        public bool IsInteger => 
               this.ValueCase == ValueOneofCase.Int64Value
            || this.ValueCase == ValueOneofCase.Int32Value
            || this.ValueCase == ValueOneofCase.UInt64Value
            || this.ValueCase == ValueOneofCase.UInt32Value;

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

        public Variant(uint value)
        {
            this.UInt32Value = value;
        }

        public Variant(ulong value)
        {
            this.UInt64Value = value;
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

        public Variant(IPAddress value)
        {
            this.IpAddressValue = value.ToString();
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
        /// <param name="obj"></param>
        public Variant(object obj)
        {
            switch(obj)
            {
                case bool value:        BoolValue = value; break;
                case string value:      StringValue = value; break;
                case SByte value:       Int32Value = value; break;
                case Int16 value:       Int32Value = value; break;
                case Int32 value:       Int32Value = value; break;
                case Int64 value:       Int64Value = value; break;
                case Byte value:        UInt32Value = value; break;
                case UInt16 value:      UInt32Value = value; break;
                case UInt32 value:      UInt64Value = value; break;
                case UInt64 value:      UInt64Value = value;  break;
                case IPAddress value:   IpAddressValue = value.ToString(); break; 
                case byte[] value:      BytesValue = ByteString.CopyFrom(value); break;
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

        public uint ToUInt32()
        {
            return (uint)Convert.ChangeType(value_, TypeCode.UInt32);
        }

        public Variant AsUInt32()
        {
            return new Variant(ToUInt32());
        }

        public ulong ToUInt64()
        {
            return (ulong)Convert.ChangeType(value_, TypeCode.UInt64);
        }

        public Variant AsUInt64()
        {
            return new Variant(ToUInt64());
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
                case TypeCode.Double: return ValueOneofCase.DoubleValue;
                case TypeCode.String: return ValueOneofCase.StringValue;
                case TypeCode.UInt32: return ValueOneofCase.UInt32Value;
                case TypeCode.UInt64: return ValueOneofCase.UInt64Value;
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

        public Variant AsIPAddress()
        {
            return new Variant(ToIPAddress());
        }
    }
}
