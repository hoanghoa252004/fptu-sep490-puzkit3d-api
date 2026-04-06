using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductOrders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;

namespace PuzKit3D.Modules.Partner.Persistence.Configurations;

internal sealed class PartnerProductOrderConfiguration : IEntityTypeConfiguration<PartnerProductOrder>
{
    public void Configure(EntityTypeBuilder<PartnerProductOrder> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion(
                id => id.Value,
                value => PartnerProductOrderId.From(value));

        builder.Property(o => o.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(o => o.PartnerProductQuotationId)
            .HasConversion(
                id => id.Value,
                value => PartnerProductQuotationId.From(value))
            .IsRequired();

        builder.HasMany(r => r.Details)
            .WithOne()
            .HasForeignKey(d => d.PartnerProductOrderId)
            .OnDelete(DeleteBehavior.Restrict);

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

        builder.Property(o => o.CustomerWardName)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(o => o.SubTotalAmount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(o => o.ShippingFee)
            .IsRequired()
            .HasPrecision(10, 2);
        
        builder.Property(o => o.BaseShippingFee)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(o => o.ImportTaxAmount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(o => o.UsedCoinAmount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(o => o.GrandTotalAmount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<string>();

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
            .IsUnique();

        builder.HasIndex(o => o.PartnerProductQuotationId)
            .IsUnique();

        builder.HasOne<PartnerProductQuotation>()
            .WithMany()
            .HasForeignKey(o => o.PartnerProductQuotationId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
