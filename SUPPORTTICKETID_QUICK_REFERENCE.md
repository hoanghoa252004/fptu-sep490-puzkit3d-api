# DeliveryTracking + SupportTicketId - Quick Reference

## ?? Property Added

```csharp
public Guid? SupportTicketId { get; private set; }
```

**Location:** `DeliveryTracking` entity  
**Type:** `Guid?` (Nullable GUID)  
**Usage:** Links delivery tracking to support ticket  
**When set:** Only for trackings created from support tickets (Type = Support)

---

## ?? Constructor & Factory Method

### Private Constructor
```csharp
private DeliveryTracking(
    DeliveryTrackingId id,
    Guid orderId,
    string deliveryOrderCode,
    DeliveryTrackingStatus status,
    DeliveryTrackingType type,
    DateTime expectedDeliveryDate,
    Guid? supportTicketId = null)  // ? NEW PARAMETER
```

### Create Method
```csharp
public static ResultT<DeliveryTracking> Create(
    Guid orderId,
    string deliveryOrderCode,
    DateTime expectedDeliveryDate,
    DeliveryTrackingType type = DeliveryTrackingType.Original,
    string? note = null,
    Guid? supportTicketId = null)  // ? NEW PARAMETER
```

---

## ?? How to Use

### Create Original Order Delivery (no support ticket)
```csharp
DeliveryTracking.Create(
    orderId: order.Id.Value,
    deliveryOrderCode: "GHN-12345",
    expectedDeliveryDate: DateTime.UtcNow.AddDays(3),
    type: DeliveryTrackingType.Original);
    // supportTicketId: null (default)
```

### Create Support/Replacement Delivery (with support ticket)
```csharp
DeliveryTracking.Create(
    orderId: order.Id.Value,
    deliveryOrderCode: "GHN-67890",
    expectedDeliveryDate: DateTime.UtcNow.AddDays(2),
    type: DeliveryTrackingType.Support,
    note: "Replacement for damaged item",
    supportTicketId: supportTicket.Id);  // ? Link to ticket
```

---

## ?? Query Methods

### Get by Support Ticket ID
```csharp
var trackings = await _repository.GetBySupportTicketIdAsync(supportTicketId);

// Returns all delivery trackings created from this support ticket
foreach (var t in trackings)
{
    Console.WriteLine($"{t.DeliveryOrderCode}: {t.Status}");
}
```

---

## ?? Database Schema

**New column:**
```sql
SupportTicketId UNIQUEIDENTIFIER NULL
```

**New index for performance:**
```sql
CREATE INDEX IX_DeliveryTracking_SupportTicketId 
    ON [delivery].[DeliveryTrackings]([SupportTicketId]);
```

---

## ? Key Points

| Aspect | Details |
|--------|---------|
| **Nullable** | ? Yes - not all trackings have a support ticket |
| **Unique** | ? No - multiple trackings can reference same ticket |
| **Foreign Key** | ? No - loose coupling between modules |
| **Index** | ? Yes - for fast queries by support ticket |
| **Required** | ? No - optional parameter |
| **Default** | `null` |

---

## ?? When to Use

| Scenario | Type | SupportTicketId |
|----------|------|-----------------|
| Original order shipment | Original | null |
| Item damaged - replacement | Support | ticket ID |
| Backorder - late shipment | Original | null |
| Resend after return | Support | ticket ID |
| Item defective - resend | Support | ticket ID |

---

## ?? Files Changed

1. ? `DeliveryTracking.cs` - Added property + factory parameter
2. ? `DeliveryTrackingConfiguration.cs` - Added column config + index
3. ? `IDeliveryTrackingRepository.cs` - Added query method
4. ? `DeliveryTrackingRepository.cs` - Implemented query method

---

## ?? Next Steps

1. Create EF Core migration
2. Apply migration to database
3. Update tests if needed
4. Ready to use!

```powershell
# Create migration
dotnet ef migrations add AddSupportTicketIdToDeliveryTracking `
  --project src/Modules/Delivery/PuzKit3D.Modules.Delivery.Persistence `
  --startup-project src/WebApi/PuzKit3D.WebApi

# Apply migration
dotnet ef database update `
  --project src/Modules/Delivery/PuzKit3D.Modules.Delivery.Persistence `
  --startup-project src/WebApi/PuzKit3D.WebApi
```

---

## ?? Pro Tips

? **Always set Type = Support when setting SupportTicketId**
? **Check Type before accessing SupportTicketId in code**
? **Use GetBySupportTicketIdAsync() for queries**
? **Document which support ticket created the tracking**

```csharp
// ? Good pattern
if (tracking.Type == DeliveryTrackingType.Support && tracking.SupportTicketId.HasValue)
{
    // Handle support-related tracking
}

// ? Also good - let DB handle it via index
var supportTrackings = await _repo.GetBySupportTicketIdAsync(ticketId);
```

---

**That's it!** Simple, clean, and effective. ??
