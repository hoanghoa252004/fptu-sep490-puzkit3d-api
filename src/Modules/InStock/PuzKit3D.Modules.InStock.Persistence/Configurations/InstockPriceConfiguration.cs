using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockPrices;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class InstockPriceConfiguration : IEntityTypeConfiguration<InstockPrice>
{
    public void Configure(EntityTypeBuilder<InstockPrice> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => InstockPriceId.From(value));

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.EffectiveFrom)
            .IsRequired();

        builder.Property(p => p.EffectiveTo)
            .IsRequired();

        builder.Property(p => p.Priority)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.Ignore(p => p.DomainEvents);
    }
}
