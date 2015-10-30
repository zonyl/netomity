using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Interfaces;
using Netomity.Interfaces.HA;
using Netomity.Utility;
using Netomity.Interfaces.Basic;
using System.Threading;
using Netomity.Core;

namespace NetomityTests.Interfaces
{
    [TestClass]
    public class W800Tests
    {
        W800 _i = null;
        Netomity.Interfaces.Basic.Fakes.StubBasicInterface _t = null;
        string _data = null;

        [TestInitialize]
        public void SetUp()
        {
            _data = null;
            _t = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface()
            {
                SendString = (data) => { _data = data;  },
            };

            _i = new W800(iface: _t);
        }

        [TestMethod]
        public void InstantiationTests()
        {
            Assert.IsNotNull(_i);
        }

        [TestMethod]
        public void OnCommandTests()
        {
            Command command = null;
            const string address = "a1";
            // A1 OFF
            //01100000 10011111 00100000 11011111
            _i.OnCommand(source: address, action: (c) => { command = c; });
            _i._DataReceived(Conversions.BinaryStrToAscii("01100000 10011111 00100000 11011111"));
            Thread.Sleep(2000);
            Assert.IsNotNull(command);
            Assert.AreEqual(command.Primary, CommandType.Off);
            Assert.AreEqual(command.Source, address);


        }
    }

}
