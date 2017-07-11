using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Proxii.Library.Interceptors;
using Proxii.Test.Integration.Interceptors;

namespace Proxii.Test.e2e
{
    [TestClass]
    public class ProxiiBenchmarkTest
    {
        // TODO encapsulate benchmark test behavior in its own class

        // ensure it's within 1 microsecond of the correct timing
        private const double TimingEpsilon = .005;

        [TestMethod]
        public void Proxii_Benchmark_RecordsAccurateTiming()
        {
            // so we can independently take the two timings
            double normalTiming = 0, proxyTiming = 0;

            Action<double> proxyTimingAction = (d) => proxyTiming = d;

            // create the object normally
            var normalObject = new BenchmarkTestObject();

            // create a proxy
            var proxy = Proxii.Proxy<IBenchmarkTestObject, BenchmarkTestObject>()
                              .Benchmark(proxyTimingAction)
                              .Create();

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
            var proxy = Proxii.Proxy<IBenchmarkTestObject, BenchmarkTestObject>()
                              .Benchmark(proxyTimingAction)
                              .Create();

            proxy.Do();

            Assert.AreEqual("Do", result);
        }

        [TestMethod]
        public void BenchmarkInterceptor_CallsWithCorrectMethodInfoAndArguments()
        {
            string result = null;

            Action<double, MethodInfo, object[]> proxyTimingAction = (d, m, args) => result = $"called method {m.Name} with arguments ({string.Join(", ", args)})";

            // create a proxy
            var proxy = Proxii.Proxy<IBenchmarkTestObject, BenchmarkTestObject>()
                              .Benchmark(proxyTimingAction)
                              .Create();

            proxy.Do(1, "foo");

            Assert.AreEqual("called method Do with arguments (1, foo)", result);
        }

        private double TakeDoTiming(IBenchmarkTestObject obj)
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
    }
}
