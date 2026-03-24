using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Wallet.Domain.Entities.Wallets;

public static class WalletError
{
    public static Error InvalidUserId() =>
        Error.Validation("Wallet.InvalidUserId", "User ID cannot be empty.");

    public static Error InvalidBalance() =>
        Error.Validation("Wallet.InvalidBalance", "Balance cannot be negative.");

    public static Error InvalidAmount() =>
        Error.Validation("Wallet.InvalidAmount", "Amount must be greater than zero.");

    public static Error InsufficientBalance() =>
        Error.Failure("Wallet.InsufficientBalance", "Insufficient balance to perform this operation.");

    public static Error WalletNotFound(Guid walletId) =>
        Error.NotFound("Wallet.WalletNotFound", $"Wallet with ID '{walletId}' was not found.");

    public static Error WalletNotFoundForUser(Guid userId) =>
        Error.NotFound("Wallet.WalletNotFoundForUser", $"No wallet found for user '{userId}'.");
}
