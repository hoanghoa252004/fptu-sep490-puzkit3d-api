using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequestDetails;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;

namespace PuzKit3D.Modules.Partner.Persistence.Configurations;

internal sealed class PartnerProductRequestDetailConfiguration : IEntityTypeConfiguration<PartnerProductRequestDetail>
{
    public void Configure(EntityTypeBuilder<PartnerProductRequestDetail> builder)
    {
        builder.ToTable("partner_product_request_item");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => PartnerProductRequestDetailId.From(value));

        builder.Property(d => d.PartnerProductRequestId)
            .HasConversion(
                id => id.Value,
                value => PartnerProductRequestId.From(value))
            .IsRequired();

        builder.Property(d => d.PartnerProductId)
            .HasConversion(
                id => id.Value,
                value => PartnerProductId.From(value))
            .IsRequired();

        builder.Property(d => d.ReferenceUnitPrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(d => d.Quantity)
            .IsRequired();

        builder.Property(d => d.ReferenceTotalAmount)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.HasIndex(d => new { d.PartnerProductRequestId, d.PartnerProductId })
            .IsUnique();

        builder.HasOne<PartnerProduct>()
            .WithMany()
            .HasForeignKey(d => d.PartnerProductId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
