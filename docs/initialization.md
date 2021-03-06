# Proxii Initialization
All Proxii instances are intialized from the `Proxii.Proxy<T>` method, which is called with the type of the interface you'd like to proxy over.

## Proxying between types
One option for initializing a proxy is to proxy between two types -- an interface and a class that implements it. This can be done using the method `Proxii.Proxy<TInterface, TImplementation>()`. Note that to use this option, your implementation class must have a valid empty constructor so Proxii can create an instance of it. If you need to proxy over an object that doesn't have a default constructor, look at the following section.
```csharp
IFoo proxy = Proxii.Proxy<IFoo, Foo>()
                   .Create(); // this is the same as the previous proxy initialization
```

## Proxying to an existing object
Another way to initialize a proxy is by connecting an interface to an existing object that implements it. This is useful if your object needs to use a non-default constructor or otherwise needs initialization logic done before it is connected to the proxy. A proxy of this type is created with the method `Proxii.Proxy<T>(U impl)`. It is not advised to create multiple proxies with one implementation, because it will become difficult to track how each proxy is affecting the base object.
```csharp
IBar proxy = Proxii.Proxy<IBar>(new Bar("foobar"))
                   .Create();
```
