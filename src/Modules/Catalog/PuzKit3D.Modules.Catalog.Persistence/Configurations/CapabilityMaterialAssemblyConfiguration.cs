using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Catalog.Domain.Entities.CapabilityMaterialAssemblies;
using PuzKit3D.Modules.Catalog.Domain.Entities.Capabilities;
using PuzKit3D.Modules.Catalog.Domain.Entities.Materials;
using PuzKit3D.Modules.Catalog.Domain.Entities.AssemblyMethods;

namespace PuzKit3D.Modules.Catalog.Persistence.Configurations;

internal sealed class CapabilityMaterialAssemblyConfiguration : IEntityTypeConfiguration<CapabilityMaterialAssembly>
{
    public void Configure(EntityTypeBuilder<CapabilityMaterialAssembly> builder)
    {
        builder.HasKey(cma => cma.Id);

        builder.Property(cma => cma.Id)
            .HasConversion(
                id => id.Value,
                value => CapabilityMaterialAssemblyId.From(value));

        builder.Property(cma => cma.CapabilityId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => CapabilityId.From(value));

        builder.Property(cma => cma.MaterialId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => MaterialId.From(value));

        builder.Property(cma => cma.AssemblyId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => AssemblyMethodId.From(value));

        builder.Property(cma => cma.IsActive)
            .IsRequired()
            .HasDefaultValue(false);
    }
}
