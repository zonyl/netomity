using NCrontab;
using Netomity.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Netomity.Devices.Behaviors
{
    public class Schedule: BaseBehavior
    {
        List<Tuple<string, CommandType, string>> _schedules = null;
        PeriodicTimer _pt = null;

        public Schedule()
        {
            Initialize();
        }

        public Schedule(string cron, CommandType primary, string secondary=null)
        {
            Initialize();
            AddSchedule(cron, primary, secondary);

        }

        private void AddSchedule(string cron, CommandType primary, string secondary=null)
        {
            _schedules.Add(new Tuple<string, CommandType, string>(cron, primary, secondary));
        }
        private void Initialize()
        {
            if (_schedules == null)
               _schedules = new List<Tuple<string, CommandType, string>>();

            if (_pt == null)
                _pt = new PeriodicTimer();
            _pt.OnInterval += _pt_OnInterval;
            _pt.Start();
        }

        void _pt_OnInterval()
        {
            foreach (var schedule in _schedules)
            {
                var current = DateTime.Now;
                var past = current.AddMinutes(-1);
                var next = CrontabSchedule.Parse(schedule.Item1).GetNextOccurrence(past);

                if (next.Hour == current.Hour &&
                    next.Minute == current.Minute &&
                    next.Month == current.Month &&
                    next.Day == current.Day)
                {
                    Targets.ForEach(t => t.Command(primary: schedule.Item2, secondary: schedule.Item3, sourceObject: this));
                }
            }
        }
    }
}
