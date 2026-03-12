namespace PuzKit3D.Modules.Payment.Domain.Entities.Payments;

public enum PaymentStatus
{
    Pending = 0,
    Paid = 1,
    Expired = 2,
    Failed = 3,
    Cancelled = 4
}
