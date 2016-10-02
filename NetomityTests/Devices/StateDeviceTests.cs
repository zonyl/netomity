using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices;
using Netomity.Interfaces.HA;
using Netomity.Interfaces.Basic;
using Netomity.Interfaces;
using System.Threading.Tasks;
using Netomity.Core;
using System.Collections.Generic;
using Netomity.Devices.Behaviors;
using Netomity.Core.Enum;

namespace NetomityTests.Devices
{
    [TestClass]
    public class StateDeviceTests
    {
        StateDevice _sd = null;
        HAInterface _ha = null;
        BasicInterface _t = null;
        Command _command = null;
        const string _address = "12.45.66";
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
            _ha = new Netomity.Interfaces.HA.Fakes.StubHAInterface(iface: _t)
            {
                CommandCommand = (command) => { _command = command; return new Task<bool>(() => {return true;}); },
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
            var r = sd1.Command(CommandType.On);

            Assert.IsTrue(r.Result);
            Assert.AreEqual(StateType.On, sd2.State.Primary);
        }

        [TestMethod]
        public void StateDeviceType()
        {
            Assert.AreEqual(NetomityObjectType.Device, _sd.Type);
        }

        [TestMethod]
        public void StateDeviceBevaiorRegister()
        {
            var WasCalled = false;
            var b = new Netomity.Devices.Behaviors.Fakes.StubBehaviorBase
            {
                RegisterStateDevice = (l) => { WasCalled = true; }
            };

            Assert.IsFalse(WasCalled);
            var sd = new StateDevice(behaviors: new List<BehaviorBase>() { b });

            Assert.IsTrue(WasCalled);
        }

        [TestMethod]
        public void FilterCommandOneTest()
        {
            var IsValid = false;

            var commandOutput = new Command() { Primary = CommandType.On };


            var b = new Netomity.Devices.Behaviors.Fakes.StubBehaviorBase
            {
                FilterCommandCommand = (c) => {
                    IsValid = c.Primary == CommandType.Off ? true : false;
                    return commandOutput;
                }
            };

            var sd = new StateDevice(behaviors: new List<BehaviorBase>() { b });
            sd.Command(primary: CommandType.Off);
            Assert.AreEqual(sd.State.Primary, StateType.On);
        }

        [TestMethod]
        public void FilterPriorityTest()
        {
            NetomityObject lastCalled=null;

            var commandOutput = new Command() { Primary = CommandType.On };


            var b1 = new Netomity.Devices.Behaviors.Fakes.StubBehaviorBase()
            {
                Priority = BehaviorPriority.Last
            };
            b1.FilterCommandCommand = (c) =>
            {
                lastCalled = b1;
                return commandOutput;
            };

            var b2 = new Netomity.Devices.Behaviors.Fakes.StubBehaviorBase()
            {
                Priority = BehaviorPriority.Medium
            };
            b2.FilterCommandCommand = (c) =>
            {
                lastCalled = b2;
                return commandOutput;
            };



            var sd = new StateDevice(behaviors: new List<BehaviorBase>() { b1, b2 });
            sd.Command(primary: CommandType.Off);
            Assert.AreEqual(sd.State.Primary, StateType.On);
            Assert.AreEqual(b1, lastCalled);

            b1.Priority = BehaviorPriority.First;
            sd.Command(primary: CommandType.Off);
            Assert.AreEqual(b2, lastCalled);

            
        }
        [TestMethod]
        public void FilterPriorityTest2()
        {
            NetomityObject lastCalled = null;

            var commandOutput = new Command() { Primary = CommandType.On };


            var b1 = new Netomity.Devices.Behaviors.Fakes.StubBehaviorBase()
            {
                Priority = BehaviorPriority.Medium
            };
            b1.FilterCommandCommand = (c) =>
            {
                lastCalled = b1;
                return commandOutput;
            };

            var b2 = new Netomity.Devices.Behaviors.Fakes.StubBehaviorBase()
            {
                Priority = BehaviorPriority.MediumMediumLast
            };
            b2.FilterCommandCommand = (c) =>
            {
                lastCalled = b2;
                return commandOutput;
            };



            var sd = new StateDevice(behaviors: new List<BehaviorBase>() { b1, b2 });
            sd.Command(primary: CommandType.Off);
            Assert.AreEqual(sd.State.Primary, StateType.On);
            Assert.AreEqual(b2, lastCalled);

            b2.Priority = BehaviorPriority.First;
            sd.Command(primary: CommandType.Off);
            Assert.AreEqual(b1, lastCalled);


        }

    }
}
