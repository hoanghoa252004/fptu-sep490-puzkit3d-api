namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

public enum InstockOrderStatus
{
    Waiting = 0,
    Pending = 1,
    Paid = 2,
    Processing = 3,
    HandedOverToDelivery = 4,
    Completed = 5,
    Expired = 6,
    Cancelled = 7,
    Returned = 8,
    Refunded = 9
}
