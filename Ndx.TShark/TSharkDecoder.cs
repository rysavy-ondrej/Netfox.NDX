﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;

namespace Ndx.TShark
{
    public static class TSharkDecoder
    {
        /// <summary>
        /// Decodes each <see cref="RawFrame"/> of a sequence into a <see cref="PacketFields"/> object.
        /// </summary>
        /// <param name="frames">A sequence of values to invoke a transform function on.</param>
        /// <param name="tsharkProcess">A decoder process to apply to each element.</param>
        /// <param name="datalinkType">The link layer type used in decoding operation. Default is <see cref="DataLinkType.Ethernet"/>.</param>
        /// <returns>
        /// An IEnumerable<PacketFields> whose elements are the result of invoking the decode function on each element of source.
        /// </returns>
        /// <remarks>
        /// This method is implemented by using deferred execution. The immediate return value is an object that stores all the information 
        /// that is required to perform the action. The query represented by this method is not executed until the object is enumerated 
        /// either by calling its GetEnumerator method directly or by using foreach.
        /// </remarks>
        public static IEnumerable<PacketFields> Decode(this IEnumerable<RawFrame> frames, TSharkProcess tsharkProcess, DataLinkType datalinkType = DataLinkType.Ethernet)
        {
            var pipename = $"ndx.tshark_{new Random().Next(Int32.MaxValue)}";
            var wsender = new TSharkSender(pipename, datalinkType);
            tsharkProcess.PipeName = pipename;

            var decodedPackets = new BlockingCollection<PacketFields>();
            void PacketDecoded(object sender, PacketFields packet)
            {
                decodedPackets.Add(packet);
            }
            tsharkProcess.PacketDecoded += PacketDecoded;
            tsharkProcess.Start();
            if (!wsender.Connected.Wait(5000))
            {
                throw new InvalidOperationException("Cannot connect to TShark process.");
            }

            var pumpTask = Task.Run(async () =>
            {
                foreach (var frame in frames)
                {
                    await wsender.SendAsync(frame);
                }
                wsender.Close();
            });

            while (tsharkProcess.IsRunning || decodedPackets.Count > 0)
            {
                yield return decodedPackets.Take();
            }
        }
    }
}