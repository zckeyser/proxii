using System;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using Proxii.Test.Util;

namespace Proxii.Test.Integration.Interceptors
{
    [TestClass]
    public class DefaultValueInterceptorTest
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();

        [TestMethod]
        public void DefaultValueInterceptor_SetsValue_Factory()
        {
            var interceptor = new DefaultValueInterceptor<string>();
            interceptor.AddDefaultForArgument("s", () => "foo");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);

            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("foo", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_SetsValue_DefaultValue()
        {
            var interceptor = new DefaultValueInterceptor<string>();
            interceptor.AddDefaultForArgument("s", "foo");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);

            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("foo", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_OverridesDefault_ValueThenFactory()
        {
            var interceptor = new DefaultValueInterceptor<string>();
            interceptor.AddDefaultForArgument("s", "foo");
            interceptor.AddDefaultForArgument("s", () => "bar");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);

            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_OverridesDefault_FactoryThenValue()
        {
            var interceptor = new DefaultValueInterceptor<string>();
            interceptor.AddDefaultForArgument("s", () => "foo");
            interceptor.AddDefaultForArgument("s", "bar");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);

            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_OverridesDefault_ValueThenValue()
        {
            var interceptor = new DefaultValueInterceptor<string>();
            interceptor.AddDefaultForArgument("s", "foo");
            interceptor.AddDefaultForArgument("s", "bar");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);

            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_OverridesDefault_FactoryThenFactory()
        {
            var interceptor = new DefaultValueInterceptor<string>();
            interceptor.AddDefaultForArgument("s", () => "foo");
            interceptor.AddDefaultForArgument("s", () => "bar");
            var interceptors = new IInterceptor[] {interceptor};

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);

            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_DoesNotChangeNonNulls_Factory()
        {
            var interceptor = new DefaultValueInterceptor<string>();
            interceptor.AddDefaultForArgument("s", () => "foo");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);
            
            var result = proxy.DoStuff(s => s, "bar");

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_DoesNotChangeNonNulls_DefaultValue()
        {
            var interceptor = new DefaultValueInterceptor<string>();
            interceptor.AddDefaultForArgument("s", "foo");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);
            
            var result = proxy.DoStuff(s => s, "bar");

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_AffectsDifferentMethods_Factory()
        {
            var interceptor = new DefaultValueInterceptor<string>();
            interceptor.AddDefaultForArgument("s", () => "foo");
            interceptor.AddDefaultForArgument("ss", () => "bar");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);
            
            var result1 = proxy.DoStuff(s => s);
            var result2 = proxy.DoOtherStuff(ss => ss);

            Assert.AreEqual("foo", result1);
            Assert.AreEqual("bar", result2);
        }

        [TestMethod]
        public void DefaultValueInterceptor_AffectsDifferentMethods_DefaultValue()
        {
            var interceptor = new DefaultValueInterceptor<string>();
            interceptor.AddDefaultForArgument("s", "foo");
            interceptor.AddDefaultForArgument("ss", "bar");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);

            var result1 = proxy.DoStuff(s => s);
            var result2 = proxy.DoOtherStuff(ss => ss);

            Assert.AreEqual("foo", result1);
            Assert.AreEqual("bar", result2);
        }

        [TestMethod]
        public void DefaultValueInterceptor_AffectsDifferentMethods_Mixed()
        {
            var interceptor = new DefaultValueInterceptor<string>();
            interceptor.AddDefaultForArgument("s", "foo");
            interceptor.AddDefaultForArgument("ss", () => "bar");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);

            var result1 = proxy.DoStuff(s => s);
            var result2 = proxy.DoOtherStuff(ss => ss);

            Assert.AreEqual("foo", result1);
            Assert.AreEqual("bar", result2);
        }
    }
}
