using System;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.Integration.Interceptors
{
    [TestClass]
    public class NullInterceptorTest
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();
        private IProxiiTester _proxy;

        [TestInitialize]
        public void SetUp()
        {
            var interceptors = new IInterceptor[] { new NullInterceptor() };

            _proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), interceptors);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Proxii_RejectNullArguments_ThrowsOnNullArgument()
        {
            _proxy.Concat(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Proxii_RejectNullArguments_GetsNullArgName_FirstArg()
        {
            try
            {
                _proxy.Concat(null, "b", "c");
            }
            catch (ArgumentNullException e)
            {
                // need to extract the parameter name from the message
                var paramName = e.Message.Substring(e.Message.IndexOf(":") + 2);

                Assert.AreEqual("a", paramName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Proxii_RejectNullArguments_GetsNullArgName_OtherArg()
        {
            try
            {
                _proxy.Concat("a", null, "c");
            }
            catch (ArgumentNullException e)
            {
                // need to extract the parameter name from the message
                var paramName = e.Message.Substring(e.Message.IndexOf(":") + 2);

                Assert.AreEqual("b", paramName);
                throw;
            }
        }

        [TestMethod]
        public void Proxii_RejectNullArguments_DoesNothingWhenNoNullArguments()
        {
            _proxy.Concat("a", "b", "c");
        }
    }
}
