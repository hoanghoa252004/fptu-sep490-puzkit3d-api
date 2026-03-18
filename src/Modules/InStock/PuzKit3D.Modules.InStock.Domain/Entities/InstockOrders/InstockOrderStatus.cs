namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

public enum InstockOrderStatus
{
    Waiting = 0,
    Pending = 1,
    Paid = 2,
    Expired = 3,
    Processing = 4,
    Shipping = 5,
    Delivered = 6,
    Completed = 7,
    Cancelled = 8,
    HandedOverToDelivery = 9,
    Rejected = 10,
    Returned = 11
}
