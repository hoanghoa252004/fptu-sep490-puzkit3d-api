# UpdateInstockOrderStatus API Documentation

## Endpoint
```
PUT /api/instock-orders/{id}/status
```

## Authorization
- **Required Roles**: `Staff` or `BusinessManager`
- Must be authenticated

## Description
Updates the status of an instock order with automatic validation of status transitions based on workflow rules.

## Request Parameters

### Path Parameters
- `id` (Guid, required): The ID of the instock order to update

### Request Body
```json
{
  "newStatus": 0
}
```

**newStatus** values (InstockOrderStatus enum):
- `0` = Waiting (COD orders initial state)
- `1` = Pending (Online payment initial state)
- `2` = Paid (Order payment completed)
- `3` = Expired (Order expired without payment)
- `4` = Processing (Order in processing)
- `5` = Shipping (Order in transit)
- `6` = Delivered (Order delivered)
- `7` = Completed (Order completed)
- `8` = Cancelled (Order cancelled)

## Valid Status Transitions

The API enforces the following workflow transitions:

### COD Payment Method
- `Waiting` ? `Processing`, `Expired`, `Cancelled`
- `Processing` ? `Shipping`
- `Shipping` ? `Delivered`
- `Delivered` ? `Completed`

### Online Payment Method
- `Pending` ? `Paid`, `Expired`, `Cancelled`
- `Paid` ? `Processing`
- `Processing` ? `Shipping`
- `Shipping` ? `Delivered`
- `Delivered` ? `Completed`

### Terminal States
- `Expired` ? (No transitions allowed)
- `Cancelled` ? (No transitions allowed)
- `Completed` ? (No transitions allowed)

## Response

### Success (204 No Content)
```
HTTP/1.1 204 No Content
```

### Error Responses

**400 Bad Request** - Invalid status transition
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Validation failed",
  "status": 400,
  "detail": "Cannot transition from 'Completed' to 'Shipping'. Allowed transitions: COD: Waiting -> [Processing | Expired | Cancelled], Processing -> Shipping -> Delivered -> Completed; Online: Pending -> [Paid | Expired | Cancelled], Paid -> Processing -> Shipping -> Delivered -> Completed"
}
```

**401 Unauthorized** - Not authenticated
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.3.1",
  "title": "Unauthorized",
  "status": 401
}
```

**403 Forbidden** - Insufficient permissions (not Staff or BusinessManager)
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.3",
  "title": "Forbidden",
  "status": 403
}
```

**404 Not Found** - Order not found
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404,
  "detail": "Instock order with ID 'xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx' was not found."
}
```

## Examples

### Example 1: Move order from Pending to Paid
```bash
curl -X PUT "https://api.example.com/api/instock-orders/550e8400-e29b-41d4-a716-446655440000/status" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "newStatus": 2
  }'
```

Response: `204 No Content`

### Example 2: Move order from Paid to Processing
```bash
curl -X PUT "https://api.example.com/api/instock-orders/550e8400-e29b-41d4-a716-446655440000/status" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "newStatus": 4
  }'
```

Response: `204 No Content`

### Example 3: Invalid transition (should fail)
```bash
curl -X PUT "https://api.example.com/api/instock-orders/550e8400-e29b-41d4-a716-446655440000/status" \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "newStatus": 6
  }'
```

Response: `400 Bad Request` - Invalid transition from Pending to Delivered

## Implementation Details

### Files Created
1. **UpdateInstockOrderStatusCommand.cs**
   - CQRS command for updating order status
   - Located: `src/Modules/InStock/PuzKit3D.Modules.InStock.Application/UseCases/InstockOrders/Commands/UpdateInstockOrderStatus/`

2. **UpdateInstockOrderStatusCommandHandler.cs**
   - Command handler that processes the status update
   - Validates transition using `InstockOrderStatusTransition.IsValidTransition()`
   - Updates order in repository within unit of work transaction
   - Located: Same directory as command

3. **UpdateInstockOrderStatus.cs**
   - API endpoint implementation
   - Maps to `PUT /api/instock-orders/{id}/status`
   - Requires Staff or BusinessManager authorization
   - Located: `src/Modules/InStock/PuzKit3D.Modules.InStock.Api/InstockOrders/UpdateInstockOrderStatus/`

### Key Validation
- **Status Transition Validation**: Uses `InstockOrderStatusTransition.IsValidTransition()` from domain
- **Authorization**: Requires `Staff` or `BusinessManager` role via `RequireRole()` policy
- **Order Existence**: Checks if order exists before updating
- **Atomic Updates**: Uses unit of work pattern to ensure transactional consistency

## Notes
- The endpoint returns `204 No Content` on success (no response body)
- The `UpdatedAt` timestamp is automatically set to UTC now on successful status update
- Invalid transitions are rejected with detailed error messages explaining allowed transitions
- All status transitions follow the domain-driven design business rules defined in `InstockOrderStatusTransition`
