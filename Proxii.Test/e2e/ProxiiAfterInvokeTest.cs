using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.e2e
{
	[TestClass]
	public class ProxiiAfterInvokeTest
	{
		[TestMethod]
		public void Proxii_AfterInvoke_InvokesAfterMethodCall()
		{
			var tester = new InvokeHookTester();

			// if this gets invoked before SetValue, this will not be the end value
			Action afterHook = () => tester.Value = 10;

			var proxy = Proxii.Proxy<IInvokeHookTester>()
				.With(tester)
				.AfterInvoke(afterHook)
				.Create();

			proxy.SetValue(15);

			Assert.AreEqual(10, tester.Value);
		}
	}
}
