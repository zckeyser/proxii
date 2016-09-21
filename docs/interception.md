# Interception Methods
These are methods which allow you to hook in new behavior in some way on any methods called on the proxy object. To limit what methods get intercepted, you can use [selection]() methods.

## AfterInvoke
Provides three different ways to hook an action after the intercepted function has been invoked. Depending on what the signature of the action passed into `AfterInvoke` is, you can call a function with either no arguments, the `MethodInfo` of the intercepted function, or the `MethodInfo` *and* an array of the arguments passed into the function.

```csharp
interface IFoo
{
    void Bar(int a, int b, int c);
}

// simple no-argument hook
var noArgProxy = Proxii.Proxy<IFoo, Foo>()
                       .AfterInvoke(() => Console.WriteLine("I'm hit!"))
                       .Create();

noArgProxy.Bar(1, 2, 3); // logs "I'm hit!"

var methodInfoProxy = Proxii.Proxy<IFoo, Foo>()
                       .AfterInvoke((method) => Console.WriteLine("{0} is hit!", method.Name))
                       .Create();

methodInfoProxy.Bar(1, 2, 3); // logs "Bar is hit!"

var methodInfoArgsProxy = Proxii.Proxy<IFoo, Foo>()
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

methodInfoArgsProxy.Bar(1, 2, 3); // logs "Bar is hit by 1 2 3"
```

## BeforeInvoke
Functionally, BeforeInvoke is equivalent to AfterInvoke except that the given hook is called *before* the method being intercepted is invoked.

```csharp
interface IFoo
{
    void Bar(int a, int b, int c);
}

// simple no-argument hook
var noArgProxy = Proxii.Proxy<IFoo, Foo>()
                       .BeforeInvoke(() => Console.WriteLine("I'm hit!"))
                       .Create();

noArgProxy.Bar(1, 2, 3); // logs "I'm hit!"

var methodInfoProxy = Proxii.Proxy<IFoo, Foo>()
                       .BeforeInvoke((method) => Console.WriteLine("{0} is hit!", method.Name))
                       .Create();

methodInfoProxy.Bar(1, 2, 3); // logs "Bar is hit!"

var methodInfoArgsProxy = Proxii.Proxy<IFoo, Foo>()
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

methodInfoArgsProxy.Bar(1, 2, 3); // logs "Bar is hit by 1 2 3"
```

## Catch
Executes the given handler when any of the given kind of exceptions are thrown from inside intercepted functions.

```csharp
interface IFoo
{
    void ThrowArgumentException();
    void ThrowIndexOutOfRangeException();
}

var proxy = Proxii.Proxy<IFoo, Foo>()
                .Catch<ArgumentException>((e) => Console.WriteLine("Caught an exception :("))
                .Create();

proxy.ThrowArgumentException(); // logs "Caught an exception :("
proxy.ThrowIndexOutOfRangeException(); // doesn't get caught -- crashes
```

## ChangeArguments
Changes the arguments of any intercepted function which matches the signature of the given function. The given function must take all of the arguments as input and output a tuple of all the modified arguments as output. Order of the original argument types must be maintained in the output tuple.

```csharp
interface IFoo
{
    int ReturnInt(int n); // returns given argument unchanged
    int Add(int a, int b); // returns a + b
}

// modifying a single argument
Func<int, int> singleArgModifier = (n) => n + 1;

var singleArgProxy = Proxii.Proxy<IFoo, Foo>()
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
