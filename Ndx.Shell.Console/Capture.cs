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
        public static RawFrame GetRawFrame(this MetaFrame metaframe, Stream stream)
        {
            var bytes = Capture.GetFrameBytes(metaframe, stream);
            if (bytes != null) return new RawFrame(metaframe, bytes);
            return null;
        }

        static byte[] GetFrameBytes(this MetaFrame metaframe, Stream stream)
        {
            try
            {
                stream.Position = metaframe.FrameOffset;
                var buffer = new byte[metaframe.FrameLength];
                var result = stream.Read(buffer, 0, metaframe.FrameLength);
                if (result == metaframe.FrameLength)
                {
                    return buffer;
                }
            }
            catch (Exception e)
            {
                System.Console.Error.WriteLine($"[ERROR] Capture.GetPacketAsync: {e}");
            }
            return null;
        }

        public static Packet GetPacket(this MetaFrame metaframe, Stream stream)
        {
            var bytes = Capture.GetFrameBytes(metaframe, stream);
            if (bytes != null) return Packet.ParsePacket((PacketDotNet.LinkLayers)metaframe.LinkType, bytes);
            return null;
        }

        /// <summary>
        /// Reads frames for the specified collection of capture files.
        /// </summary>
        /// <param name="captures">Collection of capture files.</param>
        /// <returns>A collection of frames read sequentially from the specified capture files.</returns>
        public static IEnumerable<RawFrame> Select(IEnumerable<string> captures, Action<string,int> progressCallback = null)
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

        public static IEnumerable<RawFrame> Select(string capture)
        {
                foreach (var frame in PcapReader.ReadFile(capture))
                {
                    yield return frame;
                }
        }

        public static void WriteAllFrames(string capturefile, IEnumerable<RawFrame> frames, DataLinkType link = DataLinkType.Ethernet)
        {
            LibPcapFile.WriteAllFrames(capturefile, link, frames);
        }
    }
}
