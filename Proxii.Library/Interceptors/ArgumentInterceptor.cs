using System;
using System.Linq;
using Castle.DynamicProxy;
using System.Reflection;

namespace Proxii.Library.Interceptors
{
    /// <summary>
    /// modifies input arguments using the given function, 
    /// reassigning all of them to the output values of the given function before invoking
    /// </summary>
	public class ArgumentInterceptor<T> : IInterceptor
	{
        private readonly Func<T, T> _modifier;

        public ArgumentInterceptor(Func<T, T> modifier)
        {
            _modifier = modifier;
        }

		public void Intercept(IInvocation invocation)
		{
            if(SignatureMatcher.Match(invocation.Method, this))
            {
                var arg = (T) invocation.Arguments[0];

                var modified = _modifier(arg);

                invocation.SetArgumentValue(0, modified);
            }

            invocation.Proceed();
		}
	}

    /// <summary>
    /// modifies input arguments using the given function, 
    /// reassigning all of them to the output values of the given function before invoking
    /// </summary>
	public class ArgumentInterceptor<T1, T2> : IInterceptor
    {
        private readonly Func<T1, T2, Tuple<T1, T2>> _modifier;

        public ArgumentInterceptor(Func<T1, T2, Tuple<T1, T2>> modifier)
        {
            _modifier = modifier;
        }

        public void Intercept(IInvocation invocation)
        {
            if (SignatureMatcher.Match(invocation.Method, this))
            {
                var a = (T1)invocation.Arguments[0];
                var b = (T2)invocation.Arguments[1];

                var modified = _modifier(a, b);

                invocation.SetArgumentValue(0, modified.Item1);
                invocation.SetArgumentValue(1, modified.Item2);
            }

            invocation.Proceed();
        }
    }

    /// <summary>
    /// modifies input arguments using the given function, 
    /// reassigning all of them to the output values of the given function before invoking
    /// </summary>
	public class ArgumentInterceptor<T1, T2, T3> : IInterceptor
    {
        private readonly Func<T1, T2, T3, Tuple<T1, T2, T3>> _modifier;

        public ArgumentInterceptor(Func<T1, T2, T3, Tuple<T1, T2, T3>> modifier)
        {
            _modifier = modifier;
        }

        public void Intercept(IInvocation invocation)
        {
            if (SignatureMatcher.Match(invocation.Method, this))
            {
                var a = (T1)invocation.Arguments[0];
                var b = (T2)invocation.Arguments[1];
                var c = (T3)invocation.Arguments[2];

                var modified = _modifier(a, b, c);

                invocation.SetArgumentValue(0, modified.Item1);
                invocation.SetArgumentValue(1, modified.Item2);
                invocation.SetArgumentValue(2, modified.Item3);
            }

            invocation.Proceed();
        }
    }

    /// <summary>
    /// modifies input arguments using the given function, 
    /// reassigning all of them to the output values of the given function before invoking
    /// </summary>
	public class ArgumentInterceptor<T1, T2, T3, T4> : IInterceptor
    {
        private readonly Func<T1, T2, T3, T4, Tuple<T1, T2, T3, T4>> _modifier;

        public ArgumentInterceptor(Func<T1, T2, T3, T4, Tuple<T1, T2, T3, T4>> modifier)
        {
            _modifier = modifier;
        }

        public void Intercept(IInvocation invocation)
        {
            if (SignatureMatcher.Match(invocation.Method, this))
            {
                var a = (T1)invocation.Arguments[0];
                var b = (T2)invocation.Arguments[1];
                var c = (T3)invocation.Arguments[2];
                var d = (T4)invocation.Arguments[3];

                var modified = _modifier(a, b, c, d);

                invocation.SetArgumentValue(0, modified.Item1);
                invocation.SetArgumentValue(1, modified.Item2);
                invocation.SetArgumentValue(2, modified.Item3);
                invocation.SetArgumentValue(3, modified.Item4);
            }

            invocation.Proceed();
        }
    }

    /// <summary>
    /// modifies input arguments using the given function, 
    /// reassigning all of them to the output values of the given function before invoking
    /// </summary>
	public class ArgumentInterceptor<T1, T2, T3, T4, T5> : IInterceptor
    {
        private readonly Func<T1, T2, T3, T4, T5, Tuple<T1, T2, T3, T4, T5>> _modifier;

