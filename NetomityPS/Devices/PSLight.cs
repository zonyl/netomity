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
    [Cmdlet(VerbsCommon.New, "Light")]
    public class PSLight: PSStateDevice
    {

        protected override void ProcessRecord()
        {
            WriteVerbose(String.Format("Light - Interface:{0}", Interface));
            WriteObject(new Light(iface: Interface,
                address: Address,
                behaviors: Behaviors
                ) { Name = Name });
        }
    }
}
