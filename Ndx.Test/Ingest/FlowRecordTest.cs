using Ndx.Metacap;
using Ndx.Utils;
using NUnit.Framework;
using System;
using Ndx.Model;

namespace Ndx.Test.Ingest
{
    [TestFixture]
    public class FlowRecordTest
    {
        [Test]
        public void FlowRecord_StoreLoad()
        {
            var x = new FlowRecord()
            {
                ApplicationId = (int)ApplicationProtocol.HTTP,
                FirstSeen = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                LastSeen = DateTimeOffset.Now.ToUnixTimeMilliseconds() + 1000,
                Octets = 30000,
                Packets = 150
            };

            var buffer = x.GetBytes();

            var y = new FlowRecord(buffer);

            Assert.AreEqual(x, y);
        }
    }
}
