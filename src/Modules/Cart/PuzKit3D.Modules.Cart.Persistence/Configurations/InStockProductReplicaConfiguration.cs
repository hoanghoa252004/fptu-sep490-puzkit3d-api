using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Cart.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.Cart.Persistence.Configurations;

internal sealed class InStockProductReplicaConfiguration : IEntityTypeConfiguration<InStockProductReplica>
{
    public void Configure(EntityTypeBuilder<InStockProductReplica> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.Description)
            .HasColumnType("text");

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(p => p.TotalPieceCount)
            .IsRequired();

        builder.Property(p => p.DifficultLevel)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.EstimatedBuildTime)
            .IsRequired();

        builder.Property(p => p.ThumbnailUrl)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(p => p.PreviewAsset)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(p => p.TopicId)
            .IsRequired();

        builder.Property(p => p.AssemblyMethodId)
            .IsRequired();

        builder.Property(p => p.CapabilityId)
            .IsRequired();

        builder.Property(p => p.MaterialId)
            .IsRequired();

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();
        builder.HasIndex(p => p.Slug)
            .IsUnique()
            .HasDatabaseName("UK__instock_product_replica__slug");

        builder.Ignore(p => p.DomainEvents);
    }
}
