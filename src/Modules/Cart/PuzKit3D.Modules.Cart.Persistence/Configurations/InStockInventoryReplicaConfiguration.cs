using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class InStockInventoryReplicaConfiguration : IEntityTypeConfiguration<InStockInventoryReplica>
{
    public void Configure(EntityTypeBuilder<InStockInventoryReplica> builder)
    {
        builder.ToTable("instock_inventory_replica");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(i => i.InStockProductVariantId)
            .HasColumnName("instock_product_variant_id")
            .IsRequired();

        builder.Property(i => i.TotalQuantity)
            .HasColumnName("total_quantity")
            .IsRequired();

        builder.Property(i => i.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(i => i.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();

        builder.HasIndex(i => i.InStockProductVariantId)
            .HasDatabaseName("ix_instock_inventory_replica_instock_product_variant_id");
    }
}
