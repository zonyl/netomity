using Netomity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices.Behaviors
{

    public enum BehaviorPriority
    {
        First = 1,
        FirstMedium = 3,
        Medium = 5,
        MediumLast = 8,
        Last = 9,
    }

    public class BehaviorBase: NetomityObject
    {
        public List<StateDevice> Targets { get; set; }
        public BehaviorPriority Priority { get; set; }

        public BehaviorBase()
        {
            Targets = new List<StateDevice>();
            Priority = BehaviorPriority.Medium;
        }

        public virtual void Register(Devices.StateDevice sd)
        {
            Targets.Add(sd);
        }

        public virtual Command FilterCommand(Command command)
        {
            return command;
        }
    }
}
