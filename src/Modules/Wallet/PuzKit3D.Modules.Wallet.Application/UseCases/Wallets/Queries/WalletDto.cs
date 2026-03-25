namespace PuzKit3D.Modules.Wallet.Application.UseCases.Wallets.Queries;

public sealed record WalletResponseDto(
    Guid Id,
    Guid UserId,
    decimal Balance,
    DateTime CreatedAt,
    DateTime UpdatedAt);
