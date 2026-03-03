using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class InStockProductReplicaConfiguration : IEntityTypeConfiguration<InStockProductReplica>
{
    public void Configure(EntityTypeBuilder<InStockProductReplica> builder)
    {
        builder.ToTable("instock_product_replica");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => InStockProductReplicaId.From(value))
            .HasColumnName("id");

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("code");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("name");

        builder.Property(p => p.BriefDescription)
            .HasColumnName("brief_description");

        builder.Property(p => p.DetailDescription)
            .HasColumnName("detail_description");

        builder.Property(p => p.DifficultLevel)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("difficult_level");

        builder.Property(p => p.EstimatedBuildTime)
            .IsRequired()
            .HasColumnName("estimated_build_time");

        builder.Property(p => p.ThumbnailUrl)
            .IsRequired()
            .HasColumnName("thumbnail_url");

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("slug");

        builder.Property(p => p.Specification)
            .HasColumnType("jsonb")
            .HasColumnName("specification");

        builder.Property(p => p.PreviewAsset)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasColumnName("preview_asset");

        builder.Property(p => p.TopicId)
            .IsRequired()
            .HasColumnName("topic_id");

        builder.Property(p => p.AssemblyMethod)
            .IsRequired()
            .HasColumnName("assembly_method");

        builder.Property(p => p.Capability)
            .IsRequired()
            .HasColumnName("capability");

        builder.Property(p => p.Material)
            .IsRequired()
            .HasColumnName("material");

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName("is_active");

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Property(p => p.UpdatedAt)
            .IsRequired()
            .HasColumnName("updated_at");

        builder.HasIndex(p => p.Slug)
            .IsUnique()
            .HasDatabaseName("UK__instock_product_replica__slug");

        builder.Ignore(p => p.DomainEvents);
    }
}
