using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Netomity.Core
{
    public class NetomitySystem: NetomityObject
    {
        static PeriodicTimer c_pt = null;
        static NetomitySystem c_ns = null;

        private NetomitySystem()
        {
            if (c_pt == null)
            {
                c_pt = new PeriodicTimer(30);
                c_pt.OnInterval += HeartBeat;
                c_pt.Start();
            }
            
        }

        public static NetomitySystem Factory()
        {
            if (c_ns == null)
                c_ns = new NetomitySystem();

            return c_ns;
        }

        private void HeartBeat()
        {
            Log(Core.Logger.Level.Debug, "HeartBeat");
        }

        public void Run()
        {
            while(true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
