using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Proxii.Library.Selectors
{
    public class CombinedSelector : IInterceptorSelector
    {
        private readonly IEnumerable<IInterceptorSelector> _selectors;

        public CombinedSelector(IEnumerable<IInterceptorSelector> selectors)
        {
            _selectors = selectors;
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            return _selectors.Aggregate(interceptors, (current, selector) => selector.SelectInterceptors(type, method, current));
        }
    }
}
