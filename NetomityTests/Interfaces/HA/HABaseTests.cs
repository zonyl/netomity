using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity.Interfaces.Basic;
using Netomity.Interfaces.HA;

namespace NetomityTests.Interfaces.HA
{
    [TestClass]
    public class HABaseTests
    {
        HABase _ha = null;
        BasicInterface _t = null;
        string _data = null;

        [TestInitialize]
        public void SetUp()
        {
            _t = new Netomity.Interfaces.Basic.Fakes.StubBasicInterface()
            {
                SendString = (data) => { _data = data; },
            };

            _ha = new HABase(basicInterface: _t);
        }

        [TestMethod]
        public void InitializeTest()
        {
            Assert.IsNotNull(_ha);

        }
    }
}
