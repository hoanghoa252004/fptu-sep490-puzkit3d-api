using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityDrives;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Drives;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class CapabilityDriveConfiguration : IEntityTypeConfiguration<CapabilityDrive>
{
    public void Configure(EntityTypeBuilder<CapabilityDrive> builder)
    {
        builder.HasKey(cd => cd.Id);

        builder.Property(cd => cd.Id)
            .HasConversion(
                id => id.Value,
                value => CapabilityDriveId.From(value));

        builder.Property(cd => cd.CapabilityId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => CapabilityId.From(value));

        builder.Property(cd => cd.DriveId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => DriveId.From(value));

        builder.Property(cd => cd.Quantity)
            .IsRequired();
    }
}
