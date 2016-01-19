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
    public class Door : StateDevice
    {
        public bool IsInverted { get; set; }

        public Door(string address = null, HAInterface iface = null,  List<StateDevice> devices=null, List<Behaviors.BehaviorBase> behaviors = null, bool isInverted = false) :
            base(address: address, iface: iface, devices: devices, behaviors: behaviors)
        {
            IsInverted = isInverted;
            IntrinsicBehaviors();
        }

        public void IntrinsicBehaviors()
        {
            var defaultMap = new Behaviors.MapCommand();

            if (!IsInverted)
            {
                defaultMap.Add(primaryInput: CommandType.On, primaryOutput: CommandType.Open);
                defaultMap.Add(primaryInput: CommandType.Off, primaryOutput: CommandType.Close);
            }
            else
            {
                defaultMap.Add(primaryInput: CommandType.On, primaryOutput: CommandType.Close);
                defaultMap.Add(primaryInput: CommandType.Off, primaryOutput: CommandType.Open);
            }
            RegisterBehavior(defaultMap);
        }

    }
}
