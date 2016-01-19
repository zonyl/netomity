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
    [Cmdlet(VerbsCommon.New, "Notification")]
    public class PSNotification: PSStateDevice
    {
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Subject"
        )]
        public string Subject { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Message"
        )]
        public string Message { get; set; }

        protected override void ProcessRecord()
        {

            WriteVerbose(String.Format("Notification - Interface:{0}", Interface));
            WriteObject(new Notification(iface: Interface, address: Address,
                subject: Subject,
                message: Message) { Name = Name });
        }
    }
}
