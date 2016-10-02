using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util;

namespace Proxii.Test.e2e
{
	[TestClass]
	public class ProxiiCatchTest
	{
		[TestMethod]
		public void Proxii_Catch_MatchingType_SingleCatch()
		{
			var errors = new List<Exception>();

			Action<Exception> onCatch = e => errors.Add(e);
			var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
				.Catch<ArgumentException>(onCatch)
				.Create();

			proxy.ThrowAction(new ArgumentException());

			Assert.AreEqual(1, errors.Count);
			Assert.IsInstanceOfType(errors[0], typeof(ArgumentException));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Proxii_Catch_NonMatchingType_SingleCatch()
		{
			var errors = new List<Exception>();

			Action<Exception> onCatch = e => errors.Add(e);
            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                .Catch<ArgumentOutOfRangeException>(onCatch)
                .Create();

            proxy.ThrowAction(new ArgumentException());

			Assert.Fail("Should have thrown a non-matching exception");
		}

		[TestMethod]
		public void Proxii_Catch_MatchingType_MultiCatch()
		{
			var errors = new List<Exception>();

			Action<Exception> onCatch = e => errors.Add(e);
			var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
				.Catch<ArgumentException>(onCatch)
				.Catch<ArgumentNullException>(onCatch)
				.Create();

			proxy.ThrowAction(new ArgumentException());
			proxy.ThrowAction(new ArgumentNullException());

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
			var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
				.Catch<IndexOutOfRangeException>(onCatch)
				.Catch<NullReferenceException>(onCatch)
				.Create();

			proxy.ThrowAction(new ArgumentException());

			Assert.Fail("Should have thrown a non-matching exception");
		}
	}
}
