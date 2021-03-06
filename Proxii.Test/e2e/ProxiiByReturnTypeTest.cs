﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util;

namespace Proxii.Test.e2e
{
	[TestClass]
	public class ProxiiByReturnTypeTest
	{
		[TestMethod]
		public void Proxii_ByReturnType_SingleType_Matches()
		{
			var errors = new List<Exception>();
			var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
				.Catch<Exception>(e => errors.Add(e))
				.ByReturnType(typeof(void))
				.Create();

			proxy.ThrowAction(new ArgumentException());

			Assert.AreEqual(errors.Count, 1);
			Assert.IsInstanceOfType(errors[0], typeof(ArgumentException));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Proxii_ByReturnType_SingleType_DoesNotMatch()
		{
			var errors = new List<Exception>();
            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                .Catch<Exception>(e => errors.Add(e))
                .ByReturnType(typeof(void))
                .Create();

            var result = proxy.ThrowFunc(new ArgumentException(), "foo");

			Assert.Fail("ThrowFunc should not have been intercepted.");
		}
	}
}
