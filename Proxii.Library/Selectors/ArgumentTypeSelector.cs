using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Proxii.Library.Selectors
{
    public class ArgumentTypeSelector : IInterceptorSelector
    {
        private readonly List<IList<Type>> _types = new List<IList<Type>>();

        public void AddArgumentDefinition(IList<Type> typeSet)
        {
            _types.Add(typeSet);
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            return ContainsTypeDefinition(method.GetParameters().Select(param => param.ParameterType)) ? interceptors : new IInterceptor[0];
        }

        public bool ContainsTypeDefinition(IEnumerable<Type> types)
        {
            return _types.Any(typeSet => Enumerable.SequenceEqual(typeSet, types));
        }
    }
}
