# Interception Methods
These are methods which allow you to hook in new behavior in some way on any methods called on the proxy object. To limit what methods get intercepted, you can use [selection]() methods.

## AfterInvoke
Provides three different ways to hook an action after the intercepted function has been invoked. Depending on what the signature of the action passed into `AfterInvoke` is, you can call a function with either no arguments, the `MethodInfo` of the intercepted function, or the `MethodInfo` *and* an array of the arguments passed into the function.

```

```

## BeforeInvoke
Functionally, BeforeInvoke is equivalent to AfterInvoke except that the given hook is called *before* the method being intercepted is invoked.

```

```

## Catch
Executes the given handler when any of the given kind of exceptions are thrown from inside intercepted functions.

```

```

## ChangeArguments
Changes the arguments of any intercepted function which matches the signature of the given function. The given function must take all of the arguments as input and output a tuple of all the modified arguments as output.

```

```

## ChangeReturnValue
Changes the return value of any intercepted function which matches the input type of the given function. The given function must output the same type it takes in.

```

```
