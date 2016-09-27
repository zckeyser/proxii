using System;
using System.Linq;
using Castle.DynamicProxy;
using Proxii.Library.Interceptors;
using Proxii.Library.Selectors;

namespace Proxii.Library.Other
{
    /// <summary>
    /// Creates a proxy of the given object
    /// which blocks all set_ methods,
    /// such as those used by properties
    /// </summary>
    public static class FrozenProxy
    {
        private const string PropertySetPattern = "^set_";
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

        public static TInterface Freeze<TInterface, TImplementation>(TImplementation obj, params string[] alternatePatterns) 
            where TImplementation : TInterface where TInterface : class
        {
            if(!typeof(TInterface).IsInterface)
                throw new ArgumentException("TInterface must be an interface to be proxied");

            // by default only intercept property setters
            var selector = new MethodNamePatternSelector(PropertySetPattern);

            // if we were given more patterns to block for a freeze stop those too
            if (alternatePatterns.Any())
            {
                foreach(var pattern in alternatePatterns)
                    selector.AddPattern(pattern);
            }

            // anything we're intercepting we're just blocking,
            // so no need to worry about "this" leaks
            var interceptors = new IInterceptor[] { new StopMethodInterceptor() };

            var proxy = _generator.CreateInterfaceProxyWithTarget<TInterface>(obj, new ProxyGenerationOptions {Selector = selector},
                interceptors);

            return proxy;
        }
    }
}
