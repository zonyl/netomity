using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Interfaces.Basic;

namespace NetomityTests.Interfaces.Basic
{
    [TestClass]
    public class TCPServerTests
    {
        TCPServer _tcp = null;

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
    }
}
