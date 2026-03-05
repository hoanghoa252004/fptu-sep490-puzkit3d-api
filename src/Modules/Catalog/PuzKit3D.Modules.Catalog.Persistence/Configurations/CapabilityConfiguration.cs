using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class CapabilityConfiguration : IEntityTypeConfiguration<Capability>
{
    public void Configure(EntityTypeBuilder<Capability> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CapabilityId.From(value));

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(c => c.Description);

        builder.Property(c => c.Slug)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.HasIndex(c => c.Slug)
            .IsUnique();
    }
}
