# 1.0.0
Initial release, including the following:
### Initialization
    Interface to type
    Interface to object

### Interceptors (inject behavior)
    AfterInvoke - hook behavior in after calling a method
    BeforeInvoke - hook behavior in before calling a method
    Catch - add custom error handling behavior around calls
    ChangeArguments - Intercept and change arguments with the given function
    ChangeReturnValue - Intercept and change return value with the given function

### Selectors (change what gets intercepted)
    ByArgumentType - filter by what argument types a function takes
    ByMethodName - filter by the name of the method being called
    ByReturnType - filter by return type

# 1.0.1
Changed ByMethodName to be stackable
