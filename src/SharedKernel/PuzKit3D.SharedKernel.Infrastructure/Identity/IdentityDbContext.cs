using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.SharedKernel.Infrastructure.Data;

namespace PuzKit3D.SharedKernel.Infrastructure.Identity;

/// <summary>
/// DbContext for Identity tables with schema: identity
/// </summary>
public sealed class IdentityDbContext : IdentityDbContext<
    ApplicationUser,
    ApplicationRole,
    string,
    Microsoft.AspNetCore.Identity.IdentityUserClaim<string>,
    ApplicationUserRole,
    Microsoft.AspNetCore.Identity.IdentityUserLogin<string>,
    Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>,
    Microsoft.AspNetCore.Identity.IdentityUserToken<string>>
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUserPermission> UserPermissions => Set<ApplicationUserPermission>();
    public DbSet<ApplicationRolePermission> RolePermissions => Set<ApplicationRolePermission>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(Schema.Identity);

        // Configure table names
        builder.Entity<ApplicationUser>().ToTable(IdentityTableNames.ApplicationUser);
        builder.Entity<ApplicationRole>().ToTable(IdentityTableNames.ApplicationRole);
        builder.Entity<ApplicationUserRole>().ToTable(IdentityTableNames.ApplicationUserRole);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserClaim<string>>().ToTable(IdentityTableNames.ApplicationUserClaim);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserLogin<string>>().ToTable(IdentityTableNames.ApplicationUserLogin);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>>().ToTable(IdentityTableNames.ApplicationRoleClaim);
        builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserToken<string>>().ToTable(IdentityTableNames.ApplicationUserToken);
        builder.Entity<ApplicationUserPermission>().ToTable(IdentityTableNames.ApplicationUserPermission);
        builder.Entity<ApplicationRolePermission>().ToTable(IdentityTableNames.ApplicationRolePermission);

        // Configure relationships
        builder.Entity<ApplicationUserRole>(entity =>
        {
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });

            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);
        });

        builder.Entity<ApplicationUserPermission>(entity =>
        {
            entity.HasKey(up => new { up.UserId, up.Permission });

            entity.HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(up => up.Permission)
                .HasMaxLength(200);
        });

        builder.Entity<ApplicationRolePermission>(entity =>
        {
            entity.HasKey(rp => new { rp.RoleId, rp.Permission });

            entity.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(rp => rp.Permission)
                .HasMaxLength(200);
        });

        // Seed default roles
        SeedDefaultRoles(builder);
        
        // Seed default users
        SeedDefaultUsers(builder);
        
        // Seed role permissions
        SeedRolePermissions(builder);
    }

    private static void SeedDefaultRoles(ModelBuilder builder)
    {
        string adminRoleId = "9b7da615-9c41-4700-92a9-ca17337c5724";
        string managerRoleId = "0b42c919-01c0-4109-ba04-d848c45dc413";
        string staffRoleId = "1a0d505f-46d8-4aaf-92c7-71ba90443dcb";
        string customerRoleId = "f634ede8-7091-48da-a969-2bf90ef86f2c";
                
        DateTime createdAt = new DateTime(2025, 02, 23, 0, 0, 0, DateTimeKind.Utc);

        builder.Entity<ApplicationRole>().HasData(
            new ApplicationRole
            {
                Id = adminRoleId,
                Name = "System Administrator",
                NormalizedName = "SYSTEM_ADMINISTRATOR",
                Description = "Full system access",
                CreatedAt = createdAt
            },
            new ApplicationRole
            {
                Id = managerRoleId,
                Name = "Business Manager",
                NormalizedName = "BUSINESS_MANAGER",
                Description = "Low-level business access",
                CreatedAt = createdAt
            },
            new ApplicationRole
            {
                Id = staffRoleId,
                Name = "Staff",
                NormalizedName = "STAFF",
                Description = "High-level business access",
                CreatedAt = createdAt
            },
            new ApplicationRole
            {
                Id = customerRoleId,
                Name = "Customer",
                NormalizedName = "CUSTOMER",
                Description = "Standard customer access",
                CreatedAt = createdAt
            }
        );
    }

    private static void SeedDefaultUsers(ModelBuilder builder)
    {
        string adminRoleId = "9b7da615-9c41-4700-92a9-ca17337c5724";
        string managerRoleId = "0b42c919-01c0-4109-ba04-d848c45dc413";
        string staffRoleId = "1a0d505f-46d8-4aaf-92c7-71ba90443dcb";

        DateTime createdAt = new DateTime(2025, 02, 23, 0, 0, 0, DateTimeKind.Utc);

        // Create password hasher
        var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<ApplicationUser>();

        // 1. Admin user
        var adminUser = new ApplicationUser
        {
            Id = "admin-001",
            UserName = "admin@puzkit3d.com",
            NormalizedUserName = "ADMIN@PUZKIT3D.COM",
            Email = "admin@puzkit3d.com",
            NormalizedEmail = "ADMIN@PUZKIT3D.COM",
            EmailConfirmed = true,
            FirstName = "System",
            LastName = "Administrator",
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            CreatedAt = createdAt
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@123456");

        // 2. Manager user
        var managerUser = new ApplicationUser
        {
            Id = "manager-001",
            UserName = "manager@puzkit3d.com",
            NormalizedUserName = "MANAGER@PUZKIT3D.COM",
            Email = "manager@puzkit3d.com",
            NormalizedEmail = "MANAGER@PUZKIT3D.COM",
            EmailConfirmed = true,
            FirstName = "Business",
            LastName = "Manager",
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            CreatedAt = createdAt
        };
        managerUser.PasswordHash = hasher.HashPassword(managerUser, "Manager@123456");

        // 3-5. Staff users
        var staff1 = new ApplicationUser
        {
            Id = "staff-001",
            UserName = "staff1@puzkit3d.com",
            NormalizedUserName = "STAFF1@PUZKIT3D.COM",
            Email = "staff1@puzkit3d.com",
            NormalizedEmail = "STAFF1@PUZKIT3D.COM",
            EmailConfirmed = true,
            FirstName = "John",
            LastName = "Staff",
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            CreatedAt = createdAt
        };
        staff1.PasswordHash = hasher.HashPassword(staff1, "Staff@123456");

        var staff2 = new ApplicationUser
        {
            Id = "staff-002",
            UserName = "staff2@puzkit3d.com",
            NormalizedUserName = "STAFF2@PUZKIT3D.COM",
            Email = "staff2@puzkit3d.com",
            NormalizedEmail = "STAFF2@PUZKIT3D.COM",
            EmailConfirmed = true,
            FirstName = "Jane",
            LastName = "Staff",
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            CreatedAt = createdAt
        };
        staff2.PasswordHash = hasher.HashPassword(staff2, "Staff@123456");

        var staff3 = new ApplicationUser
        {
            Id = "staff-003",
            UserName = "staff3@puzkit3d.com",
            NormalizedUserName = "STAFF3@PUZKIT3D.COM",
            Email = "staff3@puzkit3d.com",
            NormalizedEmail = "STAFF3@PUZKIT3D.COM",
            EmailConfirmed = true,
            FirstName = "Mike",
            LastName = "Staff",
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString(),
            CreatedAt = createdAt
        };
        staff3.PasswordHash = hasher.HashPassword(staff3, "Staff@123456");

        builder.Entity<ApplicationUser>().HasData(
            adminUser,
            managerUser,
            staff1,
            staff2,
            staff3
        );

        // Assign roles to users
        builder.Entity<ApplicationUserRole>().HasData(
            new ApplicationUserRole { UserId = "admin-001", RoleId = adminRoleId },
            new ApplicationUserRole { UserId = "manager-001", RoleId = managerRoleId },
            new ApplicationUserRole { UserId = "staff-001", RoleId = staffRoleId },
            new ApplicationUserRole { UserId = "staff-002", RoleId = staffRoleId },
            new ApplicationUserRole { UserId = "staff-003", RoleId = staffRoleId }
        );
    }

    private static void SeedRolePermissions(ModelBuilder builder)
    {
        string adminRoleId = "9b7da615-9c41-4700-92a9-ca17337c5724";
        string managerRoleId = "0b42c919-01c0-4109-ba04-d848c45dc413";
        string staffRoleId = "1a0d505f-46d8-4aaf-92c7-71ba90443dcb";

        var permissions = new List<ApplicationRolePermission>();

        // Admin permissions - Full access to Users management
        permissions.AddRange(new[]
        {
            new ApplicationRolePermission { RoleId = adminRoleId, Permission = "users:view" },
            new ApplicationRolePermission { RoleId = adminRoleId, Permission = "users:create" },
            new ApplicationRolePermission { RoleId = adminRoleId, Permission = "users:update" },
            new ApplicationRolePermission { RoleId = adminRoleId, Permission = "users:delete" },
            new ApplicationRolePermission { RoleId = adminRoleId, Permission = "users:roles:manage" },
            new ApplicationRolePermission { RoleId = adminRoleId, Permission = "users:permissions:manage" }
        });

        // Manager permissions - Can manage catalog
        permissions.AddRange(new[]
        {
            new ApplicationRolePermission { RoleId = managerRoleId, Permission = "catalog:assembly-methods:view" },
            new ApplicationRolePermission { RoleId = managerRoleId, Permission = "catalog:assembly-methods:manage" },
            new ApplicationRolePermission { RoleId = managerRoleId, Permission = "catalog:topics:view" },
            new ApplicationRolePermission { RoleId = managerRoleId, Permission = "catalog:topics:manage" },
            new ApplicationRolePermission { RoleId = managerRoleId, Permission = "catalog:materials:view" },
            new ApplicationRolePermission { RoleId = managerRoleId, Permission = "catalog:materials:manage" },
            new ApplicationRolePermission { RoleId = managerRoleId, Permission = "catalog:capabilities:view" },
            new ApplicationRolePermission { RoleId = managerRoleId, Permission = "catalog:capabilities:manage" },
            new ApplicationRolePermission { RoleId = managerRoleId, Permission = "instock:products:view" },
            new ApplicationRolePermission { RoleId = managerRoleId, Permission = "instock:orders:view" }
        });

        // Staff permissions - Can manage catalog
        permissions.AddRange(new[]
        {
            new ApplicationRolePermission { RoleId = staffRoleId, Permission = "catalog:assembly-methods:view" },
            new ApplicationRolePermission { RoleId = staffRoleId, Permission = "catalog:assembly-methods:manage" },
            new ApplicationRolePermission { RoleId = staffRoleId, Permission = "catalog:topics:view" },
            new ApplicationRolePermission { RoleId = staffRoleId, Permission = "catalog:topics:manage" },
            new ApplicationRolePermission { RoleId = staffRoleId, Permission = "catalog:materials:view" },
            new ApplicationRolePermission { RoleId = staffRoleId, Permission = "catalog:materials:manage" },
            new ApplicationRolePermission { RoleId = staffRoleId, Permission = "catalog:capabilities:view" },
            new ApplicationRolePermission { RoleId = staffRoleId, Permission = "catalog:capabilities:manage" },
            new ApplicationRolePermission { RoleId = staffRoleId, Permission = "instock:products:view" },
            new ApplicationRolePermission { RoleId = staffRoleId, Permission = "instock:orders:view" }
        });

        builder.Entity<ApplicationRolePermission>().HasData(permissions);
    }
}

