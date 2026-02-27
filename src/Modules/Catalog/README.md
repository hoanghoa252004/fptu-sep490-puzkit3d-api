# Catalog Module - Domain-Driven Design Structure

## ?? Module Structure

Module Catalog ???c xây d?ng theo Domain-Driven Design (DDD) pattern v?i các layer:

- **Domain**: Entities, Value Objects, Strongly Typed IDs, Domain Events
- **Application**: Use Cases, DTOs, Interfaces (UnitOfWork)
- **Persistence**: EF Core Configurations, Repositories, DbContext, Migrations
- **Infrastructure**: Module Bootstrap, Dependency Injection
- **Api**: API Endpoints

## ??? Domain Layer

### Entities & Aggregates

Module Catalog qu?n lý 4 aggregate roots chính:

1. **AssemblyMethod** - Ph??ng pháp l?p ráp
2. **Topic** - Ch? ?? (có c?u trúc phân c?p v?i parent_id)
3. **Material** - Ch?t li?u
4. **Capability** - Kh? n?ng/Tính n?ng

### Strongly Typed IDs

M?i entity có Strongly Typed ID riêng:
- `AssemblyMethodId`
- `TopicId`
- `MaterialId`
- `CapabilityId`

### Error Classes

M?i entity có Error class riêng ?? x? lý domain errors:
- `AssemblyMethodError`
- `TopicError`
- `MaterialError`
- `CapabilityError`

### Repositories

Interface repositories trong Domain layer:
- `IAssemblyMethodRepository`
- `ITopicRepository` (có thêm `GetByParentIdAsync` cho hierarchical structure)
- `IMaterialRepository`
- `ICapabilityRepository`

## ?? Persistence Layer

### DbContext

`CatalogDbContext` - Entity Framework Core DbContext v?i:
- Default schema: `catalog`
- Transaction management v?i ExecuteAsync
- Domain events dispatching
- Implementation c?a `ICatalogUnitOfWork`

### Entity Configurations

Các configuration classes:
- `AssemblyMethodConfiguration`
- `TopicConfiguration`
- `MaterialConfiguration`
- `CapabilityConfiguration`

**??c ?i?m chung:**
- Mapping ?úng v?i PostgreSQL schema và table names (snake_case)
- Unique index trên `slug`
- Default value cho `is_active` = false
- Timestamp fields: `created_at`, `updated_at`

### Repository Implementations

Implementations c?a repository interfaces v?i:
- Synchronous methods t? `IRepositoryBase`
- Async methods cho custom queries (GetBySlugAsync, v.v.)
- Include/eager loading support

### Migrations

Migration `InitialCatalogCreate` t?o 4 tables:
- `catalog.assembly_method`
- `catalog.topic`
- `catalog.material`
- `catalog.capability`

## ?? Application Layer

### Unit of Work

`ICatalogUnitOfWork` interface cho transaction management:
- `ExecuteAsync<T>` - Execute action trong transaction
- `SaveChangesAsync` - Save changes

## ?? Infrastructure Layer

### Module Bootstrap

`CatalogModule` - Extension methods ?? register module:
```csharp
services.AddCatalogModule(configuration);
```

## ?? Usage

### Run Migration

```bash
cd src\Modules\Catalog\PuzKit3D.Modules.Catalog.Persistence
dotnet ef migrations add <MigrationName> --context CatalogDbContext
dotnet ef database update --context CatalogDbContext
```

### Register Module in Program.cs

```csharp
builder.Services.AddCatalogModule(builder.Configuration);
```

## ? Completed

- ? Strongly Typed IDs for all entities
- ? Entity classes with DDD patterns
- ? Error handling classes
- ? Repository interfaces
- ? Repository implementations
- ? EF Core configurations
- ? DbContext with UnitOfWork
- ? Initial migration created and applied
- ? DependencyInjection setup
- ? Module bootstrap

## ?? Next Steps

1. Create use cases (Commands/Queries) in Application layer
2. Create API endpoints in Api layer
3. Add domain events if needed
4. Add validation for business rules
5. Add integration tests
