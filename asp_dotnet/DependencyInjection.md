# Dependency Injection

For constructor injection, use Microsoft depdency injection container.

Must be configured when the application starts using `ConfigureServices` method in Startup. 

> Any services(dependency) expected to be injected by the container must be registered to the DIC using `IServiceCollection`. The depndencies should be resolvable by `IServiceProvider`.


## How it works?
- HTTP request is received by the web server.
-

## Misc.
- Strings and primitive types should not be stored in a DIC. They are typically required for configuration.

- Value types are not supported for registration using the generic registration methods.

- Strings are reference types but they should be avoided and it is better practice to wrap them through strongly typed `Options pattern`.

## Service Lifetime
Calls GC when their lifetime is ended.

### Transient:
A new instance is created every time the service is resolved.
- Not required to be thread-safe. 
- Potentially less efficient. It has to be allocated for each request.
- Safest choice when it is not clear which lifetime to chose is not clear.
- Increases the load on GC.

### Singleton
One shared instance is created for the lifetime of the application.
- Generally more performant.
- Reduces load on GC.
- Must be thread-safe.
- Well suited to functional stateless services.
- Usage: Memory cache

### Scoped
An instance is created per scope (request)
- DbContext is scoped by default.

### Rule of thumb:
> A service should NOT depend on a service with a lifetime shorter than its own.

Singleton lifetimed service should not depend on a transient lifetimed service.

- Side effects:
1. Accidental sharing of non-thread-safe services between threads.
2. Objects living longer than their expected lifetime.

Scope validation is now default but not in production environment.

To activate:
```csharp
// PRogram.cs
public static IWebHostBuilder CreateWebHostBuilder(string[] args)=>
WebHost.CreateDefaultBuilder(args)
    .UseDefaultServiceProvider(options=>
    {
        options.ValidateScopes = true;
    })
    .UseStartup<Startup>();
```
But it has performance penalty.
