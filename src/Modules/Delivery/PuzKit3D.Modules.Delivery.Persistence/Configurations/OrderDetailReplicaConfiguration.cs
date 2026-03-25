using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Delivery.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Delivery.Persistence.Configurations;

internal sealed class OrderDetailReplicaConfiguration : IEntityTypeConfiguration<OrderDetailReplica>
{
    public void Configure(EntityTypeBuilder<OrderDetailReplica> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderId)
            .IsRequired();

        builder.Property(o => o.ProductId)
            .IsRequired();

        builder.Property(o => o.VariantId)
            .IsRequired(false);

        builder.Property(o => o.Quantity)
            .IsRequired();

        builder.Property(od => od.ProductName)
            .HasMaxLength(30);

        builder.Property(od => od.VariantName)
            .HasMaxLength(30);

        builder.HasIndex(o => o.OrderId);
        builder.HasIndex(o => o.ProductId);
    }
}

