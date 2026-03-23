using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;

namespace PuzKit3D.Modules.Delivery.Persistence.Configurations;

/// <summary>
/// EF Core configuration for DeliveryTrackingDetail entity
/// </summary>
internal sealed class DeliveryTrackingDetailConfiguration : IEntityTypeConfiguration<DeliveryTrackingDetail>
{
    public void Configure(EntityTypeBuilder<DeliveryTrackingDetail> builder)
    {
        builder.HasKey(dtd => dtd.Id);

        // DeliveryTrackingId - foreign key
        builder.Property(dtd => dtd.DeliveryTrackingId)
            .HasConversion(
                id => id.Value,
                value => DeliveryTrackingId.From(value));

        // ItemId - can be VariantId, PartId, OrderDetailId, etc.
        builder.Property(dtd => dtd.ItemId)
            .IsRequired();

        // Quantity
        builder.Property(dtd => dtd.Quantity)
            .IsRequired();

        // Indexes for common queries
        builder.HasIndex(dtd => dtd.DeliveryTrackingId);
        builder.HasIndex(dtd => dtd.ItemId);
    }
}
