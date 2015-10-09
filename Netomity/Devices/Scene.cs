using Netomity.Core;
using Netomity.Interfaces.HA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices
{
    public class Scene: StateDevice
    {
        public Scene(string address = null, HAInterface iface = null, List<StateDevice> devices = null):
            base(address: address, iface: iface, devices: devices)
        {

        }

        internal override void _Initialize()
        {
            base._Initialize();
            Type = NetomityObjectType.Scene;
        }
    }
}
