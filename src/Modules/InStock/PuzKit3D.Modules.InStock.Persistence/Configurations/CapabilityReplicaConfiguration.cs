using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class CapabilityReplicaConfiguration : IEntityTypeConfiguration<CapabilityReplica>
{
    public void Configure(EntityTypeBuilder<CapabilityReplica> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(c => c.Description)
            .HasColumnType("text");

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.HasIndex(c => c.Slug)
            .IsUnique()
            .HasDatabaseName("UK__capability_replica__slug");

        builder.Ignore(c => c.DomainEvents);
    }
}
