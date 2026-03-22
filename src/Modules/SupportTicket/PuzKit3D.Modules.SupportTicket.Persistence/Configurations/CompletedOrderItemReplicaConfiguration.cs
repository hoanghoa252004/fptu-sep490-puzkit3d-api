using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.CompletedOrderItemReplicas;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Configurations;

internal sealed class CompletedOrderItemReplicaConfiguration : IEntityTypeConfiguration<CompletedOrderItemReplica>
{
    public void Configure(EntityTypeBuilder<CompletedOrderItemReplica> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.CompletedOrderReplicaId)
            .IsRequired();

        builder.Property(i => i.ProductId)
            .IsRequired();

        builder.Property(i => i.VariantId)
            .IsRequired(false);

        builder.Property(i => i.Quantity)
            .IsRequired();

        builder.HasIndex(i => i.CompletedOrderReplicaId);
        builder.HasIndex(i => i.ProductId);
    }
}
