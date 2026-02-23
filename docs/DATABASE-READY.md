# ? DATABASE ?Ã S?N SÀNG! 

## ?? Migration ?ã ???c apply thành công vào database: `puzkit3d_test`

### ?? Database Schema ?ã t?o:
- ? **identity.identity_user** - B?ng Users
- ? **identity.identity_role** - B?ng Roles (?ã seed 4 roles: System Administrator, Business Manager, Staff, Customer)
- ? **identity.identity_user_role** - Quan h? User-Role
- ? **identity.UserPermissions** - User permissions
- ? **identity.RolePermissions** - Role permissions
- ? Các b?ng Identity khác (claims, tokens, logins)

---

## ?? CÁCH S? D?NG NGAY

### Option 1: Ch?y App (Khuy?n ngh? - T? ??ng seed test users)

```powershell
# Ch?y script helper
.\scripts\run-api.ps1

# Ho?c ch?y tr?c ti?p
cd src\WebApi\PuzKit3D.WebApi
dotnet run
```

**App s? t? ??ng t?o 2 test users:**
- ?? Admin: `admin@puzkit3d.com` / `Admin123!@#`
- ?? User: `user@puzkit3d.com` / `User123!@#`

### Option 2: Test Login v?i Swagger UI

1. Truy c?p: http://localhost:5000/swagger
2. Tìm endpoint **POST /api/auth/login** (trong group Authentication)
3. Click "Try it out"
4. Nh?p:
```json
{
  "email": "admin@puzkit3d.com",
  "password": "Admin123!@#"
}
```
5. Click "Execute"
6. Nh?n ???c JWT token! ??

### Option 3: Test b?ng PowerShell

```powershell
# Test Login
$body = @{
    email = "admin@puzkit3d.com"
    password = "Admin123!@#"
} | ConvertTo-Json

$response = Invoke-RestMethod `
    -Uri "http://localhost:5000/api/auth/login" `
    -Method Post `
    -Body $body `
    -ContentType "application/json"

# Hi?n th? k?t qu?
Write-Host "? Login Success!"
Write-Host "User ID: $($response.userId)"
Write-Host "Email: $($response.email)"
Write-Host "Token: $($response.token)"
Write-Host "Expires: $($response.expiresAt)"
```

---

## ?? Test Users Credentials

| Email | Password | Role |
|-------|----------|------|
| admin@puzkit3d.com | Admin123!@# | System Administrator |
| user@puzkit3d.com | User123!@# | Customer |

---

## ?? Ki?m tra Database

### Xem t?t c? tables:
```powershell
$env:PGPASSWORD='12345'
psql -U postgres -d puzkit3d_test -c "\dt identity.*"
```

### Xem Roles ?ã seed:
```powershell
$env:PGPASSWORD='12345'
psql -U postgres -d puzkit3d_test -c "SELECT * FROM identity.identity_role;"
```

### Xem Users (sau khi ch?y app):
```powershell
$env:PGPASSWORD='12345'
psql -U postgres -d puzkit3d_test -c "SELECT \"Id\", \"Email\", \"UserName\", \"FirstName\", \"LastName\" FROM identity.identity_user;"
```

---

## ?? API Endpoint Details

### POST /api/auth/login

**Request:**
```json
{
  "email": "admin@puzkit3d.com",
  "password": "Admin123!@#"
}
```

**Success Response (200 OK):**
```json
{
  "userId": "guid-here",
  "email": "admin@puzkit3d.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "random-refresh-token",
  "expiresAt": "2024-01-01T13:00:00Z"
}
```

**Error Response (400 Bad Request) - Validation:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Email": ["Email là b?t bu?c"],
    "Password": ["M?t kh?u ph?i có ít nh?t 6 ký t?"]
  }
}
```

**Error Response (401 Unauthorized) - Invalid Credentials:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Authentication.InvalidCredentials",
  "status": 401,
  "detail": "Invalid email or password"
}
```

---

## ?? S? d?ng JWT Token

Sau khi login thành công, s? d?ng token trong các API requests khác:

```powershell
$token = "your-jwt-token-here"

$headers = @{
    "Authorization" = "Bearer $token"
}

Invoke-RestMethod `
    -Uri "http://localhost:5000/api/protected-endpoint" `
    -Method Get `
    -Headers $headers
```

---

## ??? Các Commands H?u Ích

### Xem migrations ?ã apply:
```powershell
dotnet ef migrations list --context IdentityDbContext --project src\SharedKernel\PuzKit3D.SharedKernel.Infrastructure --startup-project src\WebApi\PuzKit3D.WebApi
```

### Rollback migration:
```powershell
dotnet ef database update 0 --context IdentityDbContext --project src\SharedKernel\PuzKit3D.SharedKernel.Infrastructure --startup-project src\WebApi\PuzKit3D.WebApi
```

### Drop database:
```powershell
$env:PGPASSWORD='12345'
psql -U postgres -c "DROP DATABASE IF EXISTS puzkit3d_test;"
```

### T?o l?i database t? ??u:
```powershell
# Drop database
$env:PGPASSWORD='12345'
psql -U postgres -c "DROP DATABASE IF EXISTS puzkit3d_test;"

# T?o l?i database
psql -U postgres -c "CREATE DATABASE puzkit3d_test;"

# Apply migration
dotnet ef database update --context IdentityDbContext --project src\SharedKernel\PuzKit3D.SharedKernel.Infrastructure --startup-project src\WebApi\PuzKit3D.WebApi
```

---

## ? T?ng k?t

? **Database**: puzkit3d_test ?ã ???c t?o và migration ?ã apply
? **Schema**: identity schema v?i ??y ?? tables
? **Roles**: 4 roles ?ã ???c seed
? **Test Users**: S? t? ??ng ???c t?o khi ch?y app
? **API Login**: S?n sàng t?i POST /api/auth/login

**Ch? c?n ch?y:**
```powershell
.\scripts\run-api.ps1
```

**Ho?c:**
```powershell
cd src\WebApi\PuzKit3D.WebApi
dotnet run
```

Sau ?ó test login t?i: **http://localhost:5000/swagger** ??
