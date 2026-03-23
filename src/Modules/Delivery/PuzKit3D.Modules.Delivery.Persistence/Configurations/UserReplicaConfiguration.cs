using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Delivery.Persistence.Configurations;

internal sealed class UserReplicaConfiguration : IEntityTypeConfiguration<UserReplica>
{
    public void Configure(EntityTypeBuilder<UserReplica> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(u => u.RoleId)
            .IsRequired();

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(u => u.DateOfBirth);

        builder.Property(u => u.PhoneNumber)
            .IsRequired()
            .HasMaxLength(15);

        // Address properties

        builder.Property(u => u.Province)
            .HasMaxLength(100);


        builder.Property(u => u.District)
            .HasMaxLength(100);


        builder.Property(u => u.Ward)
            .HasMaxLength(100);

        builder.Property(u => u.StreetAddress)
            .HasMaxLength(500);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.UpdatedAt)
            .IsRequired();

        builder.HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("UK__user_replica__email");

        builder.HasIndex(u => u.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("UK__user_replica__phone_number");

        builder.Ignore(u => u.DomainEvents);
    }
}

