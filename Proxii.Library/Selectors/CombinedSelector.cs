using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Proxii.Selectors
{
    public class CombinedSelector : IInterceptorSelector
    {
        private IEnumerable<IInterceptorSelector> _selectors;

        public CombinedSelector(IEnumerable<IInterceptorSelector> selectors)
        {
            _selectors = selectors;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var filtered = interceptors;

            foreach (var selector in _selectors)
                filtered = selector.SelectInterceptors(type, method, filtered);

            return filtered;
        }
    }
}
