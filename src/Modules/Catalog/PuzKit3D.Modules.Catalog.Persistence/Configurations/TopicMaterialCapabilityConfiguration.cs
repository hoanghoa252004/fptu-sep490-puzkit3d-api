using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.TopicMaterialCapabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Topics;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class TopicMaterialCapabilityConfiguration : IEntityTypeConfiguration<TopicMaterialCapability>
{
    public void Configure(EntityTypeBuilder<TopicMaterialCapability> builder)
    {
        builder.HasKey(tmc => tmc.Id);

        builder.Property(tmc => tmc.Id)
            .HasConversion(
                id => id.Value,
                value => TopicMaterialCapabilityId.From(value));

        builder.Property(tmc => tmc.TopicId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => TopicId.From(value));

        builder.Property(tmc => tmc.MaterialId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => MaterialId.From(value));

        builder.Property(tmc => tmc.CapabilityId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => CapabilityId.From(value));

        builder.Property(tmc => tmc.IsActive)
            .IsRequired()
            .HasDefaultValue(false);
    }
}
