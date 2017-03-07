using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Ingest.Trace;
using Ndx.Utils;
namespace Ndx.Test.Ingest
{
    [TestFixture]
    public class FlowRecordTest
    {
        [Test]
        public void FlowRecord_StoreLoad()
        {
            var x = new _FlowRecord()
            {
                application = (uint)ApplicationProtocol.HTTP,
                blocks = 10,
                first = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                last = DateTimeOffset.Now.ToUnixTimeMilliseconds() + 1000,
                octets = 30000,
                packets = 150
            };

            var buffer = x.GetBytes();

            var y = new _FlowRecord(buffer);

            Assert.AreEqual(x, y);
        }
    }
}
