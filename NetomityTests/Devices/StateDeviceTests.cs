﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices;
using Netomity.Interfaces.HA;
using Netomity.Interfaces.Basic;
using Netomity.Interfaces;
using System.Threading.Tasks;
using Netomity.Core;
using System.Collections.Generic;

namespace NetomityTests.Devices
{
    [TestClass]
    public class StateDeviceTests
    {
        StateDevice _sd = null;
        HAInterface _ha = null;
        BasicInterface _t = null;
        Command _command = null;
        string _address = "12.45.66";
        string _data;
        Action<Command> _action = null;

        [TestInitialize]
        public void SetUp()
        {
            _data = null;
            _action = null;
            _t = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface()
            {
                SendString = (data) => { _data = data; },
                DataReceivedEvent = (ev) => { _t._DataReceived(ev); },
            };
            _ha = new Netomity.Interfaces.HA.Fakes.StubHAInterface(basicInterface: _t)
            {
                CommandCommand = (command) => { _command = command; return null; },
                OnCommandStringActionOfCommand = (address, action) => {
                    _action = action;
                }

            };
            _sd = new StateDevice(iface: _ha, address: _address);

        }
        [TestMethod]
        public void TestInstantiation()
        {
            Assert.IsNotNull(_sd);
        }

        [TestMethod]
        public void CommandOnTest()
        {
            var r = _sd.Command(CommandType.Off);
            Assert.AreEqual(CommandType.Off, _command.Primary);
            Assert.AreEqual(_address, _command.Destination);
            Assert.IsNull(_command.Secondary);
        }

        [TestMethod]
        public void OnTest()
        {
            var r = _sd.Command("on");
            Assert.AreEqual(CommandType.On, _command.Primary);
            Assert.AreEqual(_address, _command.Destination);
            Assert.IsNull(_command.Secondary);
        }

        [TestMethod]
        public void OnSubLevelTests()
        {
            var r = _sd.Command("on", "50");
            Assert.AreEqual(CommandType.On, _command.Primary);
            Assert.AreEqual(_address, _command.Destination);
            Assert.AreEqual("50", _command.Secondary);
        }
        
        [TestMethod]
        public void OnCommandStateChangeTests()
        {
            _action(new Command() {
                Primary = CommandType.On,
                Destination = _address,
            });
            Assert.AreEqual(StateType.On, _sd.State.Primary);
        }

        [TestMethod]
        public void OncommandDelegateTests()
        {
            Command command = null;
            _sd.OnCommand( (c) => { command = c; });
            _action(new Command()
            {
                Primary = CommandType.On,
                Destination = _address,
            });
            Assert.IsNotNull(command);
        }

        [TestMethod]
        public void OnStateDelegateTests()
        {
            State state = null;
            _sd.OnStateChange((s) => { state = s; });
            _action(new Command()
            {
                Primary = CommandType.On,
                Destination = _address,
            });
            _action(new Command()
            {
                Primary = CommandType.Off,
                Destination = _address,
            });
            Assert.IsNotNull(state);
            Assert.AreEqual(StateType.Off, state.Primary);
        }

        [TestMethod]
        public void DeviceInitialUnkownTest()
        {
            Assert.AreEqual(StateType.Unknown, _sd.State.Primary);
        }

        [TestMethod]
        public void DeviceLinkOnTest()
        {
            var sd1 = new StateDevice()
            {
                Name = "TestDevice1"
            };
            var sd2 = new StateDevice(devices: new List<StateDevice>() { sd1 }) {
            Name = "TestDevice2"
            };
            Assert.AreEqual(StateType.Unknown, sd2.State.Primary);
            var r = sd1.Command("on");

            Assert.IsTrue(r.Result);
            Assert.AreEqual(StateType.On, sd2.State.Primary);
        }

        [TestMethod]
        public void StateDeviceType()
        {
            Assert.AreEqual(NetomityObjectType.Device, _sd.Type);
        }
    }
}