        public ArgumentInterceptor(Func<T1, T2, T3, T4, T5, Tuple<T1, T2, T3, T4, T5>> modifier)
        {
            _modifier = modifier;
        }

        public void Intercept(IInvocation invocation)
        {
            if (SignatureMatcher.Match(invocation.Method, this))
            {
                var a = (T1)invocation.Arguments[0];
                var b = (T2)invocation.Arguments[1];
                var c = (T3)invocation.Arguments[2];
                var d = (T4)invocation.Arguments[3];
                var e = (T5)invocation.Arguments[4];

                var modified = _modifier(a, b, c, d, e);

                invocation.SetArgumentValue(0, modified.Item1);
                invocation.SetArgumentValue(1, modified.Item2);
                invocation.SetArgumentValue(2, modified.Item3);
                invocation.SetArgumentValue(3, modified.Item4);
                invocation.SetArgumentValue(4, modified.Item5);
            }

            invocation.Proceed();
        }
    }

    /// <summary>
    /// modifies input arguments using the given function, 
    /// reassigning all of them to the output values of the given function before invoking
    /// </summary>
	public class ArgumentInterceptor<T1, T2, T3, T4, T5, T6> : IInterceptor
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, Tuple<T1, T2, T3, T4, T5, T6>> _modifier;

        public ArgumentInterceptor(Func<T1, T2, T3, T4, T5, T6, Tuple<T1, T2, T3, T4, T5, T6>> modifier)
        {
            _modifier = modifier;
        }

        public void Intercept(IInvocation invocation)
        {
            if (SignatureMatcher.Match(invocation.Method, this))
            {
                var a = (T1)invocation.Arguments[0];
                var b = (T2)invocation.Arguments[1];
                var c = (T3)invocation.Arguments[2];
                var d = (T4)invocation.Arguments[3];
                var e = (T5)invocation.Arguments[4];
                var f = (T6)invocation.Arguments[5];

                var modified = _modifier(a, b, c, d, e, f);

                invocation.SetArgumentValue(0, modified.Item1);
                invocation.SetArgumentValue(1, modified.Item2);
                invocation.SetArgumentValue(2, modified.Item3);
                invocation.SetArgumentValue(3, modified.Item4);
                invocation.SetArgumentValue(4, modified.Item5);
                invocation.SetArgumentValue(5, modified.Item6);
            }

            invocation.Proceed();
        }
    }

    /// <summary>
    /// modifies input arguments using the given function, 
    /// reassigning all of them to the output values of the given function before invoking
    /// </summary>
	public class ArgumentInterceptor<T1, T2, T3, T4, T5, T6, T7> : IInterceptor
    {
        private readonly Func<T1, T2, T3, T4, T5, T6, T7, Tuple<T1, T2, T3, T4, T5, T6, T7>> _modifier;

        public ArgumentInterceptor(Func<T1, T2, T3, T4, T5, T6, T7, Tuple<T1, T2, T3, T4, T5, T6, T7>> modifier)
        {
            _modifier = modifier;
        }

        public void Intercept(IInvocation invocation)
        {
            if (SignatureMatcher.Match(invocation.Method, this))
            {
                var a = (T1)invocation.Arguments[0];
                var b = (T2)invocation.Arguments[1];
                var c = (T3)invocation.Arguments[2];
                var d = (T4)invocation.Arguments[3];
                var e = (T5)invocation.Arguments[4];
                var f = (T6)invocation.Arguments[5];
                var g = (T7)invocation.Arguments[6];

                var modified = _modifier(a, b, c, d, e, f, g);

                invocation.SetArgumentValue(0, modified.Item1);
                invocation.SetArgumentValue(1, modified.Item2);
                invocation.SetArgumentValue(2, modified.Item3);
                invocation.SetArgumentValue(3, modified.Item4);
                invocation.SetArgumentValue(4, modified.Item5);
                invocation.SetArgumentValue(5, modified.Item6);
                invocation.SetArgumentValue(6, modified.Item7);
            }

            invocation.Proceed();
        }
    }

    internal static class SignatureMatcher
    {
        public static bool Match(MethodInfo method, object argumentInterceptor)
        {
            var methodParams = method.GetParameters().Select(parameter => parameter.ParameterType);
            var objectParams = argumentInterceptor.GetType().GetGenericArguments();

            return methodParams.SequenceEqual(objectParams);
        }
    }
}
