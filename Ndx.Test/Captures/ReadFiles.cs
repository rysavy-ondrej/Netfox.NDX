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



        [Test]
        public void ReadLinuxLinkType()
        {
            var input = Path.Combine(testContext.TestDirectory, @"..\..\..\TestData\CookedLink.cap");
            var items = PcapReader.ReadFile(input);
            var count = items.Count();
            foreach (var p in items.Select(x => x.Parse()))
            {
                Console.WriteLine(p);
            }
        }


        [Test]
        public void ReadLinuxLinkTypeAndStoreAsEthernet()
        {
            var input = Path.Combine(testContext.TestDirectory, @"..\..\..\TestData\CookedLink.cap");
            var output = Path.Combine(testContext.TestDirectory, @"..\..\..\TestData\CookedEthernet.cap");
            var items = PcapReader.ReadFile(input);
            var frames = items.Select(x => RawFrame.EthernetRaw(x.Parse(), x.FrameNumber, 0, x.TimeStamp));
            LibPcapFile.WriteAllFrames(output, DataLinkType.Ethernet, frames);
        }
    }
}
