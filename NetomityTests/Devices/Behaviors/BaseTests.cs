﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices.Behaviors;
using Netomity.Devices;
using Netomity.Core;
using System.Collections.Generic;
using Netomity.Core.Enum;

namespace NetomityTests.Devices.Behaviors
{
    [TestClass]
    public class BaseTests
    {
        BehaviorBase _b = null;

        [TestInitialize]
        public void SetUp()
        {
            _b = new BehaviorBase();
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
           
            var rCommand = _b.FilterCommand(c);
            Assert.AreEqual(c, rCommand);
        }

    }
}
