using System;
using System.Reflection;
using Castle.DynamicProxy;
using System.Collections.Generic;

namespace Proxii.Library.Selectors
{
    public class MethodNameSelector : IInterceptorSelector
    {
        private readonly IEnumerable<string> _methodNames;

        public MethodNameSelector(params string[] methodNames)
        {
            _methodNames = methodNames;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            foreach(var methodName in _methodNames)
            {
                // intercept the method if it has a public method with the correct name + args
                if (type.GetMethod(methodName, method.GetGenericArguments()) != null)
                    return interceptors;
            }

            // don't intercept the method
            return new IInterceptor[0];
        }
    }
}
