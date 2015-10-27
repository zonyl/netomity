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
        public void CommandOnTests()
        {
            var sentData = "026219057B0F11FF";
            var aSentData = Conversions.HexToAscii(sentData);
            var result = _i.Command(new Command()
            {
                Primary = CommandType.On,
                Destination = "19.05.7b"

            });
            Assert.IsFalse(result.Result);
//            Thread.Sleep(2000);
            var aData = Conversions.AsciiToHex(_data);
            Assert.AreEqual(sentData, aData);

        }

        [TestMethod]
        public void OnCommandTests()
        {
            Command command = null;
            //9/16/2015 5:44:22 PM|Debug|InsteonSerial:
            //Received Data:>P2z4  Ë< (02 50 32 7A 34 00 00 01 CB 11 01)
            _i.OnCommand("32.7a.34", action: (c) => {command=c;});
            _i._DataReceived(Conversions.HexToAscii("0250327A34000001CB1101"));
            Thread.Sleep(2000);
            Assert.IsNotNull(command);
            Assert.AreEqual(command.Primary, CommandType.On);
            Assert.AreEqual(command.Source, "32.7a.34");


        }
    }

}
