using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Interfaces.Basic;
using Netomity.Interfaces.HA;
using Netomity.Utility;
using System.Diagnostics;
using System.Threading;
using Netomity.Interfaces;
using Netomity.Core;

namespace NetomityTests.Interfaces.HA
{
    [TestClass]
    public class HAInterfaceTests
    {
        HAInterface _ha = null;
        BasicInterface _t = null;
        string _data = null;
        SendParams _sp = null;

        [TestInitialize]
        public void SetUp()
        {
            _data = null;
            _t = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface()
            {
                SendString = (data) => { _data = data; },
                DataReceivedEvent = (ev) => { _t._DataReceived(ev); },
            };

            _ha = new HAInterface(basicInterface: _t);
            _sp = new SendParams()
            {
                SendData = "asdf",
                SuccessResponse = "asdfgood",
                FailureResponse = "asdfbad",
                Timeout = 1,
            };
        }

        [TestMethod]
        public void InitializeTest()
        {
            Assert.IsNotNull(_ha);

        }

        [TestMethod]
        public void SendAsyncTest()
        {
            _sp.Timeout = 3000;
            _sp.SuccessResponse = null;
            _sp.FailureResponse = null;
            var s = new Stopwatch();
            s.Start();
            var result = _ha.Send(_sp);
            s.Stop();
            Assert.IsTrue(result.Result);
            Thread.Sleep(2000);
            Assert.IsTrue(s.ElapsedMilliseconds < 1000);
            Assert.AreEqual(_sp.SendData, _data);
        }

        [TestMethod]
        public void SendSyncTimeoutTest()
        {
            _sp.Timeout = 2000;
            var s = new Stopwatch();
            s.Start();
            var result = _ha.Send(_sp);
            Assert.IsFalse(result.Result);
            s.Stop();
            Assert.IsTrue(s.ElapsedMilliseconds >= 2000);
        }

        [TestMethod]
        public void SendSyncGoodTest()
        {
            _sp.Timeout = 10000;
            var result = _ha.Send(_sp);
            _ha._DataReceived("asdfgood");
            //Thread.Sleep(100000);
            Assert.IsTrue(result.Result);
            Assert.AreEqual(0, _ha.ReceiveQueue.Count);
        }

        [TestMethod]
        public void SendSyncGoodPartialTest()
        {
            _sp.Timeout = 10000;
            var result = _ha.Send(_sp);
            _ha._DataReceived("asdf");
            _ha._DataReceived("good");
            //Thread.Sleep(100000);
            Assert.IsTrue(result.Result);
            Assert.AreEqual(0, _ha.ReceiveQueue.Count);
        }


        [TestMethod]
        public void SendSyncGoodNoiseTest()
        {
            _sp.Timeout = 2000;
            var result = _ha.Send(_sp);
            _ha._DataReceived("asdfasdfasdf");
            Thread.Sleep(360);
            _ha._DataReceived("asdfgood");
            Assert.IsTrue(result.Result);
            Assert.AreEqual(0, _ha.ReceiveQueue.Count);
        }

        [TestMethod]
        public void SendSyncIsBadNoTimeoutdTest()
        {
            _sp.Timeout = 2000;
            var s = new Stopwatch();
            s.Start();
            var result = _ha.Send(_sp);
            _ha._DataReceived("asdfbad");
            Assert.IsFalse(result.Result);
            s.Stop();
            Assert.IsTrue(s.ElapsedMilliseconds < 2000);
            Assert.AreEqual(0, _ha.ReceiveQueue.Count);
        }

        [TestMethod]
        public void OnCommandTests()
        {
            _ha.OnCommand(source: null, callback: _OnCommandData);
            Thread.Sleep(1000);
            _ha._DataReceived("on");
            Thread.Sleep(2000);
            //Thread.Sleep(200000);
            Assert.AreEqual("on", _data.ToLower());
        }

        public void _OnCommandData(Command command)
        {
            _data = command.Primary.ToString();

        }

        [TestMethod]
        public void OnCommandActionTests()
        {
            _ha.OnCommand(source: null, action: (X) => _OnCommandData(X)) ;
            Thread.Sleep(1000);
            _ha._DataReceived("on");
            Thread.Sleep(2000);
            //Thread.Sleep(200000);
            Assert.AreEqual("on", _data.ToLower());
        }

        [TestMethod]
        public void CommandTests()
        {
            var result = _ha.Command(new Command() { Primary = CommandType.On }).Result;
            // We arent expecting a success or failure response so we need to wait for async
            // to catch up.
            Thread.Sleep(1000);
            Assert.AreEqual(CommandType.On.ToString(), _data);


        }
    }


//    Debug:HA Interface Sending: >b8.¢\0< (0262382EA20F1300)
//Debug:Sent: b8.¢\0
//Debug:Received: b8.
//Debug:HA Interface Received: >b8.< (0262382E)
//Debug:Received: ¢\0
//Debug:HA Interface Received: >¢\0< (A20F130006)
//Debug:Received Something in Queue: 0262382E 0262382EA20F130006 0262382EA20F130015
//Debug:Received Something in Queue: A20F130006 0262382EA20F130006 0262382EA20F130015
}
