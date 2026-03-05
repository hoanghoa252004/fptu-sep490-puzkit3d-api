using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class InStockProductPriceDetailReplicaConfiguration : IEntityTypeConfiguration<InStockProductPriceDetailReplica>
{
    public void Configure(EntityTypeBuilder<InStockProductPriceDetailReplica> builder)
    {
        builder.HasKey(pd => pd.Id);

        builder.Property(pd => pd.Id);

        builder.Property(pd => pd.InStockPriceId)
            .IsRequired();

        builder.Property(pd => pd.InStockProductVariantId)
            .IsRequired();

        builder.Property(pd => pd.UnitPrice)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.Property(pd => pd.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(pd => pd.CreatedAt)
            .IsRequired();

        builder.Property(pd => pd.UpdatedAt)
            .IsRequired();

        builder.HasIndex(pd => new { pd.InStockPriceId, pd.InStockProductVariantId })
            .IsUnique()
            .HasDatabaseName("CUK___instock_product_price_detail_replica___instock_price_id__instock_product_variant_id");

        builder.Ignore(pd => pd.DomainEvents);
    }
}
