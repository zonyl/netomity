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
using Netomity.Devices.Behaviors;

namespace NetomityPS
{
    [Cmdlet(VerbsCommon.New, "StateDevice")]
    public class PSStateDevice: PSCommon
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

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Devices"
        )]
        public List<StateDevice> Devices { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Behaviours"
        )]
        public List<BehaviorBase> Behaviors { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose(String.Format("StateDevice - Interface:{0}", Interface));
            WriteObject(new StateDevice(iface: Interface, 
                address: Address,
                devices: Devices,
                behaviors: Behaviors) { Name = Name });
        }
    }
}
