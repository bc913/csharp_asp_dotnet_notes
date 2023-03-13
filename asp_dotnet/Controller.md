# Controllers

Inheriting from `Controller` will also bring support for views so if you tend to build an API only inheriting from `ControllerBase` is a better approach.

- When you use `ApiController` attribute for a controller you must use `Routing` attribute on your actions.

## Actions
These are the methods that handles the incoming request through defined endpoint uri. Usually returns `ActionResult<T>`.
```csharp
public ActionResult<IEnumerable<Models.Author>> GetAuthors(string mainCategory)
{

}
```
### Arguments
- Complex type: 
- Simple type: 

#### Default behavior for action arguments

- **Simple Type**: If the argument of the type is not complex
Thanks to the `ApiController` attribute, the arguments qualified w/o any `From...` attribute, the following rules apply for simple types unless otherwise specified:

1. The argument is resolved for any matching parameter in the route so qualified implicitly as `[FromRoute]`.
2. If no matching route parameter is found, it will be trying to resolve against any matching Query parameter so qualified as `[FromQuery]` implicitly.

```csharp
// Default
// Looks for matching Route parameter first and then Query parameter
public ActionResult<IEnumerable<Models.Author>> GetAuthors(string mainCategory)
{

}
```

- **Complex Types**: Each of the following qualifies for complex type:
1. One or more fields wrapped with a class
2. IFileForm
3. Collection types

If that is the case, for the arguments w`[ApiController]` attribute assumes the complex argument is coming from the request body so implicitly qualified as `[FromBody]`.

```csharp
// AuthorsResourceParameter.cs
namespace CourceLibrary.API.ResourceParameters
{
    public class Authors
    {
        string mainCategory { get; set;}
        string searchQuery { get; set;}
    }
}

// AuthorsController.cs
// 
public ActionResult<IEnumerable<Models.Author>> GetAuthors([FromQuery]ResourceParameters.Authors arg)
{
    var authors = _repo.GetAuthors(arg.mainCategory, arg.searchQuery);
    return Ok(_mapper.Map<IEnumerable<Models.Author>>(authors));
}
```
> In order to avoid default behavior, always qualify the argument with a `[From...]` attribute.

#### Query arguments
```csharp
// Query par
// If names are matching already no need for Name argument for the attribute
public ActionResult<IEnumerable<Models.Author>> GetAuthors([FromQuery(Name="some_q_par_in_the query")] string mainCategory)
{

}
```
## Routing
Matches a request URI to an action on a controller. You have to adjust the middleware accordingly by defining the following two middleware setup.
- app.UseRouting(): Marks the position in the middleware pipeline where a routing decision is made. (Selection)
- app.UseEndpoints(): Marks the position in the middleware pipeline where the selected endpoint is executed. (Execution)

This provides a capability of injecting other middlewares required between these middlewares i.e. Authorization.

There are two styles of endpoint routing:
1. Convention based routing
2. Attribute-based routing
```csharp
app.UseEndpoints(endpoints=> {endpoints.MapControllers(); });
```
## Seperation of Entities from outer facing models
It is always a better and secure way to expose data to the client using model(dto or view models) classes.
Exposing the entity to the user has the following drawbacks:
- Security: Exposing the whole data structure of your entity gives the whole idea of how you store data in your application and data store.
- Heavyweight: It usually carries more information than the client needs.
- Simplicity: Some data types in the entity are overly complex to represent to the client.
### Automapper
Add Automapper.Extensions.DependencyInjection package.
1. Inject it using dependency injections, register
```csharp
// startup.cs
public void ConfigureServices(IServiceCollection services)
{   //...
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());//to scan profiles for mapping configuration
}
```
2. Define profiles. Profile neat way to orginaze mapping configuration
```csharp
public class AuthorsProfile : AutoMapper.Profile
{
    // Put a default ctor
    public AuthorsProfile()
    {
        // Choose one of them

        // 1. This is for flatten the source to destination w/o any custom conversion/projection
        CreateMap<Entities.Author, Models.Author>();
        // 2. If you want to 
        CreateMap<Entities.Author, Models.Author>();
            .ForMember(
                dest => dest.Name, // for member "Name" of the destination object
                opt => opt.MapFrom(src=>$"{src.FirstName} {src.LastName}") // Map from 
            )
            .ForMember(
                dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge())
            );


    }
}
```
3. Inject it to the controller
```csharp
public class AuthorsController : ControllerBase
{
    private readonly IMapper _mapper;
    public AuthorsController(IMapper mapper)
    {
        _mapper = mapper;
    }
}
```



## Creating Resources
Using seperate Model is a better practices. This inlines with Clean Architecture and Domain Driven Design.

1. Seperate Model
```csharp
namespace CourceLibrary.API.Models
{
    public class AuthorCreation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTimeOffset DateOfBirth { get; set;}
        public string MainCategory { get; set;}
    }
}
```

2.  Update the profile
```csharp
// AuthorsProfile.cs
{
// in the ctor
//...
CreateMap<Models.AuthorCreation, Entities.Author>();
//...
}
```

3. Define Controller action
```csharp
[HttpPost]
public ActionResult<Models.Author> CreateAuthor(Models.AuthorCreation arg)
{
    var authorEntity = _mapper.Map<Entities.Author>(arg);
    _repo.AddAuthor(authorEntity); // it is added to dbcontext session not the db itself
    _repo.Commit(); // now it is saved to the database

    var result = _mapper.Map<Models.Author>(authorEntity);
    return CreatedAtRoute("GetAuthor", new { authorId = result.Id }, result);
}
```
### Creating child resource

### Creating parent - child resource together

### Creating multiple resources
Create another controller with different route to support multiple resource creation. Do not use the same controller for creation a single resource.

### POSTing to a single resource instead of collection of resources
This should not be allowed and this is achieved by default and it returns 415 Method not allowed.


## Enhancing Response with PRoblem Detail
There is a std for to describe details about the problem in the response body. This can be used for common errors.