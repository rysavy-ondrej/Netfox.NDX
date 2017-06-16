using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Ingest;
using NUnit.Framework;
using Ndx.Captures;
using System.Linq;
using Ndx.Model;

namespace Ndx.Test
{
    /// <summary>
    /// This class test the whole Ingest Pcap processing pipeline.
    /// </summary>
    [TestFixture]
    public class ReadFiles
    {
        TestContext testContext = TestContext.CurrentContext;
        [Test]
        public void ReadNetmonEnumerable()
        {
            //var input = Path.Combine(testContext.TestDirectory, @"..\..\..\TestData\http.cap");
            var input = @"C:\Users\Ondrej\Documents\Network Monitor 3\Captures\2adc3aaa83b46ef8d86457e0209e0aa9.cap";
            var items = PcapReader.ReadFile(input);
            var count = items.Count();
        }

        [Test]
        public void ReadNetmonDataflow()
        {
            var count = 0;
            var buffer = new ActionBlock<RawFrame>((x) => count++);            
            var input = @"C:\Users\Ondrej\Documents\Network Monitor 3\Captures\2adc3aaa83b46ef8d86457e0209e0aa9.cap";
            var items = PcapReader.ReadFile(input);
            foreach (var item in items)
            {
                buffer.Post(item);
            }
            buffer.Complete();
            Task.WaitAll(buffer.Completion);
        }
    }
}
