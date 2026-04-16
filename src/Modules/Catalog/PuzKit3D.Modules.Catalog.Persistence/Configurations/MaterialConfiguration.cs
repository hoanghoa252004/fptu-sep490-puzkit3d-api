using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasConversion(
                id => id.Value,
                value => MaterialId.From(value));

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(m => m.Description)
            .HasColumnType("text");

        builder.Property(m => m.Slug)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(m => m.FactorPercentage)
            .IsRequired()
            .HasPrecision(5, 4);

        builder.Property(m => m.BasePrice)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(m => m.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();

        builder.HasIndex(m => m.Slug)
            .IsUnique();
    }
}
