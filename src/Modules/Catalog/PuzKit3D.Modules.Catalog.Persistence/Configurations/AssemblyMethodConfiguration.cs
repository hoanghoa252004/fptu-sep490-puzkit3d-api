using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class AssemblyMethodConfiguration : IEntityTypeConfiguration<AssemblyMethod>
{
    public void Configure(EntityTypeBuilder<AssemblyMethod> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => AssemblyMethodId.From(value));

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(a => a.Description)
            .HasColumnType("text");

        builder.Property(a => a.Slug)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.UpdatedAt)
            .IsRequired();

        builder.HasIndex(a => a.Slug)
            .IsUnique();
    }
}
