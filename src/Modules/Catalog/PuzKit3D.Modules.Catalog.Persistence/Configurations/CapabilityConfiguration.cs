using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class CapabilityConfiguration : IEntityTypeConfiguration<Capability>
{
    public void Configure(EntityTypeBuilder<Capability> builder)
    {
        builder.ToTable("capability");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CapabilityId.From(value))
            .HasColumnName("id");

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("name");

        builder.Property(c => c.Description)
            .HasColumnName("description");

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("slug");

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_active");

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(c => c.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(c => c.Slug)
            .IsUnique();
    }
}
