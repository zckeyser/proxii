using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Proxii.Library.Interceptors
{
    public class ExceptionInterceptor : IInterceptor
    {
        private readonly Dictionary<Type, Action<Exception>> _customCatches = new Dictionary<Type, Action<Exception>>();

        /// <summary>
        /// throw the exception after performing the custom action
        /// </summary>
        public bool Rethrow { get; set; }
        
        public ExceptionInterceptor()
        {
	        Rethrow = false;
	        // do nothing -- we can add interceptions later
        }

        /// <summary>
        /// Initialize with a custom catch
        /// </summary>
        public ExceptionInterceptor(Type exception, Action<Exception> onThrow)
        {
	        Rethrow = false;
            AddCatch(exception, onThrow);
        }

        public void AddCatch(Type exception, Action<Exception> onThrow)
        {
            if (onThrow == null)
                throw new ArgumentNullException("onThrow");

            if (!(Activator.CreateInstance(exception) is Exception))
                throw new ArgumentException("exception must be a subclass of Exception");

            _customCatches.Add(exception, onThrow);
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch(Exception e)
            {
				// only perform the custom catch if it's the right type
                foreach (var type in _customCatches.Keys.Where(type => e.GetType() == type || e.GetType().IsSubclassOf(type)))
                {
                    var onCatch = _customCatches[type];

                    onCatch(e);

	                if (!Rethrow)
	                {
		                if(invocation.Method.ReturnType != typeof(void))
							invocation.ReturnValue = GetDefault(invocation.Method.ReturnType);

		                return;
	                }
                }

                // if we didn't hit anything, re-throw the exception
                throw;
            }
        }

	    private object GetDefault(Type type)
	    {
		    return type.IsValueType ? Activator.CreateInstance(type) : null;
	    }
    }
}
