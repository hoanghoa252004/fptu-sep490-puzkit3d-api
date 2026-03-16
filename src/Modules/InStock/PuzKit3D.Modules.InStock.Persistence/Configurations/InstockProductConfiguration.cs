using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.InstockProducts;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class InstockProductConfiguration : IEntityTypeConfiguration<InstockProduct>
{
    public void Configure(EntityTypeBuilder<InstockProduct> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => InstockProductId.From(value));

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
            .HasDatabaseName("UK__instock_product__slug");

        builder.Navigation(p => p.Parts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(p => p.CapabilityDetails)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(p => p.DomainEvents);
    }
}
