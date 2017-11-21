using Ndx.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Test.Model
{
    [TestFixture]
    class VariantTest
    {
        [Test]
        public void Variant_BasicTests()
        {
            var v1 = Variant.None;
            var v2 = new Variant("12143");
            var v3 = Variant.None;
            Assert.IsFalse(v3.IsInteger);
            Assert.IsFalse(v3.IsNumeric);
            Assert.IsTrue(v3.IsEmpty);
            Assert.AreEqual(v2.StringValue, "12143");
            Assert.AreEqual(v2.BoolValue, default(bool));
            Assert.AreEqual(v2.ToInt32(), 12143);
            Assert.IsNull(v1.ToSTRING());            
        }
        [Test]
        public void Variant_ToIpAddressTest()
        {
            var v4 = new Variant((object)"123.13.44.45");
            Assert.AreEqual(v4.ToIPAddress(), IPAddress.Parse("123.13.44.45"));
            var v5 = new Variant(757861755);
            Assert.AreEqual(v5.ToIPAddress(), new IPAddress(757861755));
        }
    }
}
