using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Core
{
    [DataContract]
    public sealed class NetomityObjectType
    {
        private readonly string _name;
        public static readonly NetomityObjectType Unknown = new NetomityObjectType("unknown");
        public static readonly NetomityObjectType Device = new NetomityObjectType("device");
        public static readonly NetomityObjectType Interface = new NetomityObjectType("interface");
        public static readonly NetomityObjectType Scene = new NetomityObjectType("scene");


        private NetomityObjectType(String name)
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
