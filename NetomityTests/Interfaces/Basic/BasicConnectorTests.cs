using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Interfaces.Basic;
using Netomity.Core;
using System.Threading.Tasks;
using System.Threading;

namespace NetomityTests.Interfaces.Basic
{
    [TestClass]
    public class BasicConnectorTests
    {
        BasicConnector _bc = null;
        BasicInterface _intA = null;
        BasicInterface _intB = null;
        bool _intAOpen = false;
        bool _intBOpen = false;

        [TestInitialize]
        public void SetUp()
        {
            var logger = new Netomity.Core.Fakes.StubLogger();
            _intA = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface()
            {
                Open01 = () =>
                {
                    _intAOpen = true;
                    return Task.Factory.StartNew(() => {});
                },
                IsOpenGet = () => { return _intAOpen; },
                
            };
            _intB = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface()
            {
                Open01 = () =>
                {
                    _intBOpen = true;
                    return Task.Factory.StartNew(() => {});
                },
                IsOpenGet = () => { return _intBOpen; },
                
            };
            _bc = new BasicConnector(_intA, _intB);

        }

        [TestMethod]
        public void Instantiation()
        {
            Assert.IsNotNull(_bc);
        }

        [TestMethod]
        public void OpenTests()
        {
            Assert.IsFalse(_intA.IsOpen);
            Assert.IsFalse(_intB.IsOpen);
            _bc.Open();
            Thread.Sleep(2000);
            Assert.IsTrue(_intA.IsOpen);
            Assert.IsTrue(_intB.IsOpen);
        }

        //[TestMethod]
        //public void Functional1()
        //{
        //    var i1 = new TCPServer(2222);
        //    var i2 = new SerialIO();

        //    var bc = new BasicConnector(i1, i2);
        //    var t =  bc.Open();

        //    t.Wait(2000);
        //    Assert.IsTrue(i1.IsOpen);
        //    Assert.IsFalse(i2.IsOpen);


        //}

        //[TestMethod]
        //public void Functional2()
        //{

        //    var log = new Logger(@"c:\temp\log.txt");
        //    var n = NetomitySystem.Factory();
        //    var i1 = new TCPServer(2222);
        //    var i2 = new TCPServer(2223);

        //    var bc = new BasicConnector(i1, i2);
        //    var t = bc.Open();

        //    t.Wait(2000);
        //    Assert.IsTrue(i1.IsOpen);
        //    Assert.IsTrue(i2.IsOpen);
        //    t.Wait(5000);
        //}
    }
}
