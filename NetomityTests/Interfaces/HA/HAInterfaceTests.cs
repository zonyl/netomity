using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Interfaces.Basic;
using Netomity.Interfaces.HA;
using Netomity.Utility;
using System.Diagnostics;
using System.Threading;

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
        public void SendSyncGoodNoiseTest()
        {
            _sp.Timeout = 2000;
            var result = _ha.Send(_sp);
            _ha._DataReceived("asdfasdfasdf");
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
    }
}
