# Selection Methods
Selection methods filter what methods get intercepted, which is useful if you only want to proxy some methods from an interface while others act normally. Each selector can be stacked with multiple calls to accept multiple different criteria for a single selector.

## ByArgumentType
Only intercept arguments with the given argument types (order-sensitive).
```csharp
IFoo proxy = Proxii.Proxy<IFoo, Foo>()
                   .ByArgumentType(typeof(int), typeof(string))
                   .BeforeInvoke(() => Console.WriteLine("I'm hit!"))
                   .Create();

proxy.Bar(10, "foobar"); // logs "I'm hit!"
proxy.Buzz(10L, 1.0); // logs nothing
proxy.Bazz("foobar", 10); // logs nothing -- incorrect argument order
```

## ByMethodName
Only intercept methods with one of the given names.

```csharp
IFoo proxy = Proxii.Proxy<IFoo, Foo>()
                   .ByMethodName("Bar", "Buzz") // can pass as many as you want, or an array
                   .BeforeInvoke(() => Console.WriteLine("I'm hit!"))
                   .Create();

proxy.Bar(); // logs "I'm hit!"
proxy.Buzz(); // logs "I'm hit!"
proxy.Bazz(); // logs nothing
```

## ByReturnType
Only intercept methods with the given return type.

```csharp
interface IFoo
{
    int Bar();
    string Buzz();
}

IFoo proxy = Proxii.Proxy<IFoo, Foo>()
                   .ByReturnType(typeof(int))
                   .BeforeInvoke(() => Console.WriteLine("I'm hit!"))
                   .Create();

proxy.Bar(); // logs "I'm hit!"
proxy.Buzz(); // logs nothing
```

## Combining Selectors
You can stack multiple instances of the same selector to allow multiple of whatever is being selected to pass through. Stacking selectors works for any of the selectors. If multiple different selectors are used, functions must match all of the given selectors to be used.

```csharp
interface IFoo
{
    void Bar();
    void Bar(int i, string s);
    void Bar(int i, double d);
    void Buzz();
    void Buzz(int i, string s);
    void Bazz();
}

IFoo proxy = Proxii.Proxy<IFoo, Foo>()
                   .ByArgumentType(typeof(int), typeof(string))
                   .ByArgumentType(typeof(int), typeof(double))
                   .BeforeInvoke(() => Console.WriteLine("I'm hit!"))
                   .Create();

proxy.Bar(); // logs nothing
proxy.Bar(1, "foo"); // logs "I'm hit!"
proxy.Bar(1, 1.1); // logs "I'm hit!"

proxy = Proxii.Proxy<IFoo, Foo>()
                   .ByMethodName("Bar")
                   .ByArgumentType(typeof(int), typeof(string))
                   .BeforeInvoke(() => Console.WriteLine("I'm hit!"))
                   .Create();

proxy.Bar(); // logs nothing
proxy.Buzz(1, "foo"); // logs nothing
proxy.Bar(1, "foo"); // logs "I'm hit!"
```
