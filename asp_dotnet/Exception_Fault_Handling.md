# Exception(Fault) Handling

It is always best practice not to resturn the call stack when an exception is thrown and custom adjustments might be required to improve this.

```csharp
// Startup.cs
// Middleware
public void Confiugure(IApplicationBuilder app, IWebHostEnvironment env)
{
    if(env.IsDevelopment()) app.UseDeveloperExceptionPage();
    else
    {
        // Choose one of the ways

        // 1. Default
        app.UseExceptionHandler();

        // 2. Custom
        app.UseExceptionHandler(appBuilder =>
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An unexpected fault happened. Try again later");
        });
    }
}
```
> This is also good place to log stuff.