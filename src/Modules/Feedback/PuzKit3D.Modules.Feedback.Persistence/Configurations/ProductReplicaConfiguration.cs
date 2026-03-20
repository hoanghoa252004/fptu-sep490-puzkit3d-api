using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Feedback.Domain.Entities.ProductReplicas;

namespace PuzKit3D.Modules.Feedback.Persistence.Configurations;

internal sealed class ProductReplicaConfiguration : IEntityTypeConfiguration<ProductReplica>
{
    public void Configure(EntityTypeBuilder<ProductReplica> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(p => p.Type);
    }
}
