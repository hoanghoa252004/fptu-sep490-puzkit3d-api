using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;
using PuzKit3D.Modules.Partner.Domain.Entities.Partners;

namespace PuzKit3D.Modules.Partner.Persistence.Configurations;

internal sealed class PartnerConfiguration : IEntityTypeConfiguration<Domain.Entities.Partners.Partner>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Partners.Partner> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PartnerId.From(value));

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.Description);

        builder.Property(p => p.ContactEmail)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.ContactPhone)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(p => p.Address)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.ImportServiceConfigId)
            .HasConversion(
                id => id.Value,
                value => ImportServiceConfigId.From(value))
            .IsRequired();

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.HasIndex(p => p.Slug)
            .IsUnique();

        builder.HasOne<ImportServiceConfig>()
            .WithMany()
            .HasForeignKey(p => p.ImportServiceConfigId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}
