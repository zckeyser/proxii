using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Internal.Interceptors;
using Proxii.Test.Util;

namespace Proxii.Test.Integration.Interceptors
{
    [TestClass]
    public class MaxCallsInterceptorTest
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();

        [TestMethod]
        public void Integration_MaxCallsInterceptor_SingleMethod()
        {
            var logger = new Logger();
            var interceptors = new IInterceptor[] { new MaxCallsInterceptor(5) };

            var proxy = _generator.CreateInterfaceProxyWithTarget<IProxiiTester>(new ProxiiTester(), interceptors);

            for (var i = 0; i < 100; i++) proxy.DoAction(() => logger.Log("foo"));

            // only the first five should've done anything
            var history = logger.GetHistory();
            var expected = new[] { "foo", "foo", "foo", "foo", "foo" };

            Assert.IsTrue(history.SequenceEqual(expected));
        }

        [TestMethod]
        public void Integration_MaxCallsInterceptor_MultipleMethods()
        {
            var logger = new Logger();
            var interceptors = new IInterceptor[] { new MaxCallsInterceptor(5) };

            var proxy = _generator.CreateInterfaceProxyWithTarget<IProxiiTester>(new ProxiiTester(), interceptors);

            for (var i = 0; i < 100; i++)
            {
                proxy.DoAction(() => logger.Log("foo"));
                proxy.DoFunc<int>(() => { logger.Log("bar"); return 1; });
            }

            // only the first five should've done anything
            var history = logger.GetHistory();
            var expected = new[] { "foo", "bar", "foo", "bar", "foo", "bar", "foo", "bar", "foo", "bar" };

            Assert.IsTrue(history.SequenceEqual(expected));
        }

        [TestMethod]
        public void Integration_MaxCallsInterceptor_FuncReturnValues()
        {
            var interceptors = new IInterceptor[] { new MaxCallsInterceptor(5) };

            var proxy = _generator.CreateInterfaceProxyWithTarget<IProxiiTester>(new ProxiiTester(), interceptors);

            var valueResults = new List<int>();
            var refResults = new List<string>();

            for (var i = 0; i < 100; i++)
            {
                valueResults.Add(proxy.DoFunc(() => 1));
                refResults.Add(proxy.DoFunc(() => "foo"));
            }

            var valueExpected = new List<int> {1, 1, 1, 1, 1};
            for (var i = 0; i < 95; i++) valueExpected.Add(0);

            var refExpected = new List<string> { "foo", "foo", "foo", "foo", "foo" };
            for (var i = 0; i < 95; i++) refExpected.Add(null);

            Assert.IsTrue(valueResults.SequenceEqual(valueExpected));
            Assert.IsTrue(refResults.SequenceEqual(refExpected));
        }
    }
}
