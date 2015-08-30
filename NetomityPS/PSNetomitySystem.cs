using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Netomity.Core;

namespace NetomityPS
{
    [Cmdlet(VerbsCommon.New, "NetomitySystem")]
    public class PSNetomitySystem: PSCommon
    {

        protected override void ProcessRecord()
        {
            WriteVerbose(String.Format("New NetomitySystem"));
            WriteObject( NetomitySystem.Factory());
        }
    }
}
