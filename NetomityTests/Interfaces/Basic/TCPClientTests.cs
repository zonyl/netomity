using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Interfaces.Basic;
using System.Collections.Generic;
using System.Threading;
using Netomity.Core;

namespace NetomityTests.Interfaces.Basic
{
    [TestClass]
    public class TCPClientTasks
    {
        string _str1 = "";
        TCPClient _tcp = null;

        [TestInitialize]
        public void SetUp()
        {
            _tcp = new TCPClient(address: "127.0.0.1", port: 2222) { Name = "Client 1" };
        }

        [TestMethod]
        public void TestInitialize()
        {
            Assert.IsNotNull(_tcp);
        }

        [TestMethod]
        public void Functional1()
        {
            var log = new Logger(@"C:\temp\log.txt");
            var tcp1 = new TCPServer(2222) { Name = "Server 1" };
            var tcp2 = new TCPServer(2223) { Name = "Server 2" };
            var tcp3 = new TCPClient("127.0.0.1", 2223) { Name = "Client 2" };
            var bc = new BasicConnector(new List<BasicInterface>() { tcp1, tcp2 }) { Name = "BC" };

            tcp3.DataReceived += ReceivedData;

            var task = bc.Open();
            task.Wait(5000);


            _tcp.Open();
            Thread.Sleep(1000);
            tcp3.Open();
            Thread.Sleep(5000);

            _tcp.Send("asdf");
            Thread.Sleep(1000);
            
            Assert.AreEqual(_str1, "asdf");

            Thread.Sleep(10000);

        
        }

        public void ReceivedData(string message)
        {
            _str1 = message;
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
    }
}
