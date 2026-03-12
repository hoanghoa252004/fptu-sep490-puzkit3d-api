namespace PuzKit3D.Modules.Payment.Domain.Entities.Transactions;

public enum TransactionStatus
{
    Pending = 0,
    Success = 1, 
    Cancelled = 2,
    Failed = 3,
    Expired = 4
}
