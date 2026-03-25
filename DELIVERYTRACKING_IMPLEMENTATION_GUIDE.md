# DeliveryTracking Schema Implementation Guide

## ?? Overview

This document describes the implementation of `DeliveryTracking` and `DeliveryTrackingDetail` entities in the Delivery Module. These entities track the delivery status of shipments from orders.

## ??? Architecture

### Entities Structure

```
Delivery Module
??? Domain/
?   ??? Entities/
?       ??? DeliveryTrackings/
?           ??? DeliveryTracking.cs (Aggregate Root)
?           ??? DeliveryTrackingDetail.cs (Value Object)
?           ??? DeliveryTrackingId.cs (Strong-typed ID)
?           ??? DeliveryTrackingStatus.cs (Enum)
?           ??? DeliveryTrackingType.cs (Enum)
?
??? Application/
?   ??? Repositories/
?   ?   ??? IDeliveryTrackingRepository.cs
?   ??? UnitOfWork/
?       ??? IDeliveryUnitOfWork.cs
?
??? Persistence/
    ??? DeliveryDbContext.cs
    ??? Configurations/
    ?   ??? DeliveryTrackingConfiguration.cs
    ?   ??? DeliveryTrackingDetailConfiguration.cs
    ??? Repositories/
    ?   ??? DeliveryTrackingRepository.cs
    ??? UnitOfWork/
    ?   ??? DeliveryUnitOfWork.cs
    ??? Migrations/
        ??? DeliveryTracking_InitialCreate.sql
```

## ?? Database Schema

### DeliveryTrackings Table

```sql
CREATE TABLE [delivery].[DeliveryTrackings] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY,
    [OrderId] UNIQUEIDENTIFIER NOT NULL,
    [DeliveryOrderCode] NVARCHAR(100) NOT NULL UNIQUE,
    [Status] INT NOT NULL,  -- ReadyToPick, Picked, Shipping, Delivered, Return, Returned
    [Type] INT NOT NULL,    -- Original, Support
    [Note] NVARCHAR(1000) NULL,
    [ExpectedDeliveryDate] DATETIME2 NOT NULL,
    [DeliveredAt] DATETIME2 NULL,
    [CreatedAt] DATETIME2 NOT NULL,
    [UpdatedAt] DATETIME2 NOT NULL
)
```

### DeliveryTrackingDetails Table

```sql
CREATE TABLE [delivery].[DeliveryTrackingDetails] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY,
    [DeliveryTrackingId] UNIQUEIDENTIFIER NOT NULL (FK),
    [ItemId] UNIQUEIDENTIFIER NOT NULL,          -- Can be VariantId, PartId, etc.
    [Quantity] INT NOT NULL,
    [CreatedAt] DATETIME2 NOT NULL,
    [UpdatedAt] DATETIME2 NOT NULL
)
```

## ?? Key Enums

### DeliveryTrackingStatus

```csharp
public enum DeliveryTrackingStatus
{
    ReadyToPick = 0,  // S?n sàng ?? pickup
    Picked = 1,       // ?ã ???c pickup
    Shipping = 2,     // ?ang v?n chuy?n
    Delivered = 3,    // ?ã giao hàng ?
    Return = 4,       // Yêu c?u tr? l?i
    Returned = 5      // ?ã tr? l?i
}
```

### DeliveryTrackingType

```csharp
public enum DeliveryTrackingType
{
    Original = 0,     // Shipment ban ??u t? order
    Support = 1       // Shipment t? support ticket (replacement, resend, etc.)
}
```

## ?? Domain Methods

### DeliveryTracking

#### Create (Factory Method)

```csharp
var result = DeliveryTracking.Create(
    orderId: Guid.Parse("..."),
    deliveryOrderCode: "GHN-12345",
    expectedDeliveryDate: DateTime.UtcNow.AddDays(3),
    type: DeliveryTrackingType.Original,
    note: null);

if (result.IsSuccess)
{
    var tracking = result.Value;
    // Use tracking...
}
```

#### Status Transitions

```csharp
// ReadyToPick ? Picked
var result1 = tracking.MarkAsPicked();

// Picked ? Shipping
var result2 = tracking.MarkAsShipping();

// Shipping ? Delivered
var result3 = tracking.MarkAsDelivered(deliveredAt: DateTime.UtcNow);

// Any ? Return
var result4 = tracking.MarkAsReturn(reason: "Customer refused");

// Return ? Returned
var result5 = tracking.MarkAsReturned(returnedAt: DateTime.UtcNow);
```

#### Add Items

