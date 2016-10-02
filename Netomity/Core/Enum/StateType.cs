using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Core.Enum
{
    [DataContract]
    public sealed class StateType: StringEnum
    {

        public static readonly StateType Unknown = new StateType("unknown");
        public static readonly StateType On = new StateType("on");
        public static readonly StateType Off = new StateType("off");
        public static readonly StateType Level = new StateType("level");
        public static readonly StateType Motion = new StateType("motion");
        public static readonly StateType Still = new StateType("still");
        public static readonly StateType Light = new StateType("light");
        public static readonly StateType Dark = new StateType("dark");
        public static readonly StateType Open = new StateType("open");
        public static readonly StateType Closed = new StateType("close");
        public static readonly StateType Notify = new StateType("notify");


        public StateType(String name) : base(name: name) { }
    }
}
