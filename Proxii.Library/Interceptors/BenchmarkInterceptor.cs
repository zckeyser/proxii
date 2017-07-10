using System;
using System.Diagnostics;
using System.Reflection;
using Castle.DynamicProxy;

namespace Proxii.Library.Interceptors
{
    public class BenchmarkInterceptor : IInterceptor
    {
        private readonly Action<double, MethodInfo> _action;

        public BenchmarkInterceptor(Action<double> action)
        {
            _action = Wrap(action);
        }

        public BenchmarkInterceptor(Action<double, MethodInfo> action)
        {
            _action = action;
        }

        public void Intercept(IInvocation invocation)
        {
            var t = Stopwatch.StartNew();

            try
            {
                t.Start();

                invocation.Proceed();
            }
            finally
            {
                t.Stop();

                var timing = t.ElapsedMilliseconds;

                _action(timing, invocation.Method);
            }
        }

        private Action<double, MethodInfo> Wrap(Action<double> action)
        {
            return (d, m) => action(d);
        }
    }
}
