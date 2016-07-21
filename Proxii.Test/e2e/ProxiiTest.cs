using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.e2e
{
	[TestClass]
	public class ProxiiTest
	{
        // TODO split into multiple classes
        #region Initialization
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
        #endregion

        #region AfterInvoke
        [TestMethod]
        public void Proxii_AfterInvoke_InvokesAfterMethodCall()
        {
            var tester = new InvokeHookTester();

            // if this gets invoked before SetValue, this will not be the end value
            Action afterHook = () => tester.Value = 10;

            var proxy = Proxii.Proxy<IInvokeHookTester>()
                .With(tester)
                .AfterInvoke(afterHook)
                .Create();

            proxy.SetValue(15);

            Assert.AreEqual(10, tester.Value);
        }
        #endregion

        #region BeforeInvoke
        [TestMethod]
        public void Proxii_BeforeInvoke_InvokesAfterMethodCall()
        {
            var tester = new InvokeHookTester();

            // if this gets invoked before SetValue, this will not be the end value
            Action beforeHook = () => tester.Value = 10;

            var proxy = Proxii.Proxy<IInvokeHookTester>()
                .With(tester)
                .BeforeInvoke(beforeHook)
                .Create();

            proxy.SetValue(15);

            Assert.AreEqual(15, tester.Value);
        }
        #endregion

        #region Catch
        [TestMethod]
		public void Proxii_Catch_MatchingType_SingleCatch()
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
		public void Proxii_Catch_NonMatchingType_SingleCatch()
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
		public void Proxii_Catch_MatchingType_MultiCatch()
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
		public void Proxii_Catch_NonMatchingType_MultiCatch()
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
        #endregion

        #region ByMethodName
        [TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Proxii_ByMethodName_NoValidNames()
		{
			var errors = new List<Exception>();

			Action<Exception> onCatch = e => errors.Add(e);

			var proxy = Proxii.Proxy<IProxiiTester>()
				.With<ProxiiTester>()
				.Catch<ArgumentException>(onCatch)
				.ByMethodName()
				.Create();

			proxy.Throw(new ArgumentException());

			Assert.Fail("Should have thrown exception.");
		}

		[TestMethod]
		public void Proxii_ByMethodName_SingleValidName_Matching()
		{
			var errors = new List<Exception>();

			Action<Exception> onCatch = e => errors.Add(e);

			var proxy = Proxii.Proxy<IProxiiTester>()
				.With<ProxiiTester>()
				.Catch<ArgumentException>(onCatch)
				.ByMethodName("Throw")
				.Create();

			proxy.Throw(new ArgumentException());

			Assert.AreEqual(1, errors.Count);
			Assert.IsInstanceOfType(errors[0], typeof(ArgumentException));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Proxii_ByMethodName_SingleValidName_NonMatching()
		{
			var errors = new List<Exception>();

			Action<Exception> onCatch = e => errors.Add(e);

			var proxy = Proxii.Proxy<IProxiiTester>()
				.With<ProxiiTester>()
				.Catch<ArgumentException>(onCatch)
				.ByMethodName("DoActions")
				.Create();

			proxy.Throw(new ArgumentException());

			Assert.Fail("Should have thrown exception.");
		}

		[TestMethod]
		public void Proxii_ByMethodName_MultipleValidNames_Matching()
		{
			var errors = new List<Exception>();

			Action<Exception> onCatch = e => errors.Add(e);

			var proxy = Proxii.Proxy<IProxiiTester>()
				.With<ProxiiTester>()
				.Catch<ArgumentException>(onCatch)
				.ByMethodName("DoAction", "Throw")
				.Create();

			proxy.Throw(new ArgumentException());

			Assert.AreEqual(1, errors.Count);
			Assert.IsInstanceOfType(errors[0], typeof(ArgumentException));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Proxii_ByMethodName_MultipleValidNames_NonMatching()
		{
			var errors = new List<Exception>();

			Action<Exception> onCatch = e => errors.Add(e);

			var proxy = Proxii.Proxy<IProxiiTester>()
				.With<ProxiiTester>()
				.Catch<ArgumentException>(onCatch)
				.ByMethodName("DoAction", "DoFunc")
				.Create();

			proxy.Throw(new ArgumentException());

			Assert.Fail("Should have thrown exception.");
		}
        #endregion

        #region ByReturnType
        [TestMethod]
        public void Proxii_ByReturnType_SingleType_Matches()
        {
            var errors = new List<Exception>();
            var proxy = Proxii.Proxy<IProxiiTester>()
                .With<ProxiiTester>()
                .Catch<Exception>(e => errors.Add(e))
                .ByReturnType(typeof(void))
                .Create();

            proxy.Throw(new ArgumentException());

            Assert.AreEqual(errors.Count, 1);
            Assert.IsInstanceOfType(errors[0], typeof(ArgumentException));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Proxii_ByReturnType_SingleType_DoesNotMatch()
        {
            var errors = new List<Exception>();
            var proxy = Proxii.Proxy<IProxiiTester>()
                .With<ProxiiTester>()
                .Catch<Exception>(e => errors.Add(e))
                .ByReturnType(typeof(void))
                .Create();

            var result = proxy.ThrowWithReturn(new ArgumentException(), "foo");

            Assert.Fail("ThrowWithReturn should not have been intercepted.");
        }
        #endregion
    }
}
