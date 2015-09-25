using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Core
{
    [DataContract]
    public sealed class CommandType
    {
        private readonly string _name;

        public static readonly CommandType Uknown = new CommandType("unknown");
        public static readonly CommandType On = new CommandType("on");
        public static readonly CommandType Off = new CommandType("off");
        public static readonly CommandType Level = new CommandType("level");


        private CommandType(String name)
        {
            _name = name;
        }
        public override String ToString()
        {
            return _name;
        }
        [DataMember]
        public string Value
        {
            get { return _name; }
        }

    }
}
