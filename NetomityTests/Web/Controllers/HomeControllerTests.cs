using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Web;
using System.Web.Http;
using System.Net.Http.Formatting;
using Netomity.Devices;
using System.Collections.Generic;
using Netomity.Core;
using Netomity.Core.Enum;

namespace NetomityTests.Web.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        HomeController _hc = null;

        [TestInitialize]
        public void SetUp()
        {
            _hc = new HomeController();
            _hc.BaseFolder = @"C:\projects\Netomity\Netomity\Web\Content\";
        }

        [TestMethod]
        public void TestInitialize()
        {
            Assert.IsNotNull(_hc);
        }

        [TestMethod]
        public void DevicesGetEmptyTest()
        {
            var response = _hc.Devices(new FormDataCollection("http://www.test.com"));
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void DevicesSetStateTest()
        {
            var sd = new StateDevice() { Name = "Test Device 1" };

            Assert.AreEqual(StateType.Unknown, sd.State.Primary);
            var fd = new FormDataCollection( new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string,string>(sd.Id.ToString(), CommandType.On.ToString())
            });

            var response = _hc.Devices(fd);
            Assert.AreEqual(StateType.On, sd.State.Primary);


        }
    }
}
