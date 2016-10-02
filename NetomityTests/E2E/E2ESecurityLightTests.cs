using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices;
using System.Collections.Generic;
using Netomity.Core.Enum;
using System.Threading;

namespace NetomityTests.E2E
{
    [TestClass]
    public class E2ESecurityLightTests
    {
        Motion _motion;
        Light _light;
        Location _loc;

        [TestInitialize]
        public void SetUp()
        {
            Netomity.Utility.SystemTime.Now = new DateTime(2015, 1, 1, 12, 0, 0);
            _loc = new Location();
            _motion = new Motion();
            _light = new Light(
                devices: new List<StateDevice>() { _motion },
                restrictionDevice: _loc,
                restrictionState: StateType.Light,
                idleCommandPrimary: CommandType.Level,
                idleCommandSecondary: "20",
                idleTimeSecs: 60
                );

        }

        [TestMethod]
        public void SecurityLightMotionDuringDayShouldntTurnOn()
        {
            Netomity.Utility.SystemTime.Now = new DateTime(2015, 1, 1, 12, 0,0);
            var loc = new Location();
            var motion = new Motion();
            var light = new Light(
                devices: new List<StateDevice>() { motion },
                restrictionDevice: loc,
                restrictionState: StateType.Light
                );
            Assert.AreEqual(StateType.Unknown, light.State.Primary);
            Assert.AreEqual(StateType.Unknown, motion.State.Primary);
            Assert.AreEqual(StateType.Light, loc.State.Primary);

            motion.Command(CommandType.Still);

            Assert.AreEqual(StateType.Off, light.State.Primary);
            Assert.AreEqual(StateType.Still, motion.State.Primary);

            motion.Command(CommandType.Motion);

            Assert.AreEqual(StateType.Off, light.State.Primary);
            Assert.AreEqual(StateType.Motion, motion.State.Primary);

        }
    }
}
