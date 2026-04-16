namespace PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;

/// <summary>
/// Lo?i item trong delivery tracking
/// </summary>
public enum DeliveryTrackingDetailType
{
    /// <summary>
    /// Product variant item
    /// </summary>
    Product = 0,

    /// <summary>
    /// Part item (for assembly)
    /// </summary>
    Drive = 1
}
