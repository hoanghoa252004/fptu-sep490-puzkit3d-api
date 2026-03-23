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
                value => DeliveryTrackingId.From(value))
            .IsRequired();

        // ItemId - can be VariantId (Product) or PartId (Part)
        builder.Property(dtd => dtd.ItemId)
            .IsRequired();

        builder.Property(dtd => dtd.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(dtd => dtd.Quantity)
            .IsRequired();

        // Indexes for common queries
        builder.HasIndex(dtd => dtd.DeliveryTrackingId);
        builder.HasIndex(dtd => dtd.ItemId);
    }
}
