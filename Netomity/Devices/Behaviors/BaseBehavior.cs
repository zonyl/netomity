using Netomity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices.Behaviors
{
    public class BaseBehavior: NetomityObject
    {
        public List<StateDevice> Targets { get; set; }
        public int Priority { get; set; }

        public BaseBehavior()
        {
            Targets = new List<StateDevice>();
        }

        public virtual void Register(Devices.StateDevice sd)
        {
            Targets.Add(sd);
        }

        public virtual List<Command> FilterCommand(Command command)
        {
            return new List<Command>() { command };
        }
    }
}
