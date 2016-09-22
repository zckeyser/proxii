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

			if (_returnAction != null && typeof(T) == _returnAction.GetMethodInfo().ReturnType)
                _returnAction((T)invocation.ReturnValue);
			else if (_returnMethodInfoAction != null && typeof(T) == _returnMethodInfoAction.GetMethodInfo().ReturnType)
                _returnMethodInfoAction((T)invocation.ReturnValue, invocation.Method);
			else if (_returnArgsAction != null && typeof(T) == _returnArgsAction.GetMethodInfo().ReturnType)
                _returnArgsAction((T)invocation.ReturnValue, invocation.Arguments);
            else if(_returnMethodInfoArgsAction != null && typeof(T) == _returnMethodInfoArgsAction.GetMethodInfo().ReturnType)
                _returnMethodInfoArgsAction((T)invocation.ReturnValue, invocation.Method, invocation.Arguments);
        }
    }
}
