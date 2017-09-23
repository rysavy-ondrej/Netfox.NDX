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
        public void Console_TrackConversations_Pile()
        {
                var maproot = Path.ChangeExtension(captureFile, "map");
                if (!Directory.Exists(maproot)) Directory.CreateDirectory(maproot);
                var frames = Capture.Select(captureFile);
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

        [Test]
        public void Conversations_TrackConversations_Linq()
        {
            var frames = Capture.Select(captureFile);
            var metaFrames = Conversations.TrackConversations(frames, out IDictionary<int, Conversation> conversations);
                                                
        }
    }
}
