using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Internal.Interceptors;

namespace Proxii.Test.Integration.Interceptors
{
	[TestClass]
	public class ExceptionInterceptorTest
	{
		private readonly ProxyGenerator _generator = new ProxyGenerator();

		[TestMethod]
		public void Integration_ExceptionInterceptor_NoException_VoidTargetMethod_SingleMethod()
		{
			var exceptionList = new List<Exception>();

			var interceptors = new IInterceptor[] { new ExceptionInterceptor<Exception>(e => exceptionList.Add(e)) };

			var proxy = (IExceptionTest) _generator.CreateInterfaceProxyWithTarget(typeof (IExceptionTest), new ExceptionTest(), interceptors);

			proxy.NoOp();

			Assert.AreEqual(0, exceptionList.Count);
		}

		[TestMethod]
		public void Integration_ExceptionInterceptor_NoException_ReturningTargetMethod_SingleMethod()
		{
			var exceptionList = new List<Exception>();

			var interceptors = new IInterceptor[] { new ExceptionInterceptor<Exception>(e => exceptionList.Add(e)) };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			var result = proxy.ReturnFoo();

			Assert.AreEqual(0, exceptionList.Count);
			Assert.AreEqual("foo", result);
		}

		[TestMethod]
		public void Integration_ExceptionInterceptor_NoException_MultipleMethods()
		{
			var exceptionList = new List<Exception>();

			var interceptors = new IInterceptor[] { new ExceptionInterceptor<Exception>(e => exceptionList.Add(e)) };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			proxy.NoOp();
			var result = proxy.ReturnFoo();

			Assert.AreEqual(0, exceptionList.Count);
			Assert.AreEqual("foo", result);
		}

		[TestMethod]
		public void Integration_ExceptionInterceptor_SingleException_VoidMethod()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor<Exception>(e => exceptionList.Add(e));

			var interceptors = new IInterceptor[] { exceptionInterceptor };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			proxy.ThrowArgumentException();

			Assert.AreEqual(1, exceptionList.Count);
			Assert.IsInstanceOfType(exceptionList[0], typeof(ArgumentException));
		}

		[TestMethod]
		public void Integration_ExceptionInterceptor_SingleException_ValueMethod()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor<InvalidOperationException>(e => exceptionList.Add(e));

			var interceptors = new IInterceptor[] { exceptionInterceptor };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			var result = proxy.ThrowInvalidOperationException();

			Assert.AreEqual(1, exceptionList.Count);
			Assert.IsInstanceOfType(exceptionList[0], typeof(InvalidOperationException));
			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void Integration_ExceptionInterceptor_SingleException_ReferenceMethod()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor<NullReferenceException>(e => exceptionList.Add(e));
			var interceptors = new IInterceptor[] { exceptionInterceptor };
			
			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			var result = proxy.ThrowNullReferenceException();

			Assert.AreEqual(1, exceptionList.Count);
			Assert.IsInstanceOfType(exceptionList[0], typeof(NullReferenceException));
			Assert.IsNull(result);
		}
        
		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void Integration_ExceptionInterceptor_NonMatchingThrowType()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor<IndexOutOfRangeException>(e => exceptionList.Add(e));
			var interceptors = new IInterceptor[] { exceptionInterceptor };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			proxy.ThrowNullReferenceException();

			Assert.Fail("Exception should have been re-thrown.");
		}

		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void Integration_ExceptionInterceptor_RethrowEnabled()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor<NullReferenceException>(e => exceptionList.Add(e))
			{
				Rethrow = true
			};

			var interceptors = new IInterceptor[] { exceptionInterceptor };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			proxy.ThrowNullReferenceException();

			Assert.Fail("Exception should have been re-thrown.");
		}

		public interface IExceptionTest
		{
			void ThrowArgumentException();
			void ThrowIndexOutOfRangeException();
			object ThrowNullReferenceException();
			string ThrowArgumentNullException();
			int ThrowInvalidOperationException();
			bool ThrowFormatException();
			void NoOp();
			string ReturnFoo();
		}

		public class ExceptionTest : IExceptionTest
		{
			public void ThrowArgumentException()
			{
				throw new ArgumentException();
			}

			public void ThrowIndexOutOfRangeException()
			{
				throw new IndexOutOfRangeException();
			}

			public object ThrowNullReferenceException()
			{
				throw new NullReferenceException();
			}

			public string ThrowArgumentNullException()
			{
				throw new ArgumentNullException("arg");
			}

			public int ThrowInvalidOperationException()
			{
				throw new InvalidOperationException();
			}

			public bool ThrowFormatException()
			{
				throw new FormatException();
			}

			public void NoOp()
			{
				
			}

			public string ReturnFoo()
			{
				return "foo";
			}
		}
	}
}
