using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;

namespace PuzKit3D.Modules.Feedback.Persistence.Configurations;

internal sealed class CompletedOrderReplicaConfiguration : IEntityTypeConfiguration<CompletedOrderReplica>
{
    public void Configure(EntityTypeBuilder<CompletedOrderReplica> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.CustomerId)
            .IsRequired();

        builder.Property(o => o.ProductId)
            .IsRequired();

        builder.Property(o => o.VariantId)
            .IsRequired(false);

        builder.HasIndex(o => o.CustomerId);

        builder.HasIndex(o => o.ProductId);
    }
}

