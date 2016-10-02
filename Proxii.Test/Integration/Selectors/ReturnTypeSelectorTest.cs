using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Selectors;
using Proxii.Test.Util;

namespace Proxii.Test.Integration.Selectors
{
    [TestClass]
    public class ReturnTypeSelectorTest
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();

        [TestMethod]
        public void Integration_ReturnTypeSelector_SingleType_Matches()
        {
            var logger = new Logger();
            var selector = new ReturnTypeSelector(typeof(int));
            var options = new ProxyGenerationOptions { Selector = selector };
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };
            
            var proxy = (IProxiiTester) _generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), options, interceptors);

            var result = proxy.IntMethod();

            // the result shouldn't be affected
            Assert.AreEqual(ProxiiTester.IntRetVal, result);

            var logResults = logger.GetHistory();

            // the method should have been intercepted
            Assert.AreEqual(1, logResults.Count);
            Assert.AreEqual("IntMethod", logResults[0]);
        }

        [TestMethod]
        public void Integration_ReturnTypeSelector_SingleType_DoesNotMatch()
        {
            var logger = new Logger();
            var selector = new ReturnTypeSelector(typeof(string));
            var options = new ProxyGenerationOptions { Selector = selector };
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };

            var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), options, interceptors);

            var result = proxy.IntMethod();

            // the result shouldn't be affected
            Assert.AreEqual(ProxiiTester.IntRetVal, result);

            var logResults = logger.GetHistory();

            // the method shouldn't have been intercepted
            Assert.AreEqual(0, logResults.Count);
        }

        [TestMethod]
        public void Integration_ReturnTypeSelector_MultipleTypes_Matches()
        {
            var logger = new Logger();
            var selector = new ReturnTypeSelector(typeof(int), typeof(string));
            var options = new ProxyGenerationOptions { Selector = selector };
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };

            var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), options, interceptors);

            var result1 = proxy.IntMethod();
            var result2 = proxy.StringMethod();

            // the results shouldn't be affected
            Assert.AreEqual(ProxiiTester.IntRetVal, result1);
            Assert.AreEqual(ProxiiTester.StringRetVal, result2);

            var logResults = logger.GetHistory();

            // the methods should have been intercepted
            Assert.AreEqual(2, logResults.Count);
            Assert.AreEqual("IntMethod", logResults[0]);
            Assert.AreEqual("StringMethod", logResults[1]);
        }

        [TestMethod]
        public void Integration_ReturnTypeSelector_MultipleTypes_DoesNotMatch()
        {
            var logger = new Logger();
            var selector = new ReturnTypeSelector(typeof(long), typeof(double));
            var options = new ProxyGenerationOptions { Selector = selector };
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };

            var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), options, interceptors);

            var result1 = proxy.IntMethod();
            var result2 = proxy.StringMethod();

            // the results shouldn't be affected
            Assert.AreEqual(ProxiiTester.IntRetVal, result1);
            Assert.AreEqual(ProxiiTester.StringRetVal, result2);

            var logResults = logger.GetHistory();

            // the methods shouldn't have been intercepted
            Assert.AreEqual(0, logResults.Count);
        }

        [TestMethod]
        public void Integration_ReturnTypeSelector_MultipleInterceptors_Matches()
        {
            var logger = new Logger();
            var selector = new ReturnTypeSelector(typeof(int));
            var options = new ProxyGenerationOptions { Selector = selector };
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger), new LoggingInterceptor(logger) };

            var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), options, interceptors);

            var result = proxy.IntMethod();

            // the result shouldn't be affected
            Assert.AreEqual(ProxiiTester.IntRetVal, result);

            var logResults = logger.GetHistory();

            // the method should have been intercepted by both interceptors
            Assert.AreEqual(2, logResults.Count);
            Assert.AreEqual("IntMethod", logResults[0]);
            Assert.AreEqual("IntMethod", logResults[1]);
        }

        [TestMethod]
        public void Integration_ReturnTypeSelector_MultipleInterceptors_DoesNotMatch()
        {
            var logger = new Logger();
            var selector = new ReturnTypeSelector(typeof(string));
            var options = new ProxyGenerationOptions { Selector = selector };
            var interceptors = new IInterceptor[] { new LoggingInterceptor(logger), new LoggingInterceptor(logger) };

            var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), options, interceptors);

            var result = proxy.IntMethod();

            // the result shouldn't be affected
            Assert.AreEqual(ProxiiTester.IntRetVal, result);

            var logResults = logger.GetHistory();

            // the method shouldn't have been intercepted
            Assert.AreEqual(0, logResults.Count);
        }
    }
}
