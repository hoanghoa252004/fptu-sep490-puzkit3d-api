using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProductRequests;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;

namespace PuzKit3D.Modules.Partner.Persistence.Configurations;

internal sealed class PartnerProductRequestConfiguration : IEntityTypeConfiguration<PartnerProductRequest>
{
    public void Configure(EntityTypeBuilder<PartnerProductRequest> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => PartnerProductRequestId.From(value));

        builder.HasMany(r => r.Details)
            .WithOne()
            .HasForeignKey(d => d.PartnerProductRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(r => r.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(r => r.CustomerId)
            .IsRequired();

        builder.Property(r => r.PartnerId)
            .HasConversion(
                id => id.Value,
                value => PartnerId.From(value))
            .IsRequired();

        builder.Property(r => r.TotalRequestedQuantity)
            .IsRequired();

        builder.Property(r => r.Note);

        builder.Property(r => r.Status)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired();

        builder.HasIndex(r => r.Code)
            .IsUnique();

        builder.HasOne<Domain.Entities.Partners.Partner>()
            .WithMany()
            .HasForeignKey(r => r.PartnerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
