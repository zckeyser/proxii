using System;
using System.Diagnostics;
using System.Reflection;
using Castle.DynamicProxy;

namespace Proxii.Library.Interceptors
{
    public class BenchmarkInterceptor : IInterceptor
    {
        private readonly Action<double, MethodInfo, object[]> _action;

        public BenchmarkInterceptor(Action<double> action)
        {
            _action = Wrap(action);
        }

        public BenchmarkInterceptor(Action<double, MethodInfo> action)
        {
            _action = Wrap(action);
        }

        public BenchmarkInterceptor(Action<double, MethodInfo, object[]> action)
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

                _action(timing, invocation.Method, invocation.Arguments);
            }
        }

        private Action<double, MethodInfo, object[]> Wrap(Action<double> action)
        {
            return (d, m, args) => action(d);
        }

        private Action<double, MethodInfo, object[]> Wrap(Action<double, MethodInfo> action)
        {
            return (d, m, args) => action(d, m);
        }
    }
}
