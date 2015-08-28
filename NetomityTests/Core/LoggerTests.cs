using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Core;

namespace NetomityTests.Core
{
    [TestClass]
    public class LoggerTests
    {
        Logger _log = null;

        [TestInitialize]
        public void SetUp()
        {
            _log = new Logger();
        }

        [TestMethod]
        public void Instance()
        {
            Assert.IsNotNull(_log);
        }

        [TestMethod]
        public void LogLevelTests()
        {
            var result = _log.Log(Logger.Level.Debug, "Debug Message");
            Assert.IsTrue(result);
        }
    }
}
