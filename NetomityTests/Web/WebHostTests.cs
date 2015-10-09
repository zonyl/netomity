using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Web;
using System.Threading;
using System.Net;

namespace NetomityTests.Web
{
    [TestClass]
    public class WebHostTests
    {
        //netsh http add urlacl url=http://+:8082/ user=Everyone
        const string ADDRESS = "http://localhost:8083/";
        WebHost _wh = null;

        [TestInitialize]
        public void SetUp()
        {
            //var listener = new HttpListener();
            //listener.Prefixes.Add("http://*:8083/");
            //listener.Start();
            _wh = new WebHost(address: ADDRESS);
        }

        [TestMethod]
        public void InitializationTests()
        {
            Assert.IsNotNull(_wh);
        }

        [TestMethod]
        public void SimpleGetAllTests()
        {
            //Thread.Sleep(100000);
        }
    }
}