```csharp
// Add single detail
var detail = DeliveryTrackingDetail.Create(
    deliveryTrackingId: tracking.Id,
    itemId: Guid.Parse("..."), // VariantId, PartId, etc.
    quantity: 2);

var result = tracking.AddDetail(detail);

// Add multiple details at once
var details = new[]
{
    DeliveryTrackingDetail.Create(tracking.Id, itemId1, qty1),
    DeliveryTrackingDetail.Create(tracking.Id, itemId2, qty2),
    DeliveryTrackingDetail.Create(tracking.Id, itemId3, qty3)
};

var result = tracking.AddDetails(details);
```

#### Other Methods

```csharp
// Get total quantity
int totalQty = tracking.GetTotalQuantity(); // Sum of all details.Quantity

// Check if completed
bool isCompleted = tracking.IsCompleted(); // Delivered or Returned

// Update note
tracking.UpdateNote("GHN returned to sender - address invalid");
```

## ?? Repository Usage

### IDeliveryTrackingRepository

```csharp
public interface IDeliveryTrackingRepository
{
    // By ID
    Task<DeliveryTracking?> GetByIdAsync(DeliveryTrackingId id, ...);
    
    // By GHN order code (unique)
    Task<DeliveryTracking?> GetByDeliveryOrderCodeAsync(string deliveryOrderCode, ...);
    
    // For an order
    Task<List<DeliveryTracking>> GetByOrderIdAsync(Guid orderId, ...);
    
    // By status
    Task<List<DeliveryTracking>> GetByStatusAsync(DeliveryTrackingStatus status, ...);
    
    // By type
    Task<List<DeliveryTracking>> GetByTypeAsync(DeliveryTrackingType type, ...);
    
    // For multiple orders
    Task<List<DeliveryTracking>> GetByOrderIdsAsync(IEnumerable<Guid> orderIds, ...);
    
    // Modify
    void Add(DeliveryTracking deliveryTracking);
    void Update(DeliveryTracking deliveryTracking);
    void Delete(DeliveryTracking deliveryTracking);
    
    // Check
    Task<bool> ExistsAsync(string deliveryOrderCode, ...);
}
```

### Example Queries

```csharp
// Get all trackings for an order
var trackings = await _repository.GetByOrderIdAsync(orderId);

// Get by GHN code
var tracking = await _repository.GetByDeliveryOrderCodeAsync("GHN-12345");

// Get all shipping (in progress)
var shippingTrackings = await _repository.GetByStatusAsync(DeliveryTrackingStatus.Shipping);

// Check if GHN code exists
bool exists = await _repository.ExistsAsync("GHN-12345");
```

## ?? Unit of Work

### IDeliveryUnitOfWork

```csharp
public interface IDeliveryUnitOfWork
{
    IDeliveryTrackingRepository DeliveryTrackings { get; }
    
    Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
```

### Usage Pattern

```csharp
// In Application Handler
public async Task<Result> Handle(CreateDeliveryTrackingCommand request, ...)
{
    // Create entity
    var trackingResult = DeliveryTracking.Create(...);
    if (trackingResult.IsFailure)
        return Result.Failure(trackingResult.Error);
    
    var tracking = trackingResult.Value;
    
    // Add items
    foreach (var item in items)
    {
        var detail = DeliveryTrackingDetail.Create(tracking.Id, item.ItemId, item.Qty);
        tracking.AddDetail(detail);
    }
    
    // Persist with transaction support
    await _unitOfWork.ExecuteAsync(async () =>
    {
        _unitOfWork.DeliveryTrackings.Add(tracking);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }, cancellationToken);
    
    return Result.Success();
}
```

## ?? State Transition Diagram

```
ReadyToPick
    ?
  Picked
    ?
 Shipping
    ?
Delivered ?
    
OR from Shipped/Delivered:
    ?
  Return
    ?
 Returned ?
```

## ?? Common Scenarios

### Scenario 1: Track Original Order Delivery

```csharp
// Create tracking when GHN order is created
var tracking = DeliveryTracking.Create(
    orderId: order.Id.Value,
    deliveryOrderCode: ghnResponse.OrderCode,
    expectedDeliveryDate: ghnResponse.ExpectedDeliveryDate,
    type: DeliveryTrackingType.Original);

// Add items from order
foreach (var detail in order.OrderDetails)
{
    var item = DeliveryTrackingDetail.Create(
        tracking.Value.Id,
        detail.Id.Value,
        detail.Quantity);
    tracking.Value.AddDetail(item);
}

// Save
_unitOfWork.DeliveryTrackings.Add(tracking.Value);
await _unitOfWork.SaveChangesAsync();
```

### Scenario 2: Track Replacement Shipment

