using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;

namespace Proxii.Library.Interceptors
{
    public class MaxCallsInterceptor : IInterceptor
    {
        private readonly Dictionary<MethodInfo, int> _callCounts = new Dictionary<MethodInfo, int>();
        private readonly int _maxCalls;

        /// <summary>
        /// limit each method on the proxy which is intercepted to only be called up to maxCalls times
        /// 
        /// if a method with a return value is blocked, its default is returned (null for reference types)
        /// </summary>
        /// <param name="maxCalls"></param>
        public MaxCallsInterceptor(int maxCalls)
        {
            if (maxCalls < 1)
                throw new ArgumentException($"{nameof(maxCalls)} should be greater than or equal to 1");

            _maxCalls = maxCalls;
        }

        public void Intercept(IInvocation invocation)
        {
            if (_callCounts.ContainsKey(invocation.Method))
            {
                // only proceed it we're under the max # of calls
                if (_callCounts[invocation.Method] >= _maxCalls)
                {
                    // we need to make sure the return value gets set if we aren't executing
                    var retval = invocation.Method.ReturnType.IsValueType
                        ? Activator.CreateInstance(invocation.Method.ReturnType)
                        : null;

                    invocation.ReturnValue = retval;
                }

                invocation.Proceed();
                _callCounts[invocation.Method]++;
            }
            else
            {
                _callCounts[invocation.Method] = 1;

                invocation.Proceed();
            }
        }
    }
}
