using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => TopicId.From(value));

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(t => t.Description);

        builder.Property(t => t.Slug)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(t => t.ParentId)
            .IsRequired(false)
            .HasConversion(
                id => id == null ? (Guid?)null : id.Value,
                value => value == null ? null : TopicId.From(value.Value));

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();

        builder.HasIndex(t => t.Slug)
            .IsUnique();

        builder.HasIndex(t => t.ParentId);
    }
}
