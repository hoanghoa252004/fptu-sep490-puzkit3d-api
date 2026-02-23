```text
src/
 ├── WebApi/                === Host ===
 │     ├── PuzKit3D.WebApi/
 │     │      ├── Extensions/ : Migration, Swagger
 │     │      ├── Middlewares/
 │     │      └── Program.cs
 │
 ├── SharedKernel/          === Business concern ===
 │     ├── PuzKit3D.SharedKernel.Domain/
 │     │      ├── Entity.cs
 │     │      ├── AggregateRoot.cs
 │     │      ├── ValueObject.cs
 │     │      ├── StronglyTypedId.cs
 │     │      ├── IDomainEvent.cs
 │     │      └── DomainEventDispatcher.cs
 │     │      ├── Result/
 │     │      │      ├── Result.cs
 │     │      │      ├── ResultT.cs
 │     ├── PuzKit3D.SharedKernel.Application/
 │     │      ├── Authorization/
 │     │      ├── Behaviors/
 │     │      ├── Clock/  : IDateTimeProvider  ***
 │     │      ├── Data/ : IDbConnectionFactory & IUnitOfWork    ***
 │     │      ├── Message/
 │     │      │       ├── Command/ : ICommand & ICommandHandler
 │     │      │       ├── Query/ : IQuery & IQueryHandler
 │     │      ├── User/ : ICurrentUser
 │     │ 
 │     ├── PuzKit3D.SharedKernel.Infrastructure/
 │     │      ├── BaseDbContext.cs ****????
 │     ├── PuzKit3D.SharedKernel.Infrastructure/
 │     │      ├── Authorization/
 │     │      ├── PermissionService.cs
 │     │      └── IPermissionProvider.cs
 │     │      ├── Clock/
 │     │      │   └── SystemDateTimeProvider.cs
 │     │      ├── Data/
 │     │      │   ├── DbConnectionFactory.cs
 │     │      │   ├── BaseDbContext.cs
 │     │      │   └── IAuditableEntity.cs
 │     │      ├── User/
 │     │      │   └── CurrentUser.cs
 │     │      ├── Interceptors/
 │     │      │   └── DomainEventDispatcherInterceptor.cs
 │     │      ├── Testing/
 │     │      │   └── FakeDateTimeProvider.cs
 │     │      ├── DependencyInjection/
 │     │      │   └── Extensions/
 │     │      │       └── ServiceCollectionExtensions.cs
 │     
 ├── Modules/               === Bounded context ===
 │     ├── InstockOrdering/
 │     │      ├── Domain
 │     │      │       ├── Entities/
 │     │      │       ├── ValueObjects/
 │     │      │       ├── AggregateRoots/
 │     │      │       └── Repositories/
 │     │      ├── Application/
 │     │      │       ├── DependencyInjection/
 │     │      │       │     ├── Extensions/
 │     │      │       │     └── Options/
 │     │      │       └── UseCases/
 │     │      ├── Persistence/
 │     │      │       ├── Configurations/
 │     │      │       ├── Constants/
 │     │      │       ├── DependencyInjection/
 │     │      │       │     ├── Extensions/
 │     │      │       │     └── Options/
 │     │      │       ├── Migrations/
 │     │      │       └── Repositories/
 │     │      ├── Api/
 │     │      │       ├── .../
 │     │      │       └── .../
 │     ├── PartnerOrdering/
 │     ├── CustomDesign/
 │     ├── Support/
 │     ├── Wallet/ 
 │     ├── User/
 │     ├── Chat/ 


 │
 ├── Infrastructure/               === Cross-cutting concern ===
 │     ├── Authentication/
 │     ├── Authorization/
 │     ├── Clock/
 │     ├── Data/
 │     │      ├── IDbConnectionFactory.cs
 │     ├── Identity/
 │     │      ├── ApplicationUser.cs
 │     │      ├── IdentityConfigurations.cs
 │     │      ├── IdentityDbContext.cs
 │     ├── Notifications/
 │     │     ├── EmailSender.cs
 │     │     ├── PushNotificationService.cs
```


