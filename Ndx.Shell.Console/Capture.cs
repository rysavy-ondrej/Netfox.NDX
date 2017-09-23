using System;
using System.Collections.Generic;
using System.IO;
using Ndx.Captures;
using Ndx.Model;
using PacketDotNet;
using System.Linq;

namespace Ndx.Shell.Console
{
    public static class Capture
    {      

        /// <summary>
        /// Reads frames for the specified collection of capture files.
        /// </summary>
        /// <param name="captures">Collection of capture files.</param>
        /// <returns>A collection of frames read sequentially from the specified capture files.</returns>
        public static IEnumerable<Frame> Select(IEnumerable<string> captures, Action<string,int> progressCallback = null)
        {
            foreach (var capture in captures)
            {
                int frameCount = 0;
                foreach (var frame in PcapReader.ReadFile(capture))
                {
                    frameCount++;
                    yield return frame;                    
                }
                progressCallback?.Invoke(capture, frameCount);
            }
        }

        public static IEnumerable<Frame> Select(string capture)
        {
                foreach (var frame in PcapReader.ReadFile(capture))
                {
                    yield return frame;
                }
        }

        public static void WriteAllFrames(string capturefile, IEnumerable<Frame> frames, DataLinkType link = DataLinkType.Ethernet)
        {
            LibPcapFile.WriteAllFrames(capturefile, link, frames);
        }
    }
}
