using System;
using System.Deployment.Internal;
using System.Reflection;

namespace Proxii
{
    public interface IProxii<T> where T : class
    {
        IProxii<T> AfterInvoke(Action<MethodInfo> afterHook);
        IProxii<T> AfterInvoke(Action<MethodInfo, object[]> afterHook);
        IProxii<T> AfterInvoke(Action afterHook);
        IProxii<T> BeforeInvoke(Action<MethodInfo> beforeHook);
        IProxii<T> BeforeInvoke(Action<MethodInfo, object[]> beforeHook);
        IProxii<T> BeforeInvoke(Action beforeHook);
        IProxii<T> ByArgumentType(params Type[] argumentTypes);
        IProxii<T> ByMethodName(params string[] methodNames);
        IProxii<T> ByMethodNamePattern(params string[] patterns);
        IProxii<T> ByReturnType(params Type[] types);
        IProxii<T> Catch<TException>(Action<Exception> onCatch) where TException : Exception;
        IProxii<T> ChangeArguments<U>(Func<U, U> modifier);
        IProxii<T> ChangeArguments<U1, U2>(Func<U1, U2, Tuple<U1, U2>> modifier);
        IProxii<T> ChangeArguments<U1, U2, U3>(Func<U1, U2, U3, Tuple<U1, U2, U3>> modifier);
        IProxii<T> ChangeArguments<U1, U2, U3, U4>(Func<U1, U2, U3, U4, Tuple<U1, U2, U3, U4>> modifier);
        IProxii<T> ChangeArguments<U1, U2, U3, U4, U5>(Func<U1, U2, U3, U4, U5, Tuple<U1, U2, U3, U4, U5>> modifier);
        IProxii<T> ChangeArguments<U1, U2, U3, U4, U5, U6>(Func<U1, U2, U3, U4, U5, U6, Tuple<U1, U2, U3, U4, U5, U6>> modifier);
        IProxii<T> ChangeArguments<U1, U2, U3, U4, U5, U6, U7>(Func<U1, U2, U3, U4, U5, U6, U7, Tuple<U1, U2, U3, U4, U5, U6, U7>> modifier);
        IProxii<T> ChangeReturnValue<TReturn>(Func<TReturn, TReturn> onReturn);
        T Create();
        IProxii<T> Group(Func<IProxii<T>, IProxii<T>> configFunction);
        IProxii<T> OnReturn<U>(Action<U, object[]> onReturn);
        IProxii<T> OnReturn<U>(Action<U> onReturn);
        IProxii<T> OnReturn<U>(Action<U, MethodInfo> onReturn);
        IProxii<T> OnReturn<U>(Action<U, MethodInfo, object[]> onReturn);
        IProxii<T> RejectNullArguments();
        IProxii<T> Stop();
    }
}