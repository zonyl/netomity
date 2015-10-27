using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Utility;
using System.Threading;

namespace NetomityTests.Utility
{
    [TestClass]
    public class TimerTests
    {
        bool _isCalled = false;
        Netomity.Utility.Timer _t = null;

        [TestInitialize]
        public void SetUp()
        {
            _isCalled = false;
            _t = new Netomity.Utility.Timer(seconds: 2, action: () => TimerCallback());
        }

        [TestMethod]
        public void IntitializationTest()
        {
            Assert.IsNotNull(_t);
        }

        [TestMethod]
        public void SimpleTest()
        {
            _t.Start();
            Assert.IsFalse(_isCalled);
            Thread.Sleep(1000);
            Assert.IsFalse(_isCalled);
            Thread.Sleep(2000);
            Assert.IsTrue(_isCalled);
        }

        public void TimerCallback()
        {
            _isCalled = true;
        }
    }
}
