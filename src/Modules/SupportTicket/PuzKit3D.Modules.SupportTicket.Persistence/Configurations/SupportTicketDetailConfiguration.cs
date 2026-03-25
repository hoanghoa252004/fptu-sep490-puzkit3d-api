using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTickets;
using SupportTicketDetailEntity = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTicketDetails.SupportTicketDetail;
using SupportTicketDetailIdType = PuzKit3D.Modules.SupportTicket.Domain.Entities.SupportTicketDetails.SupportTicketDetailId;

namespace PuzKit3D.Modules.SupportTicket.Persistence.Configurations;

internal sealed class SupportTicketDetailConfiguration : IEntityTypeConfiguration<SupportTicketDetailEntity>
{
    public void Configure(EntityTypeBuilder<SupportTicketDetailEntity> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasConversion(
                id => id.Value,
                value => SupportTicketDetailIdType.From(value));

        builder.Property(d => d.SupportTicketId)
        .HasConversion(
            id => id.Value,
            value => SupportTicketId.From(value))
        .IsRequired();

        builder.Property(d => d.OrderItemId)
            .IsRequired();

        builder.Property(d => d.PartId)
            .IsRequired(false);

        builder.Property(d => d.Quantity)
            .IsRequired();

        builder.Property(d => d.Note)
            .HasColumnType("text")
            .IsRequired(false);

        builder.HasIndex(d => d.SupportTicketId);
        builder.HasIndex(d => d.PartId);
    }
}
