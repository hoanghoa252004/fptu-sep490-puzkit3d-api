using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.CustomDesign.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.CustomDesign.Persistence.Configurations;

internal sealed class MaterialReplicaConfiguration : IEntityTypeConfiguration<MaterialReplica>
{
    public void Configure(EntityTypeBuilder<MaterialReplica> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.Description)
            .HasColumnType("text");

        builder.Property(m => m.Slug)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(m => m.IsActive)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();

        builder.HasIndex(m => m.Slug)
            .IsUnique()
            .HasDatabaseName("UK__material_replica__slug");

        builder.Ignore(m => m.DomainEvents);
    }
}
