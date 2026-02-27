# Seeded Users & Create Staff/Manager API

## ?? Default Seeded Accounts

### 1. Admin Account
- **Email**: `admin@puzkit3d.com`
- **Password**: `Admin@123456`
- **Role**: System Administrator
- **Permissions**: Full user management access

### 2. Manager Account
- **Email**: `manager@puzkit3d.com`
- **Password**: `Manager@123456`
- **Role**: Business Manager
- **Permissions**: Catalog management (view/manage assembly methods, topics, materials, capabilities)

### 3. Staff Accounts

#### Staff 1
- **Email**: `staff1@puzkit3d.com`
- **Password**: `Staff@123456`
- **Name**: John Staff
- **Role**: Staff
- **Permissions**: Catalog management (same as Manager)

#### Staff 2
- **Email**: `staff2@puzkit3d.com`
- **Password**: `Staff@123456`
- **Name**: Jane Staff
- **Role**: Staff

#### Staff 3
- **Email**: `staff3@puzkit3d.com`
- **Password**: `Staff@123456`
- **Name**: Mike Staff
- **Role**: Staff

---

## ?? Role Permissions Matrix

| Permission | Admin | Manager | Staff | Customer |
|------------|-------|---------|-------|----------|
| **Users Management** |
| users:view | ? | ? | ? | ? |
| users:create | ? | ? | ? | ? |
| users:update | ? | ? | ? | ? |
| users:delete | ? | ? | ? | ? |
| users:roles:manage | ? | ? | ? | ? |
| users:permissions:manage | ? | ? | ? | ? |
| **Catalog Management** |
| catalog:assembly-methods:view | ? | ? | ? | ? |
| catalog:assembly-methods:manage | ? | ? | ? | ? |
| catalog:topics:view | ? | ? | ? | ? |
| catalog:topics:manage | ? | ? | ? | ? |
| catalog:materials:view | ? | ? | ? | ? |
| catalog:materials:manage | ? | ? | ? | ? |
| catalog:capabilities:view | ? | ? | ? | ? |
| catalog:capabilities:manage | ? | ? | ? | ? |
| **InStock** |
| instock:products:view | ? | ? | ? | ? |
| instock:orders:view | ? | ? | ? | ? |

---

## ?? New API: Create Staff or Manager

### POST /api/users/staff-or-manager

Creates a new Staff or Manager account. **Admin only**.

**Authorization**: Requires `users:create` permission (Admin only)

**Request Body:**
```json
{
  "email": "newstaff@puzkit3d.com",
  "password": "Password@123456",
  "role": "Staff",
  "firstName": "New",
  "lastName": "Staff"
}
```

**Accepted Roles:**
- `"Staff"`
- `"Manager"`
- `"Business Manager"`

**Response**: `201 Created`
```json
{
  "message": "User with email newstaff@puzkit3d.com created successfully with role Staff"
}
```

**Error Responses:**
- `400 Bad Request` - Validation failed or invalid role
- `401 Unauthorized` - Not authenticated
- `403 Forbidden` - Not Admin role
- `409 Conflict` - Email already exists
- `500 Internal Server Error` - Server error

---

## ?? Testing Scenarios

### 1. Login as Admin
```bash
POST /api/auth/login
{
  "email": "admin@puzkit3d.com",
  "password": "Admin@123456"
}
```

### 2. Create Staff Account (as Admin)
```bash
POST /api/users/staff-or-manager
Authorization: Bearer {admin_token}
{
  "email": "staff4@puzkit3d.com",
  "password": "NewStaff@123",
  "role": "Staff",
  "firstName": "New",
  "lastName": "Staff Member"
}
```

### 3. Login as Manager
```bash
POST /api/auth/login
{
  "email": "manager@puzkit3d.com",
  "password": "Manager@123456"
}
```

### 4. Test Catalog APIs as Manager
```bash
# Manager can create assembly methods
POST /api/assembly-methods
Authorization: Bearer {manager_token}
{
  "name": "Snap-Fit",
  "slug": "snap-fit",
  "description": "Test",
  "isActive": true
}

# Manager can view full details
GET /api/assembly-methods/{id}
Authorization: Bearer {manager_token}
```

### 5. Test as Anonymous User
```bash
# Can access public endpoints
GET /api/assembly-methods/slug/snap-fit

# Cannot access protected endpoints (401 Unauthorized)
POST /api/assembly-methods
```

---

## ?? Migration Applied

Migration `SeedDefaultUsersAndPermissions` includes:
- ? 5 default users (1 Admin, 1 Manager, 3 Staff)
- ? User-Role assignments
- ? Role-Permission assignments
- ? All passwords are hashed with ASP.NET Identity hasher

---

## ?? Security Notes

1. **Default passwords** should be changed immediately in production
2. **Admin role** has full control over user management
3. **Staff and Manager** can only manage catalog, NOT users
4. **Email confirmation** is set to `true` for seeded accounts
5. **Created users** are immediately active and can login

---

## ?? Role Name Mapping

| Database Role Name | Normalized Name | Used in Code |
|-------------------|-----------------|--------------|
| System Administrator | SYSTEM_ADMINISTRATOR | "System Administrator" |
| Business Manager | BUSINESS_MANAGER | "Business Manager" or "Manager" |
| Staff | STAFF | "Staff" |
| Customer | CUSTOMER | "Customer" |

**Note**: API accepts both "Manager" and "Business Manager" for the Manager role.
