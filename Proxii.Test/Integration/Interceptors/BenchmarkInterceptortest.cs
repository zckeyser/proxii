﻿using System;
using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Internal.Interceptors;
using Proxii.Test.Util;

namespace Proxii.Test.Integration.Interceptors
{
    [TestClass]
    public class BenchmarkInterceptorTest
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();

        [TestMethod]
        public void BenchmarkInterceptor_RecordsAccurateTiming()
        {
            // so we can independently take the two timings
            double normalTiming = 0, proxyTiming = 0;
            
            Action<double> proxyTimingAction = (d) => proxyTiming = d;

            // create the object normally
            var normalObject = new BenchmarkTestObject();

            // create a proxy
            var interceptors = new IInterceptor[] { new BenchmarkInterceptor(proxyTimingAction) };
            var proxy = (IBenchmarkTestObject)_generator.CreateInterfaceProxyWithTarget(typeof(IBenchmarkTestObject), new BenchmarkTestObject(), interceptors);

            // take timing via interceptor
            proxy.Do();

            // take timing manually
            normalTiming = BenchmarkTestObject.TakeDoTiming(normalObject);

            // ensure the interceptor at least called the action
            Assert.AreNotEqual(0, proxyTiming, "proxy timing is set");

            // ensure the interceptor gave an accurate timing
            Assert.IsTrue(BenchmarkTestObject.EpsilonEqual(BenchmarkTestObject.TimingEpsilon, normalTiming, proxyTiming));
        }

        [TestMethod]
        public void BenchmarkInterceptor_CallsWithCorrectMethodInfo()
        {
            string result = null;

            Action<double, MethodInfo> proxyTimingAction = (d, m) => result = m.Name;

            // create a proxy
            var interceptors = new IInterceptor[] { new BenchmarkInterceptor(proxyTimingAction) };
            var proxy = (IBenchmarkTestObject)_generator.CreateInterfaceProxyWithTarget(typeof(IBenchmarkTestObject), new BenchmarkTestObject(), interceptors);

            proxy.Do();

            Assert.AreEqual("Do", result);
        }

        [TestMethod]
        public void BenchmarkInterceptor_CallsWithCorrectMethodInfoAndArguments()
        {
            string result = null;

            Action<double, MethodInfo, object[]> proxyTimingAction = (d, m, args) => result = $"called method {m.Name} with arguments ({string.Join(", ", args)})";

            // create a proxy
            var interceptors = new IInterceptor[] { new BenchmarkInterceptor(proxyTimingAction) };
            var proxy = (IBenchmarkTestObject)_generator.CreateInterfaceProxyWithTarget(typeof(IBenchmarkTestObject), new BenchmarkTestObject(), interceptors);

            proxy.Do(1, "foo");

            Assert.AreEqual("called method Do with arguments (1, foo)", result);
        }
    }
}
