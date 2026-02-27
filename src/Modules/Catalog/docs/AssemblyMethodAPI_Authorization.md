# Assembly Method API - Authorization & Access Control

## ?? Authorization Model

### Roles
- **Staff**: Can view and manage assembly methods
- **Manager**: Can view and manage assembly methods  
- **Customer**: Can only view active assembly methods (no timestamps)
- **Admin**: Can only view active assembly methods (no timestamps)
- **Anonymous**: Can only view active assembly methods (no timestamps)

### Permissions
- `catalog:assembly-methods:view` - View assembly method details (Staff/Manager)
- `catalog:assembly-methods:manage` - Create, Update, Delete assembly methods (Staff/Manager)

## ?? API Endpoints Access Matrix

| Endpoint | Method | Anonymous | Customer | Admin | Staff | Manager |
|----------|--------|-----------|----------|-------|-------|---------|
| `/api/assembly-methods` (POST) | Create | ? | ? | ? | ? | ? |
| `/api/assembly-methods/{id}` (PUT) | Update | ? | ? | ? | ? | ? |
| `/api/assembly-methods/{id}` (DELETE) | Delete | ? | ? | ? | ? | ? |
| `/api/assembly-methods/{id}` (GET) | Get by ID | ? | ? | ? | ? | ? |
| `/api/assembly-methods/slug/{slug}` (GET) | Get by Slug | ? | ? | ? | ? | ? |
| `/api/assembly-methods` (GET) | Get All | ? | ? | ? | ? | ? |

## ?? Response Variations

### 1. **POST /api/assembly-methods** (Staff/Manager Only)
Creates a new assembly method.

**Authorization**: Requires `catalog:assembly-methods:manage` permission

**Response**: `201 Created`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

**Status Codes**:
- `201 Created` - Success
- `400 Bad Request` - Validation failed
- `401 Unauthorized` - Not authenticated
- `403 Forbidden` - No permission
- `409 Conflict` - Slug already exists

---

### 2. **PUT /api/assembly-methods/{id}** (Staff/Manager Only)
Updates an existing assembly method including `isActive` status.

**Authorization**: Requires `catalog:assembly-methods:manage` permission

**Response**: `204 No Content`

**Status Codes**:
- `204 No Content` - Success
- `400 Bad Request` - Validation failed
- `401 Unauthorized` - Not authenticated
- `403 Forbidden` - No permission
- `404 Not Found` - Assembly method not found
- `409 Conflict` - Slug already exists

---

### 3. **DELETE /api/assembly-methods/{id}** (Staff/Manager Only)
Deletes an assembly method.

**Authorization**: Requires `catalog:assembly-methods:manage` permission

**Response**: `204 No Content`

**Status Codes**:
- `204 No Content` - Success
- `401 Unauthorized` - Not authenticated
- `403 Forbidden` - No permission
- `404 Not Found` - Assembly method not found

---

### 4. **GET /api/assembly-methods/{id}** (Staff/Manager Only)
Retrieves full details of an assembly method by ID.

**Authorization**: Requires `catalog:assembly-methods:view` permission

**Response**: `200 OK`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Snap-Fit",
  "slug": "snap-fit",
  "description": "Assembly using snap-fit connections",
  "isActive": true,
  "createdAt": "2024-02-27T10:30:00Z",
  "updatedAt": "2024-02-27T10:30:00Z"
}
```

**Status Codes**:
- `200 OK` - Success
- `401 Unauthorized` - Not authenticated
- `403 Forbidden` - No permission
- `404 Not Found` - Assembly method not found

---

### 5. **GET /api/assembly-methods/slug/{slug}** (Public)
Retrieves an active assembly method by slug. **Open to everyone**.

**Authorization**: Allow Anonymous

**Response**: `200 OK` - Public DTO (no timestamps, only active items)
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Snap-Fit",
  "slug": "snap-fit",
  "description": "Assembly using snap-fit connections"
}
```

**Features**:
- ? No authentication required
- ? Only returns items where `isActive = true`
- ? No `createdAt`, `updatedAt`, or `isActive` in response
- ? Returns 404 if slug not found OR not active

**Status Codes**:
- `200 OK` - Success
- `404 Not Found` - Assembly method not found or not active

---

