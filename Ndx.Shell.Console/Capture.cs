using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Ndx.Captures;
using Ndx.Ingest;
using Ndx.Model;
using PacketDotNet;
using SharpPcap;
using SharpPcap.LibPcap;

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

        public static void WriteFrames(string capturefile, IEnumerable<RawFrame> frames)
        {
            var device = new CaptureFileWriterDevice(capturefile);
            WriteFrames(device, frames);
            device.Close();
        }

        public static void WriteFrame(CaptureFileWriterDevice device, RawFrame frame)
        {
            var capture = new RawCapture(LinkLayers.Ethernet, new PosixTimeval(frame.Seconds, frame.Microseconds), frame.Bytes);
            device.Write(capture);
        }


        public static void WriteFrames(CaptureFileWriterDevice device, IEnumerable<RawFrame> frames)
        {
            foreach(var frame in frames)
            {
                WriteFrame(device, frame);
            }
        }
    }
}
