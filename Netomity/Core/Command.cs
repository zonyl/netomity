using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Core
{
    public class Command
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public CommandType Primary { get; set; }
        public string Secondary { get; set; }
        public NetomityObject SourceObject { get; set; }
        public Dictionary<String, String> StringParams { get; set; }

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
