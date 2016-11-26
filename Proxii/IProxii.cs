using System;
using System.Reflection;

namespace Proxii
{
    public interface IProxii<T> where T : class
    {
        #region Grouping
        /// <summary>
        /// group together Proxii interceptors/selectors such that the given
        /// selectors only apply to the given interceptors and vice versa
        /// </summary>
        /// <param name="configFunction">Func to configure the Proxii sub-group</param>
        IProxii<T> Group(Func<IProxii<T>, IProxii<T>> configFunction);
        #endregion

        #region Selectors
        /// <summary>
        /// only intercept methods with the given argument signature
        /// </summary>
        /// <param name="argumentTypes">Argument types of method signatures to select (order-sensitive)</param>
        IProxii<T> ByArgumentType(params Type[] argumentTypes);

        /// <summary>
        /// only intercept methods with the given name
        /// </summary>
        IProxii<T> ByMethodName(params string[] methodNames);

        /// <summary>
        /// only interceept methods which match one of the given regex patterns
        /// </summary>
        IProxii<T> ByMethodNamePattern(params string[] patterns);

        /// <summary>
        /// only intercept methods that return one of the given types
        /// void methods can be specified by passing typeof(void) as an argument
        /// </summary>
        IProxii<T> ByReturnType(params Type[] types);
        #endregion

        #region Interceptors
        /// <summary>
        /// Executes the given action after an intercepted function is invoked
        /// </summary>
        IProxii<T> AfterInvoke(Action<MethodInfo> afterHook);

        /// <summary>
        /// Executes the given action after an intercepted function is invoked,
        /// using an action that is given the MethodInfo of the intercepted function
        /// </summary>
        IProxii<T> AfterInvoke(Action<MethodInfo, object[]> afterHook);

        /// <summary>
        /// Executes the given action before an intercepted function is invoked,
        /// using an action that is given the MethodInfo of the intercepted function
        /// and the arguments it is being called with
        /// </summary>
        IProxii<T> AfterInvoke(Action afterHook);

        /// <summary>
        /// Executes the given action before an intercepted function is invoked
        /// </summary>
        IProxii<T> BeforeInvoke(Action<MethodInfo> beforeHook);

        /// <summary>
        /// Executes the given action before an intercepted function is invoked,
        /// using an action that is given the MethodInfo of the intercepted function
        /// </summary>
        IProxii<T> BeforeInvoke(Action<MethodInfo, object[]> beforeHook);

        /// <summary>
        /// Executes the given action before an intercepted function is invoked,
        /// using an action that is given the MethodInfo of the intercepted function
        /// and the arguments it is being called with
        /// </summary>
        IProxii<T> BeforeInvoke(Action beforeHook);

        /// <summary>
		/// Perform a custom action when the given type of interception is caught.
		/// </summary>
        IProxii<T> Catch<TException>(Action<Exception> onCatch) where TException : Exception;

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        IProxii<T> ChangeArguments<U>(Func<U, U> modifier);

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        IProxii<T> ChangeArguments<U1, U2>(Func<U1, U2, Tuple<U1, U2>> modifier);

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        IProxii<T> ChangeArguments<U1, U2, U3>(Func<U1, U2, U3, Tuple<U1, U2, U3>> modifier);

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        IProxii<T> ChangeArguments<U1, U2, U3, U4>(Func<U1, U2, U3, U4, Tuple<U1, U2, U3, U4>> modifier);

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        IProxii<T> ChangeArguments<U1, U2, U3, U4, U5>(Func<U1, U2, U3, U4, U5, Tuple<U1, U2, U3, U4, U5>> modifier);

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        IProxii<T> ChangeArguments<U1, U2, U3, U4, U5, U6>(Func<U1, U2, U3, U4, U5, U6, Tuple<U1, U2, U3, U4, U5, U6>> modifier);

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        IProxii<T> ChangeArguments<U1, U2, U3, U4, U5, U6, U7>(Func<U1, U2, U3, U4, U5, U6, U7, Tuple<U1, U2, U3, U4, U5, U6, U7>> modifier);

        /// <summary>
        /// Modify the arguments going into intercepted functions using the given Func.
        ///
        /// Only intercepts functions which match the given function signature.
        /// </summary>
        IProxii<T> ChangeReturnValue<TReturn>(Func<TReturn, TReturn> onReturn);

        /// <summary>
        /// Only execute functions that are intercepted up 
        /// until the Nth time, where N is the parameter passed in.
        /// 
        /// Call counters are maintained per function. 
        /// Functions which are blocked return their default value (null for reference types)
        /// </summary>
        IProxii<T> MaxCalls(int max);

        /// <summary>
        /// Execute the given action on the return value of intercepted functions.
        ///
        /// Only intercepts functions with a matching return value.
        /// </summary>
        IProxii<T> OnReturn<U>(Action<U, object[]> onReturn);

        /// <summary>
        /// Execute the given action on the return value of intercepted functions,
        /// as well as the method info of the function being intercepted
        ///
        /// Only intercepts functions with a matching return value.
        /// </summary>
        IProxii<T> OnReturn<U>(Action<U> onReturn);

        /// <summary>
        /// Execute the given action on the return value of intercepted functions,
        /// as well as the arguments that were passed into the function that was intercepted
        ///
        /// Only intercepts functions with a matching return value.
        /// </summary>
        IProxii<T> OnReturn<U>(Action<U, MethodInfo> onReturn);

        /// <summary>
        /// Execute the given action on the return value of intercepted functions,
        /// as well as the method info of the function being intercepted
        /// and the arguments that were passed into it
        ///
        /// Only intercepts functions with a matching return value.
        /// </summary>
        IProxii<T> OnReturn<U>(Action<U, MethodInfo, object[]> onReturn);

        /// <summary>
        /// Throws a NullReferenceException whenever an intercepted method gets a null argument
        /// </summary>
        IProxii<T> RejectNullArguments();

        /// <summary>
        /// Prevent the execution of all methods caught by this interceptor
        /// 
        /// Functions which are blocked return their default value (null for reference types)
        /// </summary>
        IProxii<T> Stop();

        /// <summary>
        /// Replaces null arguments with the given name object returned by the given function
        /// </summary>
        /// <param name="argName">name of the argument to replace</param>
        /// <param name="factory">called to get the replacement object to use as a default</param>
        IProxii<T> SetDefault<U>(string argName, Func<U> factory);

        /// <summary>
        /// Replaces null arguments with the given name with an object
        /// </summary>
        /// <param name="argName">name of the argument to replace</param>
        /// <param name="defaultValue">the replacement object to use as a default</param>
        IProxii<T> SetDefault<U>(string argName, U defaultValue);
        #endregion

        #region Finalization
        T Create();
        #endregion
    }
}