using Netomity.Core;
using Netomity.Interfaces;
using Netomity.Interfaces.HA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices
{
    public class StateDevice: NetomityObject
    {
        HAInterface _iface = null;
        string _address = null;

        public StateDevice(HAInterface iface, string address)
        {
            _iface = iface;
            _address = address;

            _iface.OnCommand(address: _address, action: _CommandReceived);
        }

        private void _CommandReceived(Command command)
        {

        }

    }
}
