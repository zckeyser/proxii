using Castle.DynamicProxy;
using Proxii.Selectors;
using System;
using System.Collections.Generic;

namespace Proxii
{
    // TODO: Figure out how to lazily instantiate selectors so they are aware of interceptors created later in the call chain
    public class Proxii
    {
        private ProxyGenerator _generator = new ProxyGenerator();

        /// <summary>
        /// interface to proxy
        /// </summary>
        private Type _toProxy;

        /// <summary>
        /// implementation to target
        /// </summary>
        private Type _target;

        /// <summary>
        /// interceptors to be used when proxying
        /// </summary>
        private List<IInterceptor> _interceptors = new List<IInterceptor>();

        /// <summary>
        /// list of selectors to combine to choose what methods to intercept
        /// </summary>
        private List<IInterceptorSelector> _selectors = new List<IInterceptorSelector>();

        /// <summary>
        /// combined selector created from the current state of _selectors
        /// </summary>
        private IInterceptorSelector Selector {
            get
            {
                return new CombinedSelector(_selectors);
            }
        }

        public static Proxii Proxy(Type interfaceType)
        {
            if (!interfaceType.IsInterface)
                throw new ArgumentException("Proxii.Proxy() must be called with an interface type");

            return new Proxii(interfaceType);
        }

        private Proxii(Type interfaceType)
        {
            _toProxy = interfaceType;
        }

        public Proxii With(Type implementationType)
        {
            _target = implementationType;
            return this;
        }

        /// <summary>
        /// perform a custom action when the given type of interception is caught
        /// </summary>
        public Proxii Catch(Type exception, Action<Exception> onCatch)
        {
            if (!exception.IsSubclassOf(typeof(Exception)))
                throw new ArgumentException("type passed to Catch() must be a subclass of Exception");

            if (onCatch == null)
                throw new ArgumentNullException("onCatch");

            var exceptionInterceptor = _interceptors.Find(interceptor => interceptor is ExceptionInterceptor) as ExceptionInterceptor;

            if (exceptionInterceptor != null)
                exceptionInterceptor.AddCatch(exception, onCatch);
            else
                _interceptors.Add(new ExceptionInterceptor(exception, onCatch));

            return this;
        }

        /// <summary>
        /// only intercept methods with the given name
        /// </summary>
        public Proxii ByMethodName(params string[] methodNames)
        {
            _selectors.Add(new MethodNameSelector(methodNames));

            return this;
        }

        public object Create()
        {
            var impl = Activator.CreateInstance(_target);
            var options = new ProxyGenerationOptions { Selector = Selector };

            return _generator.CreateInterfaceProxyWithTarget(_toProxy, impl, options, _interceptors.ToArray());
        }
    }
}
