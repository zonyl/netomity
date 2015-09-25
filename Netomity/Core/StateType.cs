using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Core
{
    [DataContract]
    public sealed class StateType
    {
        private readonly string _name;

        public static readonly StateType Unknown = new StateType("uknown");
        public static readonly StateType On = new StateType("on");
        public static readonly StateType Off = new StateType("off");
        public static readonly StateType Level = new StateType("level");


        private StateType(String name)
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
            get
            {
                return _name;
            }
        }
    }
}
