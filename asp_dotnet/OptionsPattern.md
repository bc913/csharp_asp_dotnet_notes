# Options pattern

- Define a POCO class to specify specific settings or options.
- Each property (field) will match the key of the configuration setting defined in the appsettings.json.

- You have to bind it with the corresponding option's POCO class 

- You can do this under DIC through ConfigureServices method like other dependeny injections.

- Use strongly typed injection.
- appsettings.json
```json
{
    "Features": {
        "EnableWeatherForecast" : true
    }
}
```

```csharp
public class FeaturesConfiguration
{
    public bool EnableWeatherForecast { get; set;}
}

// Startup.cs
public void ConfigureServices(IServiceCollection services)
{
    // Register other app related services

    services.Configure<FeaturesConfiguration>(Configuration.GetSection("Features"));
}

```

- After this point, you can inject `IOptions<T>` to any class needs to access.

```csharp
public class SomeController : ControllerBase
{
    public SomeController(ISomeDependency some_dep,
    IOptions<FeaturesConfiguration> options)
}
```