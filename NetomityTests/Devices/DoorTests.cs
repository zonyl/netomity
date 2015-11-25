using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices;
using Netomity.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NetomityTests.Devices
{
    [TestClass]
    public class DoorTests
    {
        const string ADDRESS = "adsf";
        Door _d = null;
        Netomity.Interfaces.HA.Fakes.StubHAInterface _ha = null;

        [TestInitialize]
        public void SetUp()
        {
            var t = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface();

            _ha = new Netomity.Interfaces.HA.Fakes.StubHAInterface(iface: t);
            _d = new Door(iface: _ha);
        }
        [TestMethod]
        public void InitializeTest()
        {
            Assert.IsNotNull(_d);

        }

        [TestMethod]
        public void DoorCommandTests()
        {
            Command command = null;
            _ha.CommandCommand = (pCommand) => { command = pCommand; return null; };

            Assert.AreEqual(StateType.Unknown, _d.State.Primary);
            _d.Command(primary: CommandType.On, sourceObject: _ha);
            var r = _d.On();
            Assert.IsFalse(r.Result);

            Assert.AreEqual(CommandType.Open, command.Primary);
            Assert.AreEqual(StateType.Open, _d.State.Primary);

            r = _d.Off();
            Assert.IsFalse(r.Result);

            Assert.AreEqual(CommandType.Close, command.Primary);
            Assert.AreEqual(StateType.Closed, _d.State.Primary);
            
        }


    }
}
