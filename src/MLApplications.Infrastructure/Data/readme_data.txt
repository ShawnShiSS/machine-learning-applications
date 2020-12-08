This is where the persistence store will be configured.
Such as Repository, EF Core DbContext (if using it), Cached Repository, etc.

CosmosDb Repository is implemented here.

EF Core
- EF Core can work with CosmosDB as a database provider
- However, EF Core is not used here due to the limitations EF Core has on working with unstructured data
- see details: https://docs.microsoft.com/en-us/ef/core/providers/cosmos/unstructured-data
