# InStock - Cart Module Event-Driven Communication Setup

## ? Phase 1: Completed - Domain Events & Integration Events

### ?? Domain Events Created (Instock Module)

#### InstockProductVariant Events
- ? `InstockProductVariantCreatedDomainEvent`
- ? `InstockProductVariantUpdatedDomainEvent`
- ? `InstockProductVariantActivatedDomainEvent`

#### InstockInventory Events
- ? `InstockInventoryCreatedDomainEvent`
- ? `InstockInventoryUpdatedDomainEvent`

#### InstockPrice Events
- ? `InstockPriceCreatedDomainEvent`
- ? `InstockPriceUpdatedDomainEvent`
- ? `InstockPriceActivatedDomainEvent`

#### InstockProductPriceDetail Events
- ? `InstockProductPriceDetailCreatedDomainEvent`
- ? `InstockProductPriceDetailUpdatedDomainEvent`
- ? `InstockProductPriceDetailActivatedDomainEvent`

### ?? Integration Events Created (Contract Project)
- ? `InstockProductVariantCreatedIntegrationEvent`
- ? `InstockProductVariantUpdatedIntegrationEvent`
- ? `InstockInventoryChangedIntegrationEvent`
- ? `InstockPriceChangedIntegrationEvent`
- ? `InstockProductPriceDetailChangedIntegrationEvent`

### ?? Domain Entities Updated to Raise Events
- ? `InstockProductVariant` - Raises events on Create/Update/Activate/Deactivate
- ? `InstockInventory` - Raises events on Create/AddStock/ReduceStock/SetStock
- ? `InstockPrice` - Raises events on Create/Update/Activate/Deactivate
- ? `InstockProductPriceDetail` - Raises events on Create/Update/Activate/Deactivate

### ?? Cart Module Replicas (Already Exists)
- ? `InStockProductVariantReplica`
- ? `InStockInventoryReplica`
- ? `InStockPriceReplica`
- ? `InStockProductPriceDetailReplica`

---

## ?? Phase 2: TODO - Event Handlers

### 1. Domain Event Handlers (Instock.Application Layer)

C?n t?o các Domain Event Handlers ?? publish Integration Events:

#### Location: `src/Modules/InStock/PuzKit3D.Modules.InStock.Application/DomainEventHandlers/`

**InstockProductVariant Handlers:**
```
InstockProductVariants/
??? InstockProductVariantCreatedDomainEventHandler.cs
??? InstockProductVariantUpdatedDomainEventHandler.cs
??? InstockProductVariantActivatedDomainEventHandler.cs
```

**InstockInventory Handlers:**
```
InstockInventories/
??? InstockInventoryCreatedDomainEventHandler.cs
??? InstockInventoryUpdatedDomainEventHandler.cs
```

**InstockPrice Handlers:**
```
InstockPrices/
??? InstockPriceCreatedDomainEventHandler.cs
??? InstockPriceUpdatedDomainEventHandler.cs
??? InstockPriceActivatedDomainEventHandler.cs
```

**InstockProductPriceDetail Handlers:**
```
InstockProductPriceDetails/
??? InstockProductPriceDetailCreatedDomainEventHandler.cs
??? InstockProductPriceDetailUpdatedDomainEventHandler.cs
??? InstockProductPriceDetailActivatedDomainEventHandler.cs
```

**Pattern:**
```csharp
public sealed class InstockProductVariantCreatedDomainEventHandler 
    : IDomainEventHandler<InstockProductVariantCreatedDomainEvent>
{
    private readonly IEventBus _eventBus;

    public InstockProductVariantCreatedDomainEventHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task Handle(
        InstockProductVariantCreatedDomainEvent domainEvent, 
        CancellationToken cancellationToken)
    {
        // Convert domain event to integration event
        var integrationEvent = new InstockProductVariantCreatedIntegrationEvent(
            domainEvent.Id,
            domainEvent.OccurredOn,
            domainEvent.VariantId,
            domainEvent.ProductId,
            domainEvent.Sku,
            domainEvent.Color,
            domainEvent.AssembledLengthMm,
            domainEvent.AssembledWidthMm,
            domainEvent.AssembledHeightMm,
            domainEvent.IsActive);

        // Publish to event bus
        await _eventBus.PublishAsync(integrationEvent, cancellationToken);
    }
}
```

### 2. Integration Event Handlers (Cart.Infrastructure Layer)

C?n t?o Integration Event Handlers trong Cart Module ?? sync data vào replicas:

#### Location: `src/Modules/Cart/PuzKit3D.Modules.Cart.Infrastructure/IntegrationEventHandlers/`

