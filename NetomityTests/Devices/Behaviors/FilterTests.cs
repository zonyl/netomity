using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices.Behaviors;
using Netomity.Core;
using Netomity.Devices;
using System.Collections.Generic;
using System.Threading;
using Netomity.Core.Enum;

namespace NetomityTests.Devices.Behaviors
{
    [TestClass]
    public class FilterTests
    {
        Filter _f = null;
        StateDevice _sd = null;

        [TestInitialize]
        public void SetUp()
        {
            _sd = new StateDevice();
            _f = new Filter(primaryInput: CommandType.On,
                filterDevice: _sd, 
                filterState: StateType.Light);
        }

        [TestMethod]
        public void InitializationTest()
        {
            Assert.IsNotNull(_f);
        }

        [TestMethod]
        public void SimpleFilterTest()
        {

            var sd = new StateDevice(behaviors: new List<BehaviorBase>() { _f });
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            sd.On();
            Thread.Sleep(1000);
            Assert.AreEqual(StateType.On, sd.State.Primary);
            sd.Off();
            Thread.Sleep(1000);
            Assert.AreEqual(StateType.Off, sd.State.Primary);
            _sd.Command(primary: CommandType.Light);
            sd.On();
            Thread.Sleep(1000);
            Assert.AreEqual(StateType.Off, sd.State.Primary);
        }

    }
}
