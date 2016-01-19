using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Netomity.Interfaces.Basic;
using Netomity.Core;
using Netomity.Interfaces.HA;

namespace NetomityPS
{
    [Cmdlet(VerbsCommon.New, "EmailServer")]
    public class PSEmailServer: PSCommon
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
            HelpMessage = "FromAddress"
        )]

        public string FromAddress { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Password"
        )]
        public string Password { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "FromAddressName"
        )]
        public string FromAddressName { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "IsSSL"
        )]
        public bool IsSSL { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Name"
        )]
        public string Name { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Port"
        )]
        public int Port { get; set; }


        protected override void ProcessRecord()
        {
            WriteVerbose(String.Format("EmailServer - Address:{0} Account:{1}", 
                Address,
                FromAddress));
            WriteObject(new EmailServer(address: Address, 
                fromAddress: FromAddress, 
                password: Password, 
                port: Port, 
                isSSL: IsSSL, 
                fromAddressName: FromAddressName) { Name = Name });
        }
    }
}
