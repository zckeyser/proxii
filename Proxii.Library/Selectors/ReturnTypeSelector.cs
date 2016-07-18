using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Proxii.Library.Interceptors;

namespace Proxii.Library.Selectors
{
	/// <summary>
	/// only intercept methods with the given return types
	/// </summary>
	public class ReturnTypeSelector : IInterceptorSelector
	{
		// TODO TEST THIS
		private readonly HashSet<Type> _types;
		private readonly bool _allowExceptionInterceptor;

		public ReturnTypeSelector(bool allowExceptionInterceptor, params Type[] types)
		{
			_types = new HashSet<Type>(types);
			_allowExceptionInterceptor = allowExceptionInterceptor;
		}

		public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
		{
			var defaultInterceptors = _allowExceptionInterceptor
				? interceptors.Where(interceptor => interceptor is ExceptionInterceptor).ToArray()
				: new IInterceptor[0];

			return _types.Contains(method.ReturnType) ? interceptors : defaultInterceptors;
		}
	}
}
