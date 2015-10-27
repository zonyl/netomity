using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices.Behaviors;
using Netomity.Core;
using Netomity.Devices;
using System.Collections.Generic;
using System.Threading;

namespace NetomityTests.Devices
{
    [TestClass]
    public class DeviceFunctional
    {

        [TestInitialize]
        public void SetUp()
        {
        }

        [TestMethod]
        public void FunctionalLightGutsTest()
        {
            // Motion Sensor driven light with a delay off time of 2 secs
            var motion = new StateDevice();
            var mapping = new Map(
                primaryInput: CommandType.Motion,
                primaryOutput: CommandType.On
                );
            mapping.Add(
                primaryInput: CommandType.Motion,
                primaryOutput: CommandType.Off,
                delay: 2
                );
            var sd = new StateDevice(
                    devices: new List<StateDevice>() { motion },
                    behaviors: new List<BehaviorBase>(){mapping}
                );

            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            sd.Command(primary: CommandType.Motion);
            Assert.AreEqual(StateType.On, sd.State.Primary);
            Thread.Sleep(3000);
            Assert.AreEqual(StateType.Off, sd.State.Primary);




        }
    }
}
