using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Partner.Domain.Entities.PartnerProducts;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;

namespace PuzKit3D.Modules.Partner.Persistence.Configurations;

internal sealed class PartnerProductConfiguration : IEntityTypeConfiguration<PartnerProduct>
{
    public void Configure(EntityTypeBuilder<PartnerProduct> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PartnerProductId.From(value));

        builder.Property(p => p.PartnerId)
            .HasConversion(
                id => id.Value,
                value => PartnerId.From(value))
            .IsRequired();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.ReferencePrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(p => p.Quantity)
            .IsRequired();

        builder.Property(p => p.Description);

        builder.Property(p => p.ThumbnailUrl)
            .IsRequired();

        builder.Property(p => p.PreviewAsset)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.HasIndex(p => new { p.PartnerId, p.Slug })
            .IsUnique();

        builder.HasOne<Domain.Entities.Partners.Partner>()
            .WithMany()
            .HasForeignKey(p => p.PartnerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
