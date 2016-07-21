using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util.TestClasses;

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

			var proxy = Proxii.Proxy<IInvokeHookTester>()
				.With(tester)
				.BeforeInvoke(beforeHook)
				.Create();

			proxy.SetValue(15);

			Assert.AreEqual(15, tester.Value);
		}
	}
}
