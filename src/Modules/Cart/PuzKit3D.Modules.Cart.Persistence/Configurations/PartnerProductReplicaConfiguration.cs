using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class PartnerProductReplicaConfiguration : IEntityTypeConfiguration<PartnerProductReplica>
{
    public void Configure(EntityTypeBuilder<PartnerProductReplica> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id);

        builder.Property(p => p.PartnerId)
            .IsRequired();

        builder.Property(p => p.PartnerProductSku)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.BriefDescription);

        builder.Property(p => p.DetailDescription);

        builder.Property(p => p.ProductCatalog)
            .HasColumnType("jsonb");

        builder.Property(p => p.ThumbnailUrl)
            .IsRequired();

        builder.Property(p => p.PreviewAsset)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.HasIndex(p => p.Slug)
            .IsUnique()
            .HasDatabaseName("UK__partner_product_replica__slug");

        builder.HasIndex(p => new { p.PartnerId, p.PartnerProductSku })
            .IsUnique()
            .HasDatabaseName("CUK___partner_product_replica___partner_id__partner_product_sku");

        builder.Ignore(p => p.DomainEvents);
    }
}
