using PuzKit3D.SharedKernel.Domain.Errors;

namespace PuzKit3D.Modules.Wallet.Domain.Entities.WalletTransactions;

public static class WalletTransactionError
{
    public static Error InvalidUserId() =>
        Error.Validation("WalletTransaction.InvalidUserId", "User ID cannot be empty.");

    public static Error InvalidAmount() =>
        Error.Validation("WalletTransaction.InvalidAmount", "Amount must be greater than zero.");

    public static Error InvalidOrderId() =>
        Error.Validation("WalletTransaction.InvalidOrderId", "Order ID cannot be empty.");

    public static Error TransactionNotFound(Guid transactionId) =>
        Error.NotFound("WalletTransaction.TransactionNotFound", $"Transaction with ID '{transactionId}' was not found.");
}
