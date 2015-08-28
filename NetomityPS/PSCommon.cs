using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using Netomity.Core;

namespace NetomityPS
{
    public class PSCommon: PSCmdlet
    {
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            HelpMessage = "Logger"
        )]
        public Logger Logger { get; set; }

        public PSCommon()
        {
            if (Logger == null)
            {

            }
        }

        public void Log(Logger.Level level, string message)
        {

        }
    }
}
