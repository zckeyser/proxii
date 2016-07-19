using System;
using Castle.DynamicProxy;

namespace Proxii.Library.Interceptors
{
	public class BeforeInvokeInterceptor : IInterceptor
	{
		private readonly Action _beforeInvoke;

		public BeforeInvokeInterceptor(Action beforeInvoke)
		{
			_beforeInvoke = beforeInvoke;
		}

		public void Intercept(IInvocation invocation)
		{
			_beforeInvoke();
			invocation.Proceed();
		}
	}
}
