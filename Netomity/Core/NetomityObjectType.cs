using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Core
{
    public sealed class NetomityObjectType
    {
        private readonly string _name;

        public static readonly NetomityObjectType Unknown = new NetomityObjectType("unknown");
        public static readonly NetomityObjectType Device = new NetomityObjectType("device");
        public static readonly NetomityObjectType Interface = new NetomityObjectType("interface");


        private NetomityObjectType(String name)
        {
            _name = name;
        }
        public override String ToString()
        {
            return _name;
        }

    }
}
