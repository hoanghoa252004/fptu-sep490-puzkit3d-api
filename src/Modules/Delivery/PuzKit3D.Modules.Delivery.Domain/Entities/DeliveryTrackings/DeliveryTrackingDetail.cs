using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;

public sealed class DeliveryTrackingDetail : Entity<Guid>
{
    /// <summary>
    /// Reference t?i DeliveryTracking
    /// </summary>
    public DeliveryTrackingId DeliveryTrackingId { get; private set; } = null!;

    /// <summary>
    /// Item ID - có th? là VariantId (Product) ho?c PartId (Part)
    /// </summary>
    public Guid ItemId { get; private set; }

    /// <summary>
    /// Lo?i item (Product hay Part)
    /// </summary>
    public DeliveryTrackingDetailType Type { get; private set; }

    /// <summary>
    /// S? l??ng item trong delivery này
    /// </summary>
    public int Quantity { get; private set; }

    private DeliveryTrackingDetail(
        Guid id,
        DeliveryTrackingId deliveryTrackingId,
        Guid itemId,
        DeliveryTrackingDetailType type,
        int quantity) : base(id)
    {
        DeliveryTrackingId = deliveryTrackingId;
        ItemId = itemId;
        Type = type;
        Quantity = quantity;
    }

    private DeliveryTrackingDetail() : base()
    {
    }

    /// <summary>
    /// Factory method t?o DeliveryTrackingDetail
    /// </summary>
    public static DeliveryTrackingDetail Create(
        DeliveryTrackingId deliveryTrackingId,
        Guid itemId,
        DeliveryTrackingDetailType type,
        int quantity)
    {
        return new DeliveryTrackingDetail(
            Guid.NewGuid(),
            deliveryTrackingId,
            itemId,
            type,
            quantity);
    }

    /// <summary>
    /// Create product variant item
    /// </summary>
    public static DeliveryTrackingDetail CreateProduct(
        DeliveryTrackingId deliveryTrackingId,
        Guid variantId,
        int quantity)
    {
        return Create(deliveryTrackingId, variantId, DeliveryTrackingDetailType.Product, quantity);
    }

    /// <summary>
    /// Create part item
    /// </summary>
    public static DeliveryTrackingDetail CreatePart(
        DeliveryTrackingId deliveryTrackingId,
        Guid partId,
        int quantity)
    {
        return Create(deliveryTrackingId, partId, DeliveryTrackingDetailType.Part, quantity);
    }

    /// <summary>
    /// Update quantity
    /// </summary>
    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than 0");

        Quantity = quantity;
    }
}
