using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class InStockProductVariantReplicaConfiguration : IEntityTypeConfiguration<InStockProductVariantReplica>
{
    public void Configure(EntityTypeBuilder<InStockProductVariantReplica> builder)
    {
        builder.ToTable("instock_product_variant_replica");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasColumnName("id");

        builder.Property(v => v.InStockProductId)
            .IsRequired()
            .HasColumnName("instock_product_id");

        builder.Property(v => v.Sku)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("sku");

        builder.Property(v => v.Color)
            .IsRequired()
            .HasMaxLength(15)
            .HasColumnName("color");

        builder.Property(v => v.Size)
            .IsRequired()
            .HasMaxLength(15)
            .HasColumnName("size");

        builder.Property(v => v.IsActive)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_active");

        builder.Property(v => v.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(v => v.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(v => new { v.InStockProductId, v.Color, v.Size })
            .IsUnique()
            .HasDatabaseName("CUK___instock_product_variant_replica___instock_product_id__color__size");

        builder.HasIndex(v => v.Sku)
            .IsUnique()
            .HasDatabaseName("UK__instock_product_variant_replica__sku");

        builder.Ignore(v => v.DomainEvents);
    }
}
