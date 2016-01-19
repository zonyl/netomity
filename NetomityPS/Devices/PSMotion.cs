using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Netomity.Interfaces.Basic;
using Netomity.Core;
using Netomity.Interfaces.HA;
using Netomity.Devices;

namespace NetomityPS
{
    [Cmdlet(VerbsCommon.New, "Motion")]
    public class PSMotion: PSCommon
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Interface"
        )]
        public HAInterface Interface { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Name"
        )]
        public string Name { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Address"
        )]
        public string Address { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose(String.Format("Motion - Interface:{0}", Interface));
            WriteObject(new Motion(iface: Interface, address: Address) { Name = Name });
        }
    }
}
