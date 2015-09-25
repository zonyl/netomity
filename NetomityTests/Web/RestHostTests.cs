using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Web;
using System.Threading;
using Netomity.Devices;
using System.Net;

namespace NetomityTests.Web
{
    [TestClass]
    public class RestHostTests
    {
        RestHost _rh = null;
        const string BASE_ADDR = "http://localhost:8081/Netomity/API/v1";

        [TestInitialize]
        public void SetUp()
        {
            _rh = new RestHost(baseUrl: BASE_ADDR);
        }

        [TestMethod]
        public void ObjectsQueryTest()
        {
            var sd1 = new StateDevice();
            var sd2 = new StateDevice();
            Thread.Sleep(1000);
            WebClient c = new WebClient();
            var r = c.DownloadString(BASE_ADDR + "/objects/device");
            Assert.AreNotEqual("", r);
        }
    }
}
