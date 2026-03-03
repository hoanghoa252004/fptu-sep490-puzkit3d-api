using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class UserReplicaConfiguration : IEntityTypeConfiguration<UserReplica>
{
    public void Configure(EntityTypeBuilder<UserReplica> builder)
    {
        builder.ToTable("user_replica");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("id");

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("email");

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(300)
            .HasColumnName("password_hash");

        builder.Property(u => u.RoleId)
            .IsRequired()
            .HasColumnName("role_id");

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("full_name");

        builder.Property(u => u.DateOfBirth)
            .HasColumnName("date_of_birth");

        builder.Property(u => u.Address)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasColumnName("address");

        builder.Property(u => u.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15)
            .HasColumnName("phone_number");

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("UK__user_replica__email");

        builder.HasIndex(u => u.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("UK__user_replica__phone_number");

        builder.Ignore(u => u.DomainEvents);
    }
}
