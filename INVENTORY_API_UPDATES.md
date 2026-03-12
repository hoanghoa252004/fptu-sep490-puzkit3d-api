# Inventory Management API - Simplified Structure

## ?? Changes Summary

### ? Simplified Endpoint Structure
**Route:** `/api/instock-products/{productId}/variants/{variantId}/inventory`

Only **3 endpoints** for simple inventory management:
1. **GET** - Get current inventory
2. **PUT** - Update inventory (creates if not exists)
3. **DELETE** - Delete inventory (set quantity to 0)

### ?? Validation Flow
All inventory operations validate in this order:
1. ? Product exists
2. ? Variant exists
3. ? Variant belongs to the specified product
4. ? Inventory operation (get/update/delete)

### ?? API Endpoints

#### 1. Get Inventory by Variant
- **GET** `/api/instock-products/{productId}/variants/{variantId}/inventory`
- **Summary:** Get current inventory information for a variant
- **Auth:** Staff, BusinessManager
- **Response:** 
  ```json
  {
    "id": "guid",
    "instockProductVariantId": "guid",
    "totalQuantity": 100,
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
  ```

#### 2. Update Inventory
- **PUT** `/api/instock-products/{productId}/variants/{variantId}/inventory`
- **Body:** `{ "quantity": number }`
- **Summary:** Update inventory quantity directly
- **Behavior:**
  - If inventory doesn't exist ? **Create new** with the quantity
  - If inventory exists ? **Update** with the new quantity
- **Auth:** Staff, BusinessManager
- **Example:**
  ```json
  {
    "quantity": 150
  }
  ```

#### 3. Delete Inventory
- **DELETE** `/api/instock-products/{productId}/variants/{variantId}/inventory`
- **Summary:** Delete inventory (sets quantity to 0)
- **Behavior:** Sets the inventory quantity to 0
- **Auth:** Staff, BusinessManager
- **Note:** Returns success even if inventory doesn't exist

## ?? Updated Components

### Application Layer

**Queries:**
- ? `GetInstockInventoryByVariantIdQuery` - Gets inventory for variant
- ? `GetInstockInventoryByVariantIdQueryHandler` - Validates product ? variant ? inventory

**Commands:**
- ? `UpdateInventoryCommand` - Update quantity (creates if not exists)
- ? `UpdateInventoryCommandHandler` - Handles both create and update logic
- ? `DeleteInventoryCommand` - Set quantity to 0
- ? `DeleteInventoryCommandHandler` - Handles deletion (set to 0)

**Removed Commands:**
- ? `AddStockCommand` - Not needed (use UpdateInventory)
- ? `ReduceStockCommand` - Not needed (use UpdateInventory)
- ? `SetStockCommand` - Renamed to UpdateInventory

### Domain Layer
- ? `InstockInventory.Create()` - Creates new inventory
- ? `InstockInventory.SetStock()` - Updates quantity
- ? Added `InstockProductVariantError.VariantDoesNotBelongToProduct()` error

### API Layer
- ? `GetInstockInventoryByVariantId` endpoint
- ? `UpdateInventory` endpoint (replaces SetStock)
- ? `DeleteInventory` endpoint
- ? Removed `AddStock` endpoint
- ? Removed `ReduceStock` endpoint

## ?? Key Features

1. **Auto-Create:** Update endpoint automatically creates inventory if it doesn't exist
2. **Simple CRUD:** Only 3 operations: Get, Update, Delete
3. **Direct Quantity Control:** Client sends exact quantity, not increment/decrement
4. **Zero for Delete:** Delete sets quantity to 0 instead of removing record
5. **Consistent Validation:** All operations validate product ? variant hierarchy

## ?? Example Usage

### Get Inventory
```http
GET /api/instock-products/123e4567-e89b-12d3-a456-426614174000/variants/456e7890-e89b-12d3-a456-426614174000/inventory
Authorization: Bearer {token}
```

**Response 200 OK:**
```json
{
  "id": "789e0123-e89b-12d3-a456-426614174000",
  "instockProductVariantId": "456e7890-e89b-12d3-a456-426614174000",
  "totalQuantity": 100,
  "createdAt": "2024-01-01T10:00:00Z",
  "updatedAt": "2024-01-01T15:30:00Z"
}
```

### Update Inventory (Create or Update)
```http
PUT /api/instock-products/123e4567-e89b-12d3-a456-426614174000/variants/456e7890-e89b-12d3-a456-426614174000/inventory
Authorization: Bearer {token}
Content-Type: application/json

{
  "quantity": 150
}
```

**Response:** `204 No Content`

**Use Cases:**
- Set initial stock: `{ "quantity": 100 }`
- Increase stock: `{ "quantity": 150 }` (was 100, now 150)
- Decrease stock: `{ "quantity": 50 }` (was 100, now 50)
- Zero out: `{ "quantity": 0 }`

### Delete Inventory
```http
DELETE /api/instock-products/123e4567-e89b-12d3-a456-426614174000/variants/456e7890-e89b-12d3-a456-426614174000/inventory
Authorization: Bearer {token}
```

**Response:** `204 No Content`

**Result:** Inventory quantity is set to 0

## ?? Design Benefits

1. **Simplicity:** Only 3 endpoints vs 5+ endpoints
2. **Flexibility:** Client has full control over quantity
3. **Idempotent:** Update can be called multiple times safely
4. **Clear Intent:** PUT for update, DELETE for removal
5. **Auto-Create:** No separate create endpoint needed
6. **Consistent:** Follows RESTful principles

## ? Build Status
- All builds successful
- No compilation errors
- All validations working correctly

## ?? Migration Notes

If you had code using old endpoints:

**Old ? New:**
- `PATCH /add-stock` ? `PUT /inventory` with new quantity
- `PATCH /reduce-stock` ? `PUT /inventory` with new quantity
- `PUT /set-stock` ? `PUT /inventory` (same but renamed)
- `DELETE /{inventoryId}` ? `DELETE /inventory` (on variant route)
