using System;
using System.Reflection;
using Castle.DynamicProxy;
using System.Collections.Generic;
using System.Linq;

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
	        if (_methodNames.Any(name => name == method.Name) && type.GetMethod(method.Name) != null)
	        {
				// intercept the method if the targeted type has 
				// a public method with the correct name
				// and its name is contained in _methodNames
				return interceptors;
	        }

            // don't intercept the method
            return new IInterceptor[0];
        }
    }
}
