# DeliveryTracking SupportTicketId Enhancement

## ?? Summary

Added `SupportTicketId` (nullable `Guid`) property to `DeliveryTracking` entity to track the relationship between delivery trackings and support tickets.

## ? Changes Made

### 1. **Domain Layer** 
**File:** `src\Modules\Delivery\PuzKit3D.Modules.Delivery.Domain\Entities\DeliveryTrackings\DeliveryTracking.cs`

Added property:
```csharp
public Guid? SupportTicketId { get; private set; }
```

Updated constructor:
```csharp
private DeliveryTracking(
    DeliveryTrackingId id,
    Guid orderId,
    string deliveryOrderCode,
    DeliveryTrackingStatus status,
    DeliveryTrackingType type,
    DateTime expectedDeliveryDate,
    Guid? supportTicketId = null) : base(id)
{
    OrderId = orderId;
    SupportTicketId = supportTicketId;  // ? NEW
    // ... rest of initialization
}
```

Updated `Create` factory method:
```csharp
public static ResultT<DeliveryTracking> Create(
    Guid orderId,
    string deliveryOrderCode,
    DateTime expectedDeliveryDate,
    DeliveryTrackingType type = DeliveryTrackingType.Original,
    string? note = null,
    Guid? supportTicketId = null)  // ? NEW parameter
```

### 2. **Persistence Configuration**
**File:** `src\Modules\Delivery\PuzKit3D.Modules.Delivery.Persistence\Configurations\DeliveryTrackingConfiguration.cs`

Added configuration:
```csharp
// SupportTicketId - nullable, references SupportTicket module
builder.Property(dt => dt.SupportTicketId)
    .IsRequired(false);
```

Added index for queries:
```csharp
builder.HasIndex(dt => dt.SupportTicketId);
```

### 3. **Repository Interface**
**File:** `src\Modules\Delivery\PuzKit3D.Modules.Delivery.Application\Repositories\IDeliveryTrackingRepository.cs`

Added method:
```csharp
/// <summary>
/// Get delivery trackings by support ticket ID
/// </summary>
Task<List<DeliveryTracking>> GetBySupportTicketIdAsync(
    Guid supportTicketId,
    CancellationToken cancellationToken = default);
```

### 4. **Repository Implementation**
**File:** `src\Modules\Delivery\PuzKit3D.Modules.Delivery.Persistence\Repositories\DeliveryTrackingRepository.cs`

Implemented method:
```csharp
public async Task<List<DeliveryTracking>> GetBySupportTicketIdAsync(
    Guid supportTicketId,
    CancellationToken cancellationToken = default)
{
    return await _dbContext.DeliveryTrackings
        .Include(dt => dt.Details)
        .Where(dt => dt.SupportTicketId == supportTicketId)
        .OrderByDescending(dt => dt.CreatedAt)
        .ToListAsync(cancellationToken);
}
```

## ?? Use Cases

### Create DeliveryTracking from Support Ticket

```csharp
// When handling a replacement request from support ticket
var supportTicketId = Guid.Parse("..."); // From support ticket
var result = DeliveryTracking.Create(
    orderId: order.Id.Value,
    deliveryOrderCode: ghnResponse.OrderCode,
    expectedDeliveryDate: ghnResponse.ExpectedDeliveryDate,
    type: DeliveryTrackingType.Support,
    note: "Replacement for damaged item",
    supportTicketId: supportTicketId  // ? Link to support ticket
);
```

### Query Trackings by Support Ticket

```csharp
// Get all delivery trackings created from a specific support ticket
var trackings = await _repository.GetBySupportTicketIdAsync(supportTicketId);

foreach (var tracking in trackings)
{
    Console.WriteLine($"Support Tracking: {tracking.DeliveryOrderCode}");
    Console.WriteLine($"Status: {tracking.Status}");
    Console.WriteLine($"Items: {tracking.GetTotalQuantity()}");
}
```

## ?? Database Schema Impact

**New column in `DeliveryTrackings` table:**
```sql
SupportTicketId UNIQUEIDENTIFIER NULL,
```

**New index:**
```sql
CREATE INDEX IX_DeliveryTracking_SupportTicketId ON [delivery].[DeliveryTrackings]([SupportTicketId]);
```

## ?? Relationships

```
SupportTicket (SupportTicket Module)
    ?
    ??? DeliveryTracking.SupportTicketId (Delivery Module)
            ?? Type: Support
            ?? Items: Replacement/Resend items
            ?? Status: Track delivery of replacement
```

**Characteristics:**
- `SupportTicketId` is **nullable** (not all trackings are from support tickets)
- Only trackings with `Type = Support` should have `SupportTicketId` set
- Multiple trackings can reference the same support ticket (if multiple shipments needed)
- No foreign key constraint (loose coupling between modules)

