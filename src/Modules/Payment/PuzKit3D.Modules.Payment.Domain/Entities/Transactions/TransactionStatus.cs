namespace PuzKit3D.Modules.Payment.Domain.Entities.Transactions;

public enum TransactionStatus
{
    Pending = 0,
    Processing = 1,
    Success = 2,
    Failed = 3,
    Cancelled = 4
}
