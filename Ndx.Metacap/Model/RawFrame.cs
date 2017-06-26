using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Model
{
    public partial class RawFrame
    {
        //  January 1, 1970
        static readonly long UnixBaseTicks = new DateTime(1970, 1, 1).Ticks;
        const long TicksPerSecond = 10000000;
        const long TicksPerMicrosecond = 10;

        public uint Seconds => (uint)((TimeStamp - UnixBaseTicks) / TicksPerSecond);

        public uint Microseconds => (uint)(((TimeStamp - UnixBaseTicks) % TicksPerSecond)/ TicksPerMicrosecond);

        public DateTime DateTime => new DateTime(TimeStamp);

        public byte[] Bytes => Data.ToByteArray();
    }
}
