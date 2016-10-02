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
using Netomity.Core.Enum;

namespace NetomityPS
{
    [Cmdlet(VerbsCommon.New, "Light")]
    public class PSLight: PSStateDevice
    {
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "RestrictionDevice"
        )]
        public StateDevice RestrictionDevice { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "RestrictionState"
        )]
        public StateType RestrictionState { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "IdleCommandPrimary"
        )]
        public CommandType IdleCommandPrimary { get; set; }
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "IdleCommandSecondary"
        )]
        public string IdleCommandSecondary { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "IdleTimeSecs"
        )]
        public int IdleTimeSecs { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose(String.Format("Light - Interface:{0}", Interface));
            WriteObject(new Light(iface: Interface,
                address: Address,
                behaviors: Behaviors,
                restrictionDevice: RestrictionDevice,
                restrictionState: RestrictionState,
                idleCommandPrimary: IdleCommandPrimary,
                idleCommandSecondary: IdleCommandSecondary,
                idleTimeSecs: IdleTimeSecs
                ) { Name = Name });
        }
    }
}
