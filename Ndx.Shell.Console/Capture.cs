using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Ndx.Captures;
using Ndx.Model;

namespace Ndx.Shell.Console
{
    public class Capture
    {
        FileInfo m_fileInfo;
        public Capture(string path)
        {
            m_fileInfo = new FileInfo(path);
        }
        /// <summary>
        /// Reads frames for the specified collection of capture files.
        /// </summary>
        /// <param name="captures">Collection of capture files.</param>
        /// <returns>A collection of frames read sequentially from the specified capture files.</returns>
        public static IObservable<Frame> ReadAllFrames(IEnumerable<Capture> captures, Action<string,int> progressCallback = null)
        {
            return captures.ToObservable().SelectMany(x => PcapReader.ReadFile(x.m_fileInfo.FullName));
        }
    }
}
