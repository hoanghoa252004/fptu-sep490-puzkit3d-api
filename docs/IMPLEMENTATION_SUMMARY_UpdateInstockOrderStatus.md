# InStock Order Status Update API Implementation Summary

## Overview
?ã t?o m?t API endpoint m?i cho phép Staff và Admin thay ??i tr?ng thái c?a instock order v?i validation t? ??ng c?a status transition theo business rules.

## Files Created

### 1. Application Layer (Commands)
**File**: `src/Modules/InStock/PuzKit3D.Modules.InStock.Application/UseCases/InstockOrders/Commands/UpdateInstockOrderStatus/UpdateInstockOrderStatusCommand.cs`

```csharp
public sealed record UpdateInstockOrderStatusCommand(
    Guid OrderId,
    InstockOrderStatus NewStatus) : ICommand;
```

- CQRS Command ?? c?p nh?t status c?a order
- Nh?n orderId và newStatus

### 2. Application Layer (Command Handler)
**File**: `src/Modules/InStock/PuzKit3D.Modules.InStock.Application/UseCases/InstockOrders/Commands/UpdateInstockOrderStatus/UpdateInstockOrderStatusCommandHandler.cs`

**Key Features**:
- L?y order t? repository
- Ki?m tra order t?n t?i
- G?i `order.UpdateStatus(newStatus)` - method này t? ??ng validate transition
- C?p nh?t order trong repository trong transaction (unit of work)
- Tr? v? Result (Success ho?c Failure)

### 3. API Layer (Endpoint)
**File**: `src/Modules/InStock/PuzKit3D.Modules.InStock.Api/InstockOrders/UpdateInstockOrderStatus/UpdateInstockOrderStatus.cs`

**HTTP**: `PUT /api/instock-orders/{id}/status`

**Key Features**:
- Request body: `{ "newStatus": 2 }`
- Yêu c?u authorization (Staff ho?c BusinessManager role)
- Response: 204 No Content on success
- Validation problems tr? v? 400 Bad Request
- Order not found tr? v? 404 Not Found
- Unauthorized tr? v? 401/403

## Status Transition Validation

S? d?ng `InstockOrderStatusTransition.IsValidTransition()` t? domain layer:

### COD Orders (Waiting initial status)
```
Waiting ? Processing, Expired, Cancelled
Processing ? Shipping
Shipping ? Delivered
Delivered ? Completed
```

### Online Orders (Pending initial status)
```
Pending ? Paid, Expired, Cancelled
Paid ? Processing
Processing ? Shipping
Shipping ? Delivered
Delivered ? Completed
```

### Terminal States
```
Expired, Cancelled, Completed ? No transitions allowed
```

## Architecture Pattern

1. **API Endpoint** (UpdateInstockOrderStatus.cs)
   - Nh?n HTTP request
   - Extract d? li?u t? request
   - T?o Command object
   - G?i Command via MediatR sender

2. **Command Handler** (UpdateInstockOrderStatusCommandHandler.cs)
   - Get order t? repository
   - Validate order existence
   - G?i domain method ?? update status (có validation)
   - Update repository trong transaction
   - Tr? v? Result

3. **Domain Layer** (InstockOrder.cs)
   - Method `UpdateStatus(newStatus)` có s?n
   - T? ??ng validate transition
   - C?p nh?t `UpdatedAt` timestamp

4. **Validation Rules**
   - `InstockOrderStatusTransition.IsValidTransition()` ki?m tra transition h?p l?
   - `InstockOrderError.InvalidStatusTransition()` t?o error message

## Authorization
```csharp
.RequireAuthorization(policy => policy.RequireRole(Roles.Staff, Roles.BusinessManager))
```
- Yêu c?u user ph?i có role Staff ho?c BusinessManager
- Ng??i dùng ph?i authenticated

## Testing

### Test Case 1: Successful transition
```
PUT /api/instock-orders/{id}/status
Body: { "newStatus": 2 }  // Pending ? Paid
Result: 204 No Content
```

### Test Case 2: Invalid transition
```
PUT /api/instock-orders/{id}/status
Body: { "newStatus": 6 }  // Pending ? Delivered (không allowed)
Result: 400 Bad Request
Error: "Cannot transition from 'Pending' to 'Delivered'"
```

### Test Case 3: Order not found
```
PUT /api/instock-orders/invalid-id/status
Result: 404 Not Found
```

### Test Case 4: Unauthorized
```
PUT /api/instock-orders/{id}/status (without auth token)
Result: 401 Unauthorized
```

### Test Case 5: Forbidden (insufficient role)
```
PUT /api/instock-orders/{id}/status (user is Customer role)
Result: 403 Forbidden
```

## Documentation
**File**: `docs/API_UpdateInstockOrderStatus.md`
- Chi ti?t v? endpoint
- Valid status transitions
- Request/Response examples
- Error handling

## Integration Notes
- Endpoint ???c t? ??ng map thông qua `IEndpoint` interface pattern
- S? d?ng `MapOrdersGroup()` extension ?? group v?i orders endpoints
- CQRS pattern thông qua MediatR
- Unit of Work pattern cho transactional consistency
- Status transition validation t? domain layer

## Next Steps (Optional)
1. Có th? thêm logging ?? track status changes
2. Có th? thêm domain event ?? notify khi status thay ??i
3. Có th? thêm audit trail cho status changes
