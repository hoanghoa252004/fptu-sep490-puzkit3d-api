using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class AssemblyMethodReplicaConfiguration : IEntityTypeConfiguration<AssemblyMethodReplica>
{
    public void Configure(EntityTypeBuilder<AssemblyMethodReplica> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(a => a.Description)
            .HasColumnType("text");

        builder.Property(a => a.Slug)
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(a => a.FactorPercentage)
            .IsRequired()
            .HasPrecision(5, 4);

        builder.Property(a => a.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(a => a.CreatedAt)
            .IsRequired();

        builder.Property(a => a.UpdatedAt)
            .IsRequired();

        builder.HasIndex(a => a.Slug)
            .IsUnique()
            .HasDatabaseName("UK__assembly_method_replica__slug");

        builder.Ignore(a => a.DomainEvents);
    }
}
