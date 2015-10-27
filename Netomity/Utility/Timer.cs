using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Utility
{
    public class Timer
    {
        private System.Timers.Timer _timer = null;
        public int Seconds { get; set; }
        public Action Action { get; set; }

        public Timer(int seconds=0, Action action=null)
        {
            Seconds = seconds;
            Action = action;

            _timer = new System.Timers.Timer();
            _timer.Interval = seconds * 1000;
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(OnEvent);
        }

        private void OnEvent(object source, System.Timers.ElapsedEventArgs e)
        {
            Action();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }
}
