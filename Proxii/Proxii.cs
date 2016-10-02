﻿using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using Proxii.Library.Interceptors;
using Proxii.Library.Selectors;
using System.Reflection;
using Proxii.Library.Other;

namespace Proxii
{
	public static class Proxii
	{
        /// <summary>
        /// Proxy TInterface to an instance of TImplementation
        ///
        /// TImplementation must have a default constructor to use this method.
        /// If a non-default constructor is desired, pass in an implementation object
        /// as an argument instead of an implementation type as a typeparam
        /// </summary>
        public static IProxii<TInterface> Proxy<TInterface, TImplementation>() where TImplementation : TInterface where TInterface : class
        {
            var interfaceType = typeof(TInterface);

            if (!interfaceType.IsInterface)
                throw new ArgumentException("Proxii.Proxy<TInterface, TImplementation>() must be called with an interface type for TInterface");

            return new Proxii<TInterface>().With<TImplementation>();
        }

        /// <summary>
        /// Proxy interface T to given object
        /// </summary>
        public static IProxii<T> Proxy<T>(T impl) where T : class
        {
            var interfaceType = typeof(T);

            if (!interfaceType.IsInterface)
                throw new ArgumentException("Proxii.Proxy<T>(T impl) must be called with an interface type for T");

            return new Proxii<T>().With(impl);
        }

        /// <summary>
        /// Freezes an object, blocking all calls to its
        /// property setters, as well as any other methods
        /// specified in the string params
        /// </summary>
        /// <param name="obj"> object to freeze</param>
        /// <param name="alternatePatterns">alternate patterns to freeze methods other than Property setters</param>
        /// <returns></returns>
	    public static TInterface Freeze<TInterface>(TInterface obj, params string[] alternatePatterns)
            where TInterface : class
        {
            return FrozenProxy.Freeze(obj, alternatePatterns);
        }
 	}

