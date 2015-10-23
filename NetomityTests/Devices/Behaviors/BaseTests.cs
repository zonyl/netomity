using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices.Behaviors;
using Netomity.Devices;
using Netomity.Core;
using System.Collections.Generic;

namespace NetomityTests.Devices.Behaviors
{
    [TestClass]
    public class BaseTests
    {
        BaseBehavior _b = null;

        [TestInitialize]
        public void SetUp()
        {
            _b = new BaseBehavior();
        }
        [TestMethod]
        public void InitializationTest()
        {
            Assert.IsNotNull(_b);
        }

        [TestMethod]
        public void RegisterTests()
        {
            var sd = new StateDevice();
            _b.Register(sd);
            Assert.IsTrue(_b.Targets.Contains(sd));

        }

        [TestMethod]
        public void CommandFilterTest()
        {
            var c = new Command()
            {
                Source = "12.34.56",
                SourceObject = _b,
                Destination = "aa.bb.cc",
                Primary = CommandType.On,
                Secondary = "secondary"
            };
           
            List<Command> rCommands = _b.FilterCommand(c);
            Assert.AreEqual(c, rCommands[0]);
        }

    }
}
