using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Proxii.Test.Util;

namespace Proxii.Test.e2e
{
	[TestClass]
	public class ProxiiBeforeInvokeTest
	{
		[TestMethod]
		public void Proxii_BeforeInvoke_InvokesAfterMethodCall()
		{
			var tester = new InvokeHookTester();

			// if this gets invoked before SetValue, this will not be the end value
			Action beforeHook = () => tester.Value = 10;

			var proxy = Proxii.Proxy<IInvokeHookTester>(tester)
				.BeforeInvoke(beforeHook)
				.Create();

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

            var proxy = Proxii.Proxy<IInvokeHookTester>(tester)
                .BeforeInvoke(beforeHook)
                .Create();

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

            var proxy = Proxii.Proxy<IInvokeHookTester>(tester)
                .BeforeInvoke(beforeHook)
                .Create();

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
