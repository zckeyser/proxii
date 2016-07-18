using System;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;

namespace Proxii.Test.Integration.Interceptors
{
	[TestClass]
	public class ExceptionInterceptorTest
	{
		private readonly ProxyGenerator _generator = new ProxyGenerator();

		[TestMethod]
		public void ExceptionInterceptor_NoException_VoidTargetMethod_SingleMethod()
		{
			var exceptionList = new List<Exception>();

			var interceptors = new IInterceptor[] { new ExceptionInterceptor(typeof(Exception), e => exceptionList.Add(e)) };

			var proxy = (IExceptionTest) _generator.CreateInterfaceProxyWithTarget(typeof (IExceptionTest), new ExceptionTest(), interceptors);

			proxy.NoOp();

			Assert.AreEqual(0, exceptionList.Count);
		}

		[TestMethod]
		public void ExceptionInterceptor_NoException_ReturningTargetMethod_SingleMethod()
		{
			var exceptionList = new List<Exception>();

			var interceptors = new IInterceptor[] { new ExceptionInterceptor(typeof(Exception), e => exceptionList.Add(e)) };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			var result = proxy.ReturnFoo();

			Assert.AreEqual(0, exceptionList.Count);
			Assert.AreEqual("foo", result);
		}

		[TestMethod]
		public void ExceptionInterceptor_NoException_MultipleMethods()
		{
			var exceptionList = new List<Exception>();

			var interceptors = new IInterceptor[] { new ExceptionInterceptor(typeof(Exception), e => exceptionList.Add(e)) };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			proxy.NoOp();
			var result = proxy.ReturnFoo();

			Assert.AreEqual(0, exceptionList.Count);
			Assert.AreEqual("foo", result);
		}

		[TestMethod]
		public void ExceptionInterceptor_SingleException_VoidMethod()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor(typeof(Exception), e => exceptionList.Add(e));

			var interceptors = new IInterceptor[] { exceptionInterceptor };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			proxy.ThrowArgumentException();

			Assert.AreEqual(1, exceptionList.Count);
			Assert.IsInstanceOfType(exceptionList[0], typeof(ArgumentException));
		}

		[TestMethod]
		public void ExceptionInterceptor_MultipleExceptions_VoidMethod()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor(typeof(ArgumentException), e => exceptionList.Add(e));
			exceptionInterceptor.AddCatch(typeof(IndexOutOfRangeException), e => exceptionList.Add(e));

			var interceptors = new IInterceptor[] { exceptionInterceptor };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			proxy.ThrowArgumentException();
			proxy.ThrowIndexOutOfRangeException();

			Assert.AreEqual(2, exceptionList.Count);
			Assert.IsInstanceOfType(exceptionList[0], typeof(ArgumentException));
			Assert.IsInstanceOfType(exceptionList[1], typeof(IndexOutOfRangeException));
		}

		[TestMethod]
		public void ExceptionInterceptor_SingleException_ValueMethod()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor(typeof(InvalidOperationException), e => exceptionList.Add(e));

			var interceptors = new IInterceptor[] { exceptionInterceptor };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			var result = proxy.ThrowInvalidOperationException();

			Assert.AreEqual(1, exceptionList.Count);
			Assert.IsInstanceOfType(exceptionList[0], typeof(InvalidOperationException));
			Assert.AreEqual(0, result);
		}

		[TestMethod]
		public void ExceptionInterceptor_MultipleExceptions_ValueMethod()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor(typeof(InvalidOperationException), e => exceptionList.Add(e));
			exceptionInterceptor.AddCatch(typeof(FormatException), e => exceptionList.Add(e));

			var interceptors = new IInterceptor[] { exceptionInterceptor };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			var result1 = proxy.ThrowInvalidOperationException();
			var result2 = proxy.ThrowFormatException();

			Assert.AreEqual(2, exceptionList.Count);
			Assert.IsInstanceOfType(exceptionList[0], typeof(InvalidOperationException));
			Assert.IsInstanceOfType(exceptionList[1], typeof(FormatException));
			Assert.AreEqual(0, result1);
			Assert.AreEqual(false, result2);
		}

		[TestMethod]
		public void ExceptionInterceptor_SingleException_ReferenceMethod()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor(typeof(NullReferenceException), e => exceptionList.Add(e));
			var interceptors = new IInterceptor[] { exceptionInterceptor };
			
			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			var result = proxy.ThrowNullReferenceException();

			Assert.AreEqual(1, exceptionList.Count);
			Assert.IsInstanceOfType(exceptionList[0], typeof(NullReferenceException));
			Assert.IsNull(result);
		}

		[TestMethod]
		public void ExceptionInterceptor_MultipleExceptions_ReferenceMethod()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor(typeof(NullReferenceException), e => exceptionList.Add(e));
			exceptionInterceptor.AddCatch(typeof(ArgumentNullException), e => exceptionList.Add(e));

			var interceptors = new IInterceptor[] { exceptionInterceptor };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			var result1 = proxy.ThrowNullReferenceException();
			var result2 = proxy.ThrowArgumentNullException();

			Assert.AreEqual(2, exceptionList.Count);
			Assert.IsInstanceOfType(exceptionList[0], typeof(NullReferenceException));
			Assert.IsInstanceOfType(exceptionList[1], typeof(ArgumentNullException));
			Assert.IsNull(result1);
			Assert.IsNull(result2);
		}

		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void ExceptionInterceptor_NonMatchingThrowType()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor(typeof(IndexOutOfRangeException), e => exceptionList.Add(e));
			var interceptors = new IInterceptor[] { exceptionInterceptor };

			var proxy = (IExceptionTest)_generator.CreateInterfaceProxyWithTarget(typeof(IExceptionTest), new ExceptionTest(), interceptors);

			proxy.ThrowNullReferenceException();

			Assert.Fail("Exception should have been re-thrown.");
		}

		[TestMethod]
		[ExpectedException(typeof(NullReferenceException))]
		public void ExceptionInterceptor_RethrowEnabled()
		{
			var exceptionList = new List<Exception>();

			var exceptionInterceptor = new ExceptionInterceptor(typeof (NullReferenceException), e => exceptionList.Add(e))
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
