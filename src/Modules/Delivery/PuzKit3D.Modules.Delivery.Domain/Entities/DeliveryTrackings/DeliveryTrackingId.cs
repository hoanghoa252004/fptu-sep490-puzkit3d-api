using PuzKit3D.SharedKernel.Domain;

namespace PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;

/// <summary>
/// Strong-typed ID for DeliveryTracking
/// </summary>
public sealed class DeliveryTrackingId : StronglyTypedId<Guid>
{
    private DeliveryTrackingId(Guid value) : base(value)
    {
    }

    public static DeliveryTrackingId From(Guid value) => new(value);

    public static DeliveryTrackingId NewId() => new(Guid.NewGuid());
}
