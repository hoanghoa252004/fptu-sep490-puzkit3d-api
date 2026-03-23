# Phân Tích CreateDeliverTrackingOrder - InStock Module vs Delivery Module

## ?? T?ng Quan

Quá trình t?o ??n giao hàng (CreateDeliverTrackingOrder) ???c chia thành 3 l?p chính:

1. **API Layer** (InStock.Api) - `CreateDeliverTrackingOrder.cs` - ?i?m vào HTTP endpoint
2. **Application Layer** (InStock.Application) - `CreateDeliverTrackingOrderCommandHandler.cs` - Logic nghi?p v?
3. **Infrastructure Layer** (Delivery.Infrastructure) - `GhnDeliveryService.cs` - Tích h?p v?i GHN API

---

## ??? Ki?n Trúc Hi?n T?i

### 1. API Layer (InStock Module)
**File:** `CreateDeliverTrackingOrder.cs`

```csharp
internal sealed class CreateDeliverTrackingOrder : IEndpoint
{
    MapPost("/") ?
    - Nh?n orderId
    - G?i CreateDeliverTrackingOrderCommand qua MediatR
    - L?y GHN response
    - Fetch order t? database
    - G?i order.SetDeliveryInfo()
    - Save qua UnitOfWork
    - Return delivery order code
}
```

**??c ?i?m:**
- X? lý request/response HTTP
- Dependency injection: `ISender`, `IInstockOrderRepository`, `IInStockUnitOfWork`
- G?i handler qua MediatR pattern

### 2. Application Layer (InStock Module)
**File:** `CreateDeliverTrackingOrderCommandHandler.cs`

```csharp
CreateDeliverTrackingOrderCommandHandler
??? Validate order exists & status = Processing
??? Check delivery info not already set
??? Build shipping items t? order details
??? Create CreateShippingOrderRequest
??? Call _deliveryService.CreateShippingOrderAsync()
??? Parse GHN JSON response
??? Extract: order_code, sort_code, expected_delivery_time
??? Return CreateDeliverTrackingOrderResponseDto
```

**??c ?i?m:**
- Implement `IQueryHandler<CreateDeliverTrackingOrderCommand, CreateDeliverTrackingOrderResponseDto>`
- Logic bi?n ??i Order ? GHN Request
- JSON parsing t? GHN response
- Error handling cho GHN API

### 3. Infrastructure Layer (Delivery Module)
**File:** `GhnDeliveryService.cs` implements `IDeliveryService`

```csharp
CreateShippingOrderAsync(CreateShippingOrderRequest)
??? Get sender info t? DeliverySettings
??? Map request to GHN format (ToGhnRequest)
??? Call GHN API: POST /v2/shipping-order/create
??? Validate response status
??? Return raw JSON object
??? No parsing - Just pass through
```

**??c ?i?m:**
- Service h? t?ng - t??ng tác tr?c ti?p v?i GHN API
- Return raw `object` type (untyped JSON)
- Reusable cho nhi?u modules khác
- Centralized error handling cho GHN API

---

## ? ?u ?i?m

### Separation of Concerns
? **M?i l?p có trách nhi?m rõ ràng:**
- API Layer: HTTP request/response handling
- Application Layer: Business logic & validation
- Infrastructure Layer: External service integration

### Reusability
? **GhnDeliveryService có th? tái s? d?ng:**
- Delivery Module ??nh ngh?a interface `IDeliveryService`
- B?t k? module nào c?ng có th? inject & s? d?ng
- Không lock-in vào InStock module

### Loose Coupling
? **InStock không ph? thu?c tr?c ti?p vào GHN:**
- Dependency: InStock ? IDeliveryService ? GhnDeliveryService
- Có th? swap GhnDeliveryService v?i service khác mà không thay ??i InStock code

### Clean Architecture
? **Domain Logic ???c b?o v?:**
- `order.SetDeliveryInfo()` là pure domain method
- Validate delivery info tr??c khi set
- Timezone handling ?úng cách (UTC conversion)

### Comprehensive Validation
? **3 l?p validation:**
1. **API Layer:** Check order found, check GHN response success
2. **Application Layer:** Check order status, check delivery info not set, validate GHN response format
3. **Domain Layer:** Validate delivery order code, validate date format

### Error Handling
? **Structured error handling:**
- `Result<T>` pattern cho consistent error management
- Domain errors: `InstockOrderError`
- GHN errors: `GhnOrderError`
- API errors: HTTP status codes + error messages

### Dependency Injection
? **Proper DI pattern:**
- API Layer inject t? container
- Handler dependencies via constructor
- Service dependencies via options pattern

---

## ? Nh??c ?i?m & Issues

### 1. **Duplicate Validation Logic**
? **Order Status Check ???c validate ? 2 n?i:**

```csharp
// Application Layer - CreateDeliverTrackingOrderCommandHandler
if (order.Status != InstockOrderStatus.Processing)
{
    return Result.Failure(...);
}

// API Layer - CreateDeliverTrackingOrder
// Không có validation, ch? check result.IsFailure
if (result.IsFailure) { return Results.BadRequest(...); }
```

