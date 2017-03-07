using Ndx.Ingest.Trace;
using Ndx.Ingest.Trace.DataConsumers;
using Ndx.Ingest.Trace.Dataflow;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ndx.Test
{
    /// <summary>
    /// This class test the whole Ingest Pcap processing pipeline.
    /// </summary>
    [TestFixture]
    public class LoadFromTest
    {
        [Test]
        public void IngestTest33K()
        {
            var consumer = new NullConsumer();                       
            var cts = new CancellationTokenSource();            
            var reader = new PcapReaderProvider(32768, 1000, cts.Token);
            var ingest = new PcapFileIngestor(reader.RawFrameSource, consumer.RawFrameTarget, consumer.PacketBlockTarget, consumer.FlowRecordTarget, new IngestOptions());

            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Captures\03d1d7f3e7bc76aa22271f67463d8c3d.cap");
            var fileInfo = new FileInfo(path);
            reader.ReadFrom(fileInfo);
            reader.Complete();
            Task.WaitAll(ingest.Completion, consumer.Completion);
            Assert.AreEqual(42, consumer.RawFrameCount);
            Assert.AreEqual(14, consumer.PacketBlockCount);
            Assert.AreEqual(14, consumer.FlowRecordCount);
        }

        [Test]
        public void IngestTest5M()
        {
            var consumer = new NullConsumer();
            var cts = new CancellationTokenSource();
            var reader = new PcapReaderProvider(32768, 1000, cts.Token);
            var ingest = new PcapFileIngestor(reader.RawFrameSource, consumer.RawFrameTarget, consumer.PacketBlockTarget, consumer.FlowRecordTarget, new IngestOptions());

            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Captures\22797e5151de8ccc0ee7106707c53bdd.pcap");
            var fileInfo = new FileInfo(path);
            reader.ReadFrom(fileInfo);
            reader.Complete();
            Task.WaitAll(ingest.Completion, consumer.Completion);
            Assert.AreEqual(7567, consumer.RawFrameCount);
            Assert.AreEqual(436, consumer.PacketBlockCount);
            Assert.AreEqual(374, consumer.FlowRecordCount);
        }

        [Test]
        public void IngestTestNullFrameTarget()
        {
            var consumer = new NullConsumer();
            var cts = new CancellationTokenSource();
            var reader = new PcapReaderProvider(32768, 1000, cts.Token);
            var ingest = new PcapFileIngestor(reader.RawFrameSource, null, consumer.PacketBlockTarget, consumer.FlowRecordTarget, new IngestOptions());

            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Captures\22797e5151de8ccc0ee7106707c53bdd.pcap");
            var fileInfo = new FileInfo(path);
            reader.ReadFrom(fileInfo);
            reader.Complete();
            Task.WaitAll(ingest.Completion, consumer.Completion);
            Assert.AreEqual(0, consumer.RawFrameCount);
            Assert.AreEqual(436, consumer.PacketBlockCount);
            Assert.AreEqual(374, consumer.FlowRecordCount);

        }

        [Test]
        public void IngestTestFileOutput()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"Captures\22797e5151de8ccc0ee7106707c53bdd.pcap");
            var consumer = IngestFile(path);
            Assert.AreEqual(0, consumer.RawFrameCount);
            Assert.AreEqual(436, consumer.PacketBlockCount);
            Assert.AreEqual(374, consumer.FlowRecordCount);
        }


        [Test]
        public void IngestTestFileOutput_220M()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"C:\Users\Ondrej\Documents\Network Monitor 3\Captures\bb7de71e185a2a7818fff92d3ec0dc05.cap");
            var consumer = IngestFile(path);
            Assert.AreEqual(0, consumer.RawFrameCount);
            Assert.AreEqual(5351, consumer.PacketBlockCount);
            Assert.AreEqual(2350, consumer.FlowRecordCount);
        }
        

        [Test]
        public void IngestTestFileOutput_1p2G()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"C:\Users\Ondrej\Documents\Network Monitor 3\Captures\2adc3aaa83b46ef8d86457e0209e0aa9.cap");
            var consumer = IngestFile(path);
            Assert.AreEqual(0, consumer.RawFrameCount);
            Assert.AreEqual(20028, consumer.PacketBlockCount);
            Assert.AreEqual(1795, consumer.FlowRecordCount);
        }

        FileConsumer IngestFile(string path)
        {
            var consumer = new FileConsumer(path);
            var cts = new CancellationTokenSource();
            var reader = new PcapReaderProvider(32768, 1000, cts.Token);
            var ingest = new PcapFileIngestor(reader.RawFrameSource, null, consumer.PacketBlockTarget, consumer.FlowRecordTarget, new IngestOptions());

            var fileInfo = new FileInfo(path);
            reader.ReadFrom(fileInfo);
            reader.Complete();
            Task.WaitAll(ingest.Completion, consumer.Completion);
            consumer.Close();
            return consumer;
        }

        ZipFileConsumer IngestFileZipConsumer(string path)
        {
            var consumer = new ZipFileConsumer(path);
            var cts = new CancellationTokenSource();
            var reader = new PcapReaderProvider(32768, 1000, cts.Token);
            var ingest = new PcapFileIngestor(reader.RawFrameSource, null, consumer.PacketBlockTarget, consumer.FlowRecordTarget, new IngestOptions());

            var fileInfo = new FileInfo(path);
            reader.ReadFrom(fileInfo);
            reader.Complete();
            Task.WaitAll(ingest.Completion);
            consumer.Close();
            return consumer;
        }

        [Test]
        public void IngestTestZipOutput_220M()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"C:\Users\Ondrej\Documents\Network Monitor 3\Captures\bb7de71e185a2a7818fff92d3ec0dc05.cap");
            var consumer = IngestFileZipConsumer(path);
            Assert.AreEqual(0, consumer.RawFrameCount);
            Assert.AreEqual(5351, consumer.PacketBlockCount);
            Assert.AreEqual(2350, consumer.FlowRecordCount);
        }
        [Test]
        public void IngestTestZipOutput_1p2G()
        {
            var path = Path.Combine(TestContext.CurrentContext.TestDirectory, @"C:\Users\Ondrej\Documents\Network Monitor 3\Captures\2adc3aaa83b46ef8d86457e0209e0aa9.cap");
            var consumer = IngestFileZipConsumer(path);
            Assert.AreEqual(0, consumer.RawFrameCount);
            Assert.AreEqual(20028, consumer.PacketBlockCount);
            Assert.AreEqual(1795, consumer.FlowRecordCount);
        }

    }
}
