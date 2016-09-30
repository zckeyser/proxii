using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util.TestClasses;

namespace Proxii.Test.e2e
{
    [TestClass]
    public class ProxiiRejectNullArgumentsTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Proxii_RejectNullArguments_ThrowsOnNullArgument()
        {
            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                            .RejectNullArguments()
                            .Create();

            proxy.Concat(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Proxii_RejectNullArguments_GetsNullArgName_FirstArg()
        {
            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                            .RejectNullArguments()
                            .Create();

            try
            {
                proxy.Concat(null, "b", "c");
            }
            catch (ArgumentNullException e)
            {
                // need to extract the parameter name from the message
                var paramName = e.Message.Substring(e.Message.IndexOf(":") + 2);

                Assert.AreEqual("a", paramName);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Proxii_RejectNullArguments_GetsNullArgName_OtherArg()
        {
            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                            .RejectNullArguments()
                            .Create();

            try
            {
                proxy.Concat("a", null, "c");
            }
            catch (ArgumentNullException e)
            {
                // need to extract the parameter name from the message
                var paramName = e.Message.Substring(e.Message.IndexOf(":") + 2);

                Assert.AreEqual("b", paramName);
                throw;
            }
        }

        [TestMethod]
        public void Proxii_RejectNullArguments_DoesNothingWhenNoNullArguments()
        {
            var proxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                            .RejectNullArguments()
                            .Create();

            proxy.Concat("a", "b", "c");
        }
    }
}
