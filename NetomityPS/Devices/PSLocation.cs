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
    [Cmdlet(VerbsCommon.New, "Location")]
    public class PSLocation: PSCommon
    {
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Latitude"
        )]

        public double Latitude { get; set; }
        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Longitude"
        )]
        public double Longitude { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "TimeZoneString"
        )]
        public string TimeZoneString { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Name"
        )]
        public string Name { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            ValueFromPipeline = true,
            Position = 0,
            HelpMessage = "Address"
        )]
        public string Address { get; set; }

        protected override void ProcessRecord()
        {
            WriteVerbose(String.Format("Location - Latitude:{0}", Latitude));
            WriteObject(new Location(latitude: Latitude, longitude: Longitude, timeZone: TimeZoneString) { Name = Name });
        }
    }
}
