using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.ToTable("material");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasConversion(
                id => id.Value,
                value => MaterialId.From(value))
            .HasColumnName("id");

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("name");

        builder.Property(m => m.Description)
            .HasColumnName("description");

        builder.Property(m => m.Slug)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("slug");

        builder.Property(m => m.IsActive)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_active");

        builder.Property(m => m.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(m => m.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(m => m.Slug)
            .IsUnique();
    }
}
