using System;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.Integration.Interceptors
{
	[TestClass]
	public class AfterInvokeInterceptorTest
	{
		private readonly ProxyGenerator _generator = new ProxyGenerator();

		[TestMethod]
		public void Integration_AfterInvokeInterceptor_InvokesAfterAction()
		{
			var tester = new InvokeHookTester();

			// if this gets invoked after SetValue, this will be the end value
			Action afterHook = () => tester.Value = 10;

			var interceptors = new IInterceptor[] { new AfterInvokeInterceptor(afterHook) };
			var proxy = (IInvokeHookTester) _generator.CreateInterfaceProxyWithTarget(typeof (IInvokeHookTester), tester, interceptors);

			proxy.SetValue(15);

			Assert.AreEqual(tester.Value, 10);
		}
	}
}
