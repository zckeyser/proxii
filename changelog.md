# 1.3.0
 - new interception method `Proxii<T>.Stop()` prevents the execution of intercepted methods
 - new selection method `Proxii<T>.ByMethodNamePattern(params string[] patterns)` allows the selection of methods via a regex pattern on their name
 - new utility method `Proxii.Freeze<T>(T obj, params string[] patterns)` allows the freezing of an object, such that any property setters and methods that match a given pattern will not be executed.

# 1.2.0
 - Removed empty Proxii.Proxy<T>() method and hid .With() methods -- use the non-empty Proxii.Proxy() methods instead
 - Added context comments for all of the functions in Proxii<T>

# 1.1.1
 - Deprecated empty Proxy<T>() call and .With() in favor of the new convenience methods -- these will be removed in 1.2.0

# 1.1.0
 - Added OnReturn interceptor - hook an action into the values/method being returned from. Has options to hook in with the return value, MethodInfo of the called function and the arguments the function was called with.

# 1.0.1
 - Changed ByMethodName to be stackable

# 1.0.0
 - Initial release, including the following:

### Initialization
     - Interface to type
     - Interface to object

### Interceptors (inject behavior)
     - AfterInvoke - hook behavior in after calling a method
     - BeforeInvoke - hook behavior in before calling a method
     - Catch - add custom error handling behavior around calls
     - ChangeArguments - Intercept and change arguments with the given function
     - ChangeReturnValue - Intercept and change return value with the given function

### Selectors (change what gets intercepted)
     - ByArgumentType - filter by what argument types a function takes
     - ByMethodName - filter by the name of the method being called
     - ByReturnType - filter by return type
