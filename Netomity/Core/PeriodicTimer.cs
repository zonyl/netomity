using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Netomity.Core
{
    public class PeriodicTimer: NetomityObject
    {
        public delegate void IntervalEvent();

        public event IntervalEvent OnInterval;

        private Timer _timer = null;

        public PeriodicTimer(int secs=60)
        {
            _timer = new Timer();
            _timer.Interval = secs;
            _timer.Elapsed += new ElapsedEventHandler(OnEvent);

        }

        public double Interval
        {
            get
            {
                return _timer.Interval;
            }
            set
            {
                _timer.Interval = value;
            }
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void OnEvent(object source, ElapsedEventArgs e)
        {
            OnInterval();
        }



    }
}
