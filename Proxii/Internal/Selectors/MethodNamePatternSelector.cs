using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Castle.DynamicProxy;

namespace Proxii.Internal.Selectors
{
    public class MethodNamePatternSelector : IInterceptorSelector
    {
        private List<string> Patterns { get; }

        public MethodNamePatternSelector(params string[] patterns)
        {
            Patterns = new List<string>(patterns);
        }

        public void AddPatterns(params string[] patterns)
        {
            Patterns.AddRange(patterns);
        }

        public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            return Patterns.Any(pattern => Regex.IsMatch(method.Name, pattern)) ? interceptors : new IInterceptor[0];
        }
    }
}
