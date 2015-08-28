using Netomity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Interfaces.Basic
{
    public abstract class BasicInterface: NetomityObject
    {
        public BasicInterface(Logger logger=null)
        {
            if (logger != null)
                _logger = logger;
        }

        public BasicInterface()
        {

        }



        public abstract event DataReceivedHandler DataReceived;
        public abstract bool IsOpen { get; }
        public abstract Task Open();
        public abstract void Send(string text);


    }
}
