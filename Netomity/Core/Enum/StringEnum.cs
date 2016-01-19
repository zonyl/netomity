using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Core.Enum
{
    [DataContract]
    public abstract class StringEnum
    {
        internal readonly string _name;

        internal StringEnum(String name)
        {
            _name = name;
        }

        public override String ToString()
        {
            return _name;
        }

        [DataMember]
        public virtual string Value
        {
            get { return _name; }
        }

        public static implicit operator string(StringEnum v) { return v.ToString(); }

    }
}
