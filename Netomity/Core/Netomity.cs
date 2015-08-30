using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Core
{
    public class NetomitySystem: NetomityObject
    {
        static PeriodicTimer c_pt = null;

        public NetomitySystem()
        {
            if (c_pt == null)
            {
                c_pt = new PeriodicTimer(30);
                c_pt.OnInterval += HeartBeat;
                c_pt.Start();
            }
            
        }

        private void HeartBeat()
        {
            Log(Core.Logger.Level.Debug, "HeartBeat");
        }
    }
}
