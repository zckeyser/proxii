using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.e2e
{
	[TestClass]
	public class ProxiiInitializationTest
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
        public void Proxii_WiresInterfaceToImpl_ByType_ConvenienceMethod()
        {
            var proxy = Proxii.Proxy<IProxiiTester>(new ProxiiTester()).Create();

            var test = new[] { "foo" };

            proxy.DoAction(() => test[0] = "bar");

            Assert.AreEqual("bar", test[0]);
        }

        [TestMethod]
        public void Proxii_WiresInterfaceToImpl_ByObject_ConvenienceMethod()
        {
            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>().Create();

            var test = new[] { "foo" };

            proxy.DoAction(() => test[0] = "bar");

            Assert.AreEqual("bar", test[0]);
        }
    }
}
