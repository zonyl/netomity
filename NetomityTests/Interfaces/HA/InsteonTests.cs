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
                Type = CommandType.Off,
                Destination = "38.2E.A2"

            });
//            var aData = Conversions.AsciiToHex(_data);
            //Assert.AreEqual(sentData, aData);
            Thread.Sleep(10000);
        }
    }

//    Debug:HA Interface Received: >P2z4\0\0Ë< (0250327A34000004CB1304)
//Debug:Queueing command
//Debug:Waiting Results
//Debug:Waiting
//Debug:HA Interface Sending: >b8.¢\0< (0262382EA20F1300)
//Debug:Sent: b8.¢\0
//Debug:Received: b8.¢\0
//Debug:HA Interface Received: >b8.¢\0< (0262382EA20F130006)
//Debug:Received: P2z4\0\0Ã
//Debug:HA Interface Received: >P2z4\0\0Ã< (0250327A34000004C31304)
//Debug:Received: P2z4ÓA
//Debug:HA Interface Received: >P2z4ÓA< (0250327A3418D380411304)
//Debug:Received: P2z4ÓF
//Debug:HA Interface Received: >P2z4ÓF< (0250327A3418D380461304)
//Debug:Received: P2z4Ë\0
//Debug:HA Interface Received: >P2z4Ë\0< (0250327A34130104CB0600)
//Debug:Received: P2z4Ë\0
//Debug:HA Interface Received: >P2z4Ë\0< (0250327A34130104CB0600)
//Debug:Timeout Of Command: b8.¢\0

}
