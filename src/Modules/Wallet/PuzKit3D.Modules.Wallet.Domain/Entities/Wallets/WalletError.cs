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

    public static Error WalletConfigNotFound() =>
        Error.NotFound("Wallet.WalletConfigNotFound", "Wallet configuration was not found.");

    public static Error InvalidOnlineOrderReturnPercentage() =>
        Error.Validation("Wallet.InvalidOnlineOrderReturnPercentage", "Online order return percentage must be between 0 and 100.");

    public static Error InvalidOnlineOrderCompletedRewardPercentage() =>
        Error.Validation("Wallet.InvalidOnlineOrderCompletedRewardPercentage", "Online order completed reward percentage must be between 0 and 100.");

    public static Error InvalidCODOrderCompletedRewardPercentage() =>
        Error.Validation("Wallet.InvalidCODOrderCompletedRewardPercentage", "COD order completed reward percentage must be between 0 and 100.");
}
