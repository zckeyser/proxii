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
        // TODO add fixes for the "this" bug, maybe a default interceptor that's always there? 
        // https://github.com/castleproject/Core/blob/master/docs/dynamicproxy-leaking-this.md
        
        #region Private Fields
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
        #endregion

        #region Constructors
        internal Proxii() { }
        #endregion

        #region Initialization
        /// <summary>
        /// assigns a target type that implements the interface
        /// that's being proxied to the Proxii
        /// </summary>
        public Proxii<T> With<U>() where U : T
        {
	        var implementationType = typeof (U);
            
            try
            {
                _target = Activator.CreateInstance(implementationType);
            } 
            catch (Exception e)
            {
                throw new ArgumentException("Could not instantiate type " + implementationType.Name + " (Does it have a default constructor?)", e);
            }

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
        #endregion

        #region Selectors
        // TODO ByArgumentType?
        // TODO Figure out more useful selectors

        /// <summary>
        /// only intercept methods with the given name
        /// </summary>
        public Proxii<T> ByMethodName(params string[] methodNames)
        {
            _selectors.Add(new MethodNameSelector(methodNames));

            return this;
        }

        /// <summary>
        /// only intercept methods that return one of the given types
        /// void methods can be specified by passing typeof(void) as an argument
        /// </summary>
        public Proxii<T> ByReturnType(params Type[] types)
        {
            _selectors.Add(new ReturnTypeSelector(types));

            return this;
        }
        #endregion

        #region Interceptors
        // TODO ChangeArgument Interceptor
        // TODO ExecuteIf Interceptor

        public Proxii<T> BeforeInvoke(Action beforeHook)
        {
            _interceptors.Add(new BeforeInvokeInterceptor(beforeHook));

            return this;
        }

        public Proxii<T> AfterInvoke(Action afterHook)
        {
            _interceptors.Add(new AfterInvokeInterceptor(afterHook));

            return this;
        }

		public Proxii<T> ChangeReturnValue<TReturn>(Func<TReturn, TReturn> onReturn)
	    {
			_interceptors.Add(new ReturnValueInterceptor<TReturn>(onReturn));

		    return this;
	    }

		/// <summary>
		/// perform a custom action when the given type of interception is caught
		/// </summary>
		public Proxii<T> Catch<TException>(Action<Exception> onCatch) where TException : Exception
		{
			var exception = typeof (TException);

            if (onCatch == null)
                throw new ArgumentNullException("onCatch");

            var exceptionInterceptor = _interceptors.Find(interceptor => interceptor is ExceptionInterceptor) as ExceptionInterceptor;

            if (exceptionInterceptor != null)
                exceptionInterceptor.AddCatch(exception, onCatch);
            else
                _interceptors.Add(new ExceptionInterceptor(exception, onCatch));

            return this;
        }
        #endregion

        #region Finalization
        public T Create()
        {
            var options = new ProxyGenerationOptions { Selector = Selector };

            _interceptors.Add(new ThisInterceptor());

            return (T) _generator.CreateInterfaceProxyWithTarget(typeof(T), _target, options, _interceptors.ToArray());
        }
        #endregion
    }
}
