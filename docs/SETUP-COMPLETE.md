# ?? HOÀN T?T! API LOGIN ?Ã S?N SÀNG

## ? ?Ã HOÀN THÀNH

### 1. ? Migration & Database
- **Migration**: `InitialIdentitySchema` ?ã t?o trong `src\SharedKernel\PuzKit3D.SharedKernel.Infrastructure\Identity\Migrations\`
- **Database**: `puzkit3d_test` ?ã ???c t?o và áp d?ng migration
- **Schema**: `identity` v?i ??y ?? 9 tables
- **Roles**: 4 roles ?ã ???c seed t? ??ng:
  - System Administrator
  - Business Manager
  - Staff
  - Customer

### 2. ? API Login Module
- **Endpoint**: `POST /api/auth/login`
- **Pattern**: Clean Architecture (Domain ? Application ? API)
- **Authentication**: JWT Bearer Token
- **Validation**: FluentValidation
- **Location**: `src/Modules/User/`

### 3. ? Test Users (Auto-seed khi ch?y app)
- Admin: `admin@puzkit3d.com` / `Admin123!@#`
- User: `user@puzkit3d.com` / `User123!@#`

---

## ?? B?T ??U S? D?NG NGAY

### B??c 1: Ch?y ?ng d?ng
```powershell
.\scripts\run-api.ps1
```

Ho?c:
```powershell
cd src\WebApi\PuzKit3D.WebApi
dotnet run
```

### B??c 2: M? Swagger UI
```
http://localhost:5000/swagger
```

### B??c 3: Test Login
Trong Swagger, tìm endpoint **POST /api/auth/login** và th?:

```json
{
  "email": "admin@puzkit3d.com",
  "password": "Admin123!@#"
}
```

**Response:**
```json
{
  "userId": "guid",
  "email": "admin@puzkit3d.com",
  "token": "eyJhbGci...",
  "refreshToken": "...",
  "expiresAt": "2024-01-01T13:00:00Z"
}
```

---

## ?? C?u trúc Files ?ã t?o

### Module User
```
src/Modules/User/
??? Domain/
?   ??? Entities/
?   ?   ??? UserError.cs
?   ??? AssemblyReference.cs
??? Application/
?   ??? UseCases/Authentication/Login/
?   ?   ??? LoginCommand.cs
?   ?   ??? LoginCommandHandler.cs
?   ?   ??? LoginCommandValidator.cs
?   ??? AssemblyReference.cs
??? Api/
?   ??? Authentication/Login/
?   ?   ??? Login.cs
?   ??? EndpointGroupExtension.cs
?   ??? UserApiAssembly.cs
??? Infrastructure/
    ??? UserModule.cs
```

### WebApi
```
src/WebApi/PuzKit3D.WebApi/
??? Extensions/
?   ??? DatabaseSeeder.cs       # Seed test users
??? Program.cs                   # ?ã update
```

### Scripts
```
scripts/
??? setup-database.ps1          # Setup database 1 l?n
??? run-api.ps1                 # Ch?y API
??? test-login-api.ps1          # Test t? ??ng
```

### Documentation
```
docs/
??? DATABASE-READY.md           # T?ng quan database
??? QUICK-SETUP-DATABASE.md     # H??ng d?n setup
??? Setup-Database-For-Login.md # Chi ti?t setup
??? UserModule-Login-API.md     # API documentation
```

---

## ?? Thông tin quan tr?ng

### Database Connection
- **Host**: localhost
- **Port**: 5432
- **Database**: puzkit3d_test
- **Username**: postgres
- **Password**: 12345

### JWT Configuration
- **Issuer**: PuzKit3D
- **Audience**: PuzKit3D.WebApi
- **Token Expiration**: 60 phút
- **Refresh Token Expiration**: 7 ngày

### Test Credentials
| Email | Password | Role |
|-------|----------|------|
| admin@puzkit3d.com | Admin123!@# | System Administrator |
| user@puzkit3d.com | User123!@# | Customer |

---

## ?? API Endpoint

### POST /api/auth/login

**Request:**
```json
{
  "email": "admin@puzkit3d.com",
  "password": "Admin123!@#"
}
```

**Response (Success):**
```json
{
  "userId": "guid-string",
  "email": "admin@puzkit3d.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh-token",
  "expiresAt": "2024-01-01T13:00:00Z"
}
```

**Validation Rules:**
- Email: Required, Valid email format
- Password: Required, Min 6 characters

---

## ?? Các Commands H?u Ích

### Ki?m tra Database
```powershell
$env:PGPASSWORD='12345'
psql -U postgres -d puzkit3d_test
```

### Xem Users ?ã t?o
```sql
SELECT "Id", "Email", "UserName", "FirstName", "LastName" 
FROM identity.identity_user;
```

### Xem Roles
```sql
SELECT "Name", "Description" 
FROM identity.identity_role;
```

### Xem Migrations ?ã apply
```powershell
dotnet ef migrations list `
    --context IdentityDbContext `
    --project src\SharedKernel\PuzKit3D.SharedKernel.Infrastructure `
    --startup-project src\WebApi\PuzKit3D.WebApi
```

### Reset Database (n?u c?n)
```powershell
# Drop database
$env:PGPASSWORD='12345'
psql -U postgres -c "DROP DATABASE IF EXISTS puzkit3d_test;"

# Setup l?i t? ??u
.\scripts\setup-database.ps1
```

---

## ?? Tips

1. **Token ?ã h?t h?n?** ? Login l?i ?? l?y token m?i
2. **Test nhi?u scenarios** ? S? d?ng c? Admin và User accounts
3. **Development mode** ? Test users t? ??ng ???c t?o khi app start
4. **Production mode** ? C?n t?o users th? công ho?c qua API register

---

## ?? K?T LU?N

**? API Login ?ã hoàn toàn s?n sàng!**

B?n có th?:
- ? Login v?i test users
- ? Nh?n JWT token
- ? S? d?ng token ?? authenticate các API khác
- ? Refresh token khi h?t h?n

**Ti?p theo b?n có th? làm:**
- ?? T?o API Register
- ?? T?o API Refresh Token
- ?? T?o API Forgot Password
- ?? T?o API Change Password
- ?? T?o API Get User Profile

Chúc b?n coding vui v?! ??
