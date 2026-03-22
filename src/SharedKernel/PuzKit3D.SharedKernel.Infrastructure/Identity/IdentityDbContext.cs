using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PuzKit3D.SharedKernel.Application.Identity;
using PuzKit3D.SharedKernel.Domain.Results;
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
    Microsoft.AspNetCore.Identity.IdentityUserToken<string>>, IIdentityUnitOfWork
{
    // Role IDs as constants
    private const string AdminRoleId = "9b7da615-9c41-4700-92a9-ca17337c5724";
    private const string ManagerRoleId = "0b42c919-01c0-4109-ba04-d848c45dc413";
    private const string StaffRoleId = "1a0d505f-46d8-4aaf-92c7-71ba90443dcb";
    private const string CustomerRoleId = "f634ede8-7091-48da-a969-2bf90ef86f2c";

    // User IDs as constants
    private const string AdminUserId = "71ac7ce7-84e2-44f6-8765-209244d0cbb3";
    private const string ManagerUserId = "15e5d4ac-a548-4f8a-9846-5fddc79c79c2";
    private const string StaffUserId = "10fa5863-e39c-4876-856e-5c4cfbd321dc";
    private const string CustomerUserId = "21d3261d-9ab4-45b9-b6cd-fba0231d285c";

    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

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

        // Seed default roles
        SeedDefaultRoles(builder);
        
        // Seed default users
        SeedDefaultUsers(builder);
    }

    private static void SeedDefaultRoles(ModelBuilder builder)
    {                
        DateTime createdAt = new DateTime(2025, 02, 23, 0, 0, 0, DateTimeKind.Utc);

        builder.Entity<ApplicationRole>().HasData(
            new ApplicationRole
            {
                Id = AdminRoleId,
                Name = "System Administrator",
                NormalizedName = "SYSTEM ADMINISTRATOR",
                Description = "Full system access",
                CreatedAt = createdAt,
            },
            new ApplicationRole
            {
                Id = ManagerRoleId,
                Name = "Business Manager",
                NormalizedName = "BUSINESS MANAGER",
                Description = "High-level business access",
                CreatedAt = createdAt
            },
            new ApplicationRole
            {
                Id = StaffRoleId,
                Name = "Staff",
                NormalizedName = "STAFF",
                Description = "Low-level business access",
                CreatedAt = createdAt
            },
            new ApplicationRole
            {
                Id = CustomerRoleId,
                Name = "Customer",
                NormalizedName = "CUSTOMER",
                Description = "Standard customer access",
                CreatedAt = createdAt
            }
        );
    }

    private static void SeedDefaultUsers(ModelBuilder builder)
    {

        DateTime createdAt = new DateTime(2025, 02, 23, 0, 0, 0, DateTimeKind.Utc);

        // Create password hasher
        var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<ApplicationUser>();

        // 1. Admin user
        var adminUser = new ApplicationUser
        {
            Id = AdminUserId,
            UserName = "admin@puzkit3d.com",
            NormalizedUserName = "ADMIN@PUZKIT3D.COM",
            Email = "admin@puzkit3d.com",
            NormalizedEmail = "ADMIN@PUZKIT3D.COM",
            EmailConfirmed = true,
            FirstName = "PuzKit3D",
            LastName = "System Administrator",
            SecurityStamp = "D5F8E9A1-2B3C-4D5E-6F7A-8B9C0D1E2F3A",
            ConcurrencyStamp = "A1B2C3D4-E5F6-7890-ABCD-EF1234567890",
            CreatedAt = createdAt,
            UpdatedAt = createdAt,
            LockoutEnabled = false
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "@1");

        // 2. Manager user
        var managerUser = new ApplicationUser
        {
            Id = ManagerUserId,
            UserName = "manager@puzkit3d.com",
            NormalizedUserName = "MANAGER@PUZKIT3D.COM",
            Email = "manager@puzkit3d.com",
            NormalizedEmail = "MANAGER@PUZKIT3D.COM",
            EmailConfirmed = true,
            FirstName = "PuzKit3D",
            LastName = "Business Manager",
            SecurityStamp = "B6E7F8A9-1C2D-3E4F-5A6B-7C8D9E0F1A2B",
            ConcurrencyStamp = "B2C3D4E5-F6A7-8901-BCDE-F23456789012",
            CreatedAt = createdAt,
            UpdatedAt = createdAt,
            LockoutEnabled = false
        };
        managerUser.PasswordHash = hasher.HashPassword(managerUser, "@1");

        // 3. Staff user
        var staff = new ApplicationUser
        {
            Id = StaffUserId,
            UserName = "staff@puzkit3d.com",
            NormalizedUserName = "STAFF@PUZKIT3D.COM",
            Email = "staff@puzkit3d.com",
            NormalizedEmail = "STAFF@PUZKIT3D.COM",
            EmailConfirmed = true,
            FirstName = "PuzKit3D",
            LastName = "Staff",
            SecurityStamp = "C7D8E9F0-1A2B-3C4D-5E6F-7A8B9C0D1E2F",
            ConcurrencyStamp = "C3D4E5F6-A7B8-9012-CDEF-345678901234",
            CreatedAt = createdAt,
            UpdatedAt = createdAt,
            LockoutEnabled = false
        };
        staff.PasswordHash = hasher.HashPassword(staff, "@1");

        // 4. Customer user
        var customer = new ApplicationUser
        {
            Id = CustomerUserId,
            UserName = "customer@puzkit3d.com",
            NormalizedUserName = "CUSTOMER@PUZKIT3D.COM",
            Email = "customer@puzkit3d.com",
            NormalizedEmail = "CUSTOMER@PUZKIT3D.COM",
            EmailConfirmed = true,
            FirstName = "PuzKit3D",
            LastName = "Customer",
            SecurityStamp = "D8E9F0A1-2B3C-4D5E-6F7A-8B9C0D1E2F3A",
            ConcurrencyStamp = "D4E5F6A7-B8C9-0123-DEF4-567890123456",
            CreatedAt = createdAt,
            UpdatedAt = createdAt,
            LockoutEnabled = false
        };
        customer.PasswordHash = hasher.HashPassword(customer, "@1");

        builder.Entity<ApplicationUser>().HasData(
            adminUser,
            managerUser,
            staff,
            customer
        );

        // Assign roles to users
        builder.Entity<ApplicationUserRole>().HasData(
            new ApplicationUserRole { UserId = AdminUserId, RoleId = AdminRoleId },
            new ApplicationUserRole { UserId = ManagerUserId, RoleId = ManagerRoleId },
            new ApplicationUserRole { UserId = StaffUserId, RoleId = StaffRoleId },
            new ApplicationUserRole { UserId = CustomerUserId, RoleId = CustomerRoleId }
        );
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
    {
        var strategy = Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
            {
                var response = await action();
                if (response is Result result && result.IsFailure)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return response;
                }
                await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return response;
            }
        });
    }
}

