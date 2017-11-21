using Ndx.Decoders;
using NUnit.Framework;

namespace Ndx.Test.Decoders
{
    [TestFixture()]
    public class DecoderFactoryTest
    {
        [Test()]
        public void DecoderFactoryCreateTest()
        {
            var factory = new DecoderFactory();
            Assert.True(factory.Decoders.Count > 0, "DecoderFactory not properly initialized. No decoders found!");
        }
    }
}