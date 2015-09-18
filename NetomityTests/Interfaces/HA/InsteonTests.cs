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
            _data = null;
            _t = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface()
            {
                SendString = (data) => { _data = data;  },
            };

            _i = new Insteon(basicInterface: _t);
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
                Type = CommandType.On,
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
            Assert.AreEqual(command.Type, CommandType.On);
            Assert.AreEqual(command.Source, "32.7a.34");


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


//DOOR open / close
//9/16/2015 5:44:22 PM|Debug|InsteonSerial:Received Data:>P2z4  Ë<(0250327A34000001CB1101)
//9/16/2015 5:44:22 PM|Debug|InsteonSerial:Received Data:>P2z4  Ë<(0250327A34000001CB1101)
//9/16/2015 5:44:23 PM|Debug|InsteonSerial:Received Data:>P2z4ÓA< (0250327A3418D380411101)
//9/16/2015 5:44:23 PM|Debug|InsteonSerial:Received Data:>P2z4Ë < (0250327A34110201CB0600)
//9/16/2015 5:44:23 PM|Debug|:HeartBeat
//9/16/2015 5:44:23 PM|Debug|InsteonSerial:Received Data:>P2z4Ë < (0250327A34110201CB0600)
//9/16/2015 5:44:32 PM|Debug|InsteonSerial:Received Data:>P2z4  Ã<(0250327A34000001C31301)
//9/16/2015 5:44:32 PM|Debug|InsteonSerial:Received Data:>P2z4  < (0250327A34000001C71301)
//9/16/2015 5:44:32 PM|Debug|InsteonSerial:Received Data:>Ç<      (01C71301)
//9/16/2015 5:44:33 PM|Debug|InsteonSerial:Received Data:>P2z4ÓA< (0250327A3418D380411301)
//9/16/2015 5:44:33 PM|Debug|InsteonSerial:Received Data:>P2z4Ç < (0250327A34130201C70600)
//9/16/2015 5:44:34 PM|Debug|InsteonSerial:Received Data:>P2z4Ë < (0250327A34130201CB0600)
}
