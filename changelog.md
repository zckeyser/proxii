# 1.5.0
- Added `Benchmark` interceptor, which times intercepted method calls and calls a callback with the timing, MethodInfo (optional) and argument list (optional.)

# 1.4.4
 - Added `SetDefault` interceptor, which sets default values for null arguments either by factory method or by direct reference

# 1.4.3
 - Updated `IProxii` to block all null arguments passed into any of its methods
 - Added `MaxCalls` interceptor, which blocks any repeated calls to a method after the first N times it is called

# 1.4.2
 - Fixed bug causing `IProxii<T>.RejectNullArguments()` to block all calls that went through it
 - Moved all `Proxii\<T>` functionality behind the `IProxii\<T>` interface -- all IProxii\<T> methods now return an IProxii\<T> for chaining instead of a Proxii\<T>

# 1.4.1
 - Added `Proxii.RejectNullArguments` interceptor: throws an ArgumentNullException whenever a null argument is passed to an intercepted method, including the name of the argument in the exception message
 - Changed internal implementation of `Catch()`

# 1.4.0
 - Added grouping method `Proxii.Group` to allow grouping of selectors and interceptors on an object, so you can isolate the affects of an interceptor(s) to a given group of selectors. See the documentation [here](https://github.com/zckeyser/proxii/blob/master/docs/grouping.md).

# 1.3.1
 - Fixed bug where `Proxii.Freeze` was leaking "this"

# 1.3.0
 - Added new interception method: `Proxii<T>.Stop()` prevents the execution of intercepted methods
 - Added new selection method: `Proxii<T>.ByMethodNamePattern(params string[] patterns)` allows the selection of methods via a regex pattern on their name
 - Added new utility method: `Proxii.Freeze<T>(T obj, params string[] patterns)` allows the freezing of an object, such that any property setters and methods that match a given pattern will not be executed.

# 1.2.0
 - Removed empty `Proxii.Proxy<T>()` method and hid `.With()` methods -- use the non-empty `Proxii.Proxy()` methods instead
 - Added context comments for all of the functions in `Proxii<T>`

# 1.1.1
 - Deprecated empty `Proxy<T>()` call and `.With()` in favor of the new convenience methods -- these will be removed in 1.2.0

# 1.1.0
 - Added `OnReturn` interceptor - hook an action into the values/method being returned from. Has options to hook in with the return value, `MethodInfo` of the called function and the arguments the function was called with.

# 1.0.1
 - Changed `ByMethodName` to be stackable

# 1.0.0
 - Initial release, including the following:

### Initialization
     - Interface to type
     - Interface to object

### Interceptors (inject behavior)
     - `AfterInvoke` - hook behavior in after calling a method
     - `BeforeInvoke` - hook behavior in before calling a method
     - `Catch` - add custom error handling behavior around calls
     - `ChangeArguments` - Intercept and change arguments with the given function
     - `ChangeReturnValue` - Intercept and change return value with the given function

### Selectors (change what gets intercepted)
     - `ByArgumentType` - filter by what argument types a function takes
     - `ByMethodName` - filter by the name of the method being called
     - `ByReturnType` - filter by return type
