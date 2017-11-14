using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ndx.Shell.Commands
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public sealed class ParameterAttribute : System.Attribute
    {
        private bool m_mandatory;
        private string m_helpMessage;
        private string m_helpMessageResourceId;
        private bool m_valueFromPipeline;

        public bool Mandatory
        {
            get => this.m_mandatory;
            set => m_mandatory = value;
        }

        public bool ValueFromPipeline
        {
            get => this.m_valueFromPipeline;
            set => m_valueFromPipeline = value;
        }

        public string HelpMessage
        {
            get => this.m_helpMessage;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(nameof(value));
                }
                this.m_helpMessage = value;
            }
        }

        public string HelpMessageResourceId
        {
            get => this.m_helpMessageResourceId;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(nameof(value));
                }
                this.m_helpMessageResourceId = value;
            }
        }
    }
    

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CommandAttribute : System.Attribute
    {
        private string m_nounName;
        private string m_verbName;
        public string NounName => this.m_nounName;
        public string VerbName => this.m_verbName;
        public CommandAttribute(string verbName, string nounName)
        {
            if (nounName == null || nounName.Length == 0)
            {
                throw new ArgumentException(nameof(nounName));
            }
            if (verbName == null || verbName.Length == 0)
            {
                throw new ArgumentException(nameof(verbName));
            }
            this.m_nounName = nounName;
            this.m_verbName = verbName;
        }
    }

    /// <summary>
    /// Serves as a base class for derived commands. 
    /// </summary>
    public abstract class Command<INPUT,OUTPUT>
    {
        private ICommandRuntime<OUTPUT> m_commandRuntime;

        /// <summary>
        /// Invokes a command object and returns a collection of results.
        /// </summary>
        /// <returns>A collection that contains the results of the command call.</returns>
        public IEnumerable<OUTPUT> Invoke(IEnumerable<INPUT> input)
        {            
            var arrayList = new List<OUTPUT>();
            m_commandRuntime = new DefaultCommandRuntime<OUTPUT>(arrayList);
            this.BeginProcessing();
            foreach (var obj in input)
            {
                this.ProcessRecord(obj);
            }
            this.EndProcessing();

            for (int i = 0; i < arrayList.Count; i++)
            {
                yield return arrayList[i];
            }
        }

        public IEnumerable<OUTPUT> Invoke(params INPUT[] input)
        {
            return Invoke((IEnumerable<INPUT>)input);
        }

        public void Execute(ICommandRuntime<OUTPUT> runtime, IEnumerable<INPUT> input)
        {
            m_commandRuntime = runtime;
            this.BeginProcessing();

            foreach (var value in input)
            {
                this.ProcessRecord(value);
            }
            this.EndProcessing();
        }

        public void Execute(ICommandRuntime<OUTPUT> runtime, params INPUT[] input)
        {
            Execute(runtime, (IEnumerable<INPUT>)input);
        }

        /// <summary>
        /// Writes a single object to the output pipeline.
        /// </summary>
        /// <param name="sendToPipeline">The object to be written to the output pipeline.</param>
        public void WriteObject(OUTPUT result)
        {
            m_commandRuntime.WriteObject(result);    
        }

        /// <summary>
        /// Writes a debug message to the host.
        /// </summary>
        /// <param name="text">The debug message to be written to the host.</param>
        public void WriteDebug(string text)
        {
            m_commandRuntime.WriteDebug(text);
        }

        public void WriteError(Exception exception, string message)
        {
            m_commandRuntime.WriteError(exception, message);
        }

        public void WriteWarning(string message)
        {
            m_commandRuntime.WriteWarning(message);
        }

        public void WriteProgress(ProgressRecord progressRecord)
        {
            m_commandRuntime.WriteProgress(progressRecord);
        }

        /// <summary>
        /// Provides a one-time, preprocessing functionality for the cmdlet.
        /// </summary>
        protected virtual void BeginProcessing() { }

        /// <summary>
        /// Provides a one-time, post-processing functionality for the cmdlet.
        /// </summary>
        protected virtual void EndProcessing() { }


        /// <summary>
        /// Stops processing records when the user stops the cmdlet asynchronously.
        /// </summary>
        protected virtual void StopProcessing() { }

        /// <summary>
        /// Provides a record-by-record processing functionality for the Command.
        /// </summary>
        protected virtual void ProcessRecord(INPUT value) { }

    }
}
