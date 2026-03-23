# DeliveryTrackingDetail Type Enhancement

## ?? Summary

Added `Type` property to `DeliveryTrackingDetail` to distinguish between **Product** (VariantId) and **Part** (PartId) items in shipments.

## ? Changes Made

### 1. **New Enum**
**File:** `DeliveryTrackingDetailType.cs` (NEW)

```csharp
public enum DeliveryTrackingDetailType
{
    Product = 0,  // Product variant item
    Part = 1      // Part item (for assembly)
}
```

### 2. **Domain Entity Update**
**File:** `DeliveryTrackingDetail.cs`

Added property:
```csharp
/// <summary>
/// Lo?i item (Product hay Part)
/// </summary>
public DeliveryTrackingDetailType Type { get; private set; }
```

Added timestamps (for audit):
```csharp
public DateTime CreatedAt { get; private set; }
public DateTime UpdatedAt { get; private set; }
```

Updated constructor:
```csharp
private DeliveryTrackingDetail(
    Guid id,
    DeliveryTrackingId deliveryTrackingId,
    Guid itemId,
    DeliveryTrackingDetailType type,  // ? NEW
    int quantity) : base(id)
```

Added convenience factory methods:
```csharp
/// <summary>
/// Create product variant item
/// </summary>
public static DeliveryTrackingDetail CreateProduct(
    DeliveryTrackingId deliveryTrackingId,
    Guid variantId,
    int quantity)
{
    return Create(deliveryTrackingId, variantId, DeliveryTrackingDetailType.Product, quantity);
}

/// <summary>
/// Create part item
/// </summary>
public static DeliveryTrackingDetail CreatePart(
    DeliveryTrackingId deliveryTrackingId,
    Guid partId,
    int quantity)
{
    return Create(deliveryTrackingId, partId, DeliveryTrackingDetailType.Part, quantity);
}
```

### 3. **Persistence Configuration**
**File:** `DeliveryTrackingDetailConfiguration.cs`

Added Type mapping:
```csharp
// Type - Product or Part enum conversion
builder.Property(dtd => dtd.Type)
    .IsRequired()
    .HasConversion<int>();
```

Added audit columns:
```csharp
builder.Property(dtd => dtd.CreatedAt)
    .IsRequired();

builder.Property(dtd => dtd.UpdatedAt)
    .IsRequired();
```

Added index:
```csharp
builder.HasIndex(dtd => dtd.Type);
```

---

## ?? Database Schema Impact

**New columns in `DeliveryTrackingDetails` table:**
```sql
Type INT NOT NULL,           -- Stored as: 0=Product, 1=Part
```
```sql
CREATE INDEX IX_DeliveryTrackingDetail_Type ON [delivery].[DeliveryTrackingDetails]([Type]);
```

---

## ?? Usage Examples

### Create Product Item
```csharp
// Using convenience method
var productItem = DeliveryTrackingDetail.CreateProduct(
    deliveryTrackingId: tracking.Id,
    variantId: Guid.Parse("..."),  // Product VariantId
    quantity: 2);

// Or using generic Create method
var productItem = DeliveryTrackingDetail.Create(
    deliveryTrackingId: tracking.Id,
    itemId: variantId,
    type: DeliveryTrackingDetailType.Product,
    quantity: 2);
```

### Create Part Item
```csharp
// Using convenience method
var partItem = DeliveryTrackingDetail.CreatePart(
    deliveryTrackingId: tracking.Id,
    partId: Guid.Parse("..."),  // Part Id
    quantity: 5);

// Or using generic Create method
var partItem = DeliveryTrackingDetail.Create(
    deliveryTrackingId: tracking.Id,
    itemId: partId,
    type: DeliveryTrackingDetailType.Part,
    quantity: 5);
```

### Add Items to Tracking
```csharp
var productItem = DeliveryTrackingDetail.CreateProduct(tracking.Id, productVariantId, 1);
var partItem = DeliveryTrackingDetail.CreatePart(tracking.Id, partId, 3);

tracking.AddDetail(productItem);
tracking.AddDetail(partItem);
```

