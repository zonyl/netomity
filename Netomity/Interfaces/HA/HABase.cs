using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Netomity.Core;
using Netomity.Interfaces.Basic;
using Netomity.Utility;

namespace Netomity.Interfaces.HA
{
    public class HABase: NetomityObject
    {
        protected BasicInterface _interface = null;
        public HABase(BasicInterface basicInterface)
        {
            _interface = basicInterface;
            _interface.DataReceived += _DataReceived;
            Log("Initialized");
        }

        public virtual void _DataReceived(string data)
        {
            Log(data);
        }

        public bool Send(SendParams sp)
        {
            _interface.Send(Conversions.BytesToAscii(sp.SendData));
            return true;
        }

    }
}
