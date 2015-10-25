using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices.Behaviors;
using Netomity.Core;
using Netomity.Devices;
using System.Collections.Generic;

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
    }
}
