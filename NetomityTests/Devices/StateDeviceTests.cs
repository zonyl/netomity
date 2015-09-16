using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices;
using Netomity.Interfaces.HA;
using Netomity.Interfaces.Basic;

namespace NetomityTests.Devices
{
    [TestClass]
    public class StateDeviceTests
    {
        StateDevice _sd = null;
        HAInterface _ha = null;
        BasicInterface _t = null;

        string _data;

        [TestInitialize]
        public void SetUp()
        {
            _t = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface()
            {
                SendString = (data) => { _data = data; },
                DataReceivedEvent = (ev) => { _t._DataReceived(ev); },
            };
            _ha = new Netomity.Interfaces.HA.Fakes.StubHAInterface(basicInterface: _t)
            {

            };
            _sd = new StateDevice(iface: _ha, address: "12.34.56");
        }
        [TestMethod]
        public void TestInstantiation()
        {
            Assert.IsNotNull(_sd);
        }
    }
}
