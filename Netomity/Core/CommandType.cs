using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Interfaces
{
    public sealed class CommandType
    {
        private readonly string _name;

        public static readonly CommandType On = new CommandType("On");
        public static readonly CommandType Off = new CommandType("Off");
        public static readonly CommandType Level = new CommandType("Level");


        private CommandType(String name)
        {
            _name = name;
        }
        public override String ToString()
        {
            return _name;
        }
    }
}
