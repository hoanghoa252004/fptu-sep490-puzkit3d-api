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

        builder.Property(o => o.CustomerProvinceName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(o => o.CustomerDistrictName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(o => o.DetailAddress)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(o => o.HandoverProofImageUrl)
            .HasColumnType("text");

        builder.Property(o => o.CustomerWardName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(o => o.SubTotalAmount)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(o => o.ShippingFee)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(o => o.UsedCoinAmount)
            .IsRequired();

        builder.Property(o => o.UsedCoinAmountAsMoney)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(o => o.GrandTotalAmount)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(o => o.Status)
            .HasConversion<string>()
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

        builder.Property(o => o.DeliveryOrderCode)
            .HasMaxLength(20);

        builder.Property(o => o.ExpectedDeliveryDate);

        builder.HasIndex(o => o.Code)
            .IsUnique()
            .HasDatabaseName("UK__instock_order__code");

        builder.Navigation(o => o.OrderDetails)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(o => o.DomainEvents);
    }
}
