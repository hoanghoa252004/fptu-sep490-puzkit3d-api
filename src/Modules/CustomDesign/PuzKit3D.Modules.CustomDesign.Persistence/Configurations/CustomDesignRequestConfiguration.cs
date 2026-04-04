using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequests;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class CustomDesignRequestConfiguration : IEntityTypeConfiguration<CustomDesignRequest>
{
    public void Configure(EntityTypeBuilder<CustomDesignRequest> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => CustomDesignRequestId.From(value));

        builder.Property(r => r.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(r => r.Description)
            .HasColumnType("text")
            .IsRequired(false);

        builder.Property(r => r.CustomDesignRequirementId)
            .HasConversion(
                id => id.Value,
                value => CustomDesignRequirementId.From(value))
            .IsRequired();

        builder.Property(r => r.DesiredPartQuantity)
            .IsRequired();

        builder.Property(r => r.DesiredLengthMm)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(r => r.DesiredWidthMm)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(r => r.DesiredHeightMm)
            .IsRequired()
            .HasPrecision(10, 2);

        builder.Property(r => r.Sketches)
            .HasColumnType("text")
            .IsRequired(false);

        builder.Property(r => r.DesiredDeliveryDate)
            .IsRequired();

        builder.Property(r => r.DesiredQuantity)
            .IsRequired();

        builder.Property(r => r.TargetBudget)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(r => r.UsedSupportConceptDesignTime)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired();

        builder.HasIndex(r => r.Code)
            .IsUnique()
            .HasDatabaseName("UQ___custom_design_request___code");

        builder.Ignore(r => r.DomainEvents);
    }
}
