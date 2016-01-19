using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Netomity.Interfaces.Basic;
using Netomity.Core;
using Netomity.Interfaces.HA;

namespace NetomityPS
{
    [Cmdlet(VerbsCommon.New, "W800")]
    public class PSW800: PSCommon
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Interface"
        )]
        public BasicInterface Interface { get; set; }

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
            WriteVerbose(String.Format("W800 - Interface:{0}", Interface));
            WriteObject(new W800(iface: Interface) { Name = Name });
        }
    }
}
