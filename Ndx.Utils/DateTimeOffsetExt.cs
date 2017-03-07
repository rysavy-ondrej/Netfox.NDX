using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Utils
{
    public static class DateTimeOffsetExt
    {
        /// <summary>
        /// Converts a Unix time expressed as the number of milliseconds that have elapsed since 1970-01-01T00:00:00Z to a DateTimeOffset value.
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static DateTimeOffset FromUnixTimeMilliseconds(long ms)
        {
            return UnixOrigin.AddMilliseconds(ms);
        }

        public static DateTimeOffset UnixOrigin = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        public static long ToUnixTimeMilliseconds(this DateTimeOffset dateTimeOffset)
        {
            var offset = dateTimeOffset - UnixOrigin;
            return (long)offset.TotalMilliseconds;

        }
    }
}
