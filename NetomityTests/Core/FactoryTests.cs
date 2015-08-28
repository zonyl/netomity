using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Netomity;
using Netomity.Core;
using Netomity.Interfaces;
using Netomity.Interfaces.Basic;

namespace NetomityTests.Core
{
    [TestClass]
    public class FactoryTests
    {
        NFactory _nF = null;

        [TestInitialize]
        public void SetUp()
        {
            _nF = NFactory.Create();
        }


        [TestMethod]
        public void Instantiation()
        {
            Assert.IsNotNull(_nF);
        }

        public void BasicInterfaceInstantion()
        {
            var i = _nF.Create<TCPServer>();
            Assert.IsNotNull(i);

        }
    }
}
