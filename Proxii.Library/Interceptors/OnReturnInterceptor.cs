using Castle.DynamicProxy;
using System;
using System.Reflection;

namespace Proxii.Library.Interceptors
{
    public class OnReturnInterceptor<T> : IInterceptor
    {
        public Action<T> _returnAction;
        public Action<T, MethodInfo> _returnMethodInfoAction;
        public Action<T, MethodInfo, object[]> _returnMethodInfoArgsAction;
        public Action<T, object[]> _returnArgsAction;

        /// <summary>
        /// hook in an action that gets the return value
        /// </summary>
        public OnReturnInterceptor(Action<T> returnAction)
        {
            _returnAction = returnAction;
        }

        /// <summary>
        /// hook in an action that gets the return value and MethodInfo
        /// </summary>
        public OnReturnInterceptor(Action<T, MethodInfo> returnAction)
        {
            _returnMethodInfoAction = returnAction;
        }

        /// <summary>
        /// hook in an action that gets the return value and arguments
        /// </summary>
        public OnReturnInterceptor(Action<T, object[]> returnAction)
        {
            _returnArgsAction = returnAction;
        }

        /// <summary>
        /// hook in an action that gets the return value, MethodInfo and arguments
        /// </summary>
        public OnReturnInterceptor(Action<T, MethodInfo, object[]> returnAction)
        {
            _returnMethodInfoArgsAction = returnAction;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();

            // make sure a matching type was returned -- if not we don't intercept it
            if (invocation.Method.ReturnType != typeof(T))
                return;

			if (_returnAction != null)
                _returnAction((T)invocation.ReturnValue);
			else if (_returnMethodInfoAction != null)
                _returnMethodInfoAction((T)invocation.ReturnValue, invocation.Method);
			else if (_returnArgsAction != null)
                _returnArgsAction((T)invocation.ReturnValue, invocation.Arguments);
            else
                _returnMethodInfoArgsAction((T)invocation.ReturnValue, invocation.Method, invocation.Arguments);
        }
    }
}
