﻿using System;
using Castle.DynamicProxy;

namespace Proxii.Library.Interceptors
{
    /// <summary>
    /// Blocks null arguments on every intercepted method, 
    /// throwing a NullArgumentException if any are found
    /// 
    /// Be careful to avoid using this interceptor where 
    /// null can be a valid argument
    /// </summary>
    public class NullInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var args = invocation.Arguments;
            var parameters = invocation.Method.GetParameters();

            for (var i = 0; i < args.Length; i++)
                if (args[i] == null)
                    throw new ArgumentNullException(parameters[i].Name);
        }
    }
}
