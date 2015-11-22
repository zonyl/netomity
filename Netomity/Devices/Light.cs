using Innovative.SolarCalculator;
using Netomity.Core;
using Netomity.Interfaces.HA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices
{
    public class Light : StateDevice
    {
        public StateDevice RestrictionDevice { get; set; }
        public StateType RestrictionState { get; set; }
        public int IdleTimeSecs { get; set; }
        public CommandType IdleCommandPrimary { get; set; }
        public string IdleCommandSecondary { get; set; }

        public Light(string address = null, HAInterface iface = null, StateDevice restrictionDevice=null, StateType restrictionState=null, CommandType idleCommandPrimary=null, string idleCommandSecondary=null, int idleTimeSecs=0, List<StateDevice> devices=null, List<Behaviors.BehaviorBase> behaviors = null) :
            base(address: address, iface: iface, devices: devices, behaviors: behaviors)
        {
            RestrictionDevice = restrictionDevice;
            RestrictionState = restrictionState;
            IdleCommandPrimary = idleCommandPrimary;
            IdleCommandSecondary = idleCommandSecondary;
            IdleTimeSecs = idleTimeSecs;
            IntrinsicBehaviors();
        }

        public void IntrinsicBehaviors()
        {
            if (RestrictionDevice != null)
                RegisterBehavior(new Behaviors.Filter(CommandType.On, 
                                                    RestrictionDevice, 
                                                    RestrictionState));

            var defaultMap = new Behaviors.MapCommand(primaryInput: CommandType.Motion,
                primaryOutput: CommandType.On);
            defaultMap.Add(primaryInput: CommandType.Still, primaryOutput: CommandType.Off);
            defaultMap.Add(primaryInput: CommandType.Dark, primaryOutput: CommandType.On);
            defaultMap.Add(primaryInput: CommandType.Light, primaryOutput: CommandType.Off);

            if (IdleCommandPrimary != null)
                defaultMap.Add(primaryInput: CommandType.On,
                primaryOutput: IdleCommandPrimary,
                secondaryOutput: IdleCommandSecondary,
                delaySecs: IdleTimeSecs );

            RegisterBehavior(defaultMap);
        }

    }
}
