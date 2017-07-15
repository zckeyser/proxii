using System;
using Castle.DynamicProxy;

namespace Proxii.Internal.Interceptors
{
	public class ChangeReturnValueInterceptor<T> : IInterceptor
	{
		private readonly Func<T, T> _onReturn;

		public ChangeReturnValueInterceptor(Func<T, T> onReturn)
		{
			_onReturn = onReturn;
		}

		public void Intercept(IInvocation invocation)
		{
			invocation.Proceed();

			// only intercept the retval if it's the right type
			if (invocation.Method.ReturnType == typeof(T))
			{
				invocation.ReturnValue = _onReturn((T) invocation.ReturnValue);
			}
		}
	}
}
