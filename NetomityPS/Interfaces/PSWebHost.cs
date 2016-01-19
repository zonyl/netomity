using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using Netomity.Interfaces.Basic;
using Netomity.Core;
using Netomity.Web;

namespace NetomityPS
{
    [Cmdlet(VerbsCommon.New, "WebHost")]
    public class PSWebHost: PSCommon
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "URL"
        )]
        public string URL { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "FilePath"
        )]
        public string FilePath { get; set; }

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
            WriteVerbose(String.Format("WebHost - Port:{0}", URL));
            try
            {
                WriteObject(new WebHost(address: URL, filePath: FilePath) { Name = Name });
            }
            catch (Exception ex)
            {
                var message = "Error===>" + ex.ToString();
                if (ex.InnerException != null)
                    message += "====INNER===" + ex.InnerException.ToString();
                WriteVerbose(message);
            }
        }
    }
}
