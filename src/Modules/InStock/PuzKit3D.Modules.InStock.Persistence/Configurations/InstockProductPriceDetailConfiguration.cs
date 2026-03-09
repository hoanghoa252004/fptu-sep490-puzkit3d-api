using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class InstockProductPriceDetailConfiguration : IEntityTypeConfiguration<InstockProductPriceDetail>
{
    public void Configure(EntityTypeBuilder<InstockProductPriceDetail> builder)
    {
        builder.HasKey(pd => pd.Id);

        builder.Property(pd => pd.Id)
            .HasConversion(
                id => id.Value,
                value => InstockProductPriceDetailId.From(value));

        builder.Property(pd => pd.InstockPriceId)
            .HasConversion(
                id => id.Value,
                value => InstockPriceId.From(value))
            .IsRequired();

        builder.Property(pd => pd.InstockProductVariantId)
            .HasConversion(
                id => id.Value,
                value => InstockProductVariantId.From(value))
            .IsRequired();

        builder.OwnsOne(pd => pd.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            money.Ignore(m => m.Currency);
        });

        builder.Property(pd => pd.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(pd => pd.CreatedAt)
            .IsRequired();

        builder.Property(pd => pd.UpdatedAt)
            .IsRequired();

        builder.HasIndex(pd => new { pd.InstockPriceId, pd.InstockProductVariantId })
            .IsUnique()
            .HasDatabaseName("CUK___instock_product_price_detail___instock_price_id__instock_product_variant_id");

        builder.HasOne<InstockPrice>()
            .WithMany()
            .HasForeignKey(pd => pd.InstockPriceId)
            .HasConstraintName("FK__instock_price__instock_product_price_detail")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<InstockProductVariant>()
            .WithMany()
            .HasForeignKey(pd => pd.InstockProductVariantId)
            .HasConstraintName("FK__instock_product_variant__instock_product_price_detail")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(pd => pd.DomainEvents);
    }
}
