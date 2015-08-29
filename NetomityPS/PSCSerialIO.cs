using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Netomity.Interfaces.Basic;

namespace NetomityPS
{
    [Cmdlet(VerbsCommon.New, "SerialIO")]
    public class PSCSerialIO: PSCommon
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Port Name"
        )]
        public string PortName { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 1,
            HelpMessage = "Port Speed"
        )]
        public int PortSpeed { get; set; }

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
            WriteVerbose(String.Format("SerialIO - Port:{0} Speed:{1}", PortName, PortSpeed));
            WriteObject(new SerialIO(portName: PortName, portSpeed: PortSpeed) { Name = Name });
        }
    }
}
