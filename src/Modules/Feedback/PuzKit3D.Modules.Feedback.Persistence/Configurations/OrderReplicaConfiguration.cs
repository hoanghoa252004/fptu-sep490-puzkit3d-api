using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Feedback.Domain.Entities.OrderReplicas;

namespace PuzKit3D.Modules.Feedback.Persistence.Configurations;

internal sealed class OrderReplicaConfiguration : IEntityTypeConfiguration<OrderReplica>
{
    public void Configure(EntityTypeBuilder<OrderReplica> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Type)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(o => o.CustomerId)
            .IsRequired();

        builder.Property(o => o.CreatedAt)
            .IsRequired();

        builder.Property(o => o.UpdatedAt)
            .IsRequired();

        builder.Property(o => o.Status)
            .IsRequired()
            .HasMaxLength(30);

        builder.HasIndex(o => o.CustomerId);
    }
}

