using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotationDetails;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;

namespace PuzKit3D.Modules.Partner.Persistence.Configurations;

internal sealed class PartnerProductQuotationDetailConfiguration : IEntityTypeConfiguration<PartnerProductQuotationDetail>
{
    public void Configure(EntityTypeBuilder<PartnerProductQuotationDetail> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => PartnerProductQuotationDetailId.From(value));

        builder.Property(d => d.PartnerProductQuotationId)
            .HasConversion(
                id => id.Value,
                value => PartnerProductQuotationId.From(value))
            .IsRequired();

        builder.Property(d => d.PartnerProductId)
            .HasConversion(
                id => id.Value,
                value => PartnerProductId.From(value))
            .IsRequired();

        builder.Property(d => d.Quantity)
            .IsRequired();

        builder.Property(d => d.UnitPrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(d => d.TotalAmount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.HasIndex(d => new { d.PartnerProductQuotationId, d.PartnerProductId })
            .IsUnique();

        builder.HasOne<PartnerProductQuotation>()
            .WithMany()
            .HasForeignKey(d => d.PartnerProductQuotationId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne<PartnerProduct>()
            .WithMany()
            .HasForeignKey(d => d.PartnerProductId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
