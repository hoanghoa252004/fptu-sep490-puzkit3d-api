# Swagger JWT Authorization Setup Guide

## ? JWT Configuration Completed

Swagger UI ?ã ???c c?u hình ?? h? tr? JWT Bearer token authentication.

---

## ?? How to Use JWT Authorization in Swagger

### Step 1: Start the Application
```bash
dotnet run --project src\WebApi\PuzKit3D.WebApi
```

### Step 2: Navigate to Swagger UI
Open browser and go to:
```
http://localhost:5000/swagger
```
or
```
https://localhost:5001/swagger
```

### Step 3: Login to Get JWT Token

1. Find the **Authentication** section in Swagger
2. Expand **POST /api/auth/login**
3. Click **"Try it out"**
4. Enter credentials:

#### Admin Account:
```json
{
  "email": "admin@puzkit3d.com",
  "password": "Admin@123456"
}
```

#### Manager Account:
```json
{
  "email": "manager@puzkit3d.com",
  "password": "Manager@123456"
}
```

#### Staff Account:
```json
{
  "email": "staff1@puzkit3d.com",
  "password": "Staff@123456"
}
```

5. Click **Execute**
6. Copy the **token** (NOT accessToken) from the response

### Step 4: Authorize in Swagger

1. Click the **"Authorize"** button (?? lock icon) at the top right of Swagger UI
2. In the popup window, paste your token in the **Value** field
3. Click **"Authorize"**
4. Click **"Close"**

### Step 5: Test Protected Endpoints

Now you can test protected endpoints:

```bash
# ? These will work with Manager/Staff token:
POST /api/assembly-methods
PUT /api/assembly-methods/{id}
DELETE /api/assembly-methods/{id}
GET /api/assembly-methods/{id}

# ? These will work with Admin token:
POST /api/users/staff-or-manager
```

---

## ?? Testing Different Authorization Scenarios

### Scenario 1: Test as Anonymous (No Token)
1. **Don't** click Authorize or logout if already authorized
2. Try calling:
   - ? `GET /api/assembly-methods/slug/{slug}` - Should work
   - ? `GET /api/assembly-methods` - Should work (returns active items only)
   - ? `POST /api/assembly-methods` - Should return **401 Unauthorized**
   - ? `GET /api/assembly-methods/{id}` - Should return **401 Unauthorized**

### Scenario 2: Test as Manager
1. Login as Manager and copy token
2. Click Authorize and paste token
3. Try calling:
   - ? `POST /api/assembly-methods` - Should work
   - ? `GET /api/assembly-methods/{id}` - Should work (full details)
   - ? `GET /api/assembly-methods` - Should work (see all items)
   - ? `POST /api/users/staff-or-manager` - Should return **403 Forbidden**

### Scenario 3: Test as Admin
1. Login as Admin and copy token
2. Click Authorize and paste token
3. Try calling:
   - ? `POST /api/users/staff-or-manager` - Should work
   - ? `POST /api/assembly-methods` - Should return **403 Forbidden** (Admin cannot manage catalog)

---

## ?? Swagger Configuration Details

### Security Definition
```csharp
options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JWT Authorization header using the Bearer scheme."
});
```

### Security Requirement
```csharp
options.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }
});
```

---

## ?? Token Format

When you authorize in Swagger, just paste the token value:
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c
```

**Do NOT** add "Bearer " prefix - Swagger will add it automatically!

---

## ?? Quick Test Checklist

- [ ] Can see ?? Authorize button in Swagger UI
- [ ] Can login and get token
- [ ] Can paste token and authorize
- [ ] Protected endpoints show ?? lock icon
- [ ] Can successfully call protected endpoints with valid token
- [ ] Get 401 when calling protected endpoints without token
- [ ] Get 403 when calling endpoints without proper permission

---

## ?? Swagger UI Features After Configuration

1. **?? Authorize Button**: Top right corner - click to enter token
2. **Lock Icons**: Protected endpoints show lock icon
3. **Try It Out**: Test APIs directly from browser
4. **Token Persistence**: Token remains active during browser session
5. **Logout**: Click Authorize ? Logout to remove token

---

## ?? Ready to Test!

Your Swagger is now configured with JWT authentication. Build successful! Start the application and test the authorization flow.
