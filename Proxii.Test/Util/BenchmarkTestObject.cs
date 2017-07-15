using System;
using System.Diagnostics;
using System.Threading;

namespace Proxii.Test.Util
{
    public class BenchmarkTestObject : IBenchmarkTestObject
    {
        // ensure it's within 1 ms of the correct timing
        public const double TimingEpsilon = 1;

        public void Do()
        {
            Thread.Sleep(300);
        }

        public void Do(int i, string s)
        {
            Thread.Sleep(300);
        }

        public static double TakeDoTiming(IBenchmarkTestObject obj)
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
                timing = stopwatch.Elapsed.TotalMilliseconds;
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
        public static bool EpsilonEqual(double epsilon, double a, double b)
        {
            return Math.Abs(a - b) <= epsilon;
        }
    }

    public interface IBenchmarkTestObject
    {
        void Do();

        void Do(int i, string s);
    }
}
