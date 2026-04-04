using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class CustomDesignRequirementConfiguration : IEntityTypeConfiguration<CustomDesignRequirement>
{
    public void Configure(EntityTypeBuilder<CustomDesignRequirement> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                value => CustomDesignRequirementId.From(value));

        builder.Property(r => r.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(r => r.TopicId)
            .IsRequired();

        builder.Property(r => r.MaterialId)
            .IsRequired();

        builder.Property(r => r.AssemblyMethodId)
            .IsRequired();

        builder.Property(r => r.Difficulty)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(r => r.MinPartQuantity)
            .IsRequired();

        builder.Property(r => r.MaxPartQuantity)
            .IsRequired();

        builder.Property(r => r.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired();

        builder.HasIndex(r => r.Code)
            .IsUnique()
            .HasDatabaseName("UQ___custom_design_requirement___code");

        builder.Ignore(r => r.DomainEvents);
    }
}
