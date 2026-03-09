using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class TopicReplicaConfiguration : IEntityTypeConfiguration<TopicReplica>
{
    public void Configure(EntityTypeBuilder<TopicReplica> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(t => t.Description)
            .HasColumnType("text");

        builder.Property(t => t.Slug)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(t => t.ParentId)
            .IsRequired();

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        builder.Property(t => t.UpdatedAt)
            .IsRequired();

        builder.HasIndex(t => t.Slug)
            .IsUnique()
            .HasDatabaseName("UK__topic_replica__slug");

        builder.Ignore(t => t.DomainEvents);
    }
}
