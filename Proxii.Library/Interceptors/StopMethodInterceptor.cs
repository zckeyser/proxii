using Castle.DynamicProxy;

namespace Proxii.Library.Interceptors
{
    /// <summary>
    /// blocks the execution of intercepted methods
    /// 
    /// to avoid blocking all actions on an object use a selector
    /// </summary>
    public class StopMethodInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            // do nothing -- if we're intercepting it we're blocking it
        }
    }
}
