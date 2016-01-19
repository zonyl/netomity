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
    public class LightTests
    {
        const string ADDRESS = "adsf";
        Light _l = null;
        Netomity.Interfaces.HA.Fakes.StubHAInterface _ha = null;

        [TestInitialize]
        public void SetUp()
        {
            var t = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface();

            _ha = new Netomity.Interfaces.HA.Fakes.StubHAInterface(iface: t);
            _l = new Light(iface: _ha);
        }
        [TestMethod]
        public void InitializeTest()
        {
            Assert.IsNotNull(_l);

        }

        [TestMethod]
        public void LightOnTest()
        {
            Command command = null;
            _ha.CommandCommand = (pCommand) => { command = pCommand; return null; };
            var l = new Light(iface: _ha);
            Assert.AreEqual(StateType.Unknown, l.State.Primary);
            var r = l.On();
            Assert.IsFalse(r.Result);
            Assert.AreEqual(CommandType.On, command.Primary);
            Assert.AreEqual(StateType.On, l.State.Primary);
        }

        [TestMethod]
        public void LightMotionOnTest()
        {
            var l = new Light();
            Assert.AreEqual(StateType.Unknown, l.State.Primary);
            var r = l.Command(primary: CommandType.Motion);
            Assert.IsTrue(r.Result);
            Assert.AreEqual(StateType.On, l.State.Primary);
        }


        [TestMethod]
        public void LightMotionCustomOverrideTest()
        {
            const string level = "80";
            var m = new Netomity.Devices.Behaviors.MapCommand(
                primaryInput: CommandType.Motion, 
                primaryOutput: CommandType.Level, 
                secondaryOutput: level
            );
            var l = new Light(behaviors: new List<Netomity.Devices.Behaviors.BehaviorBase>(){m});
            Assert.AreEqual(StateType.Unknown, l.State.Primary);
            var r = l.Command(primary: CommandType.Motion);
            Assert.IsTrue(r.Result);
            Assert.AreEqual(StateType.Level, l.State.Primary);
            Assert.AreEqual(level, l.State.Secondary);
            /////
            r = l.Command(primary: CommandType.Still);
            Assert.IsTrue(r.Result);
            Assert.AreEqual(StateType.Off, l.State.Primary);
            Assert.AreEqual(null, l.State.Secondary);
        }

        [TestMethod]
        public void LightIdleTest()
        {
            Command command = null;
            _ha.CommandCommand = (pCommand) => { command = pCommand; return null; };
            var l = new Light(
                    iface: _ha,
                    idleCommandPrimary: CommandType.Level,
                    idleCommandSecondary: "80",
                    idleTimeSecs: 5
                );
            Assert.AreEqual(StateType.Unknown, l.State.Primary);
            l.On();
            Thread.Sleep(1000);
            Assert.AreEqual(StateType.Unknown, l.State.Primary);
            Thread.Sleep(5000);
            Assert.AreEqual(StateType.Level, l.State.Primary);

        }
        [TestMethod]
        public void LightFilterTest()
        {
            Command command = null;
            var sd = new StateDevice();
            var l = new Light(
                restrictionDevice: sd,
                restrictionState: StateType.Light
                );
            Assert.AreEqual(StateType.Unknown, l.State.Primary);
            var r = l.On();
            Assert.IsTrue(r.Result);
            Assert.AreEqual(StateType.On, l.State.Primary);
            r = l.Off();
            Assert.IsTrue(r.Result);
            Assert.AreEqual(StateType.Off, l.State.Primary);
            r = sd.Command(CommandType.Light);
            Assert.IsTrue(r.Result);
            Assert.AreEqual(StateType.Off, l.State.Primary);
            r = l.On();
            Assert.IsTrue(r.Result);
            Assert.AreEqual(StateType.Off, l.State.Primary);

        }

    }
}
