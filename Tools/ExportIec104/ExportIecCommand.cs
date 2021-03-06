﻿using Ndx.Model;
using PacketDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace ExportIec104
{
    internal class ExportIecCommand 
    {
        private int m_count;

        public string OutputPath { get; set; }
         
        public void Execute(string InputPath)
        {
            IEnumerable<byte[]> ExtractIefPdus(Frame frame)
            {
                var packet = frame.Parse();
                var tcpSegment = (TcpPacket)packet.Extract(typeof(TcpPacket));
                if (tcpSegment != null)
                {
                    var bytes = tcpSegment.PayloadPacket.Bytes;
                    if (bytes == null || bytes.Length == 0) yield break;
                    var ptr = 0;
                    while (ptr < bytes.Length)
                    {
                        if (bytes[ptr] == 0x68)
                        {
                            var len = bytes[ptr + 1];
                            if (ptr + 2 + len <= bytes.Length)
                            {
                                var buffer = new byte[len + 2];
                                Array.Copy(bytes, ptr, buffer, 0, len + 2);
                                ptr += len + 2;
                                yield return buffer;
                            }
                            else
                            { yield break; }
                        }
                        else
                        { yield break; }
                    }
                }
            }

            var frames = Ndx.Captures.PcapFile.ReadFile(InputPath);
            var pdus = frames.SelectMany((Frame frame, int index) =>
            {
                return ExtractIefPdus(frame).Select((byte[] arg1, int arg2) => Tuple.Create(index, arg2, arg1));
            });

            var task = pdus.ForEachAsync(pdu =>
            {
                var path = Path.Combine(OutputPath, $"{(pdu.Item1 + 1).ToString("D4")}-{(pdu.Item2 + 1).ToString("D2")}.raw");
                File.WriteAllBytes(path, pdu.Item3);
                m_count++;
                Console.WriteLine(path);
            });
            task.Wait();
        }

        internal int Count()
        {
            return m_count;
        }
    }
}