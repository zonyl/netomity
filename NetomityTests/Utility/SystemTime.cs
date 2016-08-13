using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Utility;
using System.Linq;

namespace NetomityTests.Utility.SystemTime
{
    [TestClass]
    public class SystemTimeTests
    {
        const long TIME_RANGE = 100000L;

        [TestMethod]
        public void NowTest()
        {
            var timeNow = Netomity.Utility.SystemTime.Now.Ticks;
            var timeNowExp = DateTime.Now.Ticks;
            Assert.IsTrue(timeNow > timeNowExp- TIME_RANGE && timeNow < timeNowExp+ TIME_RANGE);
        }

        [TestMethod]
        public void NowSetTest()
        {
            var testTime = new DateTime(2011, 1, 13);
            Netomity.Utility.SystemTime.Now = testTime;
            var timeNow = Netomity.Utility.SystemTime.Now;
            Assert.AreEqual(testTime, timeNow);
        }

        [TestMethod]
        public void ResetTest()
        {

            var testTime = new DateTime(2011, 1, 13);
            Netomity.Utility.SystemTime.Now = testTime;
            var timeNow = Netomity.Utility.SystemTime.Now;
            Assert.AreEqual(testTime, timeNow);
            ///
            Netomity.Utility.SystemTime.Reset();
            var timeNowCur = Netomity.Utility.SystemTime.Now.Ticks;
            var timeNowExp = DateTime.Now.Ticks;
            Assert.IsTrue(timeNowCur > timeNowExp - TIME_RANGE && timeNowCur < timeNowExp + TIME_RANGE);

        }
    }
}
