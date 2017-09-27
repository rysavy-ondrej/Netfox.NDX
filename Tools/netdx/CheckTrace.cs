using Ndx.Shell.Commands;

namespace netdx
{
    internal class CheckTrace : Command
    {
        [Parameter(Mandatory = true)]
        public string Input { get; set; }
        [Parameter(Mandatory = true)]
        public string Rules { get; set; }

        protected override void BeginProcessing()
        {
        }

        protected override void EndProcessing()
        {
        }

        protected override void ProcessRecord()
        {

        }
    }
}