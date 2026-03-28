using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.InStock.Domain.Entities.Replicas;

namespace PuzKit3D.Modules.InStock.Persistence.Configurations;

internal sealed class SupportTicketReplicaConfiguration : IEntityTypeConfiguration<SupportTicketReplica>
{
    public void Configure(EntityTypeBuilder<SupportTicketReplica> builder)
    {
        builder.HasKey(st => st.Id);

        builder.Property(st => st.UserId)
            .IsRequired();

        builder.Property(st => st.OrderId)
            .IsRequired();

        builder.Property(st => st.Type)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(st => st.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(st => st.Reason)
            .HasColumnType("text")
            .IsRequired();

        builder.Property(st => st.Proof)
            .HasColumnType("varchar(500)")
            .IsRequired();

        builder.Property(st => st.CreatedAt)
            .IsRequired();

        builder.Property(st => st.UpdatedAt)
            .IsRequired();

        builder.Property(st => st.Code)
            .IsRequired(false);


        builder.HasMany(st => st.Details)
            .WithOne()
            .HasForeignKey(d => d.SupportTicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(st => st.UserId);
        builder.HasIndex(st => st.OrderId);
        builder.HasIndex(st => st.Status);
    }
}
