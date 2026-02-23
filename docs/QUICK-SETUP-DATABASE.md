# ?? H??NG D?N SETUP DATABASE - CH? 3 B??C ??N GI?N

## ? MIGRATION ?Ã ???C T?O S?N!

Migration file ?ã có t?i:
```
src\SharedKernel\PuzKit3D.SharedKernel.Infrastructure\Identity\Migrations\
  - 20260223144741_InitialIdentitySchema.cs
  - 20260223144741_InitialIdentitySchema.Designer.cs
  - IdentityDbContextModelSnapshot.cs
```

---

## ?? B??C 1: C?P NH?T PASSWORD POSTGRESQL

M? file `src\WebApi\PuzKit3D.WebApi\appsettings.json` và s?a password ?úng:

```json
{
  "ConnectionStrings": {
    "IdentityConnection": "Host=localhost;Port=5432;Database=puzkit3d;Username=postgres;Password=YOUR_REAL_PASSWORD_HERE"
  }
}
```

**?? Thay `YOUR_REAL_PASSWORD_HERE` b?ng password PostgreSQL th?t c?a b?n!**

---

## ?? B??C 2: T?O DATABASE (n?u ch?a có)

M? PowerShell và ch?y:

```powershell
# Option 1: T?o database b?ng psql
psql -U postgres -c "CREATE DATABASE puzkit3d;"

# Option 2: Ho?c dùng pgAdmin ?? t?o database tên "puzkit3d"
```

**N?u database ?ã t?n t?i, b? qua b??c này!**

---

## ?? B??C 3: APPLY MIGRATION VÀO DATABASE

Ch?y l?nh sau t? th? m?c root c?a solution:

```powershell
dotnet ef database update `
    --context IdentityDbContext `
    --project src\SharedKernel\PuzKit3D.SharedKernel.Infrastructure `
    --startup-project src\WebApi\PuzKit3D.WebApi
```

**? Xong! Database ?ã s?n sàng!**

---

## ?? KI?M TRA DATABASE ?Ã T?O THÀNH CÔNG

```powershell
# Ki?m tra tables
psql -U postgres -d puzkit3d -c "\dt identity.*"

# Xem roles ?ã ???c seed
psql -U postgres -d puzkit3d -c "SELECT * FROM identity.\"ApplicationRole\";"
```

B?n s? th?y các roles ?ã ???c t?o s?n:
- ? System Administrator
- ? Business Manager
- ? Staff
- ? Customer

---

## ?? B??C 4: T?O USER TEST (Ch?n 1 trong 3 cách)

### Cách 1: Ch?y script PowerShell t? ??ng

L?u file này thành `create-test-user.ps1`:

```powershell
# create-test-user.ps1
$connectionString = "Host=localhost;Port=5432;Database=puzkit3d;Username=postgres;Password=YOUR_PASSWORD"

