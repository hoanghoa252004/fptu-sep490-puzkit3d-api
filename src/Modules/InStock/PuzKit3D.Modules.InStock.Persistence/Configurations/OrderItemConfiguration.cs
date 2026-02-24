using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.Orders;
using PuzKit3D.Modules.InStock.Domain.Entities.Products;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("order_items");

        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id)
            .HasConversion(
                id => id.Value,
                value => OrderItemId.From(value))
            .HasColumnName("id");

        // ? Config OrderId v?i conversion và explicit FK
        builder.Property(oi => oi.OrderId)
            .HasConversion(
                id => id.Value,
                value => OrderId.From(value))
            .HasColumnName("order_id")
            .IsRequired();

        // ? Config relationship: OrderItem -> Order
        builder.HasOne<Order>()
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(oi => oi.ProductId)
            .HasConversion(
                id => id.Value,
                value => ProductId.From(value))
            .HasColumnName("product_id")
            .IsRequired();

        builder.Property(oi => oi.Quantity)
            .IsRequired()
            .HasColumnName("quantity");

        builder.OwnsOne(oi => oi.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("unit_price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("unit_price_currency")
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.OwnsOne(oi => oi.TotalPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("total_price")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasColumnName("total_price_currency")
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.HasIndex(oi => oi.OrderId);
        builder.HasIndex(oi => oi.ProductId);
    }
}
