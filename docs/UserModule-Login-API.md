# API Login - User Module

## T?ng quan
API Login cho module User ?ã ???c t?o thành công theo ki?n trúc Clean Architecture v?i các layer:
- **Domain Layer**: ??nh ngh?a các error types
- **Application Layer**: Command, Handler và Validator
- **API Layer**: Endpoint definition

## C?u trúc File

### Domain Layer
- `src/Modules/User/PuzKit3D.Modules.User.Domain/Entities/UserError.cs` - ??nh ngh?a các error codes
- `src/Modules/User/PuzKit3D.Modules.User.Domain/AssemblyReference.cs` - Assembly reference

### Application Layer
- `src/Modules/User/PuzKit3D.Modules.User.Application/UseCases/Authentication/Login/LoginCommand.cs` - Command definition
- `src/Modules/User/PuzKit3D.Modules.User.Application/UseCases/Authentication/Login/LoginCommandHandler.cs` - Business logic
- `src/Modules/User/PuzKit3D.Modules.User.Application/UseCases/Authentication/Login/LoginCommandValidator.cs` - Validation rules
- `src/Modules/User/PuzKit3D.Modules.User.Application/AssemblyReference.cs` - Assembly reference

### API Layer
- `src/Modules/User/PuzKit3D.Modules.User.Api/Authentication/Login/Login.cs` - Endpoint definition
- `src/Modules/User/PuzKit3D.Modules.User.Api/EndpointGroupExtension.cs` - Route group helpers
- `src/Modules/User/PuzKit3D.Modules.User.Api/UserApiAssembly.cs` - Assembly reference

### Infrastructure Layer
- `src/Modules/User/PuzKit3D.Modules.User.Infrastructure/UserModule.cs` - Module registration

## API Endpoint

### POST /api/auth/login

Xác th?c ng??i dùng và tr? v? JWT token.

#### Request Body
```json
{
  "email": "user@example.com",
  "password": "YourPassword123!"
}
```

#### Response Success (200 OK)
```json
{
  "userId": "guid-here",
  "email": "user@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh-token-here",
  "expiresAt": "2024-01-01T12:00:00Z"
}
```

#### Response Error (400 Bad Request) - Validation Failed
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

#### Response Error (401 Unauthorized) - Invalid Credentials
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Authentication.InvalidCredentials",
  "status": 401,
  "detail": "Invalid email or password"
}
```

## Validation Rules

- **Email**:
  - B?t bu?c
  - Ph?i là email h?p l?
  
- **Password**:
  - B?t bu?c
  - T?i thi?u 6 ký t?

## Cách Test API

### 1. S? d?ng Swagger UI
1. Ch?y ?ng d?ng
2. Truy c?p: `http://localhost:5000/swagger` (ho?c port c?a b?n)
3. Tìm endpoint **POST /api/auth/login** trong group **Authentication**
4. Click "Try it out"
5. Nh?p email và password
6. Click "Execute"

### 2. S? d?ng curl
```bash
curl -X POST "http://localhost:5000/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "user@example.com",
    "password": "YourPassword123!"
  }'
```

### 3. S? d?ng PowerShell
```powershell
$body = @{
    email = "user@example.com"
    password = "YourPassword123!"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/auth/login" `
    -Method Post `
    -Body $body `
    -ContentType "application/json"
```

### 4. S? d?ng Postman
1. T?o request m?i v?i method POST
2. URL: `http://localhost:5000/api/auth/login`
3. Headers: `Content-Type: application/json`
4. Body (raw JSON):
```json
{
  "email": "user@example.com",
  "password": "YourPassword123!"
}
```

## T?o User Test

Tr??c khi test login, b?n c?n t?o user trong database. B?n có th?:

### Option 1: S? d?ng API Register (n?u có)
```bash
curl -X POST "http://localhost:5000/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123!@#",
    "firstName": "Test",
    "lastName": "User"
  }'
```

### Option 2: S? d?ng EF Core Migration ho?c Seed Data

Thêm vào `Program.cs` ?? t? ??ng t?o user test:

```csharp
// Trong Program.cs, sau khi build app
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    
    if (!await userManager.Users.AnyAsync())
    {
        var testUser = new ApplicationUser
        {
            UserName = "test@example.com",
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            EmailConfirmed = true
        };
        
        await userManager.CreateAsync(testUser, "Test123!@#");
        await userManager.AddToRoleAsync(testUser, "User");
    }
}
```

## JWT Token Usage

Sau khi login thành công, s? d?ng token trong các request ti?p theo:

```bash
curl -X GET "http://localhost:5000/api/protected-endpoint" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```

## Configuration

??m b?o `appsettings.json` có c?u hình ?úng:

```json
{
  "ConnectionStrings": {
    "IdentityConnection": "Host=localhost;Port=5432;Database=puzkit3d;Username=postgres;Password=your_password"
  },
  "Jwt": {
    "SecretKey": "your-super-secret-key-min-32-characters-long-for-security",
    "Issuer": "PuzKit3D",
    "Audience": "PuzKit3D.WebApi",
    "ExpirationInMinutes": 60
  }
}
```

## Dependencies

Module User s? d?ng các service t? SharedKernel:
- `IIdentityService` - X? lý authentication
- `IJwtProvider` - T?o JWT tokens
- `UserManager<ApplicationUser>` - ASP.NET Core Identity
- `SignInManager<ApplicationUser>` - ASP.NET Core Identity

## Notes

- API endpoint là **AllowAnonymous** - không c?n authentication
- Password ???c hash t? ??ng b?i ASP.NET Core Identity
- Refresh token ???c l?u trong database và có th?i h?n 7 ngày
- Token có th?i h?n 60 phút (có th? c?u hình trong appsettings.json)
- Validation ???c x? lý t? ??ng b?i FluentValidation pipeline
