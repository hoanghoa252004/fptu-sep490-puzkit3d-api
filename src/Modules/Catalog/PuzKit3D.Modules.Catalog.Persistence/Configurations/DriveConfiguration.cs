using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class DriveConfiguration : IEntityTypeConfiguration<Drive>
{
    public void Configure(EntityTypeBuilder<Drive> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => DriveId.From(value));

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(d => d.Description)
            .HasColumnType("text");

        builder.Property(d => d.MinVolume)
            .IsRequired(false);

        builder.Property(d => d.QuantityInStock)
            .IsRequired();

        builder.Property(d => d.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(d => d.CreatedAt)
            .IsRequired();

        builder.Property(d => d.UpdatedAt)
            .IsRequired();
    }
}
