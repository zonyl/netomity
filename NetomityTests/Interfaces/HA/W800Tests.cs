using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Interfaces;
using Netomity.Interfaces.HA;
using Netomity.Utility;
using Netomity.Interfaces.Basic;
using System.Threading;
using Netomity.Core;
using Netomity.Core.Enum;

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
            [TestMethod]
        public void OnCommandCorruptTests()
        {
            Command command = null;
            const string address = "o13";
            // A1 OFF
            //(2403C03F
            _i.OnCommand(source: address, action: (c) => { command = c; });
            _i._DataReceived(Conversions.BinaryStrToAscii("00 10 01 00   00 00 00 11   11 00 00 00  00 11 11 11"));
//            _i._DataReceived(Conversions.HexToAscii("2403C03F"));
            Thread.Sleep(2000);
            Assert.IsNull(command);
        }

    }
}
