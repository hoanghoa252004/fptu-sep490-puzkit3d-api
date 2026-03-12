using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrderDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductPriceDetails;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProductVariants;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class InstockOrderDetailConfiguration : IEntityTypeConfiguration<InstockOrderDetail>
{
    public void Configure(EntityTypeBuilder<InstockOrderDetail> builder)
    {
        builder.HasKey(od => od.Id);

        builder.Property(od => od.Id)
            .HasConversion(
                id => id.Value,
                value => InstockOrderDetailId.From(value));

        builder.Property(od => od.InstockOrderId)
            .HasConversion(
                id => id.Value,
                value => InstockOrderId.From(value))
            .IsRequired();

        builder.Property(od => od.InstockProductVariantId)
            .HasConversion(
                id => id.Value,
                value => InstockProductVariantId.From(value))
            .IsRequired();

        builder.Property(od => od.Sku)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(od => od.ProductName)
            .HasMaxLength(30);

        builder.Property(od => od.VariantName)
            .HasMaxLength(30);

        builder.Property(od => od.UnitPrice)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(od => od.Quantity)
            .IsRequired();

        builder.Property(od => od.InstockProductPriceDetailId)
            .HasConversion(
                id => id.Value,
                value => InstockProductPriceDetailId.From(value))
            .IsRequired();

        builder.Property(od => od.PriceName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(od => od.TotalAmount)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.HasIndex(od => new { od.InstockOrderId, od.InstockProductVariantId })
            .IsUnique()
            .HasDatabaseName("CUK___instock_order_detail___instock_order_id__instock_product_variant_id");

        builder.HasOne<InstockOrder>()
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.InstockOrderId)
            .HasConstraintName("FK__instock_order__instock_order_detail")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<InstockProductVariant>()
            .WithMany()
            .HasForeignKey(od => od.InstockProductVariantId)
            .HasConstraintName("FK__instock_order_detail__instock_product_variant")
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<InstockProductPriceDetail>()
            .WithMany()
            .HasForeignKey(od => od.InstockProductPriceDetailId)
            .HasConstraintName("FK__instock_order_detail__instock_product_price_detail")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Ignore(od => od.DomainEvents);
    }
}
