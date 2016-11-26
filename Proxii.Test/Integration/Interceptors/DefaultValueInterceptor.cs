using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;

namespace Proxii.Test.Integration.Interceptors
{
    [TestClass]
    public class DefaultValueInterceptorTest
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();

        [TestMethod]
        public void DefaultValueInterceptor_SetsValue_Factory()
        {
            var interceptor = new DefaultValueInterceptor();
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
            var interceptor = new DefaultValueInterceptor();
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
            var interceptor = new DefaultValueInterceptor();
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
            var interceptor = new DefaultValueInterceptor();
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
            var interceptor = new DefaultValueInterceptor();
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
            var interceptor = new DefaultValueInterceptor();
            interceptor.AddDefaultForArgument("s", () => "foo");
            interceptor.AddDefaultForArgument("s", () => "bar");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);

            // should just return the injected default
            var result = proxy.DoStuff(s => s);

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void DefaultValueInterceptor_ThrowsOnIncompatibleReturnType_Factory()
        {
            var interceptor = new DefaultValueInterceptor();
            interceptor.AddDefaultForArgument("s", () => 1);
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);

            // should throw because the types don't match
            proxy.DoStuff(s => s);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void DefaultValueInterceptor_ThrowsOnIncompatibleReturnType_DefaultValue()
        {
            var interceptor = new DefaultValueInterceptor();
            interceptor.AddDefaultForArgument("s", 1);
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);

            // should throw because the types don't match
            proxy.DoStuff(s => s);
        }

        [TestMethod]
        public void DefaultValueInterceptor_DoesNotChangeNonNulls_Factory()
        {
            var interceptor = new DefaultValueInterceptor();
            interceptor.AddDefaultForArgument("s", () => "foo");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);
            
            var result = proxy.DoStuff(s => s, "bar");

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_DoesNotChangeNonNulls_DefaultValue()
        {
            var interceptor = new DefaultValueInterceptor();
            interceptor.AddDefaultForArgument("s", "foo");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);
            
            var result = proxy.DoStuff(s => s, "bar");

            Assert.AreEqual("bar", result);
        }

        [TestMethod]
        public void DefaultValueInterceptor_AffectsDifferentMethods_Factory()
        {
            var interceptor = new DefaultValueInterceptor();
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
            var interceptor = new DefaultValueInterceptor();
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
            var interceptor = new DefaultValueInterceptor();
            interceptor.AddDefaultForArgument("s", "foo");
            interceptor.AddDefaultForArgument("ss", () => "bar");
            var interceptors = new IInterceptor[] { interceptor };

            var proxy = (IDefaultValueTester)_generator.CreateInterfaceProxyWithTarget(typeof(IDefaultValueTester), new DefaultValueTester(), interceptors);

            var result1 = proxy.DoStuff(s => s);
            var result2 = proxy.DoOtherStuff(ss => ss);

            Assert.AreEqual("foo", result1);
            Assert.AreEqual("bar", result2);
        }

        public interface IDefaultValueTester
        {
            string DoStuff(Func<string, string> func, string s = null);
            string DoOtherStuff(Func<string, string> func, string ss = null);
        }

        public class DefaultValueTester : IDefaultValueTester
        {
            public string DoStuff(Func<string, string> func, string s = null)
            {
                return func(s);
            }

            public string DoOtherStuff(Func<string, string> func, string ss = null)
            {
                return func(ss);
            }
        }
    }
}
