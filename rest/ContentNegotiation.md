# Content Negotiation
The processs of selecting the best representation for a given response when there are multiple representations.

- Client: The client has to pass this withing the Accept Header

## BEst practices:
- If server(API) does not support the client's requested format do NOT return the default representation.
    - return 406
    - Always include Accept-Header to specify which formats are accepted.

## AspNet
By default, Asp.Net is returning the default format when a unsupported accept-header(format) is requested. To disable this:

```csharp
//Startup.cs in ConfigureServices
services
    .AddControllers(setupAction => {setupAction.ReturnHttpNotAcceptable = true; }); // This will ensure 406 is returned
    .AddXmlDataContractSerializerFormatters(); //To add Xml formatting for output as not default but possibility

// Naive wway (//To add Xml formatting for output as not default but possibility)
services.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

```

- Accept-Header: Is the way of telling the api what reppresentation to rewturn in the response body
- Content-Type: Tells the api the format of the representation of the request body