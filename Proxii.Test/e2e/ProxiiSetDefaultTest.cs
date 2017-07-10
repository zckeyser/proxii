using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Test.Util;

namespace Proxii.Test.e2e
{
    [TestClass]
    public class ProxiiSetDefaultTest
    {
        [TestMethod]
        public void DefaultValueInterceptor_SetsValue_Factory()
        {
            var proxy = Proxii.Proxy<IDefaultValueTester, DefaultValueTester>()
                .SetDefault("s", () => "foo")
                .Create();
            
            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("foo", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_SetsValue_DefaultValue()
        {
            var proxy = Proxii.Proxy<IDefaultValueTester, DefaultValueTester>()
                .SetDefault("s", "foo")
                .Create();

            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("foo", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_OverridesDefault_ValueThenFactory()
        {
            var proxy = Proxii.Proxy<IDefaultValueTester, DefaultValueTester>()
                .SetDefault("s", "foo")
                .SetDefault("s", () => "bar")
                .Create();

            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_OverridesDefault_FactoryThenValue()
        {
            var proxy = Proxii.Proxy<IDefaultValueTester, DefaultValueTester>()
                .SetDefault("s", () => "foo")
                .SetDefault("s", "bar")
                .Create();
            
            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_OverridesDefault_ValueThenValue()
        {
            var proxy = Proxii.Proxy<IDefaultValueTester, DefaultValueTester>()
                .SetDefault("s", "foo")
                .SetDefault("s", "bar")
                .Create();
            
            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_OverridesDefault_FactoryThenFactory()
        {
            var proxy = Proxii.Proxy<IDefaultValueTester, DefaultValueTester>()
                .SetDefault("s", () => "foo")
                .SetDefault("s", () => "bar")
                .Create(); ;
            
            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_DoesNotChangeNonNulls_Factory()
        {
            var proxy = Proxii.Proxy<IDefaultValueTester, DefaultValueTester>()
                .SetDefault("s", () => "foo")
                .Create();
            
            var result = proxy.DoStuff(s => s, "bar");

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_DoesNotChangeNonNulls_DefaultValue()
        {
            var proxy = Proxii.Proxy<IDefaultValueTester, DefaultValueTester>()
                .SetDefault("s", "foo")
                .Create();
            
            var result = proxy.DoStuff(s => s, "bar");

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_AffectsDifferentMethods_Factory()
        {
            var proxy = Proxii.Proxy<IDefaultValueTester, DefaultValueTester>()
                .SetDefault("s", () => "foo")
                .SetDefault("ss", () => "bar")
                .Create();
            
            var result1 = proxy.DoStuff(s => s);
            var result2 = proxy.DoOtherStuff(ss => ss);

            Assert.AreEqual("foo", result1);
            Assert.AreEqual("bar", result2);
        }

        [TestMethod]
        public void DefaultValueInterceptor_AffectsDifferentMethods_DefaultValue()
        {
            var proxy = Proxii.Proxy<IDefaultValueTester, DefaultValueTester>()
                .SetDefault("s", "foo")
                .SetDefault("ss", "bar")
                .Create();
            
            var result1 = proxy.DoStuff(s => s);
            var result2 = proxy.DoOtherStuff(ss => ss);

            Assert.AreEqual("foo", result1);
            Assert.AreEqual("bar", result2);
        }

        [TestMethod]
        public void DefaultValueInterceptor_AffectsDifferentMethods_Mixed()
        {
            var proxy = Proxii.Proxy<IDefaultValueTester, DefaultValueTester>()
                .SetDefault("s", "foo")
                .SetDefault("ss", () => "bar")
                .Create();

            var result1 = proxy.DoStuff(s => s);
            var result2 = proxy.DoOtherStuff(ss => ss);

            Assert.AreEqual("foo", result1);
            Assert.AreEqual("bar", result2);
        }
    }
}
