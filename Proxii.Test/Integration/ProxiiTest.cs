﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.Integration
{
	[TestClass]
	public class ProxiiTest
	{
		[TestMethod]
		public void Proxii_WiresInterfaceToImpl_ByType()
		{
			var proxy = Proxii.Proxy<IProxiiTester>()
				.With<ProxiiTester>()
				.Create();

			var test = new[] { "foo" };

			proxy.DoAction(() => test[0] = "bar");

			Assert.AreEqual("bar", test[0]);
		}

		[TestMethod]
		public void Proxii_WiresInterfaceToImpl_ByObject()
		{
			var proxy = Proxii.Proxy<IProxiiTester>()
				.With(new ProxiiTester())
				.Create();

			var test = new[] { "foo" };

			proxy.DoAction(() => test[0] = "bar");

			Assert.AreEqual("bar", test[0]);
		}

		[TestMethod]
		public void Proxii_WithExceptionInterceptor_MatchingType_SingleCatch()
		{
			var errors = new List<Exception>();

			Action<Exception> onCatch = e => errors.Add(e);
			var proxy = Proxii.Proxy<IProxiiTester>()
				.With<ProxiiTester>()
				.Catch<ArgumentException>(onCatch)
				.Create();

			proxy.Throw(new ArgumentException());

			Assert.AreEqual(1, errors.Count);
			Assert.IsInstanceOfType(errors[0], typeof(ArgumentException));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Proxii_WithExceptionInterceptor_NonMatchingType_SingleCatch()
		{
			var errors = new List<Exception>();

			Action<Exception> onCatch = e => errors.Add(e);
			var proxy = Proxii.Proxy<IProxiiTester>()
				.With<ProxiiTester>()
				.Catch<IndexOutOfRangeException>(onCatch)
				.Create();

			proxy.Throw(new ArgumentException());

			Assert.Fail("Should have thrown a non-matching exception");
		}

		[TestMethod]
		public void Proxii_WithExceptionInterceptor_MatchingType_MultiCatch()
		{
			var errors = new List<Exception>();

			Action<Exception> onCatch = e => errors.Add(e);
			var proxy = Proxii.Proxy<IProxiiTester>()
				.With<ProxiiTester>()
				.Catch<ArgumentException>(onCatch)
				.Catch<ArgumentNullException>(onCatch)
				.Create();

			proxy.Throw(new ArgumentException());
			proxy.Throw(new ArgumentNullException());

			Assert.AreEqual(2, errors.Count);
			Assert.IsInstanceOfType(errors[0], typeof(ArgumentException));
			Assert.IsInstanceOfType(errors[1], typeof(ArgumentNullException));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Proxii_WithExceptionInterceptor_NonMatchingType_MultiCatch()
		{
			var errors = new List<Exception>();

			Action<Exception> onCatch = e => errors.Add(e);
			var proxy = Proxii.Proxy<IProxiiTester>()
				.With<ProxiiTester>()
				.Catch<IndexOutOfRangeException>(onCatch)
				.Catch<NullReferenceException>(onCatch)
				.Create();

			proxy.Throw(new ArgumentException());

			Assert.Fail("Should have thrown a non-matching exception");
		}
	}
}
