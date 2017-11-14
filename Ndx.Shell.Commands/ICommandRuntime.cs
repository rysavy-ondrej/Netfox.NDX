using System;

namespace Ndx.Shell.Commands
{
    /// <summary>
    /// This interface is used when you are calling the Invoke to explicitly invoke a command.
    /// </summary>
    /// <remarks>
    /// All calls to the command APIs are routed through to an instance of this interface. 
    /// Commands invoked directly from the command line uses the default implementation of the interface. 
    /// Custom implementation of this interface allows you to change the behavior of the methods called by the command you are invoking.
    /// </remarks>
    public interface ICommandRuntime<TYPE>
    {
        /// <summary>
        /// Writes a debug message that can be displayed.
        /// </summary>
        /// <param name="text"></param>
        void WriteDebug(string text);
        /// <summary>
        /// Processes a nonterminating error.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        void WriteError(Exception exception, string message);
        /// <summary>
        /// Processes a single object written by the command to the output pipeline.
        /// </summary>
        /// <param name="sendToPipeline"></param>
        void WriteObject(TYPE sendToPipeline);
        /// <summary>
        /// Process a progress message, provided by the command.
        /// </summary>
        /// <param name="progressRecord"></param>
        void WriteProgress(ProgressRecord progressRecord);
        /// <summary>
        /// Processes a warning message that is supplied by the command.
        /// </summary>
        /// <param name="text"></param>
        void WriteWarning(string text);
    }
}
