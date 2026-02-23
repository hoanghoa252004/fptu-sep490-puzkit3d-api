using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzKit3D.SharedKernel.Infrastructure.Identity;

/// <summary>
/// DbContext for Identity tables
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

        builder.HasDefaultSchema("identity");

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
                Name = "Business Manager ",
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
                NormalizedName = "Customer",
                Description = "Standard customer access",
                CreatedAt = createdAt
            }
        );
    }
}
