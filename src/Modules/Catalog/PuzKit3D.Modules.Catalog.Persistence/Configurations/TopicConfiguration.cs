using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable("topic");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => TopicId.From(value))
            .HasColumnName("id");

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("name");

        builder.Property(t => t.Description)
            .HasColumnName("description");

        builder.Property(t => t.Slug)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("slug");

        builder.Property(t => t.ParentId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => TopicId.From(value))
            .HasColumnName("parent_id");

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_active");

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(t => t.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(t => t.Slug)
            .IsUnique();

        builder.HasIndex(t => t.ParentId);
    }
}
