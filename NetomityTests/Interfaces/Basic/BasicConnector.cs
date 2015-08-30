using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Interfaces.Basic;
using Netomity.Core;

namespace NetomityTests.Interfaces.Basic
{
    [TestClass]
    public class BasicConnectorTests
    {
        BasicConnector _bc = null;


        [TestInitialize]
        public void SetUp()
        {
            var logger = new Netomity.Core.Fakes.StubLogger();
            var intA = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface();
            var intB = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface();
            _bc = new BasicConnector(intA, intB);

        }

        [TestMethod]
        public void Instantiation()
        {
            Assert.IsNotNull(_bc);
        }

        [TestMethod]
        public void Functional1()
        {
            var i1 = new TCPServer(2222);
            var i2 = new SerialIO();

            var bc = new BasicConnector(i1, i2);
            var t =  bc.Open();

            t.Wait(2000);
            Assert.IsTrue(i1.IsOpen);
            Assert.IsFalse(i2.IsOpen);


        }

        [TestMethod]
        public void Functional2()
        {

            var log = new Logger(@"c:\temp\log.txt");
            var n = NetomitySystem.Factory();
            var i1 = new TCPServer(2222);
            var i2 = new TCPServer(2223);

            var bc = new BasicConnector(i1, i2);
            var t = bc.Open();

            t.Wait(2000);
            Assert.IsTrue(i1.IsOpen);
            Assert.IsTrue(i2.IsOpen);
            t.Wait(5000);



        }
    }
}
