using System;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using Proxii.Test.Util.TestClasses;
using System.Reflection;

namespace Proxii.Test.Integration.Interceptors
{
	[TestClass]
	public class BeforeInvokeInterceptorTest
	{
		private readonly ProxyGenerator _generator = new ProxyGenerator();

		[TestMethod]
		public void Integration_BeforeInvokeInterceptor_InvokesAfterAction()
		{
			var tester = new InvokeHookTester();

			// if this gets invoked before SetValue, this will not be the end value
			Action beforeHook = () => tester.Value = 10;

			var interceptors = new IInterceptor[] { new BeforeInvokeInterceptor(beforeHook) };
			var proxy = (IInvokeHookTester)_generator.CreateInterfaceProxyWithTarget(typeof(IInvokeHookTester), tester, interceptors);

			proxy.SetValue(15);

			Assert.AreEqual(15, tester.Value);
		}

        [TestMethod]
        public void Integration_BeforeInvokeInterceptor_WithMethodInfo_InvokesAfterAction()
        {
            var tester = new InvokeHookTester();
            var logger = new Logger();

            // if this gets invoked before SetValue, this will not be the end value
            Action<MethodInfo> beforeHook = (method) => {
                tester.Value = 10;
                logger.Log(method.Name);
            };

            var interceptors = new IInterceptor[] { new BeforeInvokeInterceptor(beforeHook) };
            var proxy = (IInvokeHookTester)_generator.CreateInterfaceProxyWithTarget(typeof(IInvokeHookTester), tester, interceptors);

            proxy.SetValue(15);

            // make sure we're still invoking at the correct time
            Assert.AreEqual(15, tester.Value);

            // verify the MethodInfo passed in is correct
            var history = logger.GetHistory();
            Assert.AreEqual(1, history.Count);
            Assert.AreEqual("SetValue", history[0]);
        }

        [TestMethod]
        public void Integration_BeforeInvokeInterceptor_WithMethodInfoAndArgs_InvokesAfterAction()
        {
            var tester = new InvokeHookTester();
            var logger = new Logger();

            // if this gets invoked before SetValue, this will not be the end value
            Action<MethodInfo, object[]> beforeHook = (method, args) => {
                tester.Value = 10;
                logger.Log(method.Name);
                logger.Log(args[0].ToString());
            };

            var interceptors = new IInterceptor[] { new BeforeInvokeInterceptor(beforeHook) };
            var proxy = (IInvokeHookTester)_generator.CreateInterfaceProxyWithTarget(typeof(IInvokeHookTester), tester, interceptors);

            proxy.SetValue(15);

            // make sure we're still invoking at the correct time
            Assert.AreEqual(15, tester.Value);

            // verify the MethodInfo passed in is correct
            var history = logger.GetHistory();
            Assert.AreEqual(2, history.Count);
            Assert.AreEqual("SetValue", history[0]);

            // verify the arguments passed in are correct
            Assert.AreEqual("15", history[1]);
        }
    }
}
