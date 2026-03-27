using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class SupportTicketDetailReplicaConfiguration : IEntityTypeConfiguration<SupportTicketDetailReplica>
{
    public void Configure(EntityTypeBuilder<SupportTicketDetailReplica> builder)
    {
        builder.HasKey(std => std.Id);

        builder.Property(std => std.SupportTicketId)
            .IsRequired();

        builder.Property(std => std.OrderItemId)
            .IsRequired();

        builder.Property(std => std.PartId)
            .IsRequired(false);

        builder.Property(std => std.Quantity)
            .IsRequired();

        builder.Property(std => std.Note)
            .HasColumnType("varchar(500)")
            .IsRequired(false);

        builder.HasIndex(std => std.SupportTicketId);
        builder.HasIndex(std => std.OrderItemId);
    }
}
