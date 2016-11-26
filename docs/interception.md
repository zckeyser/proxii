# Interception Methods
These are methods which allow you to hook in new behavior in some way on any methods called on the proxy object. To limit what methods get intercepted, you can use [selection](https://github.com/zckeyser/proxii/blob/master/docs/selection.md) methods.

 - [AfterInvoke](#after-invoke)
 - [BeforeInvoke](#before-invoke)
 - [Catch](#catch)
 - [ChangeArguments](#change-arguments)
 - [ChangeReturnValue](#change-return-value)
 - [OnReturn](#on-return)
 - [RejectNullArguments](#reject-null-arguments)
 - [SetDefault](#set-default)

## AfterInvoke
`AfterInvoke(Action action)`, `AfterInvoke(Action<MethodInfo> action)`, `AfterInvoke(Action<MethodInfo, object[]> action)`

Provides three different ways to hook an action after the intercepted function has been invoked. Depending on what the signature of the action passed into `AfterInvoke` is, you can call a function with either no arguments, the `MethodInfo` of the intercepted function, or the `MethodInfo` *and* an array of the arguments passed into the function.
```csharp
interface IFoo
{
    void Bar(int a, int b, int c);
}

// simple no-argument hook
var noArgProxy = Proxii.Proxy\<IFoo, Foo>()
                       .AfterInvoke(() => Console.WriteLine("I'm hit!"))
                       .Create();

noArgProxy.Bar(1, 2, 3); // logs "I'm hit!"

var methodInfoProxy = Proxii.Proxy\<IFoo, Foo>()
                       .AfterInvoke((method) => Console.WriteLine("{0} is hit!", method.Name))
                       .Create();

methodInfoProxy.Bar(1, 2, 3); // logs "Bar is hit!"

var methodInfoArgsProxy = Proxii.Proxy\<IFoo, Foo>()
                            .AfterInvoke((method, args) =>
                                {
                                    // all args combined into a single space delimited string
                                    var argString = args.Select(arg => arg.ToString())
                                                        .Aggregate("", (curr, next) => curr + next + " ")
                                                        .Trim();

                                    Console.WriteLine("{0} is hit by {1}!", method.Name, argString);
                                }
                            )
                            .Create();

methodInfoArgsProxy.Bar(1, 2, 3); // logs "Bar is hit by 1 2 3!"
```

## BeforeInvoke
`BeforeInvoke(Action action)`, `BeforeInvoke(Action<MethodInfo> action)`, `BeforeInvoke(Action<MethodInfo, object[]> action)`

Functionally, BeforeInvoke is equivalent to AfterInvoke except that the given hook is called *before* the method being intercepted is invoked.
```csharp
interface IFoo
{
    void Bar(int a, int b, int c);
}

// simple no-argument hook
var noArgProxy = Proxii.Proxy\<IFoo, Foo>()
                       .BeforeInvoke(() => Console.WriteLine("I'm hit!"))
                       .Create();

noArgProxy.Bar(1, 2, 3); // logs "I'm hit!"

var methodInfoProxy = Proxii.Proxy\<IFoo, Foo>()
                       .BeforeInvoke((method) => Console.WriteLine("{0} is hit!", method.Name))
                       .Create();

methodInfoProxy.Bar(1, 2, 3); // logs "Bar is hit!"

var methodInfoArgsProxy = Proxii.Proxy\<IFoo, Foo>()
                            .BeforeInvoke((method, args) =>
                                {
                                    // all args combined into a single space delimited string
                                    var argString = args.Select(arg => arg.ToString())
                                                        .Aggregate("", (curr, next) => curr + next + " ")
                                                        .Trim();

                                    Console.WriteLine("{0} is hit by {1}!", method.Name, argString);
                                }
                            )
                            .Create();

methodInfoArgsProxy.Bar(1, 2, 3); // logs "Bar is hit by 1 2 3!"
```

## Catch
`Catch<T>(Action<Exception> onCatch) where T : Exception`

Executes the given handler when any of the given kind of exceptions are thrown from inside intercepted functions.
```csharp
interface IFoo
{
    void ThrowArgumentException();
    void ThrowIndexOutOfRangeException();
}

var proxy = Proxii.Proxy\<IFoo, Foo>()
                .Catch\<ArgumentException>((e) => Console.WriteLine("Caught an exception :("))
                .Create();

proxy.ThrowArgumentException(); // logs "Caught an exception :("
proxy.ThrowIndexOutOfRangeException(); // doesn't get caught -- crashes
```

## ChangeArguments
`ChangeArguments(Func<T, T> modifier)`, `ChangeArguments(Func<T1, T2, Tuple<T1, T2>> modifier)`, etc.

Changes the arguments of any intercepted function which matches the signature of the given function. The given function must take all of the arguments as input and output a tuple of all the modified arguments as output. Order of the original argument types must be maintained in the output tuple.
```csharp
interface IFoo
{
    int ReturnInt(int n); // returns given argument unchanged
    int Add(int a, int b); // returns a + b
}

// modifying a single argument
Func<int, int> singleArgModifier = (n) => n + 1;

var singleArgProxy = Proxii.Proxy\<IFoo, Foo>()
                        .ChangeArguments(singleArgModifier)
                        .Create();

singleArgProxy.ReturnInt(1); // returns 2
singleArgProxy.Add(2, 2); // returns 4

// modifying multiple arguments
Func<int, int, Tuple<int, int>> multiArgModifier = (a, b)  =>
    {
        return Tuple.Create(a * 2, b * 4);
    };

var multiArgProxy = Proxii.Proxy<IFoo, Foo>()
                        .ChangeArguments(multiArgModifier)
                        .Create();

multiArgProxy.ReturnInt(1); // returns 1
multiArgProxy.Add(2, 2); // returns 12
```

## ChangeReturnValue
`ChangeReturnValue(Func<T, T> modifier)`

Changes the return value of any intercepted function which matches the input type of the given function. The given function must output the same type it takes in.
```csharp
interface IFoo
{
    int Add(int a, int b);
}

Func<int, int> modifier = (n) => n * 10;

var proxy = Proxii.Proxy<IFoo, Foo>()
                        .ChangeReturnValue(modifier)
                        .Create();

proxy.Add(2, 2); // 40
```

## OnReturn
`OnReturn(Action<T> onReturn)`, `OnReturn(Action<T, MethodInfo> onReturn)`, `OnReturn(Action<T, object[]> onReturn)`, `OnReturn(Action<T, MethodInfo, object[]> onReturn)`

Hooks into the function on return using the passed in Action. There are four versions of this function, which all pass different information into the action. All of the overloads differ only in the signature of the Action they take, and as such the information passed in. The data that can be passed out is: return value, MethodInfo and arguments. The overload parameter types are: Action<T>, Action<T, MethodInfo>, Action<T, object[]>, Action<T, MethodInfo, object[]> where T is the type of return value you'd like to intercept. Only functions with return types matching the given action will be intercepted.
```csharp
interface IFoo
{
    // concatenate 3 strings without adding a delimiter
    // e.g. Concat("foo", "bar", "buzz") => "foobarbuzz"
    string Concat(string a, string b, string c);
}

// hook with return value
Action\<string> onReturnOnly = (s) => Console.WriteLine("returned {0}", s);

var returnProxy = Proxii.Proxy()\<IFoo, Foo>
                        .OnReturn(onReturnOnly)
                        .Create();

returnProxy.Concat("foo", "bar", "buzz"); // logs "returned foobarbuzz"

// hook with return value and method information
Action\<string, MethodInfo> onReturnWithMethod = (s, method) => Console.WriteLine("returned {0} from {1}", s, method.Name);

var returnWithMethodProxy = Proxii.Proxy()\<IFoo, Foo>
                                  .OnReturn(onReturnWithMethod)
                                  .Create();

returnWithMethodProxy.Concat("foo", "bar", "buzz"); // logs "returned foobarbuzz from Concat"

// hook with return value and arguments
Action<string, object[]> onReturnWithArgs = (s, args) => Console.WriteLine("returned {0} with input {1}", s, args.Select(arg => arg.ToString()).Aggregate("", (total, next) => total + next + " ")).Trim();

var returnWithArgsProxy = Proxii.Proxy()\<IFoo, Foo>
                                .OnReturn(onReturnWithArgs)
                                .Create();

returnWithArgsProxy.Concat("foo", "bar", "buzz"); // logs "returned foobarbuzz with input foo bar buzz"

// hook with return value, method info and arguments
Action\<string, MethodInfo, object[]> onReturnWithArgs = (s, method, args) => Console.WriteLine("returned {0} from {1} with input {2}", s, method.Name, args.Select(arg => arg.ToString()).Aggregate("", (total, next) => total + next + " ")).Trim();

var returnWithArgsProxy = Proxii.Proxy()\<IFoo, Foo>
                                .OnReturn(onReturnWithArgs)
                                .Create();

returnWithArgsProxy.Concat("foo", "bar", "buzz"); // logs "returned foobarbuzz from Concat with input foo bar buzz"
```

## RejectNullArguments
`RejectNullArguments()`

Throws an ArgumentNullException when a null argument is passed to an intercepted method. The name of the null argument is included in the exception.
```csharp
interface IFoo
{
    void DoStuff(string bar);
}

var proxy = Proxii.Proxy\<IFoo, Foo>()
                  .RejectNullArguments()
                  .Create();

proxy.DoStuff(null); // throws new ArgumentNullException("bar")
```

## Stop
`Stop()`

Prevent the execution of all intercepted methods on this proxy.
```csharp
var proxy = Proxii.Proxy<IFoo, Foo>()
                  .ByMethodName("Bar")
                  .Stop()
                  .Create();

proxy.Bar(); // does nothing
proxy.Buzz(); // acts normally
```

## SetDefault
`SetDefault<T>(string argName, Func<T> factory)`, `SetDefault<T>(string argName, T defaultValue)`

Sets the value of any passed null arguments with the given type (via generic parameter) and name using either the given factory or the given value. If this method is called multiple times for the same argument name, the most recent call will be the default used (this includes value calls canceling out factory calls and vice-versa).

The advantage of this over optional parameters is that this allows defaults which are determined at runtime, whereas optional parameters only allow `null` for reference types and constants for value types.

Value types cannot be defaulted unless they are made nullable (e.g. `int?`).

```csharp
// interface matches class signatures
public class DefaultTester : IDefaultTester
{
    public string DoStuff(string s = null)
    {
        return s;
    }

    public string DoOtherStuff(string ss = null) {
        return ss;
    }
}

var proxy = Proxii.Proxy<IFoo, Foo>()
                  .SetDefault("s", "foo")
                  .SetDefault("ss", () => "bar")
                  .Create();

proxy.DoStuff(); // "foo"
proxy.DoOtherStuff(); // "bar"

proxy = Proxii.Proxy<IFoo, Foo>()
                  .SetDefault("s", "foo")
                  .SetDefault("s", () => "bar")
                  .Create();

proxy.DoStuff(); // "bar"
```
