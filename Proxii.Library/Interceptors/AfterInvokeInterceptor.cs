using System;
using Castle.DynamicProxy;

namespace Proxii.Library.Interceptors
{
	public class AfterInvokeInterceptor : IInterceptor
	{
		private readonly Action _afterInvoke;

		public AfterInvokeInterceptor(Action afterInvoke)
		{
			_afterInvoke = afterInvoke;
		}

		public void Intercept(IInvocation invocation)
		{
			invocation.Proceed();
			_afterInvoke();
		}
	}
}
