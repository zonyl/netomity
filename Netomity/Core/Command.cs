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
        public CommandType Primary { get; set; }
        public string Secondary { get; set; }

        public static Command Create(string destination, CommandType type)
        {
            return new Command()
            {
                Destination = destination,
                Primary = type
            };
        }
    }
}
