using System;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace Ndx.Utils
{
    /// <summary>
    /// The NVariant class represents a value-type pair, which facilitates the type convertion of values, as well as mathematical and logical operations with values of different types.
    /// </summary>
    /// <remarks>
    /// TODO: Implement as in http://helpdotnetvision.nevron.com/Nevron.System~Nevron.FormulaEngine.NVariant.html.
    /// </remarks>


    public abstract class NVariant : IConvertible
    {
        protected static IFormatProvider formatProvider = new NumberFormatInfo();

        /*
        public void Make<T>(out T item)
        {
            JSON.MakeInto<T>(this, out item);
        }


        public T Make<T>()
        {
            T item;
            JSON.MakeInto<T>(this, out item);
            return item;
        }
        */
        static NVariant m_empty = new NVariant.ProxyEmpty();
        public static NVariant Empty => m_empty;


        public static NVariant Make(JValue value)
        {
            
        }

        public virtual TypeCode GetTypeCode()
        {
            return TypeCode.Object;
        }





        public virtual object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to " + conversionType.Name);
        }


        public virtual DateTime ToDateTime(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to DateTime");
        }


        public virtual bool ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to Boolean");
        }

        public virtual byte ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to Byte");
        }


        public virtual char ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to Char");
        }


        public virtual decimal ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to Decimal");
        }


        public virtual double ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to Double");
        }


        public virtual short ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to Int16");
        }


        public virtual int ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to Int32");
        }


        public virtual long ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to Int64");
        }


        public virtual sbyte ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to SByte");
        }


        public virtual float ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to Single");
        }


        public virtual string ToString(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to String");
        }


        public virtual ushort ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to UInt16");
        }


        public virtual uint ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to UInt32");
        }


        public virtual ulong ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert " + this.GetType() + " to UInt64");
        }


        public override string ToString()
        {
            return ToString(formatProvider);
        }


        public virtual NVariant this[string key]
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }


        public virtual NVariant this[int index]
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }


        public static implicit operator Boolean(NVariant variant)
        {
            return variant.ToBoolean(formatProvider);
        }


        public static implicit operator Single(NVariant variant)
        {
            return variant.ToSingle(formatProvider);
        }


        public static implicit operator Double(NVariant variant)
        {
            return variant.ToDouble(formatProvider);
        }


        public static implicit operator UInt16(NVariant variant)
        {
            return variant.ToUInt16(formatProvider);
        }


        public static implicit operator Int16(NVariant variant)
        {
            return variant.ToInt16(formatProvider);
        }


        public static implicit operator UInt32(NVariant variant)
        {
            return variant.ToUInt32(formatProvider);
        }


        public static implicit operator Int32(NVariant variant)
        {
            return variant.ToInt32(formatProvider);
        }


        public static implicit operator UInt64(NVariant variant)
        {
            return variant.ToUInt64(formatProvider);
        }


        public static implicit operator Int64(NVariant variant)
        {
            return variant.ToInt64(formatProvider);
        }


        public static implicit operator Decimal(NVariant variant)
        {
            return variant.ToDecimal(formatProvider);
        }


        public static implicit operator String(NVariant variant)
        {
            return variant.ToString(formatProvider);
        }

        // Concrete classes:
        sealed class ProxyBoolean : NVariant
        {
            private bool value;


            public ProxyBoolean(bool value)
            {
                this.value = value;
            }


            public override bool ToBoolean(IFormatProvider provider)
            {
                return value;
            }
        }

        sealed class ProxyEmpty : NVariant
        {
        }
    }
}
