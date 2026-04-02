using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductQuotations;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;

namespace PuzKit3D.Modules.Partner.Persistence.Configurations;

internal sealed class PartnerProductQuotationConfiguration : IEntityTypeConfiguration<PartnerProductQuotation>
{
    public void Configure(EntityTypeBuilder<PartnerProductQuotation> builder)
    {
        builder.HasKey(q => q.Id);

        builder.Property(q => q.Id)
            .HasConversion(
                id => id.Value,
                value => PartnerProductQuotationId.From(value));

        builder.Property(q => q.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(q => q.PartnerProductRequestId)
            .HasConversion(
                id => id.Value,
                value => PartnerProductRequestId.From(value))
            .IsRequired();

        builder.HasMany(r => r.Details)
            .WithOne()
            .HasForeignKey(d => d.PartnerProductQuotationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(q => q.SubTotalAmount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(q => q.ShippingFee)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(q => q.ImportTaxAmount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(q => q.GrandTotalAmount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(q => q.Note);

        builder.Property(q => q.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(q => q.CreatedAt)
            .IsRequired();

        builder.Property(q => q.UpdatedAt)
            .IsRequired();

        builder.HasOne<PartnerProductRequest>()
            .WithMany()
            .HasForeignKey(q => q.PartnerProductRequestId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
