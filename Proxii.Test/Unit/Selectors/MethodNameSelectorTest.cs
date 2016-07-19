using System.Linq;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using Proxii.Library.Selectors;

namespace Proxii.Test.Unit.Selectors
{
	[TestClass]
	public class MethodNameSelectorTest
	{
		[TestMethod]
		public void Unit_MethodNameSelector_NoFilter()
		{
			var type = typeof (string);
			var method = type.GetMethods().First();
			var interceptors = new IInterceptor[] { new ExceptionInterceptor() };
			var methodNames = new string[] { };
			var selector = new MethodNameSelector(methodNames);

			var result = selector.SelectInterceptors(type, method, interceptors);

			Assert.AreEqual(0, result.Length);
		}

		[TestMethod]
		public void Unit_MethodNameSelector_OneFilter_HasMatch()
		{
			var type = typeof(string);
			var method = type.GetMethods().First();
			var interceptors = new IInterceptor[] { new ExceptionInterceptor() };
			var methodNames = new [] { method.Name };
			var selector = new MethodNameSelector(methodNames);

			var result = selector.SelectInterceptors(type, method, interceptors);

			Assert.AreEqual(1, result.Length);
			Assert.IsInstanceOfType(result[0], typeof(ExceptionInterceptor));
		}

		[TestMethod]
		public void Unit_MethodNameSelector_OneFilter_NoMatches()
		{
			var type = typeof(string);
			var method = type.GetMethods().First();
			var interceptors = new IInterceptor[] { new ExceptionInterceptor() };
			var methodNames = new [] { "foo" };
			var selector = new MethodNameSelector(methodNames);

			var result = selector.SelectInterceptors(type, method, interceptors);

			Assert.AreEqual(0, result.Length);
		}

		[TestMethod]
		public void Unit_MethodNameSelector_MultipleFilters_HasMatch()
		{
			var type = typeof(string);
			var method = type.GetMethods().First();
			var interceptors = new IInterceptor[] { new ExceptionInterceptor() };
			var methodNames = new [] { "foo", method.Name, "bar" };
			var selector = new MethodNameSelector(methodNames);

			var result = selector.SelectInterceptors(type, method, interceptors);

			Assert.AreEqual(1, result.Length);
			Assert.IsInstanceOfType(result[0], typeof(ExceptionInterceptor));
		}

		[TestMethod]
		public void Unit_MethodNameSelector_MultipleFilters_NoMatches()
		{
			var type = typeof(string);
			var method = type.GetMethods().First();
			var interceptors = new IInterceptor[] { new ExceptionInterceptor() };
			var methodNames = new[] { "foo", "bar", "foobar" };
			var selector = new MethodNameSelector(methodNames);

			var result = selector.SelectInterceptors(type, method, interceptors);

			Assert.AreEqual(0, result.Length);
		}

		[TestMethod]
		public void Unit_MethodNameSelector_MultipleInterceptors_HasMatch()
		{
			var type = typeof(string);
			var method = type.GetMethods().First();
			var interceptors = new IInterceptor[] { new ExceptionInterceptor(), new ExceptionInterceptor() };
			var methodNames = new[] { method.Name };
			var selector = new MethodNameSelector(methodNames);

			var result = selector.SelectInterceptors(type, method, interceptors);

			Assert.AreEqual(2, result.Length);
			Assert.IsInstanceOfType(result[0], typeof(ExceptionInterceptor));
			Assert.IsInstanceOfType(result[1], typeof(ExceptionInterceptor));
		}

		[TestMethod]
		public void Unit_MethodNameSelector_MultipleInterceptors_NoMatches()
		{
			var type = typeof(string);
			var method = type.GetMethods().First();
			var interceptors = new IInterceptor[] { new ExceptionInterceptor(), new ExceptionInterceptor() };
			var methodNames = new[] { "foo" };
			var selector = new MethodNameSelector(methodNames);

			var result = selector.SelectInterceptors(type, method, interceptors);

			Assert.AreEqual(0, result.Length);
		}
	}
}
