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
    [Cmdlet(VerbsCommon.New, "TCPServer")]
    public class PSCTCPServer: PSCommon
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Port Number"
        )]
        public int PortNumber { get; set; }



        protected override void ProcessRecord()
        {
            WriteVerbose(String.Format("TCPServer - Port:{0}", PortNumber));
            WriteObject( new TCPServer(port: PortNumber));
        }
    }
}
