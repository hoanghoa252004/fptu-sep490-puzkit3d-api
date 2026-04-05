using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.RequirementCapabilityDetails;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.CustomDesignRequirements;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class RequirementCapabilityDetailConfiguration : IEntityTypeConfiguration<RequirementCapabilityDetail>
{
    public void Configure(EntityTypeBuilder<RequirementCapabilityDetail> builder)
    {
        builder.HasKey(rc => rc.Id);

        builder.Property(rc => rc.Id)
            .HasConversion(
                id => id.Value,
                value => RequirementCapabilityDetailId.From(value));

        builder.Property(rc => rc.CustomDesignRequirementId)
            .HasConversion(
                id => id.Value,
                value => CustomDesignRequirementId.From(value))
            .IsRequired();

        builder.Property(rc => rc.CapabilityId)
            .IsRequired();

        builder.HasIndex(rc => new { rc.CustomDesignRequirementId, rc.CapabilityId })
            .IsUnique()
            .HasDatabaseName("UQ___requirement_capability_detail___requirement_id__capability_id");

        builder.Ignore(rc => rc.DomainEvents);
    }
}
