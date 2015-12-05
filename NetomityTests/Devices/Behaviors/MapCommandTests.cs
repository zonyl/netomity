using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices.Behaviors;
using Netomity.Core;
using Netomity.Devices;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetomityTests.Devices.Behaviors
{
    [TestClass]
    public class MapCommandTests
    {
        Netomity.Devices.Behaviors.MapCommand _m = null;

        [TestInitialize]
        public void SetUp()
        {
            _m = new MapCommand(primaryInput: CommandType.On,
                primaryOutput: CommandType.Off);
        }

        [TestMethod]
        public void InitializationTest()
        {
            Assert.IsNotNull(_m);
        }

        [TestMethod]
        public void SimpleMapTest()
        {
            var sd = new StateDevice(behaviors: new List<BehaviorBase>() { _m });
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            sd.On();
            Assert.AreEqual(StateType.Off, sd.State.Primary);
        }

        [TestMethod]
        public void SimpleMapOverlayTest()
        {
            const string testSecondary = "This is a test";
            var m = new MapCommand(primaryInput: CommandType.On,
                primaryOutput: CommandType.Off);

            var c = new Command()
            {
                Primary = CommandType.On,
                Secondary = testSecondary,
            };
            var rC = m.FilterCommand(c);
            Assert.AreEqual(testSecondary, rC.Secondary);
        }

        [TestMethod]
        public void SimpleMapDictTest()
        {
            const string testString = "This is a test";
            const string testKey = "key1";
            const string testValue = "value1";
            Command pCommand = null;
            var i = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface();
            var ha = new Netomity.Interfaces.HA.Fakes.StubHAInterface(iface: i);
            ha.CommandCommand = (command) => { pCommand = command; return Task.Run(() => { return true; }); };
            var m = new MapCommand(primaryInput: CommandType.On, 
                primaryOutput: CommandType.Off,
                secondaryOutput: testString,
                stringParams: new Dictionary<string,string>(){ {testKey, testValue}});
            var sd = new StateDevice(iface: ha, behaviors: new List<BehaviorBase>() { m });
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            var r = sd.On();
            Assert.IsTrue(r.Result);
            Assert.AreEqual(StateType.Off, sd.State.Primary);
            Assert.AreEqual(CommandType.Off, pCommand.Primary);
            Assert.AreEqual(testString, pCommand.Secondary);
            Assert.AreEqual(testValue, pCommand.StringParams[testKey]);
        }

        [TestMethod]
        public void DoubleMapTest()
        {
            var m = new MapCommand(primaryInput: CommandType.On, primaryOutput: CommandType.Off);
            m.Add(primaryInput: CommandType.Off, primaryOutput: CommandType.On, secondaryOutput: "test");
            var sd = new StateDevice(behaviors: new List<BehaviorBase>() {
                m
            });
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            sd.On();
            Assert.AreEqual(StateType.Off, sd.State.Primary);
            Assert.IsNull(sd.State.Secondary);
            sd.Off();
            Assert.AreEqual(StateType.On, sd.State.Primary);
            Assert.AreEqual("test", sd.State.Secondary);

        }

        [TestMethod]
        public void SpecificMatchFirst()
        {
            var motion = new StateDevice();
            var m = new MapCommand(
              primaryInput: CommandType.On,
              primaryOutput: CommandType.Off
                );
            m.Add(
              primaryInput: CommandType.On,
              primaryOutput: CommandType.Unknown,
              sourceObject: motion
            );
            var sd = new StateDevice(
                devices: new List<StateDevice>() { motion },
                behaviors: new List<BehaviorBase>() { m });

            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            sd.Command(primary: CommandType.On);
            Assert.AreEqual(StateType.Off, sd.State.Primary);
            motion.Command(primary: CommandType.On);
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
        }


        [TestMethod]
        public void DelayTest()
        {
            var m = new MapCommand(
                primaryInput: CommandType.Motion,
                primaryOutput: CommandType.Off,
                delaySecs: 2);
            var sd = new StateDevice(behaviors: new List<BehaviorBase>() { m });
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            sd.Command(primary: CommandType.Motion);
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            Thread.Sleep(1000);
            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            Thread.Sleep(2000);
            Assert.AreEqual(StateType.Off, sd.State.Primary);

        }
    }
}
