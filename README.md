## What is Proxii?
Proxii is a library which can be used to easily implement the [proxy pattern](https://en.wikipedia.org/wiki/Proxy_pattern) to inject behavior between an interface and its implementation. Proxii utilizes a fluid interface to make code utilizing it more readable, and has a variety of different behaviors available to change things going into or out of functions, as well as options to filter what methods get intercepted. Proxii is built over Castle's [DynamicProxy](http://www.castleproject.org/projects/dynamicproxy/) library.

## Installation
This project is on NuGet as [Proxii](https://www.nuget.org/packages/Proxii/1.0.1). It can be installed by running the following in PowerShell:
```
Install-Package Proxii
```

## Quick Start
To use Proxii, you first need to have whatever object you'd like to proxy over behind an interface. A simple example of creating a proxy over an interface is as follows:

```csharp
var proxy = Proxii.Proxy<IFoo>()
                  .With<Foo>()
                  .BeforeInvoke(() => Console.WriteLine("Calling from foo!"))
                  .Create();

proxy.Bar(); // will log "Calling from foo!" to console before executing Bar
```

In addition, you can filter what methods from the interface get intercepted using the built-in selector methods, such as in the following example:

```csharp
var proxy = Proxii.Proxy<IFoo>()
                  .With<Foo>()
                  .BeforeInvoke(() => Console.WriteLine("Calling from foo!"))
                  .ByMethodName("Bar")
                  .Create();

proxy.Bar(); // will log "Calling from foo!" to console before executing bar
proxy.Buzz(); // will not log anything
```

For a more in-depth look at the library, see the full list of [initialization](https://github.com/zckeyser/proxii/blob/master/docs/initialization.md), [interception](https://github.com/zckeyser/proxii/blob/master/docs/interception.md),  [selection](https://github.com/zckeyser/proxii/blob/master/docs/selection.md), and
[grouping](https://github.com/zckeyser/proxii/blob/master/docs/grouping.md) methods.

Proxii also has some other useful [utility](https://github.com/zckeyser/proxii/blob/master/docs/utility.md) methods.
