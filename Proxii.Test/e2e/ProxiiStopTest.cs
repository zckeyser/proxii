using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util;

namespace Proxii.Test.e2e
{
    [TestClass]
    public class ProxiiStopTest
    {
        private IProxiiTester proxy;

        [TestInitialize]
        public void SetUp()
        {
            proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                        .Stop()
                        .Create();
        }

        [TestMethod]
        public void Integration_StopMethodInterceptor_VoidMethod()
        {
            // should get blocked and do nothing, so if this doesn't fail it works
            proxy.ThrowAction(new ArgumentException());
        }

        [TestMethod]
        public void Integration_StopMethodInterceptor_ReferenceReturnType()
        {
            // we should get null on returning a reference type
            var result = proxy.StringMethod();
            Assert.AreEqual(default(string), result);
        }

        [TestMethod]
        public void Integration_StopMethodInterceptor_ValueReturnType()
        {
            // we should get the default on returning a value type
            var result = proxy.IntMethod();
            Assert.AreEqual(default(int), result);
        }
    }
}
