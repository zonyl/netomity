using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Netomity.Interfaces.Basic;
using Netomity.Core;

namespace NetomityPS
{
    [Cmdlet(VerbsCommon.New, "TCPClient")]
    public class PSTCPClient: PSCommon
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Address"
        )]
        public string Address { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Port Number"
        )]
        public int PortNumber { get; set; }

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
            WriteVerbose(String.Format("TCPClient - Port:{0}", PortNumber));
            WriteObject(new TCPClient(address: Address, port: PortNumber) { Name = Name });
        }
    }
}
