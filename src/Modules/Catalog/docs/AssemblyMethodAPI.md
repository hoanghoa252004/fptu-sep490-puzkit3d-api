# Assembly Method API Documentation

## ?? Base URL
```
/api/assembly-methods
```

## ?? Endpoints

### 1. Create Assembly Method
**POST** `/api/assembly-methods`

Creates a new assembly method.

**Request Body:**
```json
{
  "name": "Snap-Fit",
  "slug": "snap-fit",
  "description": "Assembly using snap-fit connections",
  "isActive": true
}
```

**Response:** `201 Created`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

**Error Responses:**
- `400 Bad Request` - Validation failed
- `409 Conflict` - Slug already exists
- `500 Internal Server Error` - Server error

---

### 2. Update Assembly Method
**PUT** `/api/assembly-methods/{id}`

Updates an existing assembly method including its active status.

**Request Body:**
```json
{
  "name": "Snap-Fit Updated",
  "slug": "snap-fit-updated",
  "description": "Updated description",
  "isActive": false
}
```

**Response:** `204 No Content`

**Error Responses:**
- `400 Bad Request` - Validation failed
- `404 Not Found` - Assembly method not found
- `409 Conflict` - Slug already exists
- `500 Internal Server Error` - Server error

---

### 3. Delete Assembly Method
**DELETE** `/api/assembly-methods/{id}`

Deletes an assembly method by ID.

**Response:** `204 No Content`

**Error Responses:**
- `404 Not Found` - Assembly method not found
- `500 Internal Server Error` - Server error

---

### 4. Get Assembly Method by ID
**GET** `/api/assembly-methods/{id}`

Retrieves an assembly method by its unique identifier.

**Response:** `200 OK`
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

**Error Responses:**
- `404 Not Found` - Assembly method with ID not found
- `500 Internal Server Error` - Server error

---

### 5. Get Assembly Method by Slug
**GET** `/api/assembly-methods/slug/{slug}`

Retrieves an assembly method by its slug identifier.

**Example:** `/api/assembly-methods/slug/snap-fit`

**Response:** `200 OK`
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

**Error Responses:**
- `404 Not Found` - Assembly method with slug '{slug}' not found
- `500 Internal Server Error` - Server error

---

## ? Features

- ? **CQRS Pattern** - Commands and Queries separated
- ? **FluentValidation** - Input validation for all requests
- ? **Repository Pattern** - Clean data access
- ? **UnitOfWork Pattern** - Transaction management
- ? **Domain Events** - Event-driven architecture support
- ? **Result Pattern** - Type-safe error handling
- ? **Strongly Typed IDs** - Type safety for identifiers
- ? **Slug Validation** - Enforces lowercase alphanumeric with hyphens
- ? **Duplicate Prevention** - Unique slug constraint
- ? **Swagger Documentation** - Auto-generated API docs

## ?? Validation Rules

### Name
- ? Required
- ? Max length: 30 characters

### Slug
- ? Required
- ? Max length: 30 characters
- ? Format: `^[a-z0-9]+(?:-[a-z0-9]+)*$` (lowercase, alphanumeric, hyphens)
- ? Must be unique

### Description
- ? Optional
- ? No length limit (text field in DB)

### IsActive
- ? Boolean (default: false on create)
- ? Can be updated via PUT endpoint

## ?? Testing with Swagger

1. Run the application
2. Navigate to `/swagger`
3. Find the "Assembly Methods" tag
4. Try all 5 endpoints with sample data

## ?? Database Schema

Table: `catalog.assembly_method`

| Column | Type | Constraints |
|--------|------|-------------|
| id | uuid | PRIMARY KEY |
| name | varchar(30) | NOT NULL |
| slug | varchar(30) | NOT NULL, UNIQUE |
| description | text | NULL |
| is_active | boolean | NOT NULL, DEFAULT false |
| created_at | timestamp with time zone | NOT NULL |
| updated_at | timestamp with time zone | NOT NULL |
