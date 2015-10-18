using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices.Behaviors;
using Netomity.Devices;

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
    }
}
