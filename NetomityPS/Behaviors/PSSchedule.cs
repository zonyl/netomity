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
    [Cmdlet(VerbsCommon.New, "Schedule")]
    public class PSSchedule: PSCommon
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "ScheduleTimes"
        )]
        public string ScheduleTimes { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "CommandPrimary"
        )]
        public CommandType CommandPrimary { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "CommandSecondary"
        )]
        public string CommandSecondary { get; set; }

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

            WriteVerbose(String.Format("Schedule - Schedule:{0}", ScheduleTimes));
            WriteObject(new Schedule(
                cron: ScheduleTimes,
                primary: CommandPrimary,
                secondary: CommandSecondary) 
                { Name = Name}
                );

        }
    }
}
