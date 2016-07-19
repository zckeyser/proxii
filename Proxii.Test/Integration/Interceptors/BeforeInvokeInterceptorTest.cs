using System;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using Proxii.Test.Util.TestClasses;

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
	}
}
