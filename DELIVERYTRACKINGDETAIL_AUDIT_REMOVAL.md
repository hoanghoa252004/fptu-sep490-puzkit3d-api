# DeliveryTrackingDetail - Removed Audit Timestamps

## ? Changes Made

Removed `CreatedAt` and `UpdatedAt` properties from `DeliveryTrackingDetail` entity as they are not needed.

### **Files Updated:**

1. ? **`DeliveryTrackingDetail.cs`** (Domain)
   - ? Removed: `public DateTime CreatedAt { get; private set; }`
   - ? Removed: `public DateTime UpdatedAt { get; private set; }`
   - ? Removed: Initialization in constructor
   - ? Removed: UpdatedAt update in `UpdateQuantity()` method

2. ? **`DeliveryTrackingDetailConfiguration.cs`** (Persistence)
   - ? Removed: CreatedAt column configuration
   - ? Removed: UpdatedAt column configuration

---

## ?? Current Schema

**DeliveryTrackingDetails table now has:**
```sql
Id UNIQUEIDENTIFIER PRIMARY KEY,
DeliveryTrackingId UNIQUEIDENTIFIER NOT NULL (FK),
ItemId UNIQUEIDENTIFIER NOT NULL,
Type INT NOT NULL,
Quantity INT NOT NULL,
```

**Removed columns:**
```sql
-- ? No longer exists
CreatedAt DATETIME2 NOT NULL
UpdatedAt DATETIME2 NOT NULL
```

---

## ?? Current Entity Structure

```csharp
public sealed class DeliveryTrackingDetail : Entity<Guid>
{
    public DeliveryTrackingId DeliveryTrackingId { get; private set; }
    public Guid ItemId { get; private set; }
    public DeliveryTrackingDetailType Type { get; private set; }
    public int Quantity { get; private set; }
    
    // Methods: Create(), CreateProduct(), CreatePart(), UpdateQuantity()
}
```

---

## ?? Note

If you need audit tracking for `DeliveryTrackingDetail`, it can be tracked at the `DeliveryTracking` aggregate level instead, which already has:
- `CreatedAt`
- `UpdatedAt`

When you update a detail (e.g., change quantity), the parent `DeliveryTracking.UpdatedAt` will be updated, providing overall audit trail for the shipment.

---

## ?? Done

The entity is now simplified and cleaner! ?
