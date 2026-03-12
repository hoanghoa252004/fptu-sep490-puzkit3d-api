namespace PuzKit3D.Modules.InStock.Domain.Entities.InstockOrders;

public enum InstockOrderStatus
{
    PaymentPending = 0,
    Paid = 1,
    Expired = 2,
    Processing = 3,
    Shipped = 4,
    Completed = 5
}
