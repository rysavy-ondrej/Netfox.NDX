using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ndx.Model;
using Ndx.TShark;

namespace Ndx.Shell.Console
{
    public static class Shark
    {
        public static IEnumerable<PacketFields> DecodeTSharkOutput(this IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                if (line.StartsWith("{\"timestamp\""))
                {
                    yield return TSharkFieldDecoderProcess.DecodeJsonLine(line); 
                }
            }
        }
    }
}
