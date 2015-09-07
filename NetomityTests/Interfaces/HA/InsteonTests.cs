using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Interfaces;
using Netomity.Interfaces.HA;
using Netomity.Utility;
using Netomity.Interfaces.Basic;
using System.Threading;

namespace NetomityTests.Interfaces
{
    [TestClass]
    public class InsteonTests
    {
        Insteon _i = null;
        Netomity.Interfaces.Basic.Fakes.StubBasicInterface _t = null;
        string _data = null;

        [TestInitialize]
        public void SetUp()
        {
            _t = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface()
            {
                SendString = (data) => { _data = data;  },
            };

            _i = new Netomity.Interfaces.HA.Insteon(basicInterface: _t);
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
            _i.Command(new Command()
            {
                Type = CommandType.On,
                Destination = "19.05.7b"

            });
            var aData = Conversions.AsciiToHex(_data);
            Assert.AreEqual(sentData, aData);

        }

        [TestMethod]
        public void CommandOnFunctionalTests1()
        {
            var t = new TCPClient(hostName: "192.168.12.161", port: 3333);
            var i = new Insteon(basicInterface: t);
            t.Open();
            Thread.Sleep(2000);
            //var sentData = "026219057B0F11FF";
                    //        0262382E3F0F113F
            //var aSentData = Conversions.HexToAscii(sentData);
            i.Command(new Command()
            {
                Type = CommandType.On,
                Destination = "38.2E.A2"

            });
//            var aData = Conversions.AsciiToHex(_data);
            //Assert.AreEqual(sentData, aData);
            Thread.Sleep(20000);
        }
    }
}
