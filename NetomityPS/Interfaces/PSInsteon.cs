﻿using System;
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
    [Cmdlet(VerbsCommon.New, "Insteon")]
    public class PSInsteon: PSCommon
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Interface"
        )]
        public BasicInterface Interface { get; set; }

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
            WriteVerbose(String.Format("Insteon - Interface:{0}", Interface));
            WriteObject(new Insteon(iface: Interface) { Name = Name });
        }
    }
}
