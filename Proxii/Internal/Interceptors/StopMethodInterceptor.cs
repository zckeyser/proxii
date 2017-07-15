using System;
using Castle.DynamicProxy;

namespace Proxii.Internal.Interceptors
{
    /// <summary>
    /// blocks the execution of intercepted methods
    /// 
    /// causes methods that return nullable types to return null,
    /// and methods that return non-nullable types to return those types'
    /// default values
    /// 
    /// to avoid blocking all actions on an object use a selector
    /// </summary>
    public class StopMethodInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            // do nothing -- if we're intercepting it we're blocking it

            // if it's a value type, make it return the default so it 
            // doesn't break with a NullReferenceException when called
            var type = invocation.Method.ReturnType;

            if (type.IsValueType && type != typeof(void))
            {
                invocation.ReturnValue = Activator.CreateInstance(type);
            }
        }
    }
}
