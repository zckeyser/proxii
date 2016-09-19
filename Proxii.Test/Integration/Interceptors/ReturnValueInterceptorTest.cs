﻿using System;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.Integration.Interceptors
{
	[TestClass]
	public class ReturnValueInterceptorTest
	{
		private readonly ProxyGenerator _generator = new ProxyGenerator();

		[TestMethod]
		public void Integration_ReturnValueInterceptor_NonMatchingType()
		{
			Func<int, int> changeInt = i => i * 10;
			var interceptors = new IInterceptor[] { new ReturnValueInterceptor<int>(changeInt) };

			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), interceptors);

			var result = proxy.StringMethod();
			const string expected = ProxiiTester.StringRetVal;

			Assert.AreEqual(expected, result);
		}

		[TestMethod]
		public void Integration_ReturnValueInterceptor_MatchingType()
		{
			Func<int, int> changeInt = i => i * 10;
			var interceptors = new IInterceptor[] { new ReturnValueInterceptor<int>(changeInt) };

			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), interceptors);

			var result = proxy.IntMethod();
			const int expected = ProxiiTester.IntRetVal * 10;

			Assert.AreEqual(expected, result);
		}
	}
}