using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Internal.Other;
using Proxii.Test.Util;

namespace Proxii.Test.Integration.Other
{
    [TestClass]
    public class FrozenProxyTest
    {
        [TestMethod]
        public void Integration_FrozenProxy_Default()
        {
            IFreezeTester test = new FreezeTester();

            // set initial values
            test.MyInt = 10;
            test.MyString = "foo";
            test.SetMyBool(true);
            test.ChangeMyDouble(5.5);
            
            // freeze it
            test = FrozenProxy.Freeze(test);

            // try to change the values
            test.MyInt = 20;
            test.MyString = "bar";
            test.SetMyBool(false);
            test.ChangeMyDouble(11.0);

            // properties should be unchanged
            Assert.AreEqual(10, test.MyInt);
            Assert.AreEqual("foo", test.MyString);

            // we didn't stop the methods that affect internal state though
            Assert.AreEqual(false, test.GetMyBool());
            Assert.AreEqual(11.0, test.GetMyDouble());
        }

        [TestMethod]
        public void Integration_FrozenProxy_SingleAlternatePattern()
        {
            IFreezeTester test = new FreezeTester();

            // set initial values
            test.MyInt = 10;
            test.MyString = "foo";
            test.SetMyBool(true);
            test.ChangeMyDouble(5.5);

            // freeze it
            test = FrozenProxy.Freeze(test, "^Set.*");

            // try to change the values
            test.MyInt = 20;
            test.MyString = "bar";
            test.SetMyBool(false);
            test.ChangeMyDouble(11.0);

            // properties should be unchanged
            Assert.AreEqual(10, test.MyInt);
            Assert.AreEqual("foo", test.MyString);

            // we stopped one method
            Assert.AreEqual(true, test.GetMyBool());

            // but not the other
            Assert.AreEqual(11.0, test.GetMyDouble());
        }

        [TestMethod]
        public void Integration_FrozenProxy_MultipleAlternatePatterns()
        {
            IFreezeTester test = new FreezeTester();

            // set initial values
            test.MyInt = 10;
            test.MyString = "foo";
            test.SetMyBool(true);
            test.ChangeMyDouble(5.5);

            // freeze it
            test = FrozenProxy.Freeze(test, "^Set.*", "^Change.*");

            // try to change the values
            test.MyInt = 20;
            test.MyString = "bar";
            test.SetMyBool(false);
            test.ChangeMyDouble(11.0);

            // properties should be unchanged
            Assert.AreEqual(10, test.MyInt);
            Assert.AreEqual("foo", test.MyString);

            // we stopped both methods too
            Assert.AreEqual(true, test.GetMyBool());
            Assert.AreEqual(5.5, test.GetMyDouble());
        }
    }
}
