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
using System.Fakes;
using System.Threading;
using Netomity.Core.Enum;

namespace NetomityTests.Devices
{
    [TestClass]
    public class LocationTests
    {

        Location _l = null;

        [TestInitialize]
        public void SetUp()
        {
            _l = new Location(latitude: 38.00, longitude: 85.03);
        }

        [TestMethod]
        public void TestInstantiation()
        {
            Assert.IsNotNull(_l);
        }

        [TestMethod]
        public void SunsetTest()
        {
           using (ShimsContext.Create())
           {
               ShimDateTime.NowGet = () =>
               {
                   return new DateTime(2015, 1, 1, 12, 0, 0);
               };
               var l = new Location(latitude: 35, longitude: -80);
               Thread.Sleep(2000);
               Assert.AreEqual(StateType.Light, l.State.Primary);
               ShimDateTime.NowGet = () =>
               {
                   return new DateTime(2015, 1, 1, 23, 0, 0);
               };
               l.EvaluteTime();
               Thread.Sleep(2000);
               Assert.AreEqual(StateType.Dark, l.State.Primary);
           }
        }

        [TestMethod]
        public void SunRiseOffsetTest()
        {
            using (ShimsContext.Create())
            {
                const int OFFSET = 15;
                ShimDateTime.NowGet = () =>
                {
                    return new DateTime(2015, 1, 1, 1, 0, 0);
                };
                var l = new Location(latitude: 35, longitude: -80, offsetMinutes: OFFSET);
                var sunrise = l.Sunrise;
                var oSunrise = sunrise - new TimeSpan(0, OFFSET, 0);
                Thread.Sleep(2000);
                Assert.AreEqual(StateType.Dark, l.State.Primary);
                ShimDateTime.NowGet = () => new DateTime(2015, 1, 1, 
                    oSunrise.Hour, 
                    oSunrise.Minute+1, 
                    oSunrise.Second);
                Thread.Sleep(4000);
                Assert.AreEqual(StateType.Light, l.State.Primary);

            }
        }


    }
}
