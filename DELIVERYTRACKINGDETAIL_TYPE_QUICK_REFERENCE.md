# DeliveryTrackingDetail Type - Quick Reference

## ?? What's New?

`DeliveryTrackingDetail` now has a `Type` property to distinguish between:
- **Product** - Product variant items (VariantId)
- **Part** - Part items for assembly (PartId)

## ?? Enum

```csharp
public enum DeliveryTrackingDetailType
{
    Product = 0,  // Product variant
    Part = 1      // Assembly part
}
```

---

## ? New Properties

```csharp
public DeliveryTrackingDetailType Type { get; private set; }  // ? NEW
public DateTime CreatedAt { get; private set; }                // ? NEW
public DateTime UpdatedAt { get; private set; }                // ? NEW
```

---

## ?? Creation Methods

### Generic Create
```csharp
DeliveryTrackingDetail.Create(
    deliveryTrackingId: tracking.Id,
    itemId: variantId,
    type: DeliveryTrackingDetailType.Product,
    quantity: 2);
```

### Product Shortcut ?
```csharp
DeliveryTrackingDetail.CreateProduct(tracking.Id, variantId, quantity: 2);
```

### Part Shortcut ?
```csharp
DeliveryTrackingDetail.CreatePart(tracking.Id, partId, quantity: 5);
```

---

## ?? Usage Example

```csharp
// Create tracking
var tracking = DeliveryTracking.Create(
    orderId, "GHN-123", date, DeliveryTrackingType.Original).Value;

// Add product variant
var product = DeliveryTrackingDetail.CreateProduct(
    tracking.Id,
    variantId: puzzleId,
    quantity: 1);
tracking.AddDetail(product);

// Add assembly parts
var parts = DeliveryTrackingDetail.CreatePart(
    tracking.Id,
    partId: assemblyKitId,
    quantity: 5);
tracking.AddDetail(parts);

// Save
_unitOfWork.DeliveryTrackings.Add(tracking);
await _unitOfWork.SaveChangesAsync();
```

---

## ?? Query Examples

```csharp
// Products only
var products = tracking.Details
    .Where(d => d.Type == DeliveryTrackingDetailType.Product)
    .ToList();

// Parts only
var parts = tracking.Details
    .Where(d => d.Type == DeliveryTrackingDetailType.Part)
    .ToList();

// Check what's in shipment
foreach (var item in tracking.Details)
{
    var typeName = item.Type == DeliveryTrackingDetailType.Product ? "Product" : "Part";
    Console.WriteLine($"{typeName}: {item.ItemId}, Qty: {item.Quantity}");
}
```

---

## ?? Database Schema

**New columns:**
```sql
Type INT NOT NULL,              -- 0=Product, 1=Part
```

---

## ?? Best Practices

```csharp
// ? Good - Use convenience methods
var product = DeliveryTrackingDetail.CreateProduct(id, variantId, qty);
var part = DeliveryTrackingDetail.CreatePart(id, partId, qty);

// ? Good - Clear intent
if (item.Type == DeliveryTrackingDetailType.Product)
{
    // Handle product logic
}

// ?? OK but less clear - Generic create
var item = DeliveryTrackingDetail.Create(id, itemId, DeliveryTrackingDetailType.Product, qty);

// ? Don't - Forget to set type correctly
var wrong = DeliveryTrackingDetail.Create(id, variantId, DeliveryTrackingDetailType.Part, qty);
// ^ This is confusing - should be Product if using VariantId
```

---

## ?? When to Use

| Scenario | Type | ItemId |
|----------|------|--------|
| Customer ordered puzzle | Product | VariantId |
| Ordered with assembly kit | Part | PartId |
| Sending replacement product | Product | VariantId |
| Restocking assembly parts | Part | PartId |

---

## ?? Files Changed

1. ? `DeliveryTrackingDetailType.cs` - NEW enum
2. ? `DeliveryTrackingDetail.cs` - Added Type, CreatedAt, UpdatedAt + factory methods
3. ? `DeliveryTrackingDetailConfiguration.cs` - Configuration + index

---

## ?? Next Steps

1. Create EF Core migration
2. Apply migration
3. Start using new convenience methods!

```powershell
dotnet ef migrations add AddTypeToDeliveryTrackingDetail `
  --project src/Modules/Delivery/PuzKit3D.Modules.Delivery.Persistence `
  --startup-project src/WebApi/PuzKit3D.WebApi

dotnet ef database update `
  --project src/Modules/Delivery/PuzKit3D.Modules.Delivery.Persistence `
  --startup-project src/WebApi/PuzKit3D.WebApi
```

---

## ?? Related

- `DeliveryTracking` - Aggregate root
- `DeliveryTrackingDetail` - Line items (updated)
- `DeliveryTracking.Details` - Navigation property

---

**Simple. Clear. Useful.** ??
