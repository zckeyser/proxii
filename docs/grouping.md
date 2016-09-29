# Method Grouping
Normally, all selectors are applied to all interceptors when something on a proxy built by `Proxii` is called. However, using the `Proxii.Group<T>(Action<Proxii<T>>)` method, interceptors and selectors can be grouped together such that you only do some interceptions for some methods on the object. This is done by configuring an encapsulated `Proxii` instance using an `Action<Proxii<T>>` passed into `Group`. In this action, you can treat the `Proxii` that you're passed as an empty object and configure it without worrying about side affects from other groups. The internal `Proxii` that you configure in the action is of the same type as the `Proxii` that `Group` was called from. However, all proxy configuration that's done on the main `Proxii` object will be applied universally to everything called from the proxy, so make sure you're aware of what should be affecting what when using `Group`.

```csharp
Action<int> onReturn = n => Console.WriteLine(n);

var singleGroupProxy = Proxii.Proxy<IFoo, Foo>()
                          .Group(p => p.ByMethodNamePattern("^*Method$")
                                       .OnReturn(onReturn)) // should just get called on methods that match the pattern
                          .BeforeInvoke(method => Console.WriteLine("calling {0}", method.Name)) // should get called on everything
                          .Create();

singleGroupProxy.IntMethod(); // returns 10
singleGroupProxy.Concat("a", "b", "c");
// output:
// calling IntMethod
// 10
// calling Concat

var multiGroupProxy = Proxii.Proxy<IProxiiTester, ProxiiTester>()
                          .Group(p => p.ByMethodNamePattern("^Int")
                                       .OnReturn(onReturn)) // only apply to functions starting with "Int"
                          .Group(p => p.ByReturnType(typeof(string))
                                        .ChangeReturnValue((string s) => new string(s.Reverse().ToArray()))) // only apply to functions that return a string
                          .Group(p => p.ByMethodName("Concat")
                                        .AfterInvoke(method => logger.Log("finished {0}", method.Name))) // should only apply when Concat is called
                          .BeforeInvoke(method => Console.WriteLine("calling {0}", method.Name)) // should get called on everything
                          .Create();

proxy.IntMethod(); // returns 10
proxy.Concat("a", "b", "c");
var result = proxy.StringMethod(); // normally returns "foobar"
Console.WriteLine(result);

// output:
// calling IntMethod
// 10
// calling Concat
// finished Concat
// calling StringMethod
// raboof
```
