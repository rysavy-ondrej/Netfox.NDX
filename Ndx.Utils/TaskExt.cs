using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Utils
{
    public static class TaskExt
    {
        private static Task completedTask = Task.FromResult(false);
        public static Task CompletedTask => completedTask;
    }
}
