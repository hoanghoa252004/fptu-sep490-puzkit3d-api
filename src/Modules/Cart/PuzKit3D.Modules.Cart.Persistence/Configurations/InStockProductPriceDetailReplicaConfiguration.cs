using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class InStockProductPriceDetailReplicaConfiguration : IEntityTypeConfiguration<InStockProductPriceDetailReplica>
{
    public void Configure(EntityTypeBuilder<InStockProductPriceDetailReplica> builder)
    {
        builder.ToTable("instock_product_price_detail_replica");

        builder.HasKey(pd => pd.Id);

        builder.Property(pd => pd.Id)
            .HasColumnName("id");

        builder.Property(pd => pd.InStockPriceId)
            .IsRequired()
            .HasColumnName("instock_price_id");

        builder.Property(pd => pd.InStockProductVariantId)
            .IsRequired()
            .HasColumnName("instock_product_variant_id");

        builder.Property(pd => pd.IsActive)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_active");

        builder.Property(pd => pd.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(pd => pd.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(pd => new { pd.InStockPriceId, pd.InStockProductVariantId })
            .IsUnique()
            .HasDatabaseName("CUK___instock_product_price_detail_replica___instock_price_id__instock_product_variant_id");

        builder.Ignore(pd => pd.DomainEvents);
    }
}
