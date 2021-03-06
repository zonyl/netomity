﻿using Innovative.SolarCalculator;


using Netomity.Core;
using Netomity.Core.Enum;
using Netomity.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netomity.Devices
{
    public class Location : StateDevice
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string TimeZone { get; set; }
        public DateTime Sunset { get; set; }
        public DateTime Sunrise { get; set; }
        public TimeSpan Offset { get; set; }
        private PeriodicTimer pt;

        public Location(double latitude=35.2269, double longitude=-80.8433, double elevation=0, int offsetMinutes=0, string timeZone="Eastern Standard Time")
        {
            Latitude = latitude;
            Longitude = longitude;
            TimeZone = timeZone;
            Offset = new TimeSpan(0, 0, offsetMinutes, 0, 0);
            pt = new PeriodicTimer(2);
            pt.OnInterval += pt_OnInterval;
            pt.Start();
            EvaluteTime();
        }

        private void pt_OnInterval()
        {
            EvaluteTime();
        }

        private void CalcTimes(DateTime dt)
        {
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
            SolarTimes solarTimes = new SolarTimes(dt, Latitude, Longitude);
            Sunset = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunset.ToUniversalTime(), tz);
            Sunrise = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunrise.ToUniversalTime(), tz);

        }

        public void EvaluteTime()
        {
            var now = Netomity.Utility.SystemTime.Now;
            var tod = now.TimeOfDay;
            CalcTimes(now);
            var morning = Sunrise.TimeOfDay - Offset;
            var evening = Sunset.TimeOfDay + Offset;
            StateType calcState = null;
            if (tod < morning || tod > evening)
                calcState = StateType.Dark;
            else
                calcState = StateType.Light;
            if (this.State.Primary != calcState)
                this.Command(Conversions.ValueToStringEnum<CommandType>(calcState.ToString()));
        }

    }
}
