using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Proxii.Library.Interceptors;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.Integration.Interceptors
{
    [TestClass]
    public class OnReturnInterceptorTest
    {
		private readonly ProxyGenerator _generator = new ProxyGenerator();

        [TestMethod]
        public void Integration_OnReturnInterceptor_ReturnValueOnly_Matching()
        {
	        var logger = new Logger();
	        Action<string> onReturn = (s) => logger.Log(s);

			var interceptors = new IInterceptor[] { new OnReturnInterceptor<string>(onReturn) };

			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), interceptors);

			proxy.StringMethod();
			const string expected = ProxiiTester.StringRetVal;
	        var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual(expected, history[0]);
        }

		[TestMethod]
		public void Integration_OnReturnInterceptor_ReturnValueOnly_NonMatching()
		{
			var logger = new Logger();
			Action<string> onReturn = (s) => logger.Log(s);

			var interceptors = new IInterceptor[] { new OnReturnInterceptor<string>(onReturn) };

			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), interceptors);

			proxy.IntMethod();
			var history = logger.GetHistory();

			Assert.AreEqual(0, history.Count);
		}

		[TestMethod]
		public void Integration_OnReturnInterceptor_ReturnValueMethodInfo_Matching()
		{
			var logger = new Logger();
			Action<string, MethodInfo> onReturn = (s, method) => logger.Log(s + " returned from " + method.Name);

			var interceptors = new IInterceptor[] { new OnReturnInterceptor<string>(onReturn) };

			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), interceptors);

			proxy.StringMethod();
			const string expected = ProxiiTester.StringRetVal + " returned from StringMethod";
			var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
			Assert.AreEqual(expected, history[0]);
		}

		[TestMethod]
		public void Integration_OnReturnInterceptor_ReturnValueMethodInfo_NonMatching()
		{
			var logger = new Logger();
			Action<string, MethodInfo> onReturn = (s, method) => logger.Log(s + " returned from " + method.Name);

			var interceptors = new IInterceptor[] { new OnReturnInterceptor<string>(onReturn) };

			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), interceptors);

			proxy.IntMethod();
			var history = logger.GetHistory();

			Assert.AreEqual(0, history.Count);
		}

		[TestMethod]
		public void Integration_OnReturnInterceptor_ReturnValueArgs_Matching()
		{
			var logger = new Logger();
			Action<string, object[]> onReturn = (s, args) => logger.Log(s + " returned when called with " + args.Select(arg => arg.ToString()).Aggregate("", (total, next) => total + " " + next).Trim());

			var interceptors = new IInterceptor[] { new OnReturnInterceptor<string>(onReturn) };

			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), interceptors);

			proxy.Concat("foo", "bar", "buzz");
			const string expected = "foobarbuzz returned when called with foo bar buzz";
			var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual(expected, history[0]);
		}

		[TestMethod]
		public void Integration_OnReturnInterceptor_ReturnValueArgs_NonMatching()
		{
			var logger = new Logger();
			Action<string, object[]> onReturn = (s, args) => logger.Log(s + " returned when called with " + args.Select(arg => arg.ToString()).Aggregate("", (total, next) => total + " " + next).Trim());

			var interceptors = new IInterceptor[] { new OnReturnInterceptor<string>(onReturn) };

			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), interceptors);

			proxy.IntMethod();
			var history = logger.GetHistory();

			Assert.AreEqual(0, history.Count);
		}

	    [TestMethod]
	    public void Integration_OnReturnInterceptor_ReturnValueMethodInfoArgs_Matching()
	    {
			var logger = new Logger();
			Action<string, MethodInfo, object[]> onReturn = (s, method, args) => logger.Log(s + " returned when " + method.Name + " was called with " + args.Select(arg => arg.ToString()).Aggregate("", (total, next) => total + " " + next).Trim());

			var interceptors = new IInterceptor[] { new OnReturnInterceptor<string>(onReturn) };

			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), interceptors);

			proxy.Concat("foo", "bar", "buzz");
			const string expected = "foobarbuzz returned when Concat was called with foo bar buzz";
			var history = logger.GetHistory();

            Assert.AreEqual(1, history.Count);
            Assert.AreEqual(expected, history[0]);
	    }

		[TestMethod]
		public void Integration_OnReturnInterceptor_ReturnValueMethodInfoArgs_NonMatching()
		{
			var logger = new Logger();
			Action<string, MethodInfo, object[]> onReturn = (s, method, args) => logger.Log(s + " returned when " + method.Name + " was called with " + args.Select(arg => arg.ToString()).Aggregate("", (total, next) => total + " " + next).Trim());

			var interceptors = new IInterceptor[] { new OnReturnInterceptor<string>(onReturn) };

			var proxy = (IProxiiTester)_generator.CreateInterfaceProxyWithTarget(typeof(IProxiiTester), new ProxiiTester(), interceptors);

			proxy.IntMethod();
			var history = logger.GetHistory();

			Assert.AreEqual(0, history.Count);
		}
    }
}
