using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.CompletedOrderReplicas;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Configurations;

internal sealed class CompletedOrderReplicaConfiguration : IEntityTypeConfiguration<CompletedOrderReplica>
{
    public void Configure(EntityTypeBuilder<CompletedOrderReplica> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Code)
            .IsRequired();

        builder.Property(c => c.Type)
            .IsRequired();

        builder.Property(c => c.CustomerId)
            .IsRequired();

        builder.HasMany(c => c.OrderItems)
            .WithOne()
            .HasForeignKey(i => i.CompletedOrderReplicaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.CustomerId);
    }
}
