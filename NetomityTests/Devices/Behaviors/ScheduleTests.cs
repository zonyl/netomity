using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices.Behaviors;
using Netomity.Core;
using Netomity.Devices;
using System.Collections.Generic;
using System.Threading;

namespace NetomityTests.Devices.Behaviors
{
    [TestClass]
    public class ScheduleTests
    {
        Netomity.Devices.Behaviors.Schedule _s = null;
        DateTime _testTime;

        [TestInitialize]
        public void SetUp()
        {
            _testTime = DateTime.Now;
            _testTime = _testTime.AddMinutes(1);
            var cronFormat = String.Format("{0} {1} * * *",
                _testTime.Minute,
                _testTime.Hour
                );
            _s = new Schedule(cronFormat, CommandType.On);
        }

        [TestMethod]
        public void SingleScheduleTest()
        {
            var s = new StateDevice(behaviors: new List<BaseBehavior>() { _s });
            Assert.AreEqual(StateType.Unknown, s.State.Primary);
            Thread.Sleep(64000);
            Assert.AreEqual(StateType.On, s.State.Primary);


        }
    }
}
