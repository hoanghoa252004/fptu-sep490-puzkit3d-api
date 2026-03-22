using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SupportTicketEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.SupportTicket;
using SupportTicketIdType = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets.SupportTicketId;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Configurations;

internal sealed class SupportTicketConfiguration : IEntityTypeConfiguration<SupportTicketEntity>
{
    public void Configure(EntityTypeBuilder<SupportTicketEntity> builder)
    {
        builder.HasKey(st => st.Id);

        builder.Property(st => st.Id)
            .HasConversion(
                id => id.Value,
                value => SupportTicketIdType.From(value));

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

        builder.HasMany(st => st.Details)
            .WithOne()
            .HasForeignKey(d => d.SupportTicketId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(st => st.UserId);
        builder.HasIndex(st => st.OrderId);
        builder.HasIndex(st => st.Status);
    }
}
