namespace PuzKit3D.Modules.Delivery.Domain.Entities.DeliveryTrackings;


public enum DeliveryTrackingStatus
{

    ReadyToPick = 0,

    HandedOverToDelivery = 1,

    Shipping = 2,

    Delivered = 3,

    Return = 4,

    Returned = 5
}
