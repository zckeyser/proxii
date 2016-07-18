using Castle.DynamicProxy;
using System;
using System.Collections.Generic;

namespace Proxii
{
    public class ExceptionInterceptor : IInterceptor
    {
        private readonly Dictionary<Type, Action<Exception>> customCatches = new Dictionary<Type, Action<Exception>>();

        /// <summary>
        /// throw the exception after performing the custom action
        /// </summary>
        public bool Rethrow { get; set; }
        
        public ExceptionInterceptor()
        {
            // do nothing -- we can add interceptions later
        }

        /// <summary>
        /// Initialize with a custom catch
        /// </summary>
        public ExceptionInterceptor(Type exception, Action<Exception> onThrow)
        {
            AddCatch(exception, onThrow);
        }

        public void AddCatch(Type exception, Action<Exception> onThrow)
        {
            if (onThrow == null)
                throw new ArgumentNullException("onThrow");

            if (!exception.IsSubclassOf(typeof(Exception)))
                throw new ArgumentException("exception must be a subclass of Exception");

            customCatches.Add(exception, onThrow);
        }

        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch(Exception e)
            {
                foreach (var type in customCatches.Keys)
                {
                    // only perform the custom catch if it's the right type
                    if (e.GetType() == type)
                    {
                        var onCatch = customCatches[type];

                        onCatch(e);

                        if(!Rethrow)
                            return;
                    }
                }

                // if we didn't hit anything, re-throw the exception
                throw;
            }
        }
    }
}
