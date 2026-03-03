using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class PartnerProductReplicaConfiguration : IEntityTypeConfiguration<PartnerProductReplica>
{
    public void Configure(EntityTypeBuilder<PartnerProductReplica> builder)
    {
        builder.ToTable("partner_product_replica");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.PartnerId)
            .IsRequired()
            .HasColumnName("partner_id");

        builder.Property(p => p.PartnerProductSku)
            .IsRequired()
            .HasMaxLength(10)
            .HasColumnName("partner_product_sku");

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("name");

        builder.Property(p => p.BriefDescription)
            .HasColumnName("brief_description");

        builder.Property(p => p.DetailDescription)
            .HasColumnName("detail_description");

        builder.Property(p => p.ProductCatalog)
            .HasColumnType("jsonb")
            .HasColumnName("product_catalog");

        builder.Property(p => p.ThumbnailUrl)
            .IsRequired()
            .HasColumnName("thumbnail_url");

        builder.Property(p => p.PreviewAsset)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasColumnName("preview_asset");

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("slug");

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
            .HasDatabaseName("UK__partner_product_replica__slug");

        builder.HasIndex(p => new { p.PartnerId, p.PartnerProductSku })
            .IsUnique()
            .HasDatabaseName("CUK___partner_product_replica___partner_id__partner_product_sku");

        builder.Ignore(p => p.DomainEvents);
    }
}