**V?n ??:** N?u domain rules thay ??i, ph?i update ? 2 ch?.

---

### 2. **API Layer Logic Overlap**
? **API Layer làm l?i công vi?c c?a Handler:**

```csharp
// API Layer fetch order l?n 2
var order = await orderRepository.GetByIdAsync(orderIdObj, cancellationToken);

// Nh?ng Handler ?ã l?y order r?i:
var order = await _orderRepository.GetByIdWithDetailsAsync(orderId, cancellationToken);
```

**V?n ??:**
- 2 database calls thay vì 1
- `order.SetDeliveryInfo()` ???c g?i ? API, không ? Handler
- Domain method logic b? split gi?a 2 l?p

---

### 3. **Untyped JSON Response**
? **GhnDeliveryService return `Task<ResultT<object>>`:**

```csharp
public async Task<ResultT<object>> CreateShippingOrderAsync(CreateShippingOrderRequest request, ...)
{
    // ...
    return Result.Success(jsonData ?? new object()); // object type!
}
```

**V?n ??:**
- Không type-safe - có th? miss/misparse properties
- Handler ph?i parse JSON l?i:
```csharp
var jsonElement = JsonSerializer.Serialize(ghnResponse);
var jsonDocument = JsonDocument.Parse(jsonElement);
var root = jsonDocument.RootElement;
if (!root.TryGetProperty("data", out var dataElement)) { ... }
```

---

### 4. **JSON Parsing Fragility**
? **Handler manual parse GHN response:**

```csharp
try
{
    var jsonElement = JsonSerializer.Serialize(ghnResponse);
    var jsonDocument = JsonDocument.Parse(jsonElement);
    // ... many TryGetProperty calls
}
catch (Exception ex)
{
    return Result.Failure<CreateDeliverTrackingOrderResponseDto>(
        GhnOrderError.ParsingGhnResponseFailed(ex.Message));
}
```

**V?n ??:**
- Nhi?u property access, d? miss
- Fragile - thay ??i GHN response format ? crash
- Không validation cho t?ng field

---

### 5. **SaveChanges Logic ? API Layer**
? **Domain method result không ???c check ? Handler:**

```csharp
// Application Layer - Handler
var result = await _deliveryService.CreateShippingOrderAsync(...);
if (result.IsFailure) return Result.Failure(...);

// nh?ng KHÔNG g?i order.SetDeliveryInfo() ? ?ây!
// API Layer m?i g?i:
var setDeliveryResult = order.SetDeliveryInfo(...);
if (setDeliveryResult.IsFailure) { return Results.BadRequest(...); }

// R?i save:
await unitOfWork.ExecuteAsync(async () => {
    orderRepository.Update(order);
    return Result.Success();
}, cancellationToken);
```

**V?n ??:**
- Business logic (setting delivery info) ? API layer
- Domain method g?i ? sai l?p
- Handler tr? v? data nh?ng không save - save ? API layer
- UnitOfWork không return t?p h?p changes rõ ràng

---

### 6. **Missing Transactionality**
? **Không explicit transaction:**

```csharp
await unitOfWork.ExecuteAsync(async () => {
    orderRepository.Update(order);
    return Result.Success();
}, cancellationToken);

// N?u GHN API success nh?ng save fail ? inconsistent state
// GHN có order nh?ng database ch?a update
```

**V?n ??:**
- Race condition có th? x?y ra
- No rollback mechanism visible
- GHN order create tr??c, DB save sau ? khó recover n?u fail

---

### 7. **Command/Query Naming Confusion**
? **S? d?ng `IQuery<T>` nh?ng không ph?i read-only query:**

```csharp
public sealed record CreateDeliverTrackingOrderCommand(Guid OrderId) 
    : IQuery<CreateDeliverTrackingOrderResponseDto>;

// Nh?ng nó làm:
// 1. Create GHN shipping order (side effect!)
// 2. Update database (side effect!)
// 3. Return result
```

**V?n ??:**
- `IQuery` th??ng là read-only, nh?ng ?ây là write operation
- Nên là `ICommand<T>` ho?c `ICommandQuery<T>`
- Violate CQRS principles

---

### 8. **Hard-Coded Strings & Magic Values**
? **GHN request có hard-coded values:**

```csharp
RequiredNote = "CHOXEMHANGKHONGTHU",
Note = $"Order {order.Code}",
Content = "Puzzle 3D Product",
CodAmount = string.Equals(order.PaymentMethod, "COD", StringComparison.OrdinalIgnoreCase) 
    ? (int)order.SubTotalAmount 
    : 0
```

**V?n ??:**
- Magic strings không trong config
- COD logic hard-coded
- Khó maintain, khó test

---

### 9. **No Idempotency Check**
? **N?u API call 2 l?n v?i cùng orderId:**

