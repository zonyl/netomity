using Innovative.SolarCalculator;
using Netomity.Core;
using Netomity.Interfaces.HA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices
{
    public class Notification : StateDevice
    {

        public string Subject { get; set; }
        public string Message {get; set;}

        public Notification(string address = null, HAInterface iface = null,  List<StateDevice> devices=null, List<Behaviors.BehaviorBase> behaviors = null,
             string subject=null, string message=null) :
            base(address: address, iface: iface, devices: devices, behaviors: behaviors)
        {

            Subject = subject;
            Message = message;
            IntrinsicBehaviors();
        }

        public void IntrinsicBehaviors()
        {
            var defaultMap = new Behaviors.MapCommand();

            defaultMap.Add(primaryInput: CommandType.On, primaryOutput: CommandType.Notify);

            RegisterBehavior(defaultMap);
        }

        protected override Command ApplyBehaviors(Command command)
        {
            var rCommand = base.ApplyBehaviors(command);

            if (rCommand.Primary == CommandType.Notify)
            {
                rCommand.Secondary = rCommand.Secondary ?? Message;
                if (rCommand.StringParams == null)
                {
                    rCommand.StringParams = new Dictionary<string, string>();
                    rCommand.StringParams[EmailParamsType.Subject] = Subject;
                    rCommand.StringParams[EmailParamsType.ToAddress] = Address;
                }

            }
            return rCommand;
        }

    }


}
