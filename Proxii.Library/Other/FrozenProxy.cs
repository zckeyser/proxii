using System;
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

        public static TInterface Freeze<TInterface>(TInterface obj, params string[] alternatePatterns) 
            where TInterface : class
        {
            if(!typeof(TInterface).IsInterface)
                throw new ArgumentException("TInterface must be an interface to be proxied");

            // intercept setter patterns plus any other patterns we were given
            var selector = new MethodNamePatternSelector(PropertySetPattern);

            selector.AddPatterns(alternatePatterns);

            // anything we're intercepting we're just blocking,
            // so no need to worry about "this" leaks
            var interceptors = new IInterceptor[] { new StopMethodInterceptor() };

            var proxy = _generator.CreateInterfaceProxyWithTarget(obj, new ProxyGenerationOptions {Selector = selector},
                interceptors);

            return proxy;
        }
    }
}
