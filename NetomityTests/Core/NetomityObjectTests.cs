using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Core;
using Netomity.Interfaces.Basic;
using System.Collections.Generic;
using System.Reflection;

namespace NetomityTests.Core
{
    [TestClass]
    public class NetomityObjectTests
    {
        NetomityObject _no = null;

        [TestInitialize]
        public void SetUp()
        {
            // Reset the static parameters of the class
            Type staticType = typeof(NetomityObject);
            ConstructorInfo ci = staticType.TypeInitializer;
            object[] parameters = new object[0];
            ci.Invoke(null, parameters);

            // New clean instance without static leftovers from prior unit test
            _no = new NetomityObjectTestObj();
        }
        
        [TestMethod]
        public void Instantiation()
        {
            Assert.IsNotNull(_no);
        }

        [TestMethod]
        public void LoggerStaticTest()
        {
            var no_logger_yet = _no.Logger;
            Assert.IsNull(no_logger_yet);
            var static_logger = new Logger();
            _no.Logger = static_logger;
            var _no2 = new NetomityObjectTestObj();
            Assert.AreEqual(static_logger, _no2.Logger);
            

        }

        [TestMethod]
        public void OverrideStaticLoggerTest()
        {
            //Assert.IsNull(_no.Logger);
            var static_logger = new Logger();
            Assert.AreEqual(static_logger, _no.Logger);
            var instance_logger = new Logger();
            Assert.AreNotEqual(static_logger, instance_logger);
            _no.Logger = instance_logger;
            Assert.AreNotEqual(static_logger, _no.Logger);
            Assert.AreEqual(instance_logger, _no.Logger);


        }

        [TestMethod]
        public void LoggerNullTests()
        {
            _no.Log(Logger.Level.Debug, "This is a test");

        }

        class NetomityObjectTestObj : NetomityObject
        {

        }
    }
}
