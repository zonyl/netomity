using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Interfaces
{
    public class Command
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public CommandType Type { get; set; }
        public string SubCommand { get; set; }
    }
}
