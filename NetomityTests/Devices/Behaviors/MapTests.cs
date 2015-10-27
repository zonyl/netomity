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
    public class MapTests
    {
        Netomity.Devices.Behaviors.Map _m = null;

        [TestInitialize]
        public void SetUp()
        {
            _m = new Map(primaryInput: CommandType.On,
                primaryOutput: CommandType.Off);
        }

        [TestMethod]
        public void InitializationTest()
        {
            Assert.IsNotNull(_m);
        }

        [TestMethod]
        public void SimpleMapTest()
        {
            var sd = new StateDevice(behaviors: new List<BehaviorBase>() { _m });
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            sd.On();
            Assert.AreEqual(StateType.Off, sd.State.Primary);

        }

        [TestMethod]
        public void DoubleMapTest()
        {
            var m = new Map(primaryInput: CommandType.On, primaryOutput: CommandType.Off);
            m.Add(primaryInput: CommandType.Off, primaryOutput: CommandType.On, secondaryOutput: "test");
            var sd = new StateDevice(behaviors: new List<BehaviorBase>() {
                m
            });
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            sd.On();
            Assert.AreEqual(StateType.Off, sd.State.Primary);
            Assert.IsNull(sd.State.Secondary);
            sd.Off();
            Assert.AreEqual(StateType.On, sd.State.Primary);
            Assert.AreEqual("test", sd.State.Secondary);

        }

        [TestMethod]
        public void SpecificMatchFirst()
        {
            var motion = new StateDevice();
            var m = new Map(
              primaryInput: CommandType.On,
              primaryOutput: CommandType.Off
                );
            m.Add(
              primaryInput: CommandType.On,
              primaryOutput: CommandType.Unknown,
              sourceObject: motion
            );
            var sd = new StateDevice(
                devices: new List<StateDevice>() { motion },
                behaviors: new List<BehaviorBase>() { m });

            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            sd.Command(primary: CommandType.On);
            Assert.AreEqual(StateType.Off, sd.State.Primary);
            motion.Command(primary: CommandType.On);
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
        }


        [TestMethod]
        public void DelayTest()
        {
            var m = new Map(
                primaryInput: CommandType.Motion,
                primaryOutput: CommandType.Off,
                delay: 2);
            var sd = new StateDevice(behaviors: new List<BehaviorBase>() { m });
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            sd.Command(primary: CommandType.Motion);
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            Thread.Sleep(1000);
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            Thread.Sleep(2000);
            Assert.AreEqual(StateType.Off, sd.State.Primary);

        }
    }
}
