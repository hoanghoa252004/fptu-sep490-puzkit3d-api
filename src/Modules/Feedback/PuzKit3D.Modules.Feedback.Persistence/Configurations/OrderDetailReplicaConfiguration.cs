using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;

namespace PuzKit3D.Modules.Feedback.Persistence.Configurations;

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

        builder.HasIndex(o => o.OrderId);
        builder.HasIndex(o => o.ProductId);
    }
}

