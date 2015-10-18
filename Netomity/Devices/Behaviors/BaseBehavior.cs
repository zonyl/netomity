using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices.Behaviors
{
    public class BaseBehavior
    {
        public List<StateDevice> Targets { get; set; }

        public BaseBehavior()
        {
            Targets = new List<StateDevice>();
        }

        public void Register(Devices.StateDevice sd)
        {
            Targets.Add(sd);

        }
    }
}
