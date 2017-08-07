using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using Ndx.Shell.Console;
using Ndx.Utils;
using NFX.ApplicationModel.Pile;
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
        public void Console_TrackConversations_WriteToPile()
        {
            // foreach (var item in new[] { "192.168.186.18.cap", "192.168.186.21.cap", "192.168.186.22.cap", "192.168.186.43.cap", "192.168.186.44.cap", "192.168.186.45.cap", "192.168.186.46.cap", "192.168.186.47.cap", "192.168.186.168.cap" })
            {
                var item = "192.168.186.21.cap";
                var input = $@"C:\Temp\{item}";
                var maproot = Path.ChangeExtension(input, "map");
                if (!Directory.Exists(maproot)) Directory.CreateDirectory(maproot);
                var frames = Capture.Select(input);
                var pile = new MMFPile() { DataDirectoryRoot = maproot };
                pile.Start();

                var buffer = new byte[1234];
                var ptr1 = pile.Put(buffer);
                var ptr2 = pile.Put(buffer);
                var ptr3 = pile.Put(buffer);
                var got = pile.Get(ptr1) as byte[];

                var ctable = new Dictionary<int, PilePointer>();
                var ftable = new Dictionary<int, PilePointer>();
                Conversations.TrackConversations(frames, pile, ctable, ftable);
                
                pile.WaitForCompleteStop();
                pile.Dispose();
            }
        }


        [Test]
        public void BigInputFileTest()
        {
            foreach (var item in new[] { "192.168.186.18.cap", "192.168.186.21.cap", "192.168.186.22.cap", "192.168.186.43.cap", "192.168.186.44.cap", "192.168.186.45.cap", "192.168.186.46.cap", "192.168.186.47.cap", "192.168.186.168.cap" })
            {
                var path = $@"C:\Temp\{item}";
                long length = new FileInfo(path).Length;
                var sw = new Stopwatch();
                sw.Start();
                Console.WriteLine($"{sw.Elapsed}: processing '{path}', length={Format.ByteSize(length)}.");
                var sshFrames = Capture.Select(path);
                var frames = 0;
                var max = sshFrames.Select(MetaFrame.Create).Max(x => { frames++; return x.FrameLength; });
                Console.WriteLine($"{sw.Elapsed}: finished: total frames={frames},  max frame={max}B.");
            }
        }
    }
}
