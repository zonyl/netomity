using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Interfaces.Basic;
using Netomity.Interfaces.HA;
using Netomity.Utility;
using System.Diagnostics;
using System.Threading;
using Netomity.Interfaces;
using Netomity.Core;
using System.Collections.Generic;
using Netomity.Core.Enum;

namespace NetomityTests.Interfaces.HA
{
    [TestClass]
    public class EmailServerTests
    {
        EmailServer _es = null;

        [TestInitialize]
        public void SetUp()
        {
            _es = new EmailServer(
                address: "smtp.gmail.com",
                port: 587,
                isSSL: true,
                fromAddress: "netomity@sharpee.com",
                password: "1234"
                );
        }

        [TestMethod]
        public void InitializeTest()
        {
            Assert.IsNotNull(_es);

        }

        [TestMethod]
        public void EmailSendTest()
        {
            const string TO_ADDRESS = "jason@sharpee.com";
            const string TO_ADDRESS_NAME = "Jason Sharpee";
            const string SUBJECT = "Test Email from Netomoity";
            const string BODY = "This is a test email from the Netomity Automation System";
            var command = new Command()
            {
                Primary = CommandType.Notify,
                Secondary = BODY,
                StringParams = new Dictionary<StringEnum, string>(){
                    {NotificationParamType.ToAddress, TO_ADDRESS},
                    {NotificationParamType.ToAddressName, TO_ADDRESS_NAME},
                    {NotificationParamType.Subject, SUBJECT},
                },
            };
            var r = _es.Command(command);
            Assert.IsTrue(r.Result);
        }
    }
}
