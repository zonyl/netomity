using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Netomity.Core;

namespace NetomityPS
{
    [Cmdlet(VerbsCommon.New, "Logger")]
    public class PSLogger: PSCommon
    {
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "FileName"
        )]
        public string FileName { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose(String.Format("New Logger: {0}", FileName));
            WriteObject( new Logger(logFilePath: FileName));
        }
    }
}
