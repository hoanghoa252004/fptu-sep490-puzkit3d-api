using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class InStockInventoryReplicaConfiguration : IEntityTypeConfiguration<InStockInventoryReplica>
{
    public void Configure(EntityTypeBuilder<InStockInventoryReplica> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedNever();

        builder.Property(i => i.InStockProductVariantId)
            .IsRequired();

        builder.Property(i => i.TotalQuantity)
            .IsRequired();

        builder.Property(i => i.CreatedAt)
            .IsRequired();

        builder.Property(i => i.UpdatedAt)
            .IsRequired();

        builder.HasIndex(i => i.InStockProductVariantId)
            .HasDatabaseName("ix_instock_inventory_replica_instock_product_variant_id");
    }
}
