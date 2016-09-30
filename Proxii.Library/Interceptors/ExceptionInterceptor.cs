using Castle.DynamicProxy;
using System;

namespace Proxii.Library.Interceptors
{
    public class ExceptionInterceptor<T> : IInterceptor
        where T : Exception
    {
        private readonly Action<T> onCatch;

        /// <summary>
        /// throw the exception after performing the custom action
        /// </summary>
        public bool Rethrow { get; set; }

        /// <summary>
        /// Initialize with a custom catch
        /// </summary>
        public ExceptionInterceptor(Action<T> onCatch, bool rethrow = false)
        {
	        Rethrow = rethrow;

            this.onCatch = onCatch;
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch(T e)
            {
                onCatch(e);

		        if(invocation.Method.ReturnType != typeof(void))
					invocation.ReturnValue = GetDefault(invocation.Method.ReturnType);

                // re-throw if desired
                if(Rethrow) throw;
            }
        }

	    private static object GetDefault(Type type)
	    {
		    return type.IsValueType ? Activator.CreateInstance(type) : null;
	    }
    }
}
