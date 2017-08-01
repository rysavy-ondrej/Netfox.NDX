using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using Ndx.Shell.Console;
using NUnit.Framework;
namespace Ndx.Test
{
    [TestFixture]
    public class ConversationsTest
    {
        static TestContext m_testContext = TestContext.CurrentContext;
        string captureFile = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap");
        string converFile = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.conv");
        [Test]
        public void Console_Conversations_WriteTo_ReadFrom()
        {
            if (File.Exists(converFile)) File.Delete(converFile);

            var frames = Capture.Select(captureFile);
            var tables = Conversations.TrackConversations(frames);            
            Conversations.WriteTo(tables.Item1, tables.Item2, converFile);
            var ctable = new ConversationTable();
            var ftable = new FrameTable();
            Conversations.MergeFrom(converFile, ctable, ftable);
            // Print first 10 conversations including their packets
            foreach(var item in ctable.Items.Take(10))
            {
                Console.WriteLine(item.Value);
                foreach(var p in item.Value.UpflowPackets)
                {
                    Console.WriteLine($">> {ftable.Items[(int)p]}");
                }
                foreach (var p in item.Value.DownflowPackets)
                {
                    Console.WriteLine($"<< {ftable.Items[(int)p]}");
                }
            }
        }

        [Test]
        public void FindBug()
        {

            foreach (var host in new[] { "192.168.186.45" })
            {
                var files = Directory.EnumerateFiles(@"P:\Data\emea").ToArray();
                var filter = FlowExpr.Address(host);
                var frames = Capture.Select(files, (file, count) => Console.WriteLine($"{file}: {count} frames"))
                     .Select(x => new { ts = x.TimeStamp, packet = x.Parse() }).Where(x => filter.GetPacketFilter()(x.packet)).Select(x => RawFrame.EthernetRaw(x.packet, 0, 0, x.ts));
                Capture.WriteAllFrames($@"C:\Temp\{host}.cap", frames);
            }
        }
    }
}
