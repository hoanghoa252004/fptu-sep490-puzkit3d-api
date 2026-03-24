using PuzKit3D.SharedKernel.Domain;
using PuzKit3D.SharedKernel.Domain.Results;

namespace PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;

public sealed class Wallet : AggregateRoot<WalletId>
{
    public Guid UserId { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Wallet(
        WalletId id,
        Guid userId,
        decimal balance,
        DateTime createdAt) : base(id)
    {
        UserId = userId;
        Balance = balance;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
    }

    private Wallet() : base()
    {
    }

    public static ResultT<Wallet> Create(
        Guid userId,
        decimal initialBalance = 0,
        DateTime? createdAt = null)
    {
        if (userId == Guid.Empty)
            return Result.Failure<Wallet>(WalletError.InvalidUserId());

        if (initialBalance < 0)
            return Result.Failure<Wallet>(WalletError.InvalidBalance());

        var now = createdAt ?? DateTime.UtcNow;

        var wallet = new Wallet(
            WalletId.Create(),
            userId,
            initialBalance,
            now);

        return Result.Success(wallet);
    }

    public Result AddCoin(decimal amount)
    {
        if (amount <= 0)
            return Result.Failure(WalletError.InvalidAmount());

        Balance += amount;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result DeductCoin(decimal amount)
    {
        if (amount <= 0)
            return Result.Failure(WalletError.InvalidAmount());

        if (Balance < amount)
            return Result.Failure(WalletError.InsufficientBalance());

        Balance -= amount;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result RefundCoin(decimal amount)
    {
        if (amount <= 0)
            return Result.Failure(WalletError.InvalidAmount());

        Balance += amount;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}
