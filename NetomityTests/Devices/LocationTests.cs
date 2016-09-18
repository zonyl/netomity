using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices;
using Netomity.Interfaces.HA;
using Netomity.Interfaces.Basic;
using Netomity.Interfaces;
using System.Threading.Tasks;
using Netomity.Core;
using System.Collections.Generic;
using Netomity.Devices.Behaviors;
using Microsoft.QualityTools.Testing.Fakes;
using System.Threading;
using Netomity.Core.Enum;

namespace NetomityTests.Devices
{
    [TestClass]
    public class LocationTests
    {

        [TestMethod]
        public void TestInstantiation()
        {
            var l = new Location(latitude: 38.00, longitude: 85.03);
            Assert.IsNotNull(l);
        }
    
        [TestMethod]
        public void SunsetTest()
        {
            Netomity.Utility.SystemTime.Now = new DateTime(2015, 1, 1, 12, 0, 0);
            var l = new Location(latitude: 35, longitude: -80);
            l.EvaluteTime();
            Assert.AreEqual(StateType.Light, l.State.Primary);

            Netomity.Utility.SystemTime.Now = new DateTime(2015, 1, 1, 23, 0, 0);
            l.EvaluteTime();
            Assert.AreEqual(StateType.Dark, l.State.Primary);
        }

        [TestMethod]
        public void SunRiseOffsetTest()
        {
            const int OFFSET = 15;
            Netomity.Utility.SystemTime.Now = new DateTime(2015, 1, 1, 1, 0, 0);
            var l = new Location(latitude: 35, longitude: -80, offsetMinutes: OFFSET);
            var sunrise = l.Sunrise;
            var oSunrise = sunrise - new TimeSpan(0, OFFSET, 0);
            l.EvaluteTime();
            Assert.AreEqual(StateType.Dark, l.State.Primary);

            Netomity.Utility.SystemTime.Now = new DateTime(2015, 1, 1,
                oSunrise.Hour,
                oSunrise.Minute + 1,
                oSunrise.Second);
            l.EvaluteTime();
            Assert.AreEqual(StateType.Light, l.State.Primary);
        }

        [TestMethod]
        public void SteadyStateTest()
        {
            const int OFFSET = 15;
            Netomity.Utility.SystemTime.Now = new DateTime(2015, 1, 1, 1, 0, 0);
            var l = new Location(latitude: 35, longitude: -80, offsetMinutes: OFFSET);
            var sunrise = l.Sunrise;
            var oSunrise = sunrise - new TimeSpan(0, OFFSET, 0);
            Thread.Sleep(2000);
            Assert.AreEqual(StateType.Dark, l.State.Primary);
            Thread.Sleep(4000);
            Assert.AreEqual(StateType.Dark, l.State.Primary);
        }

    }
}
