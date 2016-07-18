using Castle.DynamicProxy;

namespace Proxii
{
    public class LoggingInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Logger.Log(invocation.Method.Name);
            invocation.Proceed();
        }
    }
}
