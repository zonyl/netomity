using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netomity.Core;

namespace Netomity.Interfaces.HA
{
    public class HA: NetomityObject
    {
        public virtual void _DataReceived(string data)
        {
            Log(data);
        }
    }
}
