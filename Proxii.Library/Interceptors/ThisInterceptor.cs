using Castle.DynamicProxy;

namespace Proxii.Library.Interceptors
{
	/// <summary>
	/// Interceptor put at the end of the interceptor chain to
	/// avoid leaking "this," as per the warning in
	/// https://github.com/castleproject/Core/blob/master/docs/dynamicproxy-leaking-this.md
	/// </summary>
	public class ThisInterceptor : IInterceptor
	{
        //TODO test this
		public void Intercept(IInvocation invocation)
		{
			invocation.Proceed();

			if (invocation.ReturnValue == invocation.InvocationTarget)
			{
				invocation.ReturnValue = invocation.Proxy;
			}
		}
	}
}
