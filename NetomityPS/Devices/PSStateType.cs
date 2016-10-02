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
    [Cmdlet(VerbsCommon.New, "StateType")]
    public class PSStateTypeType: PSCmdlet
    {

        protected override void ProcessRecord()
        {
            WriteVerbose(String.Format("StateType"));
            WriteObject(new Netomity.Core.Enum.StateType(""));
        }
    }
}
