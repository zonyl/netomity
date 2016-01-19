using Netomity.Core;
using Netomity.Core.Enum;
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
        FirstFirstMedium = 2,
        FirstMedium = 3,
        FirstMediumMedium = 4,
        Medium = 5,
        MediumMediumLast = 6,
        MediumLast = 7,
        MediumLastLast = 8,
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

        internal void DelegateCommand(CommandType primary, string secondary, NetomityObject sourceObject)
        {
            
            Targets.ForEach(t =>
                {
                    Log("Delegating Command: {0} to {1}", primary, t.Name);
                    t.Command(primary: primary, secondary: secondary, sourceObject: sourceObject);

                }
            );
        }

    }
}
