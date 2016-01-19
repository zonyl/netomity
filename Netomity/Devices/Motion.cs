using Innovative.SolarCalculator;
using Netomity.Core;
using Netomity.Core.Enum;
using Netomity.Interfaces.HA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices
{
    public class Motion : StateDevice
    {

        public Motion(string address = null, HAInterface iface = null,  List<StateDevice> devices=null, List<Behaviors.BehaviorBase> behaviors = null) :
            base(address: address, iface: iface, devices: devices, behaviors: behaviors)
        {

            IntrinsicBehaviors();
        }

        public void IntrinsicBehaviors()
        {
            var defaultMap = new Behaviors.MapCommand(primaryInput: CommandType.On,
                primaryOutput: CommandType.Motion);
            defaultMap.Add(primaryInput: CommandType.Off, primaryOutput: CommandType.Still);

            RegisterBehavior(defaultMap);
        }

    }
}
