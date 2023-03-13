# Filtering & Searching

## Filtering
Limiting a collection taking into account a predicate

## Searching
Adding matching items to the collection based on a predefined set of rules

## Rule of thumbs
- Filter and search options are NOT part of the resource so they should be passed as part of query string.
- Only allow filterig on fields that are part of the resource. Models (instead of entities) can be considered here as resource. ???

## How to implement?
Usually they go hand in hand.
- Always considered deferred execution thanks to the EF Core using `IQueryable<T>`

```csharp

// somerepository.cs
public IEnumerable<Entity.Author> GetAuthors(string mainCategory, string searchQuery)
{
    var collection = _dbContext.Authors as IQueryable<Models.Author>;

    // Do filtering using Where

    // Do searching where

    return collection.ToList(); // Now it will load to the memory.
}

// Somecontroller.cs
public ActionResult<IEnumerable<Models.Author>> GetAuthors(string mainCategory, string searchQuery)
{
    _
}
```

## Deferred Execution
Query execution occurs sometime after the query is constructed until the query is iterated over with one of the following ways:
- foreach
- ToList(), ToArray(), ToDictionary();
- Singleton queries

This means that it is always advised and safe to use `Where` which does not trigger an iteration and also provides filtering/searching capabilities.
