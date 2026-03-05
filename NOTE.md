# ADD MIGRATION & UPDATE DATABASE:

> ***Microsoft.EntityFrameworkCore.Design***: 
>
> 1. *Create migrations: `dotnet ef migrations add`*
>
> 2. *Apply migrations: `dotnet ef database update`*
>
> 3. *Remove migrations: `dotnet ef migrations remove`*
>
> 4. *Check migrations state: `dotnet ef migrations list`*
>
> 5. Generate SQL script from migrations: `dotnet ef migrations script`
>
> *Không cần trong runtime - chỉ cần khi development*

---

## 0. Navigate to the WebApi project directory:

```bash
cd D:\FPT_University\Semester_9\SEP490\BE\PuzKit3D\src\WebApi\PuzKit3D.WebApi
```

---

## 1. Create migration: `dotnet ef migrations add`

```bash
dotnet ef migrations add <NameOfMigration> --project ..\..\Modules\<NameOfModule>\PuzKit3D.Modules.<NameOfModule>.Persistence --context <NameOfDbContext>
```

### Example

```bash
dotnet ef migrations add AddTableUserReplica --project ..\..\Modules\Cart\PuzKit3D.Modules.Cart.Persistence --context CartDbContext
```

---

## 2. Update database: `dotnet ef database update`

```bash
dotnet ef database update --project ..\..\Modules\<NameOfModule>\PuzKit3D.Modules.<NameOfModule>.Persistence --context <NameOfDbContext>
```

### Example

```bash
dotnet ef database update --project ..\..\Modules\Cart\PuzKit3D.Modules.Cart.Persistence --context CartDbContext
```

---

## 3. Remove last migration: `dotnet ef migrations remove`

```bash
dotnet ef migrations remove --project ..\..\Modules\<NameOfModule>\PuzKit3D.Modules.<NameOfModule>.Persistence --context <NameOfDbContext>
```

> ***Chỉ remove khi migration chưa được apply lên database***

---


## 4. Check migration status: `dotnet ef migrations list`

```bash
dotnet ef migrations list
```

---

## 5. Generate SQL script from migrations: `dotnet ef migrations script`

Tạo file SQL từ các migration hiện có:

```bash
dotnet ef migrations script
```

Có thể chỉ định từ migration A đến migration B:

```bash
dotnet ef migrations script <FromMigration> <ToMigration>

```

---
> **CHECK LIST:**
>
>🔹 Always check whether exactly `--project`
>
>🔹 Always check whether exactly  `--context`
>
>🔹 Each module has their own `DbContext`
>
>🔹 Must run at `WebApi` project directory