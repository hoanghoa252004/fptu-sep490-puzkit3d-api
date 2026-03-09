using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class InstockProductVariantConfiguration : IEntityTypeConfiguration<InstockProductVariant>
{
    public void Configure(EntityTypeBuilder<InstockProductVariant> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => InstockProductVariantId.From(value));

        builder.Property(v => v.InstockProductId)
            .HasConversion(
                id => id.Value,
                value => InstockProductId.From(value))
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
            .HasDatabaseName("UK__instock_product_variant__sku");

        builder.HasOne<InstockProduct>()
            .WithMany()
            .HasForeignKey(v => v.InstockProductId)
            .HasConstraintName("FK__instock_product__instock_product_variant")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(v => v.DomainEvents);
    }
}
