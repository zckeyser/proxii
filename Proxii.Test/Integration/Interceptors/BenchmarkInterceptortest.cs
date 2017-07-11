using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;

namespace Proxii.Test.Integration.Interceptors
{
    [TestClass]
    public class BenchmarkInterceptorTest
    {
        // ensure it's within 1 microsecond of the correct timing
        private const double TimingEpsilon = .001;

        private readonly ProxyGenerator _generator = new ProxyGenerator();

        [TestMethod]
        public void BenchmarkInterceptor_CallsWithAccurateTiming()
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
            normalTiming = TakeDoTiming(normalObject);

            // ensure the interceptor at least called the action
            Assert.AreNotEqual(0, proxyTiming, "proxy timing is set");

            // ensure the interceptor gave an accurate timing
            Assert.IsTrue(EpsilonEqual(TimingEpsilon, normalTiming, proxyTiming));
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

        /// <summary>
        /// compares two doubles for equality, allowing for a maximum difference of epsilon
        /// </summary>
        /// <param name="epsilon">maximum difference allowed for the numbers to be considered equal (inclusive)</param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>true if the numbers fall within epsilon range of one another</returns>
        private bool EpsilonEqual(double epsilon, double a, double b)
        {
            return Math.Abs(a - b) <= epsilon;
        }

        private double TakeDoTiming(IBenchmarkTestObject obj, int times = 1)
        {
            double timing;

            var stopwatch = Stopwatch.StartNew();

            // time the proxy
            try
            {
                obj.Do();
            }
            finally
            {
                stopwatch.Stop();
                timing = stopwatch.ElapsedMilliseconds;
            }

            return timing;
        }

        public class BenchmarkTestObject : IBenchmarkTestObject
        {
            public void Do()
            {
                Thread.Sleep(300);
            }

            public void Do(int i, string s)
            {
                Thread.Sleep(300);
            }
        }

        public interface IBenchmarkTestObject
        {
            void Do();

            void Do(int i, string s);
        }
    }
}