### 6. **GET /api/assembly-methods** (Public with Role-based Response)
Retrieves paginated list of assembly methods. **Response varies by role**.

**Authorization**: Allow Anonymous

**Query Parameters**:
```
pageNumber: int = 1
pageSize: int = 10 (max 100)
searchTerm: string? (searches name, slug, description)
isActive: bool? (only works for Staff/Manager)
```

#### For Staff/Manager (Full Access):
**Response**: `200 OK`
```json
{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "Snap-Fit",
      "slug": "snap-fit",
      "description": "Assembly using snap-fit connections",
      "isActive": true,
      "createdAt": "2024-02-27T10:30:00Z",
      "updatedAt": "2024-02-27T10:30:00Z"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 50,
  "totalPages": 5,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

**Features**:
- ? See ALL items (active and inactive)
- ? Full details with timestamps
- ? Can filter by `isActive`
- ? Can search

#### For Anonymous/Customer/Admin (Limited Access):
**Response**: `200 OK`
```json
{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "Snap-Fit",
      "slug": "snap-fit",
      "description": "Assembly using snap-fit connections"
    }
  ],
  "pageNumber": 1,
  "pageSize": 10,
  "totalCount": 30,
  "totalPages": 3,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

**Features**:
- ? See ONLY active items (`isActive = true`)
- ? No timestamps in response
- ? No `isActive` field in response
- ? `isActive` filter parameter is ignored
- ? Can search

**Status Codes**:
- `200 OK` - Success
- `400 Bad Request` - Invalid pagination parameters

---

## ?? Permission Setup

### Staff Role Permissions
```csharp
catalog:assembly-methods:view
catalog:assembly-methods:manage
```

### Manager Role Permissions
```csharp
catalog:assembly-methods:view
catalog:assembly-methods:manage
```

### Customer/Admin/Anonymous
No special permissions needed - only access public endpoints.

---

## ?? Testing Scenarios

### Scenario 1: Anonymous User
```bash
# ? Can access
GET /api/assembly-methods/slug/snap-fit
GET /api/assembly-methods?pageNumber=1&pageSize=10

# ? Cannot access (401 Unauthorized)
POST /api/assembly-methods
PUT /api/assembly-methods/{id}
DELETE /api/assembly-methods/{id}
GET /api/assembly-methods/{id}
```

### Scenario 2: Customer/Admin User (Authenticated but not Staff/Manager)
```bash
# ? Can access (same as anonymous)
GET /api/assembly-methods/slug/snap-fit
GET /api/assembly-methods?pageNumber=1&pageSize=10

# ? Cannot access (403 Forbidden)
POST /api/assembly-methods
PUT /api/assembly-methods/{id}
DELETE /api/assembly-methods/{id}
GET /api/assembly-methods/{id}
```

### Scenario 3: Staff/Manager User
```bash
# ? Can access ALL endpoints
POST /api/assembly-methods
PUT /api/assembly-methods/{id}
DELETE /api/assembly-methods/{id}
GET /api/assembly-methods/{id}
GET /api/assembly-methods/slug/snap-fit
GET /api/assembly-methods?pageNumber=1&pageSize=10&isActive=false
```

---

## ?? Data Visibility Matrix

| Field | Anonymous/Customer/Admin | Staff/Manager |
|-------|-------------------------|---------------|
| id | ? | ? |
| name | ? | ? |
| slug | ? | ? |
| description | ? | ? |
| isActive | ? | ? |
| createdAt | ? | ? |
| updatedAt | ? | ? |

---

## ?? Implementation Details

### Role Detection
```csharp
var isStaffOrManager = _currentUser.IsAuthenticated && 
    (_currentUser.IsInRole("Staff") || _currentUser.IsInRole("Manager"));
```

### Data Filtering
- **Anonymous/Customer/Admin**: Automatically filtered to `isActive = true`
- **Staff/Manager**: Can see all items, can filter by `isActive`

### Response DTOs
- `GetAllAssemblyMethodsResponseDto` - Full details with timestamps (Staff/Manager)
- `GetAllAssemblyMethodsPublicResponseDto` - Limited details without timestamps (Public)
- `GetAssemblyMethodByIdResponseDto` - Full details (Staff/Manager only)
- `GetAssemblyMethodBySlugPublicResponseDto` - Public details (Everyone)
