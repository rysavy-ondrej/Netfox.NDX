using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Shell.Console
{
    public class Shell
    {     
        public static TResult Exec<TResult>(Func<TResult> action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            TResult result = action();        
            stopwatch.Stop();
            System.Console.WriteLine($"Time elapsed: {stopwatch}");
            return result;
        }
    }
}
