using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices;
using Netomity.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Netomity.Core.Enum;

namespace NetomityTests.Devices
{
    [TestClass]
    public class MotionTests
    {
        const string ADDRESS = "adsf";
        Motion _m = null;
        Netomity.Interfaces.HA.Fakes.StubHAInterface _ha = null;

        [TestInitialize]
        public void SetUp()
        {
            var t = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface();

            _ha = new Netomity.Interfaces.HA.Fakes.StubHAInterface(iface: t);
            _m = new Motion(iface: _ha);
        }
        [TestMethod]
        public void InitializeTest()
        {
            Assert.IsNotNull(_m);

        }

        [TestMethod]
        public void MotionOnTests()
        {
            Command command = null;
            _ha.CommandCommand = (pCommand) => { command = pCommand; return null; };

            Assert.AreEqual(StateType.Unknown, _m.State.Primary);
            _m.Command(primary: CommandType.On, sourceObject: _ha);
            var r = _m.On();
            Assert.IsFalse(r.Result);

            Assert.AreEqual(CommandType.Motion, command.Primary);
            Assert.AreEqual(StateType.Motion, _m.State.Primary);
        }


    }
}
