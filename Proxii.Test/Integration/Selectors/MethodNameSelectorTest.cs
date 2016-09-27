using System;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Selectors;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.Integration.Selectors
{
	[TestClass]
	public class MethodNameSelectorTest
	{
		private readonly ProxyGenerator _generator = new ProxyGenerator();

		[TestMethod]
		public void Integration_MethodNameSelector_NoFilter()
		{
			var logger = new Logger();
			var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };
			var options = new ProxyGenerationOptions { Selector = new MethodNameSelector() };
			var proxy = (IProxiiTester) _generator.CreateInterfaceProxyWithTarget(typeof (IProxiiTester), new ProxiiTester(), options, interceptors);

			proxy.DoAction(() => Console.Write("foo"));

			var history = logger.GetHistory();

			Assert.AreEqual(0, history.Count);
		}

		[TestMethod]
		public void Integration_MethodNameSelector_OneFilter_HasMatch()
		{
			var logger = new Logger();
			var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };
			var options = new ProxyGenerationOptions { Selector = new MethodNameSelector("DoAction") };
			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), options, interceptors);

			proxy.DoAction(() => Console.Write("foo"));

			var history = logger.GetHistory();

			Assert.AreEqual(1, history.Count);
			Assert.AreEqual("DoAction", history[0]);
		}

		[TestMethod]
		public void Integration_MethodNameSelector_OneFilter_NoMatches()
		{
			var logger = new Logger();
			var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };
			var options = new ProxyGenerationOptions { Selector = new MethodNameSelector("DoFunc") };
			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), options, interceptors);

			proxy.DoAction(() => Console.Write("foo"));

			var history = logger.GetHistory();

			Assert.AreEqual(0, history.Count);
		}

		[TestMethod]
		public void Integration_MethodNameSelector_MultipleFilters_HasMatch()
		{
			var logger = new Logger();
			var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };
			var options = new ProxyGenerationOptions { Selector = new MethodNameSelector("DoFunc", "DoAction") };
			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), options, interceptors);

			proxy.DoAction(() => Console.Write("foo"));

			var history = logger.GetHistory();

			Assert.AreEqual(1, history.Count);
			Assert.AreEqual("DoAction", history[0]);
		}

		[TestMethod]
		public void Integration_MethodNameSelector_MultipleFilters_NoMatches()
		{
			var logger = new Logger();
			var interceptors = new IInterceptor[] { new LoggingInterceptor(logger) };
			var options = new ProxyGenerationOptions { Selector = new MethodNameSelector("DoFunc", "ThrowAction") };
			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), options, interceptors);

			proxy.DoAction(() => Console.Write("foo"));

			var history = logger.GetHistory();

			Assert.AreEqual(0, history.Count);
		}

		[TestMethod]
		public void Integration_MethodNameSelector_MultipleInterceptors_HasMatch()
		{
			var logger = new Logger();
			var interceptors = new IInterceptor[] { new LoggingInterceptor(logger), new LoggingInterceptor(logger) };
			var options = new ProxyGenerationOptions { Selector = new MethodNameSelector("DoFunc", "DoAction") };
			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), options, interceptors);

			proxy.DoAction(() => Console.Write("foo"));

			var history = logger.GetHistory();

			// logging is doubled since we have 2 LoggingInterceptors with the same logger
			Assert.AreEqual(2, history.Count);
			Assert.AreEqual("DoAction", history[0]);
			Assert.AreEqual("DoAction", history[1]);
		}

		[TestMethod]
		public void Integration_MethodNameSelector_MultipleInterceptors_NoMatches()
		{
			var logger = new Logger();
			var interceptors = new IInterceptor[] { new LoggingInterceptor(logger), new LoggingInterceptor(logger) };
			var options = new ProxyGenerationOptions { Selector = new MethodNameSelector("DoFunc", "ThrowAction") };
			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), options, interceptors);

			proxy.DoAction(() => Console.Write("foo"));

			var history = logger.GetHistory();

			Assert.AreEqual(0, history.Count);
		}
	}
}
