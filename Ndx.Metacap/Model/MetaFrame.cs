using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Model
{
    public partial class MetaFrame
    {
        public static MetaFrame Create(RawFrame frame)
        {
            return new MetaFrame()
            {
                FrameLength = frame.FrameLength,
                FrameOffset = frame.FrameOffset,
                LinkType = frame.LinkType,
                TimeStamp = frame.TimeStamp
            };
        }
    }
}
