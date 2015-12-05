using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Devices;
using Netomity.Core;
using System.Collections.Generic;
using Netomity.Interfaces.HA;
using System.Threading.Tasks;

namespace NetomityTests.Devices
{
    [TestClass]
    public class NotificationTests
    {
        Notification _e = null;
        EmailServer _es = null;
        Command _emailServerCommand = null;

        [TestInitialize]
        public void SetUp()
        {
            _es = new Netomity.Interfaces.HA.Fakes.StubEmailServer("", "", "", 123, true, "");
            _e = new Notification(iface: _es);
        }

        [TestMethod]
        public void InitializeTest()
        {
            Assert.IsNotNull(_e);
        }

        [TestMethod]
        public void SendTest()
        {
            const string TO_ADDRESS = "jason@sharpee.com";
            const string TO_NAME = "Jason Sharpee";
            const string SUBJECT = "Test Subject";
            const string BODY = "This is a test Netomity Message";


            Command pCommand = null;

            var es = new Netomity.Interfaces.HA.Fakes.StubEmailServer("", "", "", 123, true, "")
            {
                CommandCommand = (command) => { pCommand = command; return Task.Run(() => { return true; }); },
            };
            var e = new Notification(iface: es);
            e.Subject = SUBJECT;
            e.Message = BODY;
            e.Address = TO_ADDRESS;

            var r = e.Command(primary: CommandType.Notify);

            Assert.IsTrue(r.Result);
            Assert.AreEqual(CommandType.Notify, pCommand.Primary);
            Assert.AreEqual(BODY, pCommand.Secondary);
            Assert.AreEqual(TO_ADDRESS, pCommand.StringParams[EmailParamsType.ToAddress]);
            Assert.AreEqual(SUBJECT, pCommand.StringParams[EmailParamsType.Subject]);

            
        }
    }
}
