using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Carts;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("cart_item");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .HasConversion(
                id => id.Value,
                value => CartItemId.From(value))
            .HasColumnName("id");

        builder.Property(ci => ci.CartId)
            .HasConversion(
                id => id.Value,
                value => CartId.From(value))
            .IsRequired()
            .HasColumnName("cart_id");

        builder.Property(ci => ci.ItemId)
            .IsRequired()
            .HasColumnName("item_id");

        builder.OwnsOne(ci => ci.UnitPrice, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnName("unit_price")
                .HasColumnType("decimal(10,2)");

            money.Ignore(m => m.Currency);
        });

        builder.Property(ci => ci.InStockProductPriceDetailId)
            .HasColumnName("instock_product_price_detail_id");

        builder.Property(ci => ci.Quantity)
            .IsRequired()
            .HasDefaultValue(1)
            .HasColumnName("quantity");

        builder.HasOne<Domain.Entities.Carts.Cart>()
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CartId)
            .HasConstraintName("FK__cart__cart_item")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(ci => new { ci.CartId, ci.ItemId })
            .IsUnique()
            .HasDatabaseName("CUK___cart___cart_id__item_id");

        builder.Ignore(ci => ci.DomainEvents);
        builder.Ignore(ci => ci.TotalPrice);
    }
}
