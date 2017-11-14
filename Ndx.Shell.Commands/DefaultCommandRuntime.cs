using System;
using System.Collections.Generic;

namespace Ndx.Shell.Commands
{
    public class DefaultCommandRuntime<TYPE> : ICommandRuntime<TYPE>
    {
        private IList<TYPE> m_output;

        public DefaultCommandRuntime(IList<TYPE> outputArrayList)
        {
            m_output = outputArrayList ?? throw new ArgumentNullException("outputArrayList");
        }

        public void WriteDebug(string text)
        {
            Console.Error.WriteLine($"DEBUG: {text}");
        }

        public void WriteError(Exception exception, string message)
        {
            Console.Error.WriteLine($"{message}: {exception.Message}");
        }

        public void WriteObject(TYPE sendToPipeline)
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
