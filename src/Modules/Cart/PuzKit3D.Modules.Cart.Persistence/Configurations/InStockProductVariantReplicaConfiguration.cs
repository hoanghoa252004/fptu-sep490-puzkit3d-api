using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class InStockProductVariantReplicaConfiguration : IEntityTypeConfiguration<InStockProductVariantReplica>
{
    public void Configure(EntityTypeBuilder<InStockProductVariantReplica> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .ValueGeneratedNever();

        builder.Property(v => v.InStockProductId)
            .IsRequired();

        builder.Property(v => v.Sku)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(v => v.Color)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(v => v.AssembledLengthMm)
            .IsRequired();

        builder.Property(v => v.AssembledWidthMm)
            .IsRequired();

        builder.Property(v => v.AssembledHeightMm)
            .IsRequired();

        builder.Property(v => v.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(v => v.CreatedAt)
            .IsRequired();

        builder.Property(v => v.UpdatedAt)
            .IsRequired();

        builder.HasIndex(v => v.Sku)
            .IsUnique()
            .HasDatabaseName("UK__instock_product_variant_replica__sku");

        builder.Ignore(v => v.DomainEvents);
    }
}
