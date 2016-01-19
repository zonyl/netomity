using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Core.Enum
{
    [DataContract]
    public sealed class CommandType: StringEnum
    {
        public static readonly CommandType Unknown = new CommandType("unknown");
        public static readonly CommandType On = new CommandType("on");
        public static readonly CommandType Off = new CommandType("off");
        public static readonly CommandType Level = new CommandType("level");
        public static readonly CommandType Motion = new CommandType("motion");
        public static readonly CommandType Still = new CommandType("still");
        public static readonly CommandType Light = new CommandType("light");
        public static readonly CommandType Dark = new CommandType("dark");
        public static readonly CommandType Open = new CommandType("open");
        public static readonly CommandType Close = new CommandType("close");
        public static readonly CommandType Notify = new CommandType("notify");

        public CommandType(string name): base(name) {}
    }
}
