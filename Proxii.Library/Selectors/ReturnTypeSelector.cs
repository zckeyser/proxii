using System;
using System.Collections.Generic;
using System.Reflection;
using Castle.DynamicProxy;

namespace Proxii.Library.Selectors
{
	/// <summary>
	/// only intercept methods with the given return types
	/// </summary>
	public class ReturnTypeSelector : IInterceptorSelector
	{
		private readonly HashSet<Type> _types;
        
        public ReturnTypeSelector(params Type[] types)
        {
            _types = new HashSet<Type>(types);
        }

		public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
		{
            return _types.Contains(method.ReturnType) ? interceptors : new IInterceptor[0];
		}
	}
}