**Structure:**
```
InStock/
??? InstockProductVariantCreatedIntegrationEventHandler.cs
??? InstockProductVariantUpdatedIntegrationEventHandler.cs
??? InstockInventoryChangedIntegrationEventHandler.cs
??? InstockPriceChangedIntegrationEventHandler.cs
??? InstockProductPriceDetailChangedIntegrationEventHandler.cs
```

**Pattern:**
```csharp
public sealed class InstockProductVariantCreatedIntegrationEventHandler 
    : IIntegrationEventHandler<InstockProductVariantCreatedIntegrationEvent>
{
    private readonly CartDbContext _context;

    public InstockProductVariantCreatedIntegrationEventHandler(CartDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        InstockProductVariantCreatedIntegrationEvent integrationEvent, 
        CancellationToken cancellationToken)
    {
        // Create or update replica
        var replica = InStockProductVariantReplica.Create(
            integrationEvent.VariantId,
            integrationEvent.ProductId,
            integrationEvent.Sku,
            integrationEvent.Color,
            $"{integrationEvent.AssembledLengthMm}x{integrationEvent.AssembledWidthMm}x{integrationEvent.AssembledHeightMm}",
            integrationEvent.IsActive,
            integrationEvent.OccurredOn,
            integrationEvent.OccurredOn);

        _context.InStockProductVariantReplicas.Add(replica);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
```

### 3. Register Event Handlers

**In Instock.Application (DI Registration):**
```csharp
services.AddScoped<IDomainEventHandler<InstockProductVariantCreatedDomainEvent>, 
    InstockProductVariantCreatedDomainEventHandler>();
// ... register all domain event handlers
```

**In Cart.Infrastructure (DI Registration):**
```csharp
services.AddScoped<IIntegrationEventHandler<InstockProductVariantCreatedIntegrationEvent>, 
    InstockProductVariantCreatedIntegrationEventHandler>();
// ... register all integration event handlers
```

---

## ?? Event Flow

```
???????????????????????????????????????????????????????????????????
?                     INSTOCK MODULE                              ?
???????????????????????????????????????????????????????????????????
?                                                                 ?
?  1. Entity.RaiseDomainEvent()                                   ?
?     ?                                                           ?
?  2. Domain Event Handler (Application Layer)                    ?
?     ?                                                           ?
?  3. Publish Integration Event to EventBus                       ?
?                                                                 ?
???????????????????????????????????????????????????????????????????
                             ?
                             ? Event Bus (In-Memory/RabbitMQ)
                             ?
???????????????????????????????????????????????????????????????????
?                      CART MODULE                                ?
???????????????????????????????????????????????????????????????????
?                                                                 ?
?  4. Integration Event Handler (Infrastructure Layer)            ?
?     ?                                                           ?
?  5. Update Replica in CartDbContext                             ?
?     ?                                                           ?
?  6. SaveChangesAsync()                                          ?
?                                                                 ?
???????????????????????????????????????????????????????????????????
```

---

## ?? Implementation Checklist

### Phase 1: ? Completed
- [x] Create Domain Events in Instock.Domain
- [x] Update entities to raise Domain Events
- [x] Create Integration Events in Contract project
- [x] Verify Cart Module Replicas exist

### Phase 2: ?? TODO
- [ ] Create Domain Event Handlers in Instock.Application
- [ ] Create Integration Event Handlers in Cart.Infrastructure
- [ ] Register all handlers in DI containers
- [ ] Test event flow end-to-end

---

## ?? Key Points

1. **Domain Events** are raised in the Instock Module when entities change
2. **Domain Event Handlers** (Instock.Application) convert domain events to integration events
3. **Integration Events** are published via Event Bus (in-memory or message broker)
4. **Integration Event Handlers** (Cart.Infrastructure) receive events and update replicas
5. **Replicas** in Cart Module always stay in sync with Instock Module data

## ?? File Locations

**Instock Module:**
- Domain Events: `src/Modules/InStock/PuzKit3D.Modules.InStock.Domain/Events/`
- Domain Event Handlers: `src/Modules/InStock/PuzKit3D.Modules.InStock.Application/DomainEventHandlers/`

**Cart Module:**
- Integration Event Handlers: `src/Modules/Cart/PuzKit3D.Modules.Cart.Infrastructure/IntegrationEventHandlers/`
- Replicas: `src/Modules/Cart/PuzKit3D.Modules.Cart.Domain/Entities/Replicas/`

**Contract:**
- Integration Events: `src/Contract/PuzKit3D.Contract/InStock/`

---

## ?? Next Steps

1. Implement Domain Event Handlers in `Instock.Application`
2. Implement Integration Event Handlers in `Cart.Infrastructure`
3. Register handlers in DI
4. Test the complete flow
5. Verify replicas are updated correctly when Instock data changes
