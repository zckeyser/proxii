using System;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.Integration.Interceptors
{
    [TestClass]
    public class StopMethodInterceptorTest
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();
        private IProxiiTester proxy;

        [TestInitialize]
        public void SetUp()
        {
            var interceptors = new IInterceptor[] { new StopMethodInterceptor() };

            proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(),
                new ProxyGenerationOptions(), interceptors);
        }

        [TestMethod]
        public void Integration_StopMethodInterceptor_VoidMethod()
        {
            // should get blocked and do nothing, so if this doesn't fail it works
            proxy.Throw(new ArgumentException());
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
