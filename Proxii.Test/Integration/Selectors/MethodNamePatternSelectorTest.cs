using System;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using Proxii.Library.Selectors;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.Integration.Selectors
{
    [TestClass]
    public class MethodNamePatternSelectorTest
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Integration_MethodNamePatternSelector_NoPatterns()
        {
            var logger = new Logger();
            
            var selector = new MethodNamePatternSelector();
            var interceptors = new IInterceptor[] { new ExceptionInterceptor(typeof(ArgumentNullException), e => logger.Log("foo")) };

            var proxy =
                (IProxiiTester)
                    _generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(),
                        new ProxyGenerationOptions {Selector = selector},
                        interceptors);

            proxy.ThrowAction(new ArgumentNullException());
        }

        [TestMethod]
        public void Integration_MethodNamePatternSelector_SinglePattern_Matching()
        {
            var logger = new Logger();

            var selector = new MethodNamePatternSelector("^Thr.*");
            var interceptors = new IInterceptor[] { new ExceptionInterceptor(typeof(ArgumentNullException), e => logger.Log("foo")) };

            var proxy =
                (IProxiiTester)
                    _generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(),
                        new ProxyGenerationOptions { Selector = selector },
                        interceptors);

            proxy.ThrowAction(new ArgumentNullException());

            var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual("foo", history[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Integration_MethodNamePatternSelector_SinglePattern_NonMatching()
        {
            var logger = new Logger();

            var selector = new MethodNamePatternSelector("^Thr$");
            var interceptors = new IInterceptor[] { new ExceptionInterceptor(typeof(ArgumentNullException), e => logger.Log("foo")) };

            var proxy =
                (IProxiiTester)
                    _generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(),
                        new ProxyGenerationOptions { Selector = selector },
                        interceptors);

            proxy.ThrowAction(new ArgumentNullException());

            Assert.Fail("ThrowAction should not have been intercepted");
        }

        [TestMethod]
        public void Integration_MethodNamePatternSelector_MultiplePatterns_Matching()
        {
            var logger = new Logger();

            var selector = new MethodNamePatternSelector("^Thr.*");
            var interceptors = new IInterceptor[] { new ExceptionInterceptor(typeof(ArgumentNullException), e => logger.Log("foo")) };

            var proxy =
                (IProxiiTester)
                    _generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(),
                        new ProxyGenerationOptions { Selector = selector },
                        interceptors);

            proxy.ThrowAction(new ArgumentNullException());
            proxy.ThrowFunc(new ArgumentNullException(), 1);

            var history = logger.GetHistory();

            Assert.AreEqual(2, history.Count);
            Assert.AreEqual("foo", history[0]);
            Assert.AreEqual("foo", history[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Integration_MethodNamePatternSelector_MultiplePatterns_NonMatching()
        {
            var logger = new Logger();

            var selector = new MethodNamePatternSelector("^Thr$");
            var interceptors = new IInterceptor[] { new ExceptionInterceptor(typeof(ArgumentNullException), e => logger.Log("foo")) };

            var proxy =
                (IProxiiTester)
                    _generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(),
                        new ProxyGenerationOptions { Selector = selector },
                        interceptors);

            proxy.ThrowAction(new ArgumentNullException());

            Assert.Fail("ThrowAction should not have been intercepted");
        }
    }
}
