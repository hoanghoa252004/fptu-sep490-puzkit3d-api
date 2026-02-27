using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class AssemblyMethodConfiguration : IEntityTypeConfiguration<AssemblyMethod>
{
    public void Configure(EntityTypeBuilder<AssemblyMethod> builder)
    {
        builder.ToTable("assembly_method");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => AssemblyMethodId.From(value))
            .HasColumnName("id");

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("name");

        builder.Property(a => a.Description)
            .HasColumnName("description");

        builder.Property(a => a.Slug)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("slug");

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_active");

        builder.Property(a => a.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(a => a.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(a => a.Slug)
            .IsUnique();
    }
}
