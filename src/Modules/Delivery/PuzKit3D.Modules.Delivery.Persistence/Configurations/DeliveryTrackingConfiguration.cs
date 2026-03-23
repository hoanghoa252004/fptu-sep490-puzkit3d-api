using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;

namespace PuzKit3D.Modules.Delivery.Persistence.Configurations;

/// <summary>
/// EF Core configuration for DeliveryTracking entity
/// </summary>
internal sealed class DeliveryTrackingConfiguration : IEntityTypeConfiguration<DeliveryTracking>
{
    public void Configure(EntityTypeBuilder<DeliveryTracking> builder)
    {
        builder.HasKey(dt => dt.Id);

        // Map Id property
        builder.Property(dt => dt.Id)
            .HasConversion(
                id => id.Value,
                value => DeliveryTrackingId.From(value));

        // OrderId
        builder.Property(dt => dt.OrderId)
            .IsRequired();

        // SupportTicketId - nullable, references SupportTicket module
        builder.Property(dt => dt.SupportTicketId)
            .IsRequired(false);

        // DeliveryOrderCode - GHN order code, must be unique
        builder.Property(dt => dt.DeliveryOrderCode)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(dt => dt.DeliveryOrderCode)
            .IsUnique();

        // Status enum conversion
        builder.Property(dt => dt.Status)
            .IsRequired()
            .HasConversion<string>();

        // Type enum conversion
        builder.Property(dt => dt.Type)
            .IsRequired()
            .HasConversion<string>();

        // Note - optional text
        builder.Property(dt => dt.Note)
            .IsRequired(false)
            .HasMaxLength(1000);

        // Dates
        builder.Property(dt => dt.ExpectedDeliveryDate)
            .IsRequired();

        builder.Property(dt => dt.DeliveredAt)
            .IsRequired(false);

        builder.Property(dt => dt.CreatedAt)
            .IsRequired();

        builder.Property(dt => dt.UpdatedAt)
            .IsRequired();

        // Navigation - DeliveryTrackingDetails
        builder.HasMany(dt => dt.Details)
            .WithOne()
            .HasForeignKey(dtd => dtd.DeliveryTrackingId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for common queries
        builder.HasIndex(dt => dt.OrderId);
        builder.HasIndex(dt => dt.SupportTicketId);
        builder.HasIndex(dt => dt.Status);
        builder.HasIndex(dt => dt.Type);
        builder.HasIndex(dt => dt.CreatedAt);
    }
}
