# H??ng d?n Setup Database cho User Module

## ?? Module User s? d?ng IdentityDbContext có s?n t? SharedKernel

Module User **KHÔNG C?N** t?o DbContext riêng vì nó s? d?ng **IdentityDbContext** t? SharedKernel.Infrastructure ?? qu?n lý:
- Users (ApplicationUser)
- Roles (ApplicationRole)
- Permissions
- UserRoles, RolePermissions, UserPermissions

## ?? B??c 1: Cài ??t EF Core Tools (n?u ch?a có)

```powershell
dotnet tool install --global dotnet-ef
# Ho?c update n?u ?ã có
dotnet tool update --global dotnet-ef
```

## ?? B??c 2: Ki?m tra Connection String

M? `src/WebApi/PuzKit3D.WebApi/appsettings.json` và c?p nh?t:

```json
{
  "ConnectionStrings": {
    "IdentityConnection": "Host=localhost;Port=5432;Database=puzkit3d;Username=postgres;Password=your_password"
  }
}
```

**Chú ý**: Thay `your_password` b?ng password PostgreSQL c?a b?n.

## ?? B??c 3: ??m b?o PostgreSQL ?ang ch?y

Ki?m tra PostgreSQL service:

```powershell
# Ki?m tra service PostgreSQL
Get-Service -Name postgresql*

# Ho?c ki?m tra b?ng psql
psql -U postgres -c "SELECT version();"
```

## ?? B??c 4: T?o Migration cho IdentityDbContext

T? th? m?c root c?a solution, ch?y:

```powershell
# Di chuy?n ??n th? m?c WebApi
cd src\WebApi\PuzKit3D.WebApi

# T?o migration ??u tiên cho Identity
dotnet ef migrations add InitialIdentitySchema `
    --context IdentityDbContext `
    --project ..\..\SharedKernel\PuzKit3D.SharedKernel.Infrastructure `
    --startup-project . `
    --output-dir Identity/Migrations

# Quay l?i root
cd ..\..\..
```

## ?? B??c 5: T?o Database và Apply Migration

```powershell
cd src\WebApi\PuzKit3D.WebApi

# T?o/Update database
dotnet ef database update `
    --context IdentityDbContext `
    --project ..\..\SharedKernel\PuzKit3D.SharedKernel.Infrastructure `
    --startup-project .

cd ..\..\..
```

## ?? B??c 6: Seed Data Test User (Optional)

### Option A: S? d?ng Code

Thêm vào `Program.cs` sau dòng `var app = builder.Build();`:

```csharp
// Seed test user for development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    
    // Ensure roles exist
    var roles = new[] { "Admin", "User", "Manager" };
    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new ApplicationRole 
            { 
                Name = roleName,
                NormalizedName = roleName.ToUpper()
            });
        }
    }
    
    // Create test user
    var testEmail = "admin@puzkit3d.com";
    var existingUser = await userManager.FindByEmailAsync(testEmail);
    
    if (existingUser == null)
    {
        var testUser = new ApplicationUser
        {
            UserName = testEmail,
            Email = testEmail,
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };
        
        var result = await userManager.CreateAsync(testUser, "Admin123!@#");
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(testUser, "Admin");
        }
    }
}
```

### Option B: S? d?ng SQL Script

```sql
-- K?t n?i vào database
psql -U postgres -d puzkit3d

-- Ki?m tra tables ?ã ???c t?o
\dt identity.*

-- Test query
SELECT * FROM identity."ApplicationUser";
```

## ?? B??c 7: Test API Login

### 1. Ch?y ?ng d?ng
```powershell
cd src\WebApi\PuzKit3D.WebApi
dotnet run
```

### 2. Truy c?p Swagger
```
http://localhost:5000/swagger
```

### 3. Test Login Endpoint

**Endpoint**: POST /api/auth/login

**Request Body**:
```json
{
  "email": "admin@puzkit3d.com",
  "password": "Admin123!@#"
}
```

### 4. Test v?i curl (PowerShell)
```powershell
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
$response | ConvertTo-Json -Depth 10

# L?u token ?? s? d?ng
$token = $response.token
Write-Host "Token: $token"
```

## ?? Troubleshooting

### L?i: "Connection refused" ho?c không connect ???c PostgreSQL

```powershell
# Ki?m tra PostgreSQL có ?ang ch?y không
Get-Service -Name postgresql*

# Ho?c start service
Start-Service postgresql-x64-14  # Thay ??i tên service theo version
```

### L?i: "Database does not exist"

```bash
# T?o database th? công
psql -U postgres
CREATE DATABASE puzkit3d;
\q
```

### L?i: "Migration already applied"

```powershell
# Xem danh sách migrations
dotnet ef migrations list --context IdentityDbContext --project src\SharedKernel\PuzKit3D.SharedKernel.Infrastructure --startup-project src\WebApi\PuzKit3D.WebApi

# Rollback n?u c?n
dotnet ef database update <previous-migration-name> --context IdentityDbContext --project src\SharedKernel\PuzKit3D.SharedKernel.Infrastructure --startup-project src\WebApi\PuzKit3D.WebApi

# Xóa migration
dotnet ef migrations remove --context IdentityDbContext --project src\SharedKernel\PuzKit3D.SharedKernel.Infrastructure --startup-project src\WebApi\PuzKit3D.WebApi
```

## ?? Schema ???c t?o

Migration s? t?o các tables trong schema `identity`:

- `ApplicationUser` - Thông tin users
- `ApplicationRole` - Các roles (Admin, User, Manager, Staff, Customer)
- `ApplicationUserRole` - Quan h? User-Role
- `ApplicationUserPermission` - Permissions cho t?ng user
- `ApplicationRolePermission` - Permissions cho t?ng role
- `ApplicationUserClaim` - User claims
- `ApplicationUserLogin` - External logins
- `ApplicationUserToken` - User tokens
- `ApplicationRoleClaim` - Role claims

## ? Xác nh?n Setup thành công

Sau khi ch?y migration, ki?m tra:

```powershell
# 1. Ki?m tra tables ?ã ???c t?o
psql -U postgres -d puzkit3d -c "\dt identity.*"

# 2. Ki?m tra roles ?ã ???c seed
psql -U postgres -d puzkit3d -c "SELECT * FROM identity.\"ApplicationRole\";"

# 3. Ki?m tra users (n?u ?ã seed)
psql -U postgres -d puzkit3d -c "SELECT \"Id\", \"Email\", \"UserName\" FROM identity.\"ApplicationUser\";"
```

## ?? Tóm t?t Các B??c

```powershell
# 1. Cài ??t EF Core Tools
dotnet tool install --global dotnet-ef

# 2. T?o Migration
cd src\WebApi\PuzKit3D.WebApi
dotnet ef migrations add InitialIdentitySchema --context IdentityDbContext --project ..\..\SharedKernel\PuzKit3D.SharedKernel.Infrastructure --startup-project . --output-dir Identity/Migrations

# 3. T?o/Update Database
dotnet ef database update --context IdentityDbContext --project ..\..\SharedKernel\PuzKit3D.SharedKernel.Infrastructure --startup-project .

# 4. Ch?y ?ng d?ng
dotnet run

# 5. Test v?i Swagger: http://localhost:5000/swagger
```

Sau ?ó b?n có th? login v?i user ?ã seed ho?c t?o user m?i!
