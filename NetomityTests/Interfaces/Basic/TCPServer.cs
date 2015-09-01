using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Interfaces.Basic;
using System.Threading;

namespace NetomityTests.Interfaces.Basic
{
    [TestClass]
    public class TCPServerTests
    {
        TCPServer _tcp = null;
        string _msg = null;

        [TestInitialize]
        public void SetUp()
        {
            _tcp = new TCPServer(port: 13000);
        }

        [TestMethod]
        public void TestInitialize()
        {
            Assert.IsNotNull(_tcp);
        }

//        [TestMethod]
        public void Open()
        {
            _tcp.DataReceived += OpenTestMethod;
            var task = _tcp.Open();
            task.Wait(60000);

        }

        public void OpenTestMethod(string data)
        {

        }
        [TestMethod]
        public void DoubleConnectTest()
        {
            _tcp.DataReceived += DoubleConnectMethod;
            _tcp.Open();
            var client1 = new TCPClient("127.0.0.1", 13000);
            client1.Open();
            client1.Send("asdf1\n");
            Thread.Sleep(1000);
            client1.Close();
            Thread.Sleep(1000);
            client1.Open();
            Thread.Sleep(1000);
            client1.Send("asdf2");
            Thread.Sleep(1000);
            Assert.AreEqual(_msg, "asdf2");
            //Thread.Sleep(10000);

        }

        public void DoubleConnectMethod(string message)
        {
            _msg = message;
        }
    }
}
