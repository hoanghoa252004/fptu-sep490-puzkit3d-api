using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;

public sealed class WalletTransaction : Entity<WalletTransactionId>
{
    public Guid UserId { get; private set; }
    public decimal Amount { get; private set; }
    public WalletTransactionType Type { get; private set; }
    public Guid OrderId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private WalletTransaction(
        WalletTransactionId id,
        Guid userId,
        decimal amount,
        WalletTransactionType type,
        Guid orderId,
        DateTime createdAt) : base(id)
    {
        UserId = userId;
        Amount = amount;
        Type = type;
        OrderId = orderId;
        CreatedAt = createdAt;
    }

    private WalletTransaction() : base()
    {
    }

    public static ResultT<WalletTransaction> Create(
        Guid userId,
        decimal amount,
        WalletTransactionType type,
        Guid orderId,
        DateTime? createdAt = null)
    {
        if (userId == Guid.Empty)
            return Result.Failure<WalletTransaction>(WalletTransactionError.InvalidUserId());

        if (amount <= 0)
            return Result.Failure<WalletTransaction>(WalletTransactionError.InvalidAmount());

        if (orderId == Guid.Empty)
            return Result.Failure<WalletTransaction>(WalletTransactionError.InvalidOrderId());

        var now = createdAt ?? DateTime.UtcNow;

        var transaction = new WalletTransaction(
            WalletTransactionId.Create(),
            userId,
            amount,
            type,
            orderId,
            now);

        return Result.Success(transaction);
    }
}
