using System;
using System.Collections;

namespace Ndx.Shell.Commands
{
    internal class DefaultCommandRuntime : ICommandRuntime
    {
        private ArrayList m_output;


        public DefaultCommandRuntime(ArrayList outputArrayList)
        {
            if (outputArrayList == null)
            {
                throw new ArgumentNullException("outputArrayList");
            }
            this.m_output = outputArrayList;
        }

        public void WriteDebug(string text)
        {
            Console.Error.WriteLine($"DEBUG: {text}");
        }

        public void WriteError(Exception exception, string message)
        {
            Console.Error.WriteLine($"{message}: {exception.Message}");
        }

        public void WriteObject(object sendToPipeline)
        {
            this.m_output.Add(sendToPipeline);
        }
        public void WriteProgress(ProgressRecord progressRecord)
        {
            Console.Error.WriteLine($"PROGRESS: Complete {progressRecord.PercentComplete} %.");
        }

        public void WriteWarning(string text)
        {
            Console.Error.WriteLine($"WARNING: {text}");
        }
    }
}