# SQL ?? t?o user test
$sql = @"
-- T?o user test
INSERT INTO identity.\"ApplicationUser\" 
(\"Id\", \"UserName\", \"NormalizedUserName\", \"Email\", \"NormalizedEmail\", \"EmailConfirmed\", 
 \"PasswordHash\", \"SecurityStamp\", \"ConcurrencyStamp\", \"FirstName\", \"LastName\", 
 \"CreatedAt\", \"IsDeleted\", \"PhoneNumberConfirmed\", \"TwoFactorEnabled\", \"LockoutEnabled\", \"AccessFailedCount\")
VALUES 
('test-user-001', 'test@puzkit3d.com', 'TEST@PUZKIT3D.COM', 'test@puzkit3d.com', 'TEST@PUZKIT3D.COM', true,
 'AQAAAAIAAYagAAAAEJ5xJzMHZqYvN8vYVxZGz1234567890abcdefghijklmnopqrstuvwxyz==', -- Password: Test123!@#
 NEWID(), NEWID(), 'Test', 'User', NOW(), false, false, false, false, 0)
ON CONFLICT (\"Id\") DO NOTHING;

-- Gán role User cho user test
INSERT INTO identity.\"ApplicationUserRole\" (\"UserId\", \"RoleId\")
SELECT 'test-user-001', \"Id\" 
FROM identity.\"ApplicationRole\" 
WHERE \"NormalizedName\" = 'CUSTOMER'
ON CONFLICT DO NOTHING;
"@

# Note: B?n c?n t?o password hash th?c b?ng ASP.NET Core Identity
Write-Host "?? Dùng cách 2 ho?c 3 ?? t?o user v?i password ?úng!" -ForegroundColor Yellow
```

### Cách 2: ? KHUY?N NGH? - Thêm seed code vào Program.cs

Thêm ?o?n code sau vào `src\WebApi\PuzKit3D.WebApi\Program.cs` **SAU** dòng `var app = builder.Build();`:

```csharp
// Seed test user for development
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    
    var testEmail = "test@puzkit3d.com";
    var existingUser = await userManager.FindByEmailAsync(testEmail);
    
    if (existingUser == null)
    {
        var testUser = new ApplicationUser
        {
            UserName = testEmail,
            Email = testEmail,
            FirstName = "Test",
            LastName = "User",
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };
        
        var result = await userManager.CreateAsync(testUser, "Test123!@#");
        
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(testUser, "Customer");
            Console.WriteLine($"? Test user created: {testEmail}");
        }
    }
}
```

**Sau ?ó ch?y ?ng d?ng 1 l?n ?? seed user:**
```powershell
cd src\WebApi\PuzKit3D.WebApi
dotnet run
```

### Cách 3: T?o API Register và dùng API

N?u mu?n, tôi có th? t?o thêm API Register ?? ??ng ký user m?i.

---

## ?? TEST LOGIN

Sau khi có user test, test ngay:

### Test v?i PowerShell:
```powershell
$body = @{
    email = "test@puzkit3d.com"
    password = "Test123!@#"
} | ConvertTo-Json

$response = Invoke-RestMethod `
    -Uri "http://localhost:5000/api/auth/login" `
    -Method Post `
    -Body $body `
    -ContentType "application/json"

# Hi?n th? token
Write-Host "? Login thành công!" -ForegroundColor Green
$response | ConvertTo-Json
```

### Test v?i Swagger:
1. Truy c?p: `http://localhost:5000/swagger`
2. Tìm endpoint: **POST /api/auth/login**
3. Click "Try it out"
4. Nh?p:
   - Email: `test@puzkit3d.com`
   - Password: `Test123!@#`
5. Click "Execute"

---

## ?? TÓM T?T: CH? C?N 3 L?NH

```powershell
# 1. S?a password trong appsettings.json (password: aa)

# 2. Apply migration (t?o tables)
dotnet ef database update --context IdentityDbContext --project src\SharedKernel\PuzKit3D.SharedKernel.Infrastructure --startup-project src\WebApi\PuzKit3D.WebApi

# 3. Thêm seed code vào Program.cs và ch?y app (t?o user test)
cd src\WebApi\PuzKit3D.WebApi
dotnet run
```

---

## ? LÀM SAO BI?T ?Ã SETUP THÀNH CÔNG?

Ki?m tra database:

```powershell
# Ki?m tra tables ?ã t?o
psql -U postgres -d puzkit3d -c "\dt identity.*"

# K?t qu? mong ??i:
# identity.ApplicationRole
# identity.ApplicationRoleClaim
# identity.ApplicationRolePermission
# identity.ApplicationUser
# identity.ApplicationUserClaim
# identity.ApplicationUserLogin
# identity.ApplicationUserPermission
# identity.ApplicationUserRole
# identity.ApplicationUserToken

# Ki?m tra roles
psql -U postgres -d puzkit3d -c "SELECT \"Name\" FROM identity.\"ApplicationRole\";"

# K?t qu?:
# System Administrator
# Business Manager
# Staff
# Customer
```

---

## ?? SAU KHI SETUP XONG

B?n có th?:
1. ? Login v?i user test: `test@puzkit3d.com` / `Test123!@#`
2. ? Nh?n JWT token
3. ? S? d?ng token ?? g?i các API ???c b?o v?

**Migration ?ã có s?n r?i, ch? c?n apply vào database thôi!** ??
