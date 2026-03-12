using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class InstockOrderConfiguration : IEntityTypeConfiguration<InstockOrder>
{
    public void Configure(EntityTypeBuilder<InstockOrder> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion(
                id => id.Value,
                value => InstockOrderId.From(value));

        builder.Property(o => o.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(o => o.CustomerId)
            .IsRequired();

        builder.Property(o => o.CustomerName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(o => o.CustomerPhone)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(o => o.CustomerEmail)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(o => o.CustomerProvinceCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(o => o.CustomerProvinceName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(o => o.CustomerDistrictCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(o => o.CustomerDistrictName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(o => o.CustomerWardCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(o => o.CustomerWardName)
            .IsRequired()
            .HasMaxLength(30);

        builder.OwnsOne(o => o.SubTotalAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            money.Ignore(m => m.Currency);
        });

        builder.OwnsOne(o => o.ShippingFee, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            money.Ignore(m => m.Currency);
        });

        builder.Property(o => o.UsedCoinAmount)
            .IsRequired();

        builder.OwnsOne(o => o.UsedCoinAmountAsMoney, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            money.Ignore(m => m.Currency);
        });

        builder.OwnsOne(o => o.GrandTotalAmount, money =>
        {
            money.Property(m => m.Amount)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            money.Ignore(m => m.Currency);
        });

        builder.Property(o => o.Status)
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.UpdatedAt)
            .IsRequired();

        builder.Property(o => o.PaymentMethod)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(o => o.IsPaid)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(o => o.PaidAt);

        builder.HasIndex(o => o.Code)
            .IsUnique()
            .HasDatabaseName("UK__instock_order__code");

        builder.Navigation(o => o.OrderDetails)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(o => o.DomainEvents);
    }
}
