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
Only intercept methods with the given name.

```csharp
IFoo proxy = Proxii.Proxy<IFoo, Foo>()
                   .ByMethodName("Bar")
                   .BeforeInvoke(() => Console.WriteLine("I'm hit!"))
                   .Create();

proxy.Bar(); // logs "I'm hit!"
proxy.Buzz(); // logs nothing
```

## ByReturnType
Only intercept methods with the given return type.

```csharp
interface IFoo
{

}

IFoo proxy = Proxii.Proxy<IFoo, Foo>()
                   .ByReturnType(typeof(int))
```

## Combining Selectors
