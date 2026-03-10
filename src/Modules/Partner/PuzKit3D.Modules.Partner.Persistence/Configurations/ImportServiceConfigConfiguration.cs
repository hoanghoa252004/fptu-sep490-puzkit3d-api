using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Partner.Domain.Entities.ImportServiceConfigs;

namespace PuzKit3D.Modules.Partner.Persistence.Configurations;

internal sealed class ImportServiceConfigConfiguration : IEntityTypeConfiguration<ImportServiceConfig>
{
    public void Configure(EntityTypeBuilder<ImportServiceConfig> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => ImportServiceConfigId.From(value));

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(c => c.BaseShippingFee)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(c => c.CountryCode)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(c => c.CountryName)
            .IsRequired()
            .HasMaxLength(15);

        builder.Property(c => c.ImportTaxPercentage)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();
    }
}