```csharp
// Create support shipment
var tracking = DeliveryTracking.Create(
    orderId: order.Id.Value,
    deliveryOrderCode: ghnResponse.OrderCode,
    expectedDeliveryDate: ghnResponse.ExpectedDeliveryDate,
    type: DeliveryTrackingType.Support,
    note: "Replacement for damaged item A");

// Add only replacement items
var replacementItem = DeliveryTrackingDetail.Create(
    tracking.Value.Id,
    itemId: itemAId,
    quantity: 1);
tracking.Value.AddDetail(replacementItem);

_unitOfWork.DeliveryTrackings.Add(tracking.Value);
await _unitOfWork.SaveChangesAsync();
```

### Scenario 3: Update Delivery Status (from GHN Webhook)

```csharp
// Get tracking by GHN order code
var tracking = await _repository.GetByDeliveryOrderCodeAsync(ghnOrderCode);

if (tracking != null)
{
    // Update status based on webhook status
    switch (ghnStatus)
    {
        case "ready_to_pick":
            tracking.MarkAsPicked();
            break;
        case "picking":
            // Already Picked, no change needed
            break;
        case "delivering":
            tracking.MarkAsShipping();
            break;
        case "delivered":
            tracking.MarkAsDelivered(deliveredAt);
            break;
        case "return":
            tracking.MarkAsReturn(reason: ghnNote);
            break;
    }
    
    // Update note with GHN tracking URL
    tracking.UpdateNote($"Tracking: {ghnTrackingUrl}");
    
    _repository.Update(tracking);
    await _unitOfWork.SaveChangesAsync();
}
```

### Scenario 4: Get All Trackings for Customer Order

```csharp
// Customer wants to see all shipments for their order
var trackings = await _repository.GetByOrderIdAsync(orderId);

var response = trackings.Select(t => new DeliveryTrackingDto
{
    Id = t.Id.Value,
    DeliveryOrderCode = t.DeliveryOrderCode,
    Status = t.Status.ToString(),
    Type = t.Type.ToString(),
    ExpectedDeliveryDate = t.ExpectedDeliveryDate,
    DeliveredAt = t.DeliveredAt,
    Note = t.Note,
    Items = t.Details.Select(d => new
    {
        ItemId = d.ItemId,
        Quantity = d.Quantity
    }).ToList()
}).ToList();

return response;
```

## ?? Key Constraints & Validations

1. **Unique DeliveryOrderCode** - GHN codes cannot be duplicated
2. **Status Transitions** - Only valid transitions allowed:
   - ReadyToPick ? Picked
   - Picked ? Shipping
   - Shipping ? Delivered
   - Any ? Return
   - Return ? Returned
3. **Quantity > 0** - Items quantity must be positive
4. **Expected Date in Future** - Cannot create with past dates
5. **OrderId Required** - Must reference a valid order
6. **DeliveryOrderCode Required** - Cannot be empty

## ?? Testing Tips

```csharp
// Test valid creation
[Fact]
public void DeliveryTracking_Create_WithValidData_ShouldSucceed()
{
    var result = DeliveryTracking.Create(
        orderId: Guid.NewGuid(),
        deliveryOrderCode: "GHN-12345",
        expectedDeliveryDate: DateTime.UtcNow.AddDays(3),
        type: DeliveryTrackingType.Original);
    
    Assert.True(result.IsSuccess);
    Assert.NotNull(result.Value);
}

// Test invalid creation
[Fact]
public void DeliveryTracking_Create_WithEmptyOrderId_ShouldFail()
{
    var result = DeliveryTracking.Create(
        orderId: Guid.Empty,
        deliveryOrderCode: "GHN-12345",
        expectedDeliveryDate: DateTime.UtcNow.AddDays(3));
    
    Assert.False(result.IsSuccess);
}

// Test status transition
[Fact]
public void DeliveryTracking_MarkAsPicked_FromReadyToPick_ShouldSucceed()
{
    var tracking = DeliveryTracking.Create(...).Value;
    var result = tracking.MarkAsPicked();
    
    Assert.True(result.IsSuccess);
    Assert.Equal(DeliveryTrackingStatus.Picked, tracking.Status);
}
```

## ?? Next Steps

1. ? Create EF Core migration: `dotnet ef migrations add AddDeliveryTracking`
2. ? Update database: `dotnet ef database update`
3. Create handlers for:
   - CreateDeliveryTrackingCommand
   - UpdateDeliveryTrackingStatusCommand
   - GetDeliveryTrackingQuery
4. Create API endpoints:
   - POST /delivery/trackings (create)
   - PUT /delivery/trackings/{id}/status (update status)
   - GET /delivery/trackings/{orderId} (get all for order)
5. Create webhook handler for GHN status updates

## ?? Related Entities

- **InstockOrder** (InStock Module) - Orders being tracked
- **SupportTicket** (SupportTicket Module) - Support-type trackings come from support tickets
- **GHN Integration** (Delivery Infrastructure) - External delivery partner API

