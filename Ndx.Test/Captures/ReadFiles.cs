using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Ingest;
using NUnit.Framework;
using Ndx.Captures;
using System.Linq;
using Ndx.Model;
using PacketDotNet;
using System.Reactive.Linq;

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
            var input = Path.Combine(testContext.TestDirectory, @"..\..\..\TestData\http.cap");
            var items = PcapFile.ReadFile(input);
            var count = items.Count();
        }

        [Test]
        public void ReadNetmonDataflow()
        {
            var count = 0;
            var buffer = new ActionBlock<Frame>((x) => count++);
            var input = @"C:\Users\Ondrej\Documents\Network Monitor 3\Captures\2adc3aaa83b46ef8d86457e0209e0aa9.cap";
            var items = PcapFile.ReadFile(input);
            var task = items.ForEachAsync(async item =>
            {
                await buffer.SendAsync(item);
            });
            buffer.Complete();
            Task.WaitAll(task, buffer.Completion);
        }



        [Test]
        public void ReadLinuxLinkType()
        {
            var input = Path.Combine(testContext.TestDirectory, @"..\..\..\TestData\CookedLink.cap");
            var items = PcapFile.ReadFile(input);
            var count = items.Count();
            items.Select(x => x.Parse()).ForEach(p => Console.WriteLine(p));
        }


        [Test]
        public async Task ReadLinuxLinkTypeAndStoreAsEthernet()
        {
            var input = Path.Combine(testContext.TestDirectory, @"..\..\..\TestData\CookedLink.cap");
            var output = Path.Combine(testContext.TestDirectory, @"..\..\..\TestData\CookedEthernet.cap");
            var items = PcapFile.ReadFile(input);
            var frames = items.Select(x => Frame.EthernetRaw(x.Parse(), x.FrameNumber, 0, x.TimeStamp));
            await LibPcapStream.WriteAllFramesAsync(output, DataLinkType.Ethernet, frames);
        }


        [Test]
        public async Task ReadPcapJson()
        {
            var input = Path.Combine(testContext.TestDirectory, @"..\..\..\TestData\http.json");
            var items = PcapFile.ReadJson(input);
            await items.ForEachAsync(Console.WriteLine);            
        }
    }
}
