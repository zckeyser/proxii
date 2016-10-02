using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxii.Test.Util;

namespace Proxii.Test.Integration.Interceptors
{
    [TestClass]
    public class ArgumentInterceptorTest
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();

        [TestMethod]
        public void Integration_ArgumentInterceptor_1Arg_MatchingSignature()
        {
            Func<int, int> modifier = (a) => a + 1;

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.OneArg1(1);

            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_1Arg_NonMatchingSignature()
        {
            Func<int, int> modifier = (a) => a + 1;

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.OneArg2("a");

            Assert.AreEqual("a", result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_2Args_MatchingSignature()
        {
            Func<int, int, Tuple<int, int>> modifier = (a, b) =>
            {
                return Tuple.Create
                (
                    a + 1,
                    b + 2
                );
            };

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int, int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.TwoArg1(1, 2);

            Assert.AreEqual(6, result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_2Args_NonMatchingSignature()
        {
            Func<int, int, Tuple<int, int>> modifier = (a, b) =>
            {
                return Tuple.Create
                (
                    a + 1,
                    b + 2
                );
            };

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int, int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.TwoArg2("a", "b");

            Assert.AreEqual("ab", result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_3Args_MatchingSignature()
        {
            Func<int, int, int, Tuple<int, int, int>> modifier = (a, b, c) =>
            {
                return Tuple.Create
                (
                    a + 1,
                    b + 2,
                    c + 3
                );
            };

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int, int, int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.ThreeArg1(1, 2, 3);

            Assert.AreEqual(12, result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_3Args_NonMatchingSignature()
        {
            Func<int, int, int, Tuple<int, int, int>> modifier = (a, b, c) =>
            {
                return Tuple.Create
                (
                    a + 1,
                    b + 2,
                    c + 3
                );
            };

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int, int, int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.ThreeArg2("a", "b", "c");

            Assert.AreEqual("abc", result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_4Args_MatchingSignature()
        {
            Func<int, int, int, int, Tuple<int, int, int, int>> modifier = (a, b, c, d) =>
            {
                return Tuple.Create
                (
                    a + 1,
                    b + 2,
                    c + 3,
                    d + 4
                );
            };

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int, int, int, int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.FourArg1(1, 2, 3, 4);

            Assert.AreEqual(20, result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_4Args_NonMatchingSignature()
        {
            Func<int, int, int, int, Tuple<int, int, int, int>> modifier = (a, b, c, d) =>
            {
                return Tuple.Create
                (
                    a + 1,
                    b + 2,
                    c + 3,
                    d + 4
                );
            };

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int, int, int, int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.FourArg2("a", "b", "c", "d");

            Assert.AreEqual("abcd", result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_5Args_MatchingSignature()
        {
            Func<int, int, int, int, int, Tuple<int, int, int, int, int>> modifier = (a, b, c, d, e) =>
            {
                return Tuple.Create
                (
                    a + 1,
                    b + 2,
                    c + 3,
                    d + 4,
                    e + 5
                );
            };

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int, int, int, int, int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.FiveArg1(1, 2, 3, 4, 5);

            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_5Args_NonMatchingSignature()
        {
            Func<int, int, int, int, int, Tuple<int, int, int, int, int>> modifier = (a, b, c, d, e) =>
            {
                return Tuple.Create
                (
                    a + 1,
                    b + 2,
                    c + 3,
                    d + 4,
                    e + 5
                );
            };

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int, int, int, int, int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.FiveArg2("a", "b", "c", "d", "e");

            Assert.AreEqual("abcde", result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_6Args_MatchingSignature()
        {
            Func<int, int, int, int, int, int, Tuple<int, int, int, int, int, int>> modifier = (a, b, c, d, e, f) =>
            {
                return Tuple.Create
                (
                    a + 1,
                    b + 2,
                    c + 3,
                    d + 4,
                    e + 5,
                    f + 6
                );
            };

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int, int, int, int, int, int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.SixArg1(1, 2, 3, 4, 5, 6);

            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_6Args_NonMatchingSignature()
        {
            Func<int, int, int, int, int, int, Tuple<int, int, int, int, int, int>> modifier = (a, b, c, d, e, f) =>
            {
                return Tuple.Create
                (
                    a + 1,
                    b + 2,
                    c + 3,
                    d + 4,
                    e + 5,
                    f + 6
                );
            };

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int, int, int, int, int, int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.SixArg2("a", "b", "c", "d", "e", "f");

            Assert.AreEqual("abcdef", result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_7Args_MatchingSignature()
        {
            Func<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>> modifier = (a, b, c, d, e, f, g) =>
            {
                return Tuple.Create
                (
                    a + 1,
                    b + 2,
                    c + 3,
                    d + 4,
                    e + 5,
                    f + 6,
                    g + 7
                );
            };

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int, int, int, int, int, int, int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.SevenArg1(1, 2, 3, 4, 5, 6, 7);

            Assert.AreEqual(56, result);
        }

        [TestMethod]
        public void Integration_ArgumentInterceptor_7Args_NonMatchingSignature()
        {
            Func<int, int, int, int, int, int, int, Tuple<int, int, int, int, int, int, int>> modifier = (a, b, c, d, e, f, g) =>
            {
                return Tuple.Create
                (
                    a + 1,
                    b + 2,
                    c + 3,
                    d + 4,
                    e + 5,
                    f + 6,
                    g + 7
                );
            };

            var interceptors = new IInterceptor[] { new ArgumentInterceptor<int, int, int, int, int, int, int>(modifier) };
            var proxy = (IArgumentTester)_generator.CreateInterfaceProxyWithTarget(typeof(IArgumentTester), new ArgumentTester(), interceptors);

            var result = proxy.SevenArg2("a", "b", "c", "d", "e", "f", "g");

            Assert.AreEqual("abcdefg", result);
        }
    }
}
