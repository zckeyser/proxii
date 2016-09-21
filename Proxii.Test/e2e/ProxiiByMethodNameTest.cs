using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.e2e
{
	[TestClass]
	public class ProxiiByMethodNameTest
	{
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
            proxy.DoAction(() =>
            {
                throw new ArgumentException();
            });

			Assert.AreEqual(2, errors.Count);
			Assert.IsInstanceOfType(errors[0], typeof(ArgumentException));
            Assert.IsInstanceOfType(errors[1], typeof(ArgumentException));
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

        [TestMethod]
        public void Proxii_ByMethodName_MultipleCalls_MatchesBothCalls()
        {
            var errors = new List<Exception>();

            Action<Exception> onCatch = e => errors.Add(e);

            var proxy = Proxii.Proxy<IProxiiTester>()
                .With<ProxiiTester>()
                .Catch<ArgumentException>(onCatch)
                .ByMethodName("DoAction")
                .ByMethodName("Throw")
                .Create();

            proxy.Throw(new ArgumentException());
            proxy.DoAction(() =>
            {
                throw new ArgumentException();
            });

            Assert.AreEqual(2, errors.Count);
            Assert.IsInstanceOfType(errors[0], typeof(ArgumentException));
            Assert.IsInstanceOfType(errors[1], typeof(ArgumentException));
        }
	}
}
