using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Delivery.Persistence.Configurations;

internal sealed class PartReplicaConfiguration : IEntityTypeConfiguration<PartReplica>
{
    public void Configure(EntityTypeBuilder<PartReplica> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.PartType)
            .IsRequired();

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.Quantity)
            .IsRequired();

        builder.Property(p => p.InstockProductId)
            .IsRequired();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.Ignore(p => p.DomainEvents);
    }
}
