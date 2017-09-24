using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Ingest;
using Ndx.Model;
using Ndx.Shell.Console;
using Ndx.Utils;
using NFX.ApplicationModel.Pile;
using NUnit.Framework;
namespace Ndx.Test
{
    /// <summary>
    /// This test uses NFX Object Pile to store conversations and frame metadata.
    /// https://github.com/aumcode/nfx/tree/master/Source/NFX/ApplicationModel/Pile
    /// </summary>
    [TestFixture]
    public class ConversationsTest
    {
        static TestContext m_testContext = TestContext.CurrentContext;
        string captureFile = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.cap");
        string converFile = Path.Combine(m_testContext.TestDirectory, @"..\..\..\TestData\http.conv");
       
        [Test]
        public void Conversations_TrackConversations_Linq()
        {
            var frames = Capture.ReadAllFrames(new[] { new Capture(captureFile) });
            var tracker = new ConversationTracker();
            var _conversations = new Dictionary<int, Conversation>();
            var observer = new Ndx.Utils.Observer<Conversation>(x => _conversations.Add(x.ConversationId, x));
            using (var t = tracker.Subscribe(observer))
            {
                var labeledFrames = frames.Select(x => { var c = tracker.ProcessFrame(x); x.ConversationId = c.ConversationId; return x; }).Where(x => x != null).ToList();
                tracker.Complete();
            }
        }

        [Test]
        public void Conversations_TrackConversations_WriteTo()
        {
            var zipfile = Path.ChangeExtension(captureFile, "zip");
            var frames = Capture.ReadAllFrames(new[] { new Capture(captureFile) });

            var conversationsTable = new Dictionary<int, Conversation>();

            var tracker = new ConversationTracker();
            Frame ProcessFrame(Frame f)
            {
                var c = tracker.ProcessFrame(f);
                f.ConversationId = c.ConversationId;
                if (!conversationsTable.ContainsKey(c.ConversationId))
                {
                    conversationsTable[c.ConversationId] = c;
                }
                return f;
            }

            var framesTable = frames.Select(ProcessFrame).Where(x => x != null).ToEnumerable().ToDictionary(x=>x.FrameNumber);

            File.Delete(zipfile);
            using (var archive = ZipFile.Open(zipfile, ZipArchiveMode.Create))
            {
                archive.WriteTo("conversations", conversationsTable, x => x.Value);
                archive.WriteTo("frames", framesTable, x => x.Value);
            }
        }
    }
}