    public sealed class Proxii<T> : IProxii<T>
        where T : class
    {
        #region Private Fields
        private static readonly ProxyGenerator _generator = new ProxyGenerator();

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
	    private IInterceptorSelector Selector => new CombinedSelector(_selectors);

        /// <summary>
        /// holds additional Proxii layers created with Group()
        /// </summary>
        private readonly List<Proxii<T>> _additionalProxiis = new List<Proxii<T>>();
        #endregion

        #region Constructors
        internal Proxii() { }
        #endregion

        #region Grouping
        /// <summary>
        /// group together Proxii interceptors/selectors such that the given
        /// selectors only apply to the given interceptors and vice versa
        /// </summary>
        /// <param name="configFunction">Func to configure the Proxii sub-group</param>
        public IProxii<T> Group(Func<IProxii<T>, IProxii<T>> configFunction)
        {
            // make a new proxy with the current target
            IProxii<T> proxy = new Proxii<T>().With(_target);

            // configure the proxy as requested
            proxy = configFunction(proxy);

            // add it to the list of proxies to combine in Create
            _additionalProxiis.Add((Proxii<T>) proxy);

            return this;
        }
        #endregion

        #region Initialization
        /// <summary>
        /// assigns a target type that implements the interface
        /// that's being proxied to the Proxii
        /// </summary>
        internal Proxii<T> With<U>() where U : T
        {
	        var implementationType = typeof (U);

            try
            {
                _target = Activator.CreateInstance(implementationType);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Could not instantiate type {implementationType.Name} (Does it have a default constructor?)", e);
            }

            return this;
        }

		/// <summary>
		/// assigns a target object that implements the interface
		/// that's being proxied to the Proxii
		/// </summary>
        internal Proxii<T> With(object target)
	    {
			var implementationType = target.GetType();

			// make sure the type implements our interface
			if (!typeof(T).IsAssignableFrom(implementationType))
				throw new ArgumentException("Proxii.With<T>() must be called with an object that implements the interface of that proxy");

			_target = target;

			return this;
	    }
        #endregion

        #region Selectors
        /// <summary>
        /// only intercept methods with the given argument signature
        /// </summary>
        /// <param name="argumentTypes">Argument types of method signatures to select (order-sensitive)</param>
        public IProxii<T> ByArgumentType(params Type[] argumentTypes)
        {
            var selector = _selectors.Find(sel => sel is ArgumentTypeSelector) as ArgumentTypeSelector;

            if (selector == null)
            {
                selector = new ArgumentTypeSelector();
                selector.AddArgumentDefinition(argumentTypes);
                _selectors.Add(selector);
            }
            else
                selector.AddArgumentDefinition(argumentTypes);

            return this;
        }

        /// <summary>
        /// only intercept methods with the given name
        /// </summary>
        public IProxii<T> ByMethodName(params string[] methodNames)
        {
            var nameSelector = _selectors.Find(selector => selector is MethodNameSelector) as MethodNameSelector;

            if(nameSelector != null)
                nameSelector.AddNames(methodNames);
            else
                _selectors.Add(new MethodNameSelector(methodNames));

            return this;
        }

        /// <summary>
        /// only interceept methods which match one of the given regex patterns
        /// </summary>
        public IProxii<T> ByMethodNamePattern(params string[] patterns)
        {
            var selector = _selectors.Find(s => s is MethodNamePatternSelector) as MethodNamePatternSelector;

            if (selector != null)
                selector.AddPatterns(patterns);
            else
                _selectors.Add(new MethodNamePatternSelector(patterns));

            return this;
        }

        /// <summary>
        /// only intercept methods that return one of the given types
        /// void methods can be specified by passing typeof(void) as an argument
        /// </summary>
        public IProxii<T> ByReturnType(params Type[] types)
        {
            _selectors.Add(new ReturnTypeSelector(types));

            return this;
        }
        #endregion

        #region Interceptors
        /// <summary>
        /// Executes the given action before an intercepted function is invoked
        /// </summary>
        public IProxii<T> BeforeInvoke(Action beforeHook)
        {
            // infoless hook
            _interceptors.Add(new BeforeInvokeInterceptor(beforeHook));

            return this;
        }

        /// <summary>
        /// Executes the given action before an intercepted function is invoked,
        /// using an action that is given the MethodInfo of the intercepted function
        /// </summary>
        public IProxii<T> BeforeInvoke(Action<MethodInfo> beforeHook)
        {
            // hook with method info
            _interceptors.Add(new BeforeInvokeInterceptor(beforeHook));

            return this;
        }

        /// <summary>
        /// Executes the given action before an intercepted function is invoked,
        /// using an action that is given the MethodInfo of the intercepted function
        /// and the arguments it is being called with
        /// </summary>
        public IProxii<T> BeforeInvoke(Action<MethodInfo, object[]> beforeHook)
        {
            // hook with method info and args
            _interceptors.Add(new BeforeInvokeInterceptor(beforeHook));

            return this;
        }

        /// <summary>
        /// Executes the given action after an intercepted function is invoked
        /// </summary>
        public IProxii<T> AfterInvoke(Action afterHook)
        {
            // infoless hook
            _interceptors.Add(new AfterInvokeInterceptor(afterHook));

            return this;
        }

        /// <summary>
        /// Executes the given action after an intercepted function is invoked,
        /// using an action that is given the MethodInfo of the intercepted function
        /// </summary>
        public IProxii<T> AfterInvoke(Action<MethodInfo> afterHook)
        {
            // hook with method info
            _interceptors.Add(new AfterInvokeInterceptor(afterHook));

            return this;
        }

        /// <summary>
        /// Executes the given action before an intercepted function is invoked,
        /// using an action that is given the MethodInfo of the intercepted function
        /// and the arguments it is being called with
        /// </summary>
        public IProxii<T> AfterInvoke(Action<MethodInfo, object[]> afterHook)
        {
            // hook with method info and args
            _interceptors.Add(new AfterInvokeInterceptor(afterHook));

            return this;
        }

        /// <summary>
        /// Execute the given action on the return value of intercepted functions.
        ///
        /// Only intercepts functions with a matching return value.
        /// </summary>
        public IProxii<T> OnReturn<U>(Action<U> onReturn)
        {
            // return value handler
            _interceptors.Add(new OnReturnInterceptor<U>(onReturn));

            return this;
        }

        /// <summary>
        /// Execute the given action on the return value of intercepted functions,
        /// as well as the method info of the function being intercepted
        ///
        /// Only intercepts functions with a matching return value.
        /// </summary>
        public IProxii<T> OnReturn<U>(Action<U, MethodInfo> onReturn)
        {
            // return value and method info handler
            _interceptors.Add(new OnReturnInterceptor<U>(onReturn));

            return this;
        }

        /// <summary>
        /// Execute the given action on the return value of intercepted functions,
        /// as well as the arguments that were passed into the function that was intercepted
        ///
        /// Only intercepts functions with a matching return value.
        /// </summary>
        public IProxii<T> OnReturn<U>(Action<U, object[]> onReturn)
        {
            // return value and arguments handler
            _interceptors.Add(new OnReturnInterceptor<U>(onReturn));

            return this;
        }

        /// <summary>
        /// Execute the given action on the return value of intercepted functions,
        /// as well as the method info of the function being intercepted
        /// and the arguments that were passed into it
        ///
        /// Only intercepts functions with a matching return value.
        /// </summary>
        public IProxii<T> OnReturn<U>(Action<U, MethodInfo, object[]> onReturn)
        {
            // return value, method info, and arguments handler
            _interceptors.Add(new OnReturnInterceptor<U>(onReturn));

            return this;
        }

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        public IProxii<T> ChangeArguments<U>(Func<U, U> modifier)
        {
            _interceptors.Add(new ArgumentInterceptor<U>(modifier));

            return this;
        }

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        public IProxii<T> ChangeArguments<U1, U2>(Func<U1, U2, Tuple<U1, U2>> modifier)
        {
            _interceptors.Add(new ArgumentInterceptor<U1, U2>(modifier));

            return this;
        }

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        public IProxii<T> ChangeArguments<U1, U2, U3>(Func<U1, U2, U3, Tuple<U1, U2, U3>> modifier)
        {
            _interceptors.Add(new ArgumentInterceptor<U1, U2, U3>(modifier));

            return this;
        }

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        public IProxii<T> ChangeArguments<U1, U2, U3, U4>(Func<U1, U2, U3, U4, Tuple<U1, U2, U3, U4>> modifier)
        {
            _interceptors.Add(new ArgumentInterceptor<U1, U2, U3, U4>(modifier));

            return this;
        }

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        public IProxii<T> ChangeArguments<U1, U2, U3, U4, U5>(Func<U1, U2, U3, U4, U5, Tuple<U1, U2, U3, U4, U5>> modifier)
        {
            _interceptors.Add(new ArgumentInterceptor<U1, U2, U3, U4, U5>(modifier));

            return this;
        }

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        public IProxii<T> ChangeArguments<U1, U2, U3, U4, U5, U6>(Func<U1, U2, U3, U4, U5, U6, Tuple<U1, U2, U3, U4, U5, U6>> modifier)
        {
            _interceptors.Add(new ArgumentInterceptor<U1, U2, U3, U4, U5, U6>(modifier));

            return this;
        }

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        public IProxii<T> ChangeArguments<U1, U2, U3, U4, U5, U6, U7>(Func<U1, U2, U3, U4, U5, U6, U7, Tuple<U1, U2, U3, U4, U5, U6, U7>> modifier)
        {
            _interceptors.Add(new ArgumentInterceptor<U1, U2, U3, U4, U5, U6, U7>(modifier));

            return this;
        }

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        public IProxii<T> ChangeReturnValue<TReturn>(Func<TReturn, TReturn> onReturn)
	    {
			_interceptors.Add(new ChangeReturnValueInterceptor<TReturn>(onReturn));

		    return this;
	    }

		/// <summary>
		/// Perform a custom action when the given type of interception is caught.
		/// </summary>
		public IProxii<T> Catch<TException>(Action<Exception> onCatch) 
            where TException : Exception
		{
            if (onCatch == null)
                throw new ArgumentNullException(nameof(onCatch));

            _interceptors.Add(new ExceptionInterceptor<TException>(onCatch));

            return this;
        }

        /// <summary>
        /// prevent the execution of all methods caught by this interceptor
        /// 
        /// causes methods that return things to return their defaults (null for reference types)
        /// </summary>
        public IProxii<T> Stop()
        {
            _interceptors.Add(new StopMethodInterceptor());

            return this;
        }

        /// <summary>
        /// throws a NullReferenceException whenever an intercepted method gets a null argument
        /// </summary>
        /// <returns></returns>
        public IProxii<T> RejectNullArguments()
        {
            _interceptors.Add(new NullInterceptor());

            return this;
        }
        #endregion

        #region Finalization
        /// <summary>
        /// Create the actual proxy object
        ///
        /// This should be called at the end of every chain of Proxii calls
        /// </summary>
        public T Create()
        {
            var options = new ProxyGenerationOptions { Selector = Selector };

            // add interceptor to prevent "this" leaks
            _interceptors.Add(new ThisInterceptor());

            // create the chain of Proxiis for grouping, 
            // with the items in this Proxii being added last
            // so that they're applied universally
            var proxy = (T) _target;

            foreach (var proxii in _additionalProxiis)
            {
                // if this is the first proxy in the chain, attach it to our actual object
                // otherwise attach it to the most recently created proxy
                proxii._target = proxy;

                proxy = proxii.Create();
            }

            return (T) _generator.CreateInterfaceProxyWithTarget(typeof(T), proxy, options, _interceptors.ToArray());
        }
        #endregion
    }
}
