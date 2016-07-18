﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.Integration
{
	[TestClass]
	public class ProxiiTest
	{
		[TestMethod]
		public void Proxii_WiresInterfaceToImpl_ByType()
		{
			var proxy = Proxii.Proxy<IProxiiTester>().With<ProxiiTester>().Create();

			var test = new[] { "foo" };

			proxy.DoAction(() => test[0] = "bar");

			Assert.AreEqual("bar", test[0]);
		}

		[TestMethod]
		public void Proxii_WiresInterfaceToImpl_ByObject()
		{
			var proxy = Proxii.Proxy<IProxiiTester>().With(new ProxiiTester()).Create();

			var test = new[] { "foo" };

			proxy.DoAction(() => test[0] = "bar");

			Assert.AreEqual("bar", test[0]);
		}
	}
}
