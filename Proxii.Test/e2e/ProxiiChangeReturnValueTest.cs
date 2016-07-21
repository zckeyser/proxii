using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.e2e
{
	[TestClass]
	public class ProxiiChangeReturnValueTest
	{
		[TestMethod]
		public void Integration_ReturnValueInterceptor_NonMatchingType()
		{
			Func<int, int> changeInt = i => i * 10;

			var proxy = Proxii.Proxy<IProxiiTester>()
				.With<ProxiiTester>()
				.ChangeReturnValue(changeInt)
				.Create();
			
			var result = proxy.StringMethod();
			const string expected = ProxiiTester.StringRetVal;

			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void Integration_ReturnValueInterceptor_MatchingType()
		{
			Func<int, int> changeInt = i => i * 10;

			var proxy = Proxii.Proxy<IProxiiTester>()
				.With<ProxiiTester>()
				.ChangeReturnValue(changeInt)
				.Create();

			var result = proxy.IntMethod();
			const int expected = ProxiiTester.IntRetVal * 10;

			Assert.AreEqual(expected, result);
		}
	}
}
