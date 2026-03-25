using PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;

namespace PuzKit3D.Modules.Wallet.Application.UseCases.Wallets.Queries;

public sealed record WalletTransactionDto(
    Guid Id,
    Guid UserId,
    decimal Amount,
    WalletTransactionType Type,
    Guid OrderId,
    DateTime CreatedAt);