### Query by Type
```csharp
// Get all product items in a delivery
var productItems = tracking.Details
    .Where(d => d.Type == DeliveryTrackingDetailType.Product)
    .ToList();

// Get all part items in a delivery
var partItems = tracking.Details
    .Where(d => d.Type == DeliveryTrackingDetailType.Part)
    .ToList();
```

---

## ?? Real-World Scenario

```csharp
// Customer orders: 1 Puzzle (Product) + 5 Assembly Parts
var order = /* ... */;

// Create delivery tracking
var tracking = DeliveryTracking.Create(
    orderId: order.Id.Value,
    deliveryOrderCode: "GHN-12345",
    expectedDeliveryDate: DateTime.UtcNow.AddDays(3),
    type: DeliveryTrackingType.Original);

if (tracking.IsSuccess)
{
    var t = tracking.Value;
    
    // Add product (the Puzzle)
    var product = DeliveryTrackingDetail.CreateProduct(
        t.Id,
        variantId: puzzleVariantId,
        quantity: 1);  // 1 Puzzle
    t.AddDetail(product);
    
    // Add parts (assembly components)
    var parts = DeliveryTrackingDetail.CreatePart(
        t.Id,
        partId: assemblyPartsId,
        quantity: 5);  // 5 Parts
    t.AddDetail(parts);
    
    // Now we know:
    // - This delivery has 1 product and 5 parts
    // - Total items: 6
    // - Can query separately if needed
}

// Later: Check what's in this shipment
foreach (var item in tracking.Details)
{
    if (item.Type == DeliveryTrackingDetailType.Product)
        Console.WriteLine($"Product: {item.ItemId}, Qty: {item.Quantity}");
    else
        Console.WriteLine($"Part: {item.ItemId}, Qty: {item.Quantity}");
}
```

---

## ?? Method Signatures

**Generic Create:**
```csharp
public static DeliveryTrackingDetail Create(
    DeliveryTrackingId deliveryTrackingId,
    Guid itemId,
    DeliveryTrackingDetailType type,
    int quantity)
```

**Product Convenience:**
```csharp
public static DeliveryTrackingDetail CreateProduct(
    DeliveryTrackingId deliveryTrackingId,
    Guid variantId,
    int quantity)
```

**Part Convenience:**
```csharp
public static DeliveryTrackingDetail CreatePart(
    DeliveryTrackingId deliveryTrackingId,
    Guid partId,
    int quantity)
```

---

## ?? Constraints

1. **Type Required** - Every item must have a type
2. **Valid Values** - Only Product (0) or Part (1)
3. **Quantity > 0** - Already existed, still enforced
4. **Database Constraint** - Check constraint ensures valid values

---

## ? Benefits

? **Type Safety** - Know what kind of item without guessing
? **Queryable** - Filter items by type efficiently
? **Flexible** - Support both products and parts
? **Audit Trail** - Know when items added via CreatedAt
? **Audit Updates** - Track updates via UpdatedAt
? **Semantic** - CreateProduct/CreatePart methods are clear

---

## ?? Changed Files Summary

| File | Changes |
|------|---------|
| `DeliveryTrackingDetail.cs` | Added Type, CreatedAt, UpdatedAt + factory methods |
| `DeliveryTrackingDetailType.cs` | NEW - Enum definition |
| `DeliveryTrackingDetailConfiguration.cs` | Added Type config, timestamps, index, constraint |

---

## ?? Migration Steps

```powershell
# Create migration
dotnet ef migrations add AddTypeToDeliveryTrackingDetail `
  --project src/Modules/Delivery/PuzKit3D.Modules.Delivery.Persistence `
  --startup-project src/WebApi/PuzKit3D.WebApi

# Apply migration
dotnet ef database update `
  --project src/Modules/Delivery/PuzKit3D.Modules.Delivery.Persistence `
  --startup-project src/WebApi/PuzKit3D.WebApi
```

---

## ?? Checklist

- [x] Enum created
- [x] Entity updated
- [x] Factory methods added
- [x] Persistence configuration updated
- [x] Indexes added
- [x] Constraints added
- [ ] EF Core migration created
- [ ] Migration applied
- [ ] Tests updated
- [ ] API documentation updated

---

**Ready for migration!** ??
