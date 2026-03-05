using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.Orders;
using PuzKit3D.Modules.InStock.Domain.Entities.Products;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id)
            .HasConversion(
                id => id.Value,
                value => OrderItemId.From(value));

        builder.Property(oi => oi.OrderId)
            .HasConversion(
                id => id.Value,
                value => OrderId.From(value))
            .IsRequired();

        builder.HasOne<Order>()
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(oi => oi.ProductId)
            .HasConversion(
                id => id.Value,
                value => ProductId.From(value))
            .IsRequired();

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.OwnsOne(oi => oi.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.OwnsOne(oi => oi.TotalPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            money.Property(m => m.Currency)
                .HasMaxLength(10)
                .IsRequired();
        });

        builder.HasIndex(oi => oi.OrderId);
        builder.HasIndex(oi => oi.ProductId);
    }
}
