using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using Proxii.Library.Interceptors;
using Proxii.Library.Selectors;

namespace Proxii
{
	public class Proxii
	{
		public static Proxii<T> Proxy<T>()
		{
			var interfaceType = typeof(T);

			if (!interfaceType.IsInterface)
				throw new ArgumentException("Proxii.Proxy<T>() must be called with an interface type");

			return new Proxii<T>();
		}
	}

    public class Proxii<T>
    {
        private readonly ProxyGenerator _generator = new ProxyGenerator();
		
        /// <summary>
        /// implementation to target
        /// </summary>
        private object _target;

        /// <summary>
        /// interceptors to be used when proxying
        /// </summary>
        private readonly List<IInterceptor> _interceptors = new List<IInterceptor>();

        /// <summary>
        /// list of selectors to combine to choose what methods to intercept
        /// </summary>
        private readonly List<IInterceptorSelector> _selectors = new List<IInterceptorSelector>();

	    /// <summary>
	    /// combined selector created from the current state of _selectors
	    /// </summary>
	    private IInterceptorSelector Selector
	    {
		    get { return new CombinedSelector(_selectors); }
	    }

	    internal Proxii() { }

		/// <summary>
		/// assigns a target type that implements the interface
		/// that's being proxied to the Proxii
		/// </summary>
        public Proxii<T> With<U>()
        {
	        var implementationType = typeof (U);

			// make sure the type implements our interface
			if(!typeof(T).IsAssignableFrom(implementationType))
				throw new ArgumentException("Proxii.With<T>() must be called with a type that implements the interface of that proxy");

            _target = Activator.CreateInstance(implementationType);

            return this;
        }

		/// <summary>
		/// assigns a target object that implements the interface
		/// that's being proxied to the Proxii
		/// </summary>
	    public Proxii<T> With(object target)
	    {
			var implementationType = target.GetType();

			// make sure the type implements our interface
			if (!typeof(T).IsAssignableFrom(implementationType))
				throw new ArgumentException("Proxii.With<T>() must be called with a type that implements the interface of that proxy");

			_target = target;

			return this;
	    }

        /// <summary>
        /// perform a custom action when the given type of interception is caught
        /// </summary>
        public Proxii<T> Catch(Type exception, Action<Exception> onCatch)
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
        public Proxii<T> ByMethodName(params string[] methodNames)
        {
            _selectors.Add(new MethodNameSelector(methodNames));

            return this;
        }

        public T Create()
        {
            var options = new ProxyGenerationOptions { Selector = Selector };

            return (T) _generator.CreateInterfaceProxyWithTarget(typeof(T), _target, options, _interceptors.ToArray());
        }
    }
}