## ?? Migration Steps

1. **Create EF Core Migration:**
   ```powershell
   dotnet ef migrations add AddSupportTicketIdToDeliveryTracking `
     --project src/Modules/Delivery/PuzKit3D.Modules.Delivery.Persistence `
     --startup-project src/WebApi/PuzKit3D.WebApi
   ```

2. **Review Migration** (should add column and index)

3. **Apply Migration:**
   ```powershell
   dotnet ef database update `
     --project src/Modules/Delivery/PuzKit3D.Modules.Delivery.Persistence `
     --startup-project src/WebApi/PuzKit3D.WebApi
   ```

## ? Benefits

? **Traceability** - Know which support ticket triggered which delivery
? **Cross-Module Link** - Connect Delivery and SupportTicket modules
? **Flexible** - Not all trackings need a support ticket
? **Queryable** - Can get all trackings for a specific support ticket
? **Auditable** - Full history of replacements from support tickets

## ?? Usage Examples

### Example 1: Handling RMA (Return Merchandise Authorization)

```csharp
// Support ticket: Customer reports item damaged
var supportTicketId = supportTicket.Id;

// Create replacement shipment
var tracking = DeliveryTracking.Create(
    orderId: order.Id.Value,
    deliveryOrderCode: ghnResponse.OrderCode,
    expectedDeliveryDate: ghnResponse.ExpectedDeliveryTime,
    type: DeliveryTrackingType.Support,
    note: "RMA - Item damaged in shipping",
    supportTicketId: supportTicketId);

if (tracking.IsSuccess)
{
    var t = tracking.Value;
    
    // Add replacement items
    var item = DeliveryTrackingDetail.Create(
        t.Id,
        itemId: itemIdToReplace,
        quantity: 1);
    t.AddDetail(item);
    
    unitOfWork.DeliveryTrackings.Add(t);
    await unitOfWork.SaveChangesAsync();
}
```

### Example 2: Query All Replacements for Support Ticket

```csharp
public async Task<SupportTicketTrackingDto> GetSupportTicketDeliveryStatusAsync(
    Guid supportTicketId,
    CancellationToken ct)
{
    var trackings = await _repository.GetBySupportTicketIdAsync(supportTicketId, ct);
    
    return new SupportTicketTrackingDto
    {
        SupportTicketId = supportTicketId,
        TotalTrackings = trackings.Count,
        AllDelivered = trackings.All(t => t.IsCompleted()),
        Trackings = trackings.Select(t => new
        {
            t.DeliveryOrderCode,
            t.Status,
            t.Type,
            t.ExpectedDeliveryDate,
            t.DeliveredAt,
            ItemCount = t.GetTotalQuantity(),
            t.Note
        }).ToList()
    };
}
```

### Example 3: Dashboard - Track Support Ticket Resolution

```csharp
// Show customer: "Your replacement is on its way"
var trackings = await _repository.GetBySupportTicketIdAsync(ticketId);

var status = trackings switch
{
    [] => "Not yet processed",
    var t when t.All(x => x.Status == DeliveryTrackingStatus.ReadyToPick) 
        => "Ready for pickup",
    var t when t.All(x => x.Status == DeliveryTrackingStatus.Shipped) 
        => "On the way",
    var t when t.All(x => x.IsCompleted()) 
        => "Delivered",
    _ => "In progress"
};

return new { status, tracking = trackings.First() };
```

## ?? Data Integrity

**Validations:**
- `SupportTicketId` is optional - no required constraint
- Type should be `Support` when `SupportTicketId` is not null
- Multiple trackings can share same `SupportTicketId`
- No cascading delete (if support ticket deleted, tracking remains)

**Best Practices:**
```csharp
// ? Good - Normal original order delivery
DeliveryTracking.Create(orderId, code, date, DeliveryTrackingType.Original);

// ? Good - Support replacement with ticket reference
DeliveryTracking.Create(orderId, code, date, DeliveryTrackingType.Support, 
    supportTicketId: ticketId);

// ?? Inconsistent - Support type without ticket (should have ticket)
DeliveryTracking.Create(orderId, code, date, DeliveryTrackingType.Support);

// ?? Inconsistent - Original type with ticket (don't need ticket for original)
DeliveryTracking.Create(orderId, code, date, DeliveryTrackingType.Original,
    supportTicketId: ticketId);
```

## ?? Checklist

- [x] Domain entity updated
- [x] Factory method updated
- [x] Constructor updated
- [x] Persistence configuration added
- [x] Index added for performance
- [x] Repository interface updated
- [x] Repository implementation added
- [ ] EF Core migration created
- [ ] Migration applied to database
- [ ] Tests updated
- [ ] API documentation updated

---

**Ready for migration!** ??
