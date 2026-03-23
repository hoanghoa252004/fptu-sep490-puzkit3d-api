using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;

public sealed class DeliveryTrackingDetail : Entity<Guid>
{

    public DeliveryTrackingId DeliveryTrackingId { get; private set; } = null!;
    public Guid ItemId { get; private set; }
    public int Quantity { get; private set; }

    private DeliveryTrackingDetail(
        Guid id,
        DeliveryTrackingId deliveryTrackingId,
        Guid itemId,
        int quantity) : base(id)
    {
        DeliveryTrackingId = deliveryTrackingId;
        ItemId = itemId;
        Quantity = quantity;
    }

    private DeliveryTrackingDetail() : base()
    {
    }

    public static DeliveryTrackingDetail Create(
        DeliveryTrackingId deliveryTrackingId,
        Guid itemId,
        int quantity)
    {
        return new DeliveryTrackingDetail(
            Guid.NewGuid(),
            deliveryTrackingId,
            itemId,
            quantity);
    }

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");

        Quantity = quantity;
    }
}
