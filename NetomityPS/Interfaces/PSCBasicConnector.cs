using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Netomity.Interfaces.Basic;

namespace NetomityPS
{
    [Cmdlet(VerbsCommon.New, "BasicConnector")]
    public class PSCBasicConnector: PSCommon
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Interfaces"
        )]
        public List<BasicInterface> Interfaces { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Name"
        )]
        public string Name { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose(String.Format("Adding Interface: {0}", Interfaces.Count()));
            WriteObject(new BasicConnector(interfaces: Interfaces) { Name = Name });
        }
    }
}
