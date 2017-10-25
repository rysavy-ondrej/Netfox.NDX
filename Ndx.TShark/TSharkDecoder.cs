using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;

namespace Ndx.TShark
{
    public static class TSharkDecoder
    {
        /// <summary>
        /// Decodes each <see cref="Frame"/> of a sequence into a <see cref="DecodedFrame"/> object.
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
        public static IObservable<DecodedFrame> Decode(this IObservable<Frame> frames, TSharkProcess tsharkProcess, DataLinkType datalinkType = DataLinkType.Ethernet)
        {
            
            var pipename = $"ndx.tshark_{new Random().Next(Int32.MaxValue)}";
            var wsender = new TSharkSender(pipename, datalinkType);
            tsharkProcess.PipeName = pipename;

            var decodedPackets = new BlockingCollection<DecodedFrame>();
            void PacketDecoded(object sender, DecodedFrame packet)
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
                await frames.ForEachAsync(async frame =>
                {
                    await wsender.SendAsync(frame);
                });
                wsender.Close();
            });

            var observable = Observable.Create<DecodedFrame>(obs =>
            {
                while (tsharkProcess.IsRunning || decodedPackets.Count > 0)
                {
                    obs.OnNext(decodedPackets.Take());
                }
                obs.OnCompleted();
                return Disposable.Create(() => { });
            });
            return observable;

        }


        /// <summary>
        /// Decodes each <see cref="Frame"/> of a sequence into a <see cref="DecodedFrame"/> object.
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
        public static IEnumerable<DecodedFrame> Decode(this IEnumerable<Frame> frames, TSharkProcess tsharkProcess, DataLinkType datalinkType = DataLinkType.Ethernet)
        {
            var pipename = $"ndx.tshark_{new Random().Next(Int32.MaxValue)}";
            var wsender = new TSharkSender(pipename, datalinkType);
            tsharkProcess.PipeName = pipename;

            var decodedPackets = new BlockingCollection<DecodedFrame>();
            void PacketDecoded(object sender, DecodedFrame packet)
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
