# QuickReference: Update Instock Order Status API

## ?? API Endpoint
```
PUT /api/instock-orders/{id}/status
```

## ?? Request
```json
{
  "newStatus": 2
}
```

**Status Codes**:
- 0 = Waiting
- 1 = Pending  
- 2 = Paid
- 3 = Expired
- 4 = Processing
- 5 = Shipping
- 6 = Delivered
- 7 = Completed
- 8 = Cancelled

## ?? Authorization
Required: `Staff` or `BusinessManager` role

## ? Valid Transitions
```
Waiting      ? [Processing, Expired, Cancelled]
Pending      ? [Paid, Expired, Cancelled]
Paid         ? [Processing]
Processing   ? [Shipping]
Shipping     ? [Delivered]
Delivered    ? [Completed]
Expired      ? (terminal state)
Cancelled    ? (terminal state)
Completed    ? (terminal state)
```

## ?? Project Structure
```
Application Layer:
??? Commands/
?   ??? UpdateInstockOrderStatus/
?       ??? UpdateInstockOrderStatusCommand.cs
?       ??? UpdateInstockOrderStatusCommandHandler.cs

API Layer:
??? InstockOrders/
?   ??? UpdateInstockOrderStatus/
?       ??? UpdateInstockOrderStatus.cs
```

## ?? Response Codes
- `204 No Content` - Success
- `400 Bad Request` - Invalid transition
- `401 Unauthorized` - Not authenticated
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Order not found

## ?? Example cURL
```bash
curl -X PUT "http://localhost:5000/api/instock-orders/{id}/status" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{"newStatus": 4}'
```

## ?? Full Documentation
See: `docs/API_UpdateInstockOrderStatus.md`
See: `docs/IMPLEMENTATION_SUMMARY_UpdateInstockOrderStatus.md`

## ?? Key Classes
- `InstockOrderStatusTransition` - Validates transitions
- `InstockOrder` - Domain entity with `UpdateStatus()` method
- `InstockOrderError` - Error handling
