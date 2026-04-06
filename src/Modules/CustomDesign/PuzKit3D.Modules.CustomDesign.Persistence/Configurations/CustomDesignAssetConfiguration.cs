using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignAssets;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class CustomDesignAssetConfiguration : IEntityTypeConfiguration<CustomDesignAsset>
{
    public void Configure(EntityTypeBuilder<CustomDesignAsset> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasConversion(
                id => id.Value,
                value => CustomDesignAssetId.From(value));

        builder.Property(a => a.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(a => a.CustomDesignRequestId)
            .HasConversion(
                id => id.Value,
                value => CustomDesignRequestId.From(value))
            .IsRequired();

        builder.Property(a => a.Version)
            .IsRequired();

        builder.Property(a => a.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(a => a.MultiviewImages)
            .HasColumnType("text")
            .IsRequired(false);

        builder.Property(a => a.CompositeMultiviewImage)
            .HasColumnType("text")
            .IsRequired(false);

        builder.Property(a => a.Rough3DModel)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(a => a.Rough3DModelTaskId)
            .HasMaxLength(500)
            .IsRequired(false);

        builder.Property(a => a.CustomerPrompt)
            .HasColumnType("text")
            .IsRequired(false);

        builder.Property(a => a.NormalizePrompt)
            .HasColumnType("text")
            .IsRequired(false);

        builder.Property(a => a.IsNeedSupport)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.IsFinalDesign)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.UpdatedAt)
            .IsRequired();

        builder.HasIndex(a => a.Code)
            .IsUnique()
            .HasDatabaseName("UQ___custom_design_asset___code");

        builder.Ignore(a => a.DomainEvents);
    }
}