```csharp
// L?n 1: Success, GHN order created, DB updated
// L?n 2: order.DeliveryOrderCode != null, nh?ng GHN API create khác order code
//        ? Có 2 GHN orders cho 1 Instock order!
```

**V?n ??:**
- Duplicate GHN orders có th? ???c t?o
- Không idempotent - retry có v?n ??
- C?n unique constraint ho?c better logic

---

### 10. **Error Messages Not Localized**
? **Error messages hard-coded English:**

```csharp
Error.NotFound("INSTOCK_ORDER_NOT_FOUND", $"Order with ID {request.OrderId} not found")
```

**V?n ??:**
- Không i18n support
- User API return English messages

---

## ?? So Sánh 3 L?p

| Aspect | API Layer | Application Layer | Infrastructure Layer |
|--------|-----------|-------------------|----------------------|
| **Responsibility** | HTTP handling | Business logic | External service |
| **Validation** | ? (partial) | ? (business) | ? (API response) |
| **Data Transformation** | ? | ? (Order?GHN) | ? |
| **Domain Methods** | ? | ? | ? |
| **Type Safety** | ? | ? | ? (object type) |
| **Testability** | Medium | High | High |
| **Reusability** | ? (specific endpoint) | ? (command/handler) | ? (service interface) |

---

## ?? Chi Ti?t Lu?ng X? Lý

```
1. API Request
   ?
2. CreateDeliverTrackingOrder.MapEndpoint()
   ?? Extract: orderId
   ?? Call: sender.Send(CreateDeliverTrackingOrderCommand)
   ?   ?
   ?   3. CreateDeliverTrackingOrderCommandHandler.Handle()
   ?      ?? Fetch order with details (DB)
   ?      ?? Validate order exists & status = Processing
   ?      ?? Check delivery info not already set
   ?      ?? Build shipping items
   ?      ?? Call: _deliveryService.CreateShippingOrderAsync()
   ?      ?   ?
   ?      ?   4. GhnDeliveryService.CreateShippingOrderAsync()
   ?      ?      ?? Build GHN request with sender info
   ?      ?      ?? Call GHN API: POST /v2/shipping-order/create
   ?      ?      ?? Validate response.IsSuccessStatusCode
   ?      ?      ?? Return: Result.Success(jsonData)
   ?      ?
   ?      ?? Parse GHN JSON response
   ?      ?? Extract: order_code, sort_code, expected_delivery_time
   ?      ?? Return: CreateDeliverTrackingOrderResponseDto
   ?
   ?? Get ghnResponse
   ?? Fetch order t? DB L?N 2 ?
   ?? Call: order.SetDeliveryInfo()
   ?? Save via unitOfWork
   ?? Return: Results.Ok(deliveryOrderCode)

```

**V?n ??:** 2 database calls khi 1 là ??!

---

## ?? Khuy?n Ngh? C?i Thi?n

### Priority 1: Remove Duplicate DB Call
```csharp
// Option 1: Return order t? Handler
// Handler tr? v? toàn b? response + order
// API layer ch? save

// Option 2: Move SetDeliveryInfo to Handler
// Handler t? g?i SetDeliveryInfo
// Handler t? save
// API layer ch? return
```

### Priority 2: Type-Safe GHN Response
```csharp
// T?o DTO cho GHN response
public record GhnCreateOrderResponse
{
    public GhnOrderData Data { get; set; }
}

public record GhnOrderData
{
    public string OrderCode { get; set; }
    public string SortCode { get; set; }
    public DateTime ExpectedDeliveryTime { get; set; }
}

// GhnDeliveryService return ResultT<GhnCreateOrderResponse>
```

### Priority 3: Fix Command/Query Mismatch
```csharp
// ??i t? IQuery sang ICommand
public sealed record CreateDeliverTrackingOrderCommand(Guid OrderId) 
    : ICommand<CreateDeliverTrackingOrderResponseDto>;

// Implement ICommandHandler thay vì IQueryHandler
```

### Priority 4: Move Domain Logic to Handler
```csharp
// Handler g?i order.SetDeliveryInfo()
// Handler save order via unitOfWork
// API layer ch? return HTTP response
```

### Priority 5: Config Hard-Coded Values
```csharp
public interface IDeliveryOptions
{
    string RequiredNote { get; }
    string Content { get; }
}

// Use in Handler
var ghnRequest = request.ToGhnRequest(senderInfo, _deliveryOptions);
```

---

## ?? K?t Lu?n

**?u ?i?m chính:**
- ? Separation of concerns rõ ràng
- ? Reusable infrastructure services
- ? Comprehensive validation
- ? Clean architecture layers

**Nh??c ?i?m chính:**
- ? Duplicate DB calls
- ? Domain logic split gi?a API & Handler
- ? Untyped JSON parsing
- ? Command/Query terminology mismatch
- ? No idempotency protection

**Overall:** Ki?n trúc c? b?n t?t, nh?ng c?n refactor ?? lo?i b? duplicate logic và c?i thi?n type safety.

